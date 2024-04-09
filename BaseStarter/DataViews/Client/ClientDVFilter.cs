using BaseStarter.Filters;
using BaseStarter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseStarter.DataViews
{
    
    /// <summary>
    /// Implements filtering for List of Client
    /// </summary>
    public static class ClientDVFilter
    {
        public static IQueryable<BaseStarter.Models.Client> FilterClients(this IQueryable<BaseStarter.Models.Client> list, ClientFilter filter)
        {
            if (filter != null)
            {
                if (filter.Id != null)
                {
                    list = list.Where(f => filter.Id.Contains(f.Id));
                }
                if (!string.IsNullOrWhiteSpace(filter.Fulltext))
                {
                    string[] patterns = filter.Fulltext.ToUpper().Split(" ");
                    foreach (string pattern in patterns)
                    {
                        if (!string.IsNullOrWhiteSpace(pattern))
                        {
                            list = list.Where(
                                f => (!string.IsNullOrEmpty(f.FirstName) && f.FirstName.Contains(pattern))
                                || (!string.IsNullOrEmpty(f.LastName) && f.LastName.Contains(pattern))
                                );
                        }
                    }
                }

            }
            return list;
        }
    }
}
