using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseStarter.DataViews
{
    /// <summary>
    /// Request for call List of object (paging, sorting)
    /// </summary>
    public class DataViewRequest
    {
        /// <summary>
        /// Default number of items in one page
        /// </summary>
        private const int DefaultPageSize = 20;

        /// <summary>
        /// 
        /// </summary>
        public DataViewRequest()
        {
            PageSize = DefaultPageSize;
            PageIndex = 1;
            SortName = "id";
            SortAsc = true;
        }

        /// <summary>
        /// Sort name
        /// </summary>
        public string? SortName { get; set; }
        
        /// <summary>
        /// Sort ascendent (true), descendent (false)
        /// </summary>
        public bool SortAsc { get; set; }

        /// <summary>
        /// Page number (first page = 1)
        /// </summary>
        public int PageIndex { get; set; }
        
        /// <summary>
        /// Number of items in one page
        /// </summary>
        public int PageSize { get; set; }
    }
}
