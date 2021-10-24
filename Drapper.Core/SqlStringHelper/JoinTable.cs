using System;
using System.Collections.Generic;
using System.Linq;

namespace Drapper.Core.SqlStringHelper
{
    public struct JoinTable
    {
        private JoinTable(string tableName)
        {
            TableName = tableName?.Trim();
            OnFilters = new List<ValueTuple<string, ValueTuple<string, string>>>();
            AliasTableName = string.Empty;
        }

        /// <summary>
        /// 左联表名
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public static JoinTable LeftJoin(string tableName)
            => new JoinTable(tableName);

        /// <summary>
        /// On选择条件
        /// </summary>
        /// <param name="column">本表字段名</param>
        /// <param name="targetColumn">他表字段名</param>
        /// <param name="targetTableName">他表名称</param>
        /// <returns></returns>
        public JoinTable On(string column, string targetColumn, string targetTableName)
        {
            OnFilters.Add((column?.Trim(),
                (targetTableName?.Trim(), targetColumn?.Trim())
                ));
            return this;
        }

        internal JoinTable SetAliasName(string aliasName)
        {
            AliasTableName = aliasName;
            return this;
        }

        internal string GetLeftJoinString(Func<string, string> func)
        {
            var alias = AliasTableName;

            var tmp = OnFilters.Select(x => $"{alias}.{x.Item1}={func(x.Item2.Item1)}.{x.Item2.Item2}");


            return $"left join {TableName} {AliasTableName} on {string.Join(" and ", tmp)}";
        }

        /// <summary>
        /// item1是本表字段名 item2.item1是他表表明 item2.item2他表字段名
        /// </summary>
        private List<ValueTuple<string, ValueTuple<string, string>>> OnFilters;

        internal string TableName { get; private set; }

        internal string AliasTableName { get; private set; }
    }
}
