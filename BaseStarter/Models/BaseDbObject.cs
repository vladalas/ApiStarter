using BaseStarter.DAL;
using BaseStarter.Environment;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BaseStarter.Models
{
    
    /// <summary>
    /// Object stored in database
    /// </summary>
    public abstract class BaseDbObject
    {
        public const int MaxLenghtDefaultString = 256;
        public const int MaxLenghtUrlString = 2000;

        public BaseDbObject()
        {

        }

        public int Id { get; set; }

        [Display(Name = "Datum of creation")]
        public DateTime DateOfCreation { get; private set; }

        [Display(Name = "Datum of last update")]
        public DateTime DateOfUpdate { get; private set; }

        [Timestamp]
        public byte[]? ConcurrencyCheck { get; set; }


        /// <summary>
        /// Validation before object Save
        /// </summary>
        /// <param name="globalEnvironment"></param>
        /// <returns></returns>
        public virtual IEnumerable<ValidationMessage> Validate(GlobalEnvironment globalEnvironment)
        {
            IEnumerable<ValidationMessage> result = new List<ValidationMessage>();
            return result;
        }

        /// <summary>
        /// Validation before Delete
        /// </summary>
        /// <param name="globalEnvironment"></param>
        /// <returns></returns>
        public virtual IEnumerable<ValidationMessage> CanDelete(GlobalEnvironment globalEnvironment)
        {
            IEnumerable<ValidationMessage> result = new List<ValidationMessage>();
            return result;
        }

        /// <summary>
        /// Method called before Save
        /// </summary>
        /// <param name="globalEnvironment"></param>
        public virtual void BeforeSave(GlobalEnvironment globalEnvironment)
        {
            DateOfUpdate = DateTime.Now;
        }

        /// <summary>
        /// Method called before Delete
        /// </summary>
        /// <param name="globalEnvironment"></param>
        public virtual void BeforeDelete(GlobalEnvironment globalEnvironment)
        {
        }

        /// <summary>
        /// List of objects that are deleted because of cascade
        /// </summary>
        /// <returns></returns>
        public virtual IList<BaseDbObject>? ObjectsDeleteBecauseCascade()
        {
            return null;
        }


        /// <summary>
        /// Checks if the new ConcurrencyCheck value matches the object. If not, it throws an exception.
        /// </summary>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public void CheckConcurrencyCheckWithValue(byte[] newValue)
        {
            if (ConcurrencyCheck != null)
            {
                if (!ConcurrencyCheck.SequenceEqual(newValue))
                {
                    throw new DbUpdateConcurrencyException("Object was edited by someone else.");
                }
            }
        }
    }
}
