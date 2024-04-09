using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseStarter.DataViews
{
    /// <summary>
    /// the result of a paged list of objects with the total number of objects and the total number of pages
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataViewResult<T>
    {
        /// <summary>
        /// List of Objects for required page
        /// </summary>
        public IEnumerable<T>? Data { get; set; }
        
        /// <summary>
        /// Total number of pages
        /// </summary>
        public int TotalPages { get; set; }
        
        /// <summary>
        /// Total number of objects
        /// </summary>
        public int TotalCount { get; set; }
    }
}
