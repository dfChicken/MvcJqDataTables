using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcJqDataTables.Attribute
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumStringAttribute : System.Attribute
    {
        private readonly string _value;
        public EnumStringAttribute(string value)
        {
            _value = value;
        }
        public string Value
        {
            get { return _value; }
        }
    }
}
