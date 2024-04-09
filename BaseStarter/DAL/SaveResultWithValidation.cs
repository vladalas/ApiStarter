using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseStarter.DAL
{
    /// <summary>
    /// Result from Saving object
    /// </summary>
    public class SaveResultWithValidation
    {
        /// <summary>
        /// Save is successfull
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Id of saved object
        /// </summary>
        public int? Id { get; set; }
        
        /// <summary>
        /// List of validation problem
        /// </summary>
        public List<ValidationMessage>? validationResults { get; set; }
    }
}
