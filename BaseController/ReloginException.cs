using System;
using System.Collections.Generic;
using System.Text;

namespace BasicController
{
    internal class ReloginException : Exception
    {
        public ReloginException(string message) : base(message) { }
    }
}
