using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using MvcJqDataTables.Enums;

namespace MvcJqDataTables
{
    [ModelBinder(typeof(DataTableModelBinder))]
    public class DataTableSettings
    {
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public List<Column> columns { get; set; }
        public Search search { get; set; }
        public List<Order> order { get; set; }
    }

    public class Order
    {
        private int column { get; set; }
        private OrderDirection dir { get; set; }

        public Order(int column, OrderDirection direction)
        {
            this.column = column;
            this.dir = direction;
        }

        public override string ToString()
        {
            return "[ " + column + ", " + "'" + dir.ToString().ToLower() + "' ]";
        }
    }
}
