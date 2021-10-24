using System;

namespace ArgCheck.Core
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class Condition : Attribute
    {
        public Condition(string tips)
        {
            Tips = tips;
        }

        internal TestCondition TestCondition { get; set; }

        internal string Tips { get; set; }
    }
}
