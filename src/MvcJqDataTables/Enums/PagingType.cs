using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcJqDataTables.Attribute;

namespace MvcJqDataTables.Enums
{
    public enum PagingType
    {
        [EnumString("numbers")]
        Numbers,
        [EnumString("simple")]
        Simple,
        [EnumString("simple_numbers")]
        SimpleNumbers,
        [EnumString("full")]
        Full,
        [EnumString("full_numbers")]
        FullNumbers,
        [EnumString("first_last_numbers")]
        FirstLastNumber
    }
}
