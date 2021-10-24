using System;
using System.Collections.Generic;
using System.Text;

namespace Drapper.Core
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class PrimaryKeyAttribute : Attribute
    {
        public PrimaryKeyAttribute(bool autoInc = true)
        {
            AutoIncreament = autoInc;
        }

        public bool AutoIncreament { get; set; }
    }
}
