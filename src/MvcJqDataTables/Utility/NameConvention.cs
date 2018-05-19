using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcJqDataTables.Utility
{
    public static class NameConvention
    {
        #region Order
        public static string OrderColumn { get { return "order[{0}][column]"; } }
        public static string OrderDirection { get { return "order[{0}][dir]"; } }
        #endregion

        #region Column     
        public static string ColumnName { get { return "columns[{0}][name]"; } }
        public static string ColumnData { get { return "columns[{0}][data]"; } }
        public static string ColumnSearchable { get { return "columns[{0}][searchable]"; } }
        public static string ColumnOrderable { get { return "columns[{0}][orderable]"; } }
        public static string ColumnSearchValue { get { return "columns[{0}][search][value]"; } }
        public static string ColumnSearchRegex { get { return "columns[{0}][search][regex]"; } }
        #endregion
    }
}
