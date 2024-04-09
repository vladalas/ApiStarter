using BaseStarter.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseStarter.DataViews
{
    /// <summary>
    /// DataViewRequest which contains Filter for filtering DataView
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataViewRequestFilter<T>: DataViewRequest where T : BaseFilter 
    {
        public required T Filter { get; set; }
    }
}
