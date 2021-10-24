using System;
using System.Collections.Generic;
using System.Text;

namespace BasicController
{
    public sealed class PageArgs<T>
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public T Data { get; set; }
    }
}
