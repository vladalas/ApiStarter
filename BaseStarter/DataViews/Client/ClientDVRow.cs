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

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

    }
}
