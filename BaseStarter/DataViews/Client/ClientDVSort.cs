

namespace BaseStarter.DataViews
{
    public enum enmClientSortColumn
    {
        Id = 0,
        FirstName = 1,
        LastName = 2,   

    }

    /// <summary>
    /// Implements sorting for List of Client
    /// </summary>
    public static class CientDVSort
    {
        private static enmClientSortColumn SortColumnFromName(string? name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return enmClientSortColumn.Id;
            }
            else
            {
                switch (name.ToLower())
                {
                    case "firstname":
                        return enmClientSortColumn.FirstName;
                    case "lastname":
                        return enmClientSortColumn.LastName;
                    default:
                        return enmClientSortColumn.Id;
                }
            }
        }

        public static IQueryable<ClientDVRow> OrderClients(this IQueryable<ClientDVRow> list, string? sortName, bool sortAsc)
        {
            enmClientSortColumn _sortColumn = SortColumnFromName(sortName);

            if (sortAsc)
            {
                switch (_sortColumn)
                {
                    case enmClientSortColumn.FirstName:
                        return list.OrderBy(f => f.FirstName);
                    case enmClientSortColumn.LastName:
                        return list.OrderBy(f => f.LastName);
                    default:
                        return list.OrderBy(f => f.Id);
                }
            }
            else
            {
                switch (_sortColumn)
                {
                    case enmClientSortColumn.FirstName:
                        return list.OrderByDescending(f => f.FirstName);
                    case enmClientSortColumn.LastName:
                        return list.OrderByDescending(f => f.LastName);
                    default:
                        return list.OrderByDescending(f => f.Id);
                }
            }
        }
    }
}
