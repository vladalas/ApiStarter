using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseStarter.Filters
{
    /// <summary>
    /// Ancester of filters of base objects
    /// </summary>
    public abstract class BaseFilter
    {
        public List<int>? Id { get; set; }
    }
}
