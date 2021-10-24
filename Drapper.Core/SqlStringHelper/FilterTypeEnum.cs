using System;
using System.Collections.Generic;
using System.Text;

namespace Drapper.Core.SqlStringHelper
{
    internal enum FilterTypeEnum
    {
        Equal,
        Unequal,
        Like,
        In,
        NotIn,
        BetweenAnd,
        Less,
        LessEqual,
        Greater,
        GreaterEqual
    }
}
