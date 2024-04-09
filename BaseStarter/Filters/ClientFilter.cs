using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseStarter.Filters
{
    
    /// <summary>
    /// Object for filtering Client
    /// </summary>
    public class ClientFilter : BaseFilter
    {
        public string? Fulltext { get; set; }
    }
}
