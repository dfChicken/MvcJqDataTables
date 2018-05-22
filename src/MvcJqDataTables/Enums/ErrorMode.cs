using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcJqDataTables.Attribute;

namespace MvcJqDataTables.Enums
{
    public enum ErrorMode
    {
        [EnumString("alert")]
        Alert,
        [EnumString("throw")]
        Throw,
        [EnumString("none")]
        None
    }
}
