using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MvcJqDataTables
{
    public static class DataTableHelper
    {
        public static DataTable DataTable(this HtmlHelper helper, string id)
        {
            return new DataTable(id);
        }
    }
}
