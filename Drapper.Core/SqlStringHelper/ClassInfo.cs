using System.Collections.Generic;
using System.Linq;

namespace Drapper.Core.SqlStringHelper
{
    internal struct ClassInfo
    {
        /// <summary>
        /// 类名
        /// </summary>
        internal string ClassName { get; set; }

        /// <summary>
        /// 主键特性属性名称
        /// </summary>
        internal string PrimaryKeyProperty { get; set; }

        /// <summary>
        /// 主键特性是否自增
        /// </summary>
        internal bool PrimaryKeyAutoInc { get; set; }

        /// <summary>
        /// 主键以及Extra特性外的属性
        /// </summary>
        internal IEnumerable<string> PropertyNames { get; set; }

        internal IEnumerable<string> AllPropertyNames
        {
            get
            {
                if (string.IsNullOrEmpty(PrimaryKeyProperty))
                    return PropertyNames;
                return PropertyNames?.Append(PrimaryKeyProperty);
            }
        }
    }
}
