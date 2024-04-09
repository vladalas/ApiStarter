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
        public const int DefaultPageSize = 20;

        public DataViewRequest()
        {
            PageSize = DefaultPageSize;
            PageIndex = 1;
            SortName = "id";
            SortAsc = true;
        }

        public string? SortName { get; set; }
        public bool SortAsc { get; set; }


        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
