using System;
using System.Text.RegularExpressions;

namespace ArgCheck.Core
{
    /// <summary>
    /// 测试条件
    /// </summary>
    public struct TestCondition
    {
        public static readonly TestCondition NotNull =
            new TestCondition
            {
                TestConditon = TestConditonEnum.NotNull
            };

        /// <summary>
        /// small <= value <=big
        /// </summary>
        public static TestCondition Range(decimal small, decimal big)
        {
            if (small > big)
                throw new ArgumentException("The range condition's arguments error!");

            return new TestCondition
            {
                range = (small, big),
                TestConditon = TestConditonEnum.Range
            };
        }

        /// <summary>
        /// value > other
        /// </summary>
        public static TestCondition Bigger(decimal other)
            => new TestCondition
            {
                OtherDec = other,
                TestConditon = TestConditonEnum.Bigger
            };

        /// <summary>
        /// value < other
        /// </summary>
        public static TestCondition Smaller(decimal other)
            => new TestCondition()
            {
                OtherDec = other,
                TestConditon = TestConditonEnum.Smaller
            };

        public static TestCondition Regex(string regexStr)
            => new TestCondition()
            {
                RegexString = regexStr?.Trim(),
                TestConditon = TestConditonEnum.Regex
            };

        public static TestCondition Regex(RegexStrEnum regexStr)
            => new TestCondition()
            {
                RegexString = RegexStringHelper.GetRegexString(regexStr),
                TestConditon = TestConditonEnum.Regex
            };

        public static TestCondition Equal(object obj)
            => new TestCondition()
            {
                OtherObj = obj,
                TestConditon = TestConditonEnum.Equal
            };

        public static TestCondition Unequal(object obj)
            => new TestCondition()
            {
                OtherObj = obj,
                TestConditon = TestConditonEnum.Unequl
            };

        internal bool Verification(object value, Type type, out string msg)
        {
            msg = string.Empty;

            switch (TestConditon)
            {
                case TestConditonEnum.Range:
                case TestConditonEnum.Bigger:
                case TestConditonEnum.Smaller:
                    {
                        if (!type.IsValueType)
                        {
                            msg = $"{TestConditon} just support some type can be converted to decimal.";
                            return false;
                        }

                        var v = Convert.ToDecimal(value);

                        if (TestConditon == TestConditonEnum.Range)
                            return (v >= range.Item1 && v <= range.Item2);

                        return (TestConditon == TestConditonEnum.Smaller
                            ? v < OtherDec
                            : v > OtherDec);
                    }
                case TestConditonEnum.NotNull:
                    {
                        if (!type.IsClass)
                        {
                            msg = $"{TestConditon} just support reference type.";
                            return false;
                        }

                        return value != null;
                    }
                case TestConditonEnum.Regex:
                    {
                        if (type != typeof(string))
                        {
                            msg = $"{TestConditon} just support some type can be converted to string.";
                            return false;
                        }

                        return (new Regex(RegexString)).IsMatch(value.ToString());
                    }
                case TestConditonEnum.Equal:
                    return value.Equals(OtherObj);
                case TestConditonEnum.Unequl:
                    return !value.Equals(OtherObj);
                default:
                    return false;
            }
        }

        private string RegexString { get; set; }

        private ValueTuple<decimal, decimal> range { get; set; }

        private decimal OtherDec { get; set; }

        private object OtherObj { get; set; }

        private TestConditonEnum TestConditon { get; set; }
    }
}
