using System;

namespace Drapper.Core
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ExtraAttribute : Attribute
    {
        public ExtraAttribute()
        { }

        public ExtraAttribute(string description)
        {
            _description = description;
        }

        private string _description = string.Empty;
    }
}
