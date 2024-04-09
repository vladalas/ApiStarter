using BaseStarter.Environment;
using BaseStarter.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BaseStarter.DAL
{

    /// <summary>
    /// Manager for saving objects in the database.
    /// Each object to be save (insert, update, delete) will undergo appropriate validation, i.e. the Validate / CanDelete methods are called
    /// and the BeforeSave / BeforeDelete methods are called - if additional objects are added to insert / update / delete within these methods,
    /// the appropriate validations and methods are applied to them again
    /// </summary>
    public static class DBContextValidationManager
    {
        public static async Task<List<ValidationMessage>>
            SaveChangesWithValidationAsync(this GlobalEnvironment globalEnvironment, IDbContextTransaction? transaction)
        {
            List<BaseDbObject> processedEntitiesBeforeSave = new List<BaseDbObject>();
            List<BaseDbObject> processedEntitiesBeforeDelete = new List<BaseDbObject>();
            globalEnvironment.CallBeforeSaveAndBeforeDelete(1, ref processedEntitiesBeforeSave, ref processedEntitiesBeforeDelete);
            var result = globalEnvironment.ExecuteValidation();
            result.AddRange(globalEnvironment.ValidateDelete());
            if (result.Any()) return result.ToList();

            //Dictionary<EventLog, EntityEntry> eventLogs = globalEnvironment.LogChangesToEventLog();

            globalEnvironment.DbContext.ChangeTracker.AutoDetectChangesEnabled = false; 


            var transactionIsOpen = transaction != null;
            try
            {

                if (!transactionIsOpen)
                {
                    transaction = globalEnvironment.DbContext.Database.BeginTransaction();
                }
               
                await globalEnvironment.DbContext.SaveChangesAsync();
                //globalEnvironment.SaveInEventLogs(eventLogs);

                if (!transactionIsOpen)
                {
                    transaction.Commit();
                }
               

            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
            }
            finally
            {
                globalEnvironment.DbContext.ChangeTracker.AutoDetectChangesEnabled = true;       //LEAVE OUT OF CHAPTER 4 -      

                if (!transactionIsOpen)
                {
                    transaction.Dispose();
                }
            }
            return result.ToList(); //#F
        }

        private static void CallBeforeSaveAndBeforeDelete(this GlobalEnvironment globalEnvironment, int counter, ref List<BaseDbObject> processedEntitiesBeforeSave, ref List<BaseDbObject> processedEntitiesBeforeDelete)
        {
            if (counter > 5) { throw new Exception("Hluboká rekurze CallBeforeSaveAndBeforeDelete"); }

            int numberOfProcededObjectsBeforeSave = globalEnvironment.CallBeforeSave(ref processedEntitiesBeforeSave);
            int numberOfProcededObjectsBeforeDelete = globalEnvironment.CallBeforeDelete(ref processedEntitiesBeforeDelete);
            if (numberOfProcededObjectsBeforeSave + numberOfProcededObjectsBeforeDelete > 0)
            {
                globalEnvironment.CallBeforeSaveAndBeforeDelete(counter + 1, ref processedEntitiesBeforeSave, ref processedEntitiesBeforeDelete);
            }
        }

        //public static async Task<ImmutableList<ValidationResult>> SaveChangesWithValidationAsync(this GlobalEnvironment globalEnvironment)
        //{
        //    CallBeforeSave(globalEnvironment);
        //    CallBeforeDelete(globalEnvironment);
        //    var result = globalEnvironment.ExecuteValidation();
        //    result.AddRange(globalEnvironment.ValidateDelete());
        //    if (result.Any()) return result.ToImmutableList();

        //    globalEnvironment.LogChangesToEventLog();

        //    globalEnvironment.DbContext.ChangeTracker.AutoDetectChangesEnabled = false;
        //    try
        //    {
        //        await globalEnvironment.DbContext.SaveChangesAsync().ConfigureAwait(false);
        //    }
        //    finally
        //    {
        //        globalEnvironment.DbContext.ChangeTracker.AutoDetectChangesEnabled = true;
        //    }
        //    return result.ToImmutableList();
        //}

        /// <summary>
        /// Vrací počet objektů, které byly zpracovány
        /// </summary>
        /// <param name="globalEnvironment"></param>
        /// <param name="processedEntitiesBeforeSave"></param>
        /// <returns></returns>
        private static int CallBeforeSave(this GlobalEnvironment globalEnvironment, ref List<BaseDbObject> processedEntitiesBeforeSave)
        {
            List<BaseDbObject> objectsToProcess = new List<BaseDbObject>();
            foreach (var entry in
                globalEnvironment.DbContext.ChangeTracker.Entries() //#A
                    .Where(e =>
                       (e.State == EntityState.Added) ||   //#B
                       (e.State == EntityState.Modified))) //#B
            {
                var entity = entry.Entity;
                if (entity is BaseDbObject)
                {
                    var BaseDbObject = (BaseDbObject)entity;
                    if (!processedEntitiesBeforeSave.Contains(BaseDbObject))
                    {
                        objectsToProcess.Add(BaseDbObject);
                    }
                }
            }
            foreach (var entity in objectsToProcess)
            {
                entity.BeforeSave(globalEnvironment);
                processedEntitiesBeforeSave.Add(entity);
            }
            return objectsToProcess.Count();
        }

        /// <summary>
        /// Vrací počet objektů, které byly zpracovány
        /// </summary>
        /// <param name="globalEnvironment"></param>
        /// <param name="processedEntitiesBeforeDelete"></param>
        /// <returns></returns>
        private static int CallBeforeDelete(this GlobalEnvironment globalEnvironment, ref List<BaseDbObject> processedEntitiesBeforeDelete)
        {
            List<BaseDbObject> objectsToProcess = new List<BaseDbObject>();
            var deletedEntries = globalEnvironment.DbContext.ChangeTracker.Entries() //#A
            .Where(e =>
                       (e.State == EntityState.Deleted)).ToList();
            foreach (var entry in deletedEntries)
            {
                var entity = entry.Entity;
                if (entity is BaseDbObject)
                {
                    var BaseDbObject = (BaseDbObject)entity;
                    if (!processedEntitiesBeforeDelete.Contains(BaseDbObject))
                    {
                        objectsToProcess.Add(BaseDbObject);
                    }
                }
            }

            List<BaseDbObject> objectsToProcessAdd = new List<BaseDbObject>();
            foreach (var entity in objectsToProcess)
            {
                objectsToProcessAdd.AddRange(AddObjectsDeleteCascade(entity, ref processedEntitiesBeforeDelete));

            }
            objectsToProcess.AddRange(objectsToProcessAdd);

            foreach (var entity in objectsToProcess)
            {
                entity.BeforeDelete(globalEnvironment);
                processedEntitiesBeforeDelete.Add(entity);
            }
            return objectsToProcess.Count();
        }

        private static List<BaseDbObject> AddObjectsDeleteCascade(BaseDbObject entity, ref List<BaseDbObject> processedEntitiesBeforeDelete)
        {
            List<BaseDbObject> result = new List<BaseDbObject>();
            if (entity.ObjectsDeleteBecauseCascade() != null)
            {
                foreach (var obj in entity.ObjectsDeleteBecauseCascade())
                {
                    if (!processedEntitiesBeforeDelete.Contains(obj))
                    {
                        result.Add(obj);
                        result.AddRange(AddObjectsDeleteCascade(obj, ref processedEntitiesBeforeDelete));
                    }
                }
            }
            return result;
        }

        private static List<ValidationMessage> ExecuteValidation(this GlobalEnvironment globalEnvironment)
        {
            var result = new List<ValidationMessage>();
            foreach (var entry in
                globalEnvironment.DbContext.ChangeTracker.Entries() //#A
                    .Where(e =>
                       (e.State == EntityState.Added) ||   //#B
                       (e.State == EntityState.Modified))) //#B
            {
                var entity = entry.Entity;
                if (entity is BaseDbObject)
                {
                    result.AddRange(((BaseDbObject)entity).Validate(globalEnvironment));
                }
            }
            return result; //#F
        }


        private static List<ValidationMessage> ValidateDelete(this GlobalEnvironment globalEnvironment)
        {
            var result = new List<ValidationMessage>();
            foreach (var entry in globalEnvironment.DbContext.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Deleted))
            {
                var entity = entry.Entity;
                if (entity is BaseDbObject)
                {
                    result.AddRange(((BaseDbObject)entity).CanDelete(globalEnvironment));
                }
            }
            return result;
        }

        private static string[] NotLogNames = { "ConcurrencyCheck" };

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="globalEnvironment"></param>
        ///// <returns></returns>
        ///// <exception cref="Exception"></exception>
        //private static Dictionary<EventLog, EntityEntry> LogChangesToEventLog(this GlobalEnvironment globalEnvironment)
        //{
        //    var result = new Dictionary<EventLog, EntityEntry>();
        //    foreach (var entity in globalEnvironment.DbContext.ChangeTracker.Entries()
        //        .Where(e => e.State == EntityState.Added))
        //    {
        //        var bo = entity.Entity as BaseDbObject;
        //        if (bo == null)
        //        {
        //            // Vazebni objekty nejsou BaseDbObject
        //        }
        //        else
        //        {
        //            EventLog log = bo.GetNewEventLog(EventLog.enmType.Insert);

        //            foreach (var property in entity.Metadata.GetProperties())
        //            {
        //                if (!NotLogNames.Contains(property.Name) && property.Name != "Id")
        //                {
        //                    MemberInfo? prop = entity.Entity.GetType().GetProperty(property.Name);
        //                    if (prop == null) { throw new Exception("Why?"); }
        //                    var atr = prop.GetCustomAttributes(typeof(DisplayAttribute), true).Cast<DisplayAttribute>().SingleOrDefault();
        //                    string displayName;
        //                    if (atr != null && atr.Name != null)
        //                    {
        //                        displayName = atr.Name;// globalEnvironment.Localizer[atr.Name];
        //                    }
        //                    else { displayName = property.Name; }
        //                    log.AddPropertyToData(property.Name, displayName, "", entity.Property(property.Name).CurrentValue);
        //                }
        //            }
        //            result.Add(log, entity);
        //        }
        //    }

        //    foreach (var entity in globalEnvironment.DbContext.ChangeTracker.Entries()
        //        .Where(e => e.State == EntityState.Modified))
        //    {
        //        var T = entity.Entity.GetType();
        //        var bo = entity.Entity as BaseDbObject;
        //        if (bo == null)
        //        {
        //            // Vazebni objekty nejsou BaseDbObject
        //        }
        //        else
        //        {
        //            EventLog log = bo.GetNewEventLog(EventLog.enmType.Update);

        //            foreach (var property in entity.Metadata.GetProperties())
        //            {
        //                if (entity.Property(property.Name).IsModified)
        //                {
        //                    if (!NotLogNames.Contains(property.Name))
        //                    {
        //                        MemberInfo? prop = entity.Entity.GetType().GetProperty(property.Name);
        //                        if (prop == null) { throw new Exception("Why?"); }
        //                        var atr = prop.GetCustomAttributes(typeof(DisplayAttribute), true).Cast<DisplayAttribute>().SingleOrDefault();
        //                        string displayName;
        //                        if (atr != null && atr.Name != null)
        //                        {
        //                            displayName = atr.Name; // globalEnvironment.Localizer[atr.Name];
        //                        }
        //                        else { displayName = property.Name; }
        //                        log.AddPropertyToData(property.Name, displayName, entity.Property(property.Name).OriginalValue, entity.Property(property.Name).CurrentValue);
        //                    }
        //                }
        //            }
        //            result.Add(log, entity);
        //        }
        //    }

        //    foreach (var entity in globalEnvironment.DbContext.ChangeTracker.Entries()
        //        .Where(e => e.State == EntityState.Deleted))
        //    {
        //        var T = entity.Entity.GetType();
        //        var bo = entity.Entity as BaseDbObject;
        //        if (bo == null)
        //        {
        //            // Vazebni objekty nejsou BaseDbObject
        //        }
        //        else
        //        {
        //            EventLog log = bo.GetNewEventLog(EventLog.enmType.Delete);
        //            result.Add(log, entity);
        //        }
        //    }
        //    return result;
        //}

        ///// <summary>
        ///// Uloží do databáze všechny EventLog. Předtím do logů objektů, které se přidávali do databáze, doplní jejich Id
        ///// </summary>
        ///// <param name="eventLogObjects"></param>
        ///// <param name="globalEnvironment"></param>
        //private static void SaveInEventLogs(this GlobalEnvironment globalEnvironment, Dictionary<EventLog, EntityEntry> eventLogObjects)
        //{
        //    //Do logů, kde probíhal insert, doplním Id
        //    foreach (var log in eventLogObjects.Keys.Where(e => e.Type == EventLog.enmType.Insert))
        //    {
        //        var entity = eventLogObjects[log];
        //        var prop = entity.Property("Id");
        //        if (prop.CurrentValue != null) { log.ObjectId = int.Parse(prop.CurrentValue.ToString()); }
        //        log.AddPropertyToData("Id", "Id", "", prop.CurrentValue);
        //    }

        //    foreach (var log in eventLogObjects.Keys)
        //    {
        //        globalEnvironment.DbContext.Add(log);
        //    }
        //    globalEnvironment.DbContext.SaveChanges();
        //}
    }
}
