using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MvcJqDataTables.Attribute;

namespace MvcJqDataTables.Extensions
{
    public static class EnumExtensions
    {
        public static string GetStringValue(this Enum value)
        {
            string output = "";
            var type = value.GetType();
            var fi = type.GetField(value.ToString());

            if (fi.GetCustomAttributes(typeof(EnumStringAttribute), false) is EnumStringAttribute[] attrs && attrs.Length > 0)
            {
                output = attrs[0].Value;
            }

            return output;
        }
    }
}
