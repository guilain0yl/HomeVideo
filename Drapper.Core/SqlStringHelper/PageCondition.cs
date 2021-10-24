using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Drapper.Core.SqlStringHelper
{
    public class PageCondition : Condition
    {
        public PageCondition()
            : base()
        { }

        public PageCondition SetPage(int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            return this;
        }

        internal override string GetKey<TIn>(OperationEnum operation, IEnumerable<string> insertColumns = null)
        {
            var tmp = base.GetKey<TIn>(operation, null);
            return $"{tmp}_{PageIndex}_{PageSize}";
        }

        internal int PageIndex { get; set; }

        internal int PageSize { get; set; }
    }
}
