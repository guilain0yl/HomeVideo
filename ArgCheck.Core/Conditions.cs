using System;
using System.Collections.Generic;
using System.Text;

namespace ArgCheck.Core
{
    public class RangeCondition : Condition
    {
        public RangeCondition(int small, int big, string tips = "") : base(tips)
        {
            TestCondition = TestCondition.Range(small, big);
        }
    }

    public class BiggerCondition : Condition
    {
        public BiggerCondition(int other, string tips = "") : base(tips)
        {
            TestCondition = TestCondition.Bigger(other);
        }
    }

    public class SmallerCondition : Condition
    {
        public SmallerCondition(int other, string tips = "") : base(tips)
        {
            TestCondition = TestCondition.Smaller(other);
        }
    }

    public class NotNullCondition : Condition
    {
        public NotNullCondition(string tips = "") : base(tips)
        {
            TestCondition = TestCondition.NotNull;
        }
    }

    public class RegexCondition : Condition
    {
        public RegexCondition(string regexStr, string tips = "") : base(tips)
        {
            TestCondition = TestCondition.Regex(regexStr);
        }

        public RegexCondition(RegexStrEnum regexStr, string tips = "") : base(tips)
        {
            TestCondition = TestCondition.Regex(regexStr);
        }
    }

    public class EqualCondition : Condition
    {
        public EqualCondition(object obj, string tips = "") : base(tips)
        {
            TestCondition = TestCondition.Equal(obj);
        }
    }

    public class Unequal : Condition
    {
        public Unequal(object obj, string tips = "") : base(tips)
        {
            TestCondition = TestCondition.Unequal(obj);
        }
    }
}
