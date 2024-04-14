using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseStarter.DTO
{
    /// <summary>
    /// Object of client for sending by API
    /// </summary>
    public class Client : BaseDtoObject
    {
        /// <summary>
        /// First Name
        /// </summary>
        public string FirstName { get; set; } = "";

        /// <summary>
        /// Last Name
        /// </summary>
        public string LastName { get; set; } = "";
    }
}
