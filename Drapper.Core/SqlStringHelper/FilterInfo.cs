using System;
using System.Collections.Generic;
using System.Text;

namespace Drapper.Core.SqlStringHelper
{
    public struct FilterInfo
    {
        public static FilterInfo Like(string column, string tableName = "", string variableName = "", FuzzyPatternEnum fuzzyPattern = FuzzyPatternEnum.Backward)
        {
            if (string.IsNullOrEmpty(variableName))
                variableName = column;

            return new FilterInfo()
            {
                FilterType = FilterTypeEnum.Like,
                FuzzyPattern = fuzzyPattern,
                Value = (tableName, column, variableName)
            };
        }

        public static FilterInfo Equal(string column, string tableName = "", string variableName = "")
        {
            if (string.IsNullOrEmpty(variableName))
                variableName = column;

            return new FilterInfo()
            {
                FilterType = FilterTypeEnum.Equal,
                Value = (tableName, column, variableName)
            };
        }

        public static FilterInfo Unequal(string column, string tableName = "", string variableName = "")
        {
            if (string.IsNullOrEmpty(variableName))
                variableName = column;

            return new FilterInfo()
            {
                FilterType = FilterTypeEnum.Unequal,
                Value = (tableName, column, variableName)
            };
        }

        public static FilterInfo BetweenAnd(string column, string variableName1, string variableName2, string tableName = "")
            => new FilterInfo()
            {
                FilterType = FilterTypeEnum.BetweenAnd,
                Value = (tableName, column, string.Empty),
                BetweenAndVar = (variableName1, variableName2)
            };

        public static FilterInfo Greater(string column, string tableName = "", string variableName = "")
        {
            if (string.IsNullOrEmpty(variableName))
                variableName = column;

            return new FilterInfo()
            {
                FilterType = FilterTypeEnum.Greater,
                Value = (tableName, column, variableName)
            };
        }

        public static FilterInfo GreaterEqual(string column, string tableName = "", string variableName = "")
        {
            if (string.IsNullOrEmpty(variableName))
                variableName = column;

            return new FilterInfo()
            {
                FilterType = FilterTypeEnum.GreaterEqual,
                Value = (tableName, column, variableName)
            };
        }

        public static FilterInfo Less(string column, string tableName = "", string variableName = "")
        {
            if (string.IsNullOrEmpty(variableName))
                variableName = column;

            return new FilterInfo()
            {
                FilterType = FilterTypeEnum.Less,
                Value = (tableName, column, variableName)
            };
        }

        public static FilterInfo LessEqual(string column, string tableName = "", string variableName = "")
        {
            if (string.IsNullOrEmpty(variableName))
                variableName = column;

            return new FilterInfo()
            {
                FilterType = FilterTypeEnum.LessEqual,
                Value = (tableName, column, variableName)
            };
        }

        public static FilterInfo NotIn(string column, string tableName = "", string variableName = "")
        {
            if (string.IsNullOrEmpty(variableName))
                variableName = column;

            return new FilterInfo()
            {
                FilterType = FilterTypeEnum.NotIn,
                Value = (tableName, column, variableName)
            };
        }

        public static FilterInfo In(string column, string tableName = "", string variableName = "")
        {
            if (string.IsNullOrEmpty(variableName))
                variableName = column;

            return new FilterInfo()
            {
                FilterType = FilterTypeEnum.In,
                Value = (tableName, column, variableName)
            };
        }

        internal string GetConditionString(string aliasTableName)
        {
            if (!string.IsNullOrEmpty(aliasTableName))
                aliasTableName += ".";

            return FilterType switch
            {
                FilterTypeEnum.BetweenAnd => $"{aliasTableName}{Value.Item2} between @{BetweenAndVar.Item1} and @{BetweenAndVar.Item2}",
                FilterTypeEnum.Equal => $"{aliasTableName}{Value.Item2}=@{Value.Item3}",
                FilterTypeEnum.Unequal => $"{aliasTableName}{Value.Item2}!=@{Value.Item3}",
                FilterTypeEnum.In => $"{aliasTableName}{Value.Item2} in @{Value.Item3}",
                FilterTypeEnum.NotIn => $"{aliasTableName}{Value.Item2} not in @{Value.Item3}",
                FilterTypeEnum.Like => $"{aliasTableName}{Value.Item2} like @{Value.Item3}",
                FilterTypeEnum.Greater => $"{aliasTableName}{Value.Item2} > @{Value.Item3}",
                FilterTypeEnum.GreaterEqual => $"{aliasTableName}{Value.Item2} >= @{Value.Item3}",
                FilterTypeEnum.Less => $"{aliasTableName}{Value.Item2} < @{Value.Item3}",
                FilterTypeEnum.LessEqual => $"{aliasTableName}{Value.Item2} <= @{Value.Item3}",
                _ => string.Empty
            };
        }

        internal string GetLikeString(string value)
        {
            return FuzzyPattern switch
            {
                FuzzyPatternEnum.ALL => $"%{value}%",
                FuzzyPatternEnum.Backward => $"{value}%",
                FuzzyPatternEnum.Forward => $"%{value}",
                _ => value,
            };
        }

        internal string TableName => Value.Item1;

        /// <summary>
        /// item1为表名 item2为字段名 item3为变量名（为空时和字段名一致）
        /// </summary>
        private ValueTuple<string, string, string> Value { get; set; }

        internal string VariableName => Value.Item3;

        internal string ColumnName => Value.Item2;

        internal FilterTypeEnum FilterType { get; set; }

        internal FuzzyPatternEnum FuzzyPattern { get; set; }

        private ValueTuple<string, string> BetweenAndVar { get; set; }
    }
}
