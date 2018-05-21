using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using MvcJqDataTables.Enums;
using MvcJqDataTables.Utility;

namespace MvcJqDataTables
{
    public class DataTableModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var values = bindingContext.ValueProvider;

            var draw = Convert.ToInt32(values.GetValue("draw").AttemptedValue);
            var start = Convert.ToInt32(values.GetValue("start").AttemptedValue);
            var length = Convert.ToInt32(values.GetValue("length").AttemptedValue);
            var search = new Search
            {
                value = Convert.ToString(values.GetValue("search[value]").AttemptedValue),
                regex = Convert.ToBoolean(values.GetValue("search[regex]").AttemptedValue)
            };
            var columns = BindColumns(values).ToList();
            var order = BindOrders(values).ToList();

            var model = new DataTableSettings { draw = draw, start = start, length = length, search = search, columns = columns, order = order };

            return model;
        }

        private static IEnumerable<Column> BindColumns(IValueProvider values)
        {
            var columns = new List<Column>();

            var index = 0;

            while (true)
            {
                #region Name, Data, Orderable, Searchable

                var _columnData = values.GetValue(String.Format(NameConvention.ColumnData, index));
                if (!TryParse(_columnData, out string columnData)) break;

                var _columnName = values.GetValue(String.Format(NameConvention.ColumnName, index));
                if (!TryParse(_columnName, out string columnName)) break;

                var _columnOrderable = values.GetValue(String.Format(NameConvention.ColumnOrderable, index));
                if (!TryParse(_columnOrderable, out bool columnOrderable)) break;

                var _columnSearchable = values.GetValue(String.Format(NameConvention.ColumnSearchable, index));
                if (!TryParse(_columnSearchable, out bool columnSearchable)) break;
                #endregion

                #region Search

                var _columnSearchValue = values.GetValue(String.Format(NameConvention.ColumnSearchValue, index));
                TryParse(_columnSearchValue, out string columnSearchValue);

                var _columnSearchRegex = values.GetValue(String.Format(NameConvention.ColumnSearchRegex, index));
                TryParse(_columnSearchRegex, out bool columnSearchRegex);

                var search = new Search { value = columnSearchValue, regex = columnSearchRegex };

                #endregion

                var column = new Column(columnName, columnData)
                    .SetOrderable(columnOrderable)
                    .SetSearchable(columnSearchable)
                    .SetSearch(search);

                columns.Add(column);

                index++;
            }

            return columns;
        }

        private static IEnumerable<Order> BindOrders(IValueProvider values)
        {
            var orders = new List<Order>();

            var index = 0;

            while (true)
            {
                var _orderColumn = values.GetValue(string.Format(NameConvention.OrderColumn, index));
                var orderColumn = 0;
                if (!TryParse(_orderColumn, out orderColumn)) break;

                var _orderDirection = values.GetValue(string.Format(NameConvention.OrderDirection, index));
                TryParse(_orderDirection, out string stringOrderDirection);

                var orderDirection = stringOrderDirection.Equals(OrderDirection.Asc.ToString().ToLower()) ? OrderDirection.Asc : OrderDirection.Desc;

                var order = new Order(orderColumn, orderDirection);

                orders.Add(order);

                index++;
            }

            return orders;
        }

        private static bool TryParse<T>(ValueProviderResult value, out T result)
        {
            result = default(T);
            if (value?.RawValue == null) return false;
            try
            {
                result = (T)Convert.ChangeType(value.AttemptedValue, typeof(T));
                return true;
            }
            catch { return false; }
        }
    }
}
