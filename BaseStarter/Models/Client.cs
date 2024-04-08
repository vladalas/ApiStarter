using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BaseStarter.Environment;

namespace BaseStarter.Models
{
    
    /// <summary>
    /// Model of object Client
    /// </summary>
    public class Client : BaseDbObject
    {
        #region "Constructor"

        public Client()
        {
           
        }

        #endregion

        #region "Properties"

       
        [MaxLength(MaxLenghtDefaultString)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [MaxLength(MaxLenghtDefaultString)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        

        #endregion

        #region "Methods"
        /// <summary>
        /// Validation before object Save
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<ValidationResult> Validate(GlobalEnvironment globalEnvironment)
        {
            List<ValidationResult> result = new List<ValidationResult>();

            //Example condition
            if (FirstName.Length < 2)
            {
                result.Add(new ValidationResult("Minimal length of last name is 2 character."));
            }

            return result;
        }

        /// <summary>
        /// Validation before Delete
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<ValidationResult> CanDelete(GlobalEnvironment globalEnvironment)
        {
            List<ValidationResult> result = new List<ValidationResult>();



            return result;
        }

        /// <summary>
        /// Method called before Delete
        /// </summary>
        /// <param name="globalEnvironment"></param>
        public override void BeforeDelete(GlobalEnvironment globalEnvironment)
        {
            base.BeforeDelete(globalEnvironment);

           
        }



        /// <summary>
        /// Method called before Save
        /// </summary>
        /// <param name="globalEnvironment"></param>
        public override void BeforeSave(GlobalEnvironment globalEnvironment)
        {
            base.BeforeSave(globalEnvironment);


        }

       
       

        #endregion
    }
}
