using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseStarter.DataViews
{
    
    /// <summary>
    /// Row in list for object Client
    /// </summary>
    public class ClientDVRow : BaseDVRow
    {

        /// <summary>
        /// First Name
        /// </summary>
        public required string FirstName { get; set; }

        /// <summary>
        /// Last Name
        /// </summary>
        public required string LastName { get; set; }

    }
}
