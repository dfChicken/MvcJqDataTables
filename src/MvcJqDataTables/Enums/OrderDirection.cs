using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcJqDataTables.Attribute;

namespace MvcJqDataTables.Enums
{
    public enum OrderDirection
    {
        [EnumString("asc")]
        Asc,
        [EnumString("desc")]
        Desc
    }
}
