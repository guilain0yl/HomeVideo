using System;
using System.Collections.Generic;
using System.Text;

namespace Drapper.Core.SqlStringHelper
{
    internal enum OperationEnum
    {
        INSERT = 0,
        BACTHINSAERT = 1,
        INSERTIFNOTEXIST = 2,
        UPDATE = 3,
        QUERY = 4,
        EXIST = 5,
        DELETE = 6,
        PAGE = 7
    }
}
