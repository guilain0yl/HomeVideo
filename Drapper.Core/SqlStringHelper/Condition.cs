using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Data;
using System.Security.Cryptography;
using System.Collections.Concurrent;

namespace Drapper.Core.SqlStringHelper
{
    public class Condition
    {
        #region Tran

        public Condition SetTransaction(IDbTransaction dbTransaction)
        {
            transaction = dbTransaction;
            return this;
        }

        #endregion

        #region MainTableName

        public Condition WithTable<TClass>(string tableName = "")
        {
            TableType = typeof(TClass);
            TableName = (string.IsNullOrEmpty(tableName)
                ? TableType.Name
                : tableName).Trim();
            return this;
        }

        /// <summary>
        /// 主表名
        /// </summary>
        internal string TableName { get; set; }

        internal Type TableType { get; set; }

        #endregion

        #region Select

        public Condition Select(string column, string aliasName = "", string tableName = "")
        {
            SelectColumns.Add((tableName, column, aliasName));
            return this;
        }

        public Condition Select(IEnumerable<string> columns, string tableName = "")
        {
            if (columns?.Count() > 0)
            {
                var tmp = columns.Select(x => (tableName, x, string.Empty));
                SelectColumns.AddRange(tmp);
            }

            return this;
        }

        /// <summary>
        /// 筛选字段
        /// </summary>
        /// <param name="columns">(columnName,aliasName)</param>
        /// <param name="tableName">字段所属表名</param>
        /// <returns></returns>
        public Condition Select(IEnumerable<ValueTuple<string, string>> columns, string tableName = "")
        {
            if (columns?.Count() > 0)
            {
                var tmp = columns.Select(x => (tableName, x.Item1, x.Item2));
                SelectColumns.AddRange(tmp);
            }

            return this;
        }

        /// <summary>
        /// 连表时 注入当前DTO的他表字段（通常使用Extra标注）
        /// </summary>
        /// <param name="column">字段名称</param>
        /// <param name="aliasName">别名</param>
        /// <param name="tableName">字段所属表名</param>
        /// <returns></returns>
        public Condition SelectExtra(string column, string aliasName, string tableName)
        {
            SelectExtraColumns.Add((tableName, column, aliasName));
            return this;
        }

        /// <summary>
        /// 连表时 注入当前DTO的他表字段（通常使用Extra标注）
        /// </summary>
        /// <param name="columns">(columnName,aliasName)</param>
        /// <param name="tableName">字段所属表名</param>
        /// <returns></returns>
        public Condition SelectExtra(IEnumerable<ValueTuple<string, string>> columns, string tableName)
        {
            if (columns?.Count() > 0)
            {
                var tmp = columns.Select(x => (tableName, x.Item1, x.Item2));
                SelectExtraColumns.AddRange(tmp);
            }

            return this;
        }

        internal string GetSelectString(IEnumerable<string> defaultOrderColumn)
        {
            var extra = SelectExtraColumns.Select(x => MutiTable
              ? $"{GetAliasTableNameByTableName(x.Item1)}.{x.Item2} {x.Item3}"
              : $"{x.Item2} {x.Item3}");

            List<string> result = MutiTable ?
                defaultOrderColumn.Select(x => $"{AliasTableName}.{x}").ToList()
                :
                defaultOrderColumn.ToList();

            if (SelectColumns.Count > 0)
            {
                result = SelectColumns.Select(x => MutiTable
                ? $"{GetAliasTableNameByTableName(x.Item1)}.{x.Item2} {x.Item3}"
                : $"{x.Item2} {x.Item3}").ToList();
            }

            result.AddRange(extra);

            return string.Join(',', result);
        }

        internal int SelectColumnsCount => SelectColumns.Count;

        /// <summary>
        /// 选择字段 Item1 是表明 item2是字段名 item3是别名
        /// </summary>
        private List<ValueTuple<string, string, string>> SelectColumns { get; set; }

        /// <summary>
        /// 选择字段 Item1 是表明 item2是字段名 item3是别名
        /// </summary>
        private List<ValueTuple<string, string, string>> SelectExtraColumns { get; set; }

        #endregion

        #region Update

        /// <summary>
        /// 更新字段
        /// </summary>
        /// <param name="column">更新字段</param>
        /// <param name="variable">变量名</param>
        /// <returns></returns>
        public Condition Update(string column, string variable = "")
        {
            UpdateColumns.Add((column, variable));
            return this;
        }

        /// <summary>
        /// 更新字段和变量名一致
        /// </summary>
        public Condition Update(IEnumerable<string> columns)
        {
            if (columns?.Count() > 0)
            {
                var tmp = columns.Select(x => (x, string.Empty));
                UpdateColumns.AddRange(tmp);
            }

            return this;
        }

        /// <summary>
        /// 更新字段
        /// </summary>
        /// <param name="columns"><column,variable></param>
        /// <returns></returns>
        public Condition Update(IEnumerable<ValueTuple<string, string>> columns)
        {
            if (columns?.Count() > 0)
            {
                var tmp = columns.Select(x => (x.Item1, x.Item2));
                UpdateColumns.AddRange(tmp);
            }

            return this;
        }

        internal string GetUpdateString(IEnumerable<string> defaultColumns)
        {
            if (UpdateColumns.Count < 1)
                return string.Join(',', defaultColumns.Select(x => $"{x}=@{x}"));

            var tmp = UpdateColumns.Select(x =>
            string.IsNullOrEmpty(x.Item2)
                ? $"{x.Item1}=@{x.Item1}"
                : $"{x.Item1}=@{x.Item2}"
                );

            return string.Join(',', tmp);
        }

        /// <summary>
        /// 更新字段 Item1是字段名 item2是变量名
        /// </summary>
        private List<ValueTuple<string, string>> UpdateColumns { get; set; }

        #endregion

        #region Order

        public Condition SetOrder(OrderEnum order = OrderEnum.desc)
        {
            Order = order;
            return this;
        }

        public Condition OrderBy(string tableName, string column)
        {
            OrderColumns.Add((tableName, column));
            return this;
        }

        public Condition OrderByNone()
        {
            OrderNone = true;
            return this;
        }

        public Condition OrderBy(string column)
        {
            OrderColumns.Add((string.Empty, column));
            return this;
        }

        public Condition OrderBy(IEnumerable<string> columns, string tableName = "")
        {
            if (columns?.Count() > 0)
            {
                var tmp = columns.Select(x => (tableName, x));
                OrderColumns.AddRange(tmp);
            }

            return this;
        }

        internal string GetOrderString(string defaultOrderColumnl)
        {
            if (OrderNone)
                return string.Empty;

            if (OrderColumns.Count < 1)
            {
                return $"order by {(MutiTable ? $"{AliasTableName}." : string.Empty)}{defaultOrderColumnl} {Order}";
            }

            var tmp = OrderColumns.Select(x =>
            MutiTable
            ? $"{GetAliasTableNameByTableName(x.Item1)}.{x.Item2}"
            : x.Item2);

            return $"order by {string.Join(",", tmp)} {Order}";
        }

        /// <summary>
        /// 排序字段
        /// </summary>
        /// <param name="columns">(table,name)</param>
        /// <returns></returns>
        public Condition OrderBy(IEnumerable<ValueTuple<string, string>> columns)
        {
            if (columns?.Count() > 0)
            {
                var tmp = columns.Select(x => (x.Item1, x.Item2));
                OrderColumns.AddRange(tmp);
            }

            return this;
        }

        /// <summary>
        /// 排序字段
        /// </summary>
        private OrderEnum Order { get; set; } = OrderEnum.desc;

        /// <summary>
        /// 排序字段 item1 是表名 item2是字段名
        /// </summary>
        private List<ValueTuple<string, string>> OrderColumns { get; set; }

        private bool OrderNone { get; set; } = false;

        #endregion

        #region LeftJoin

        public Condition LeftJoin(IEnumerable<JoinTable> joinTables)
        {
            if (joinTables == null)
                return this;

            foreach (var item in joinTables)
            {
                LeftJoin(item);
            }
            return this;
        }

        public Condition LeftJoin(JoinTable joinTable)
        {
            joinTable.SetAliasName($"t{JoinTables.Count + 2}");
            JoinTables.Add(joinTable);
            return this;
        }

        internal string GetLeftJoinString()
        {
            if (JoinTables.Count < 1)
                return string.Empty;

            var tmp = JoinTables.Select(x => x.GetLeftJoinString(GetAliasTableNameByTableName));

            return string.Join(" ", tmp);
        }

        /// <summary>
        /// 连表信息
        /// </summary>
        private List<JoinTable> JoinTables { get; set; }

        #endregion

        #region Where

        public Condition Where(FilterInfo filterInfo)
        {
            FilterInfos.Add(filterInfo);
            return this;
        }

        public Condition Where(IEnumerable<FilterInfo> filterInfos)
        {
            if (filterInfos?.Count() > 0)
            {
                FilterInfos.AddRange(filterInfos);
            }

            return this;
        }

        public Condition WhereEqual(string column, string tableName = "", string variableName = "")
        {
            FilterInfos.Add(FilterInfo.Equal(column, tableName, variableName));
            return this;
        }

        /// <summary>
        /// 例：Table1.Id=@Id
        /// </summary>
        public Condition WhereEqual(IEnumerable<string> columns, string tableName = "")
        {
            if (columns?.Count() > 0)
            {
                var tmp = columns.Select(x => FilterInfo.Equal(x, tableName, string.Empty));
                FilterInfos.AddRange(tmp);
            }

            return this;
        }

        /// <summary>
        /// 例：Table1.Id=@Id
        /// </summary>
        /// <param name="columns">(column,variable)</param>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public Condition WhereEqual(IEnumerable<ValueTuple<string, string>> columns, string tableName = "")
        {
            if (columns?.Count() > 0)
            {
                var tmp = columns.Select(x => FilterInfo.Equal(x.Item1, tableName, x.Item2));
                FilterInfos.AddRange(tmp);
            }

            return this;
        }

        public Condition WhereUnequal(string column, string tableName = "", string variableName = "")
        {
            FilterInfos.Add(FilterInfo.Unequal(column, tableName, variableName));
            return this;
        }

        /// <summary>
        /// 例：Table1.Id!=@Id
        /// </summary>
        public Condition WhereUnequal(IEnumerable<string> columns, string tableName = "")
        {
            if (columns?.Count() > 0)
            {
                var tmp = columns.Select(x => FilterInfo.Unequal(x, tableName, string.Empty));
                FilterInfos.AddRange(tmp);
            }

            return this;
        }

        /// <summary>
        /// 例：Table1.Id!=@Id
        /// </summary>
        /// <param name="columns">(column,variable)</param>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public Condition WhereUnequal(IEnumerable<ValueTuple<string, string>> columns, string tableName = "")
        {
            if (columns?.Count() > 0)
            {
                var tmp = columns.Select(x => FilterInfo.Unequal(x.Item1, tableName, x.Item2));
                FilterInfos.AddRange(tmp);
            }

            return this;
        }

        public Condition WhereIn(string column, string tableName = "", string variableName = "")
        {
            FilterInfos.Add(FilterInfo.In(column, tableName, variableName));
            return this;
        }

        /// <summary>
        /// 例：Table1.Id in @Ids
        /// </summary>
        public Condition WhereIn(IEnumerable<string> columns, string tableName = "")
        {
            if (columns?.Count() > 0)
            {
                var tmp = columns.Select(x => FilterInfo.In(x, tableName, string.Empty));
                FilterInfos.AddRange(tmp);
            }

            return this;
        }

        /// <summary>
        /// 例：Table1.Id in @Ids
        /// </summary>
        /// <param name="columns">(column,variable)</param>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public Condition WhereIn(IEnumerable<ValueTuple<string, string>> columns, string tableName = "")
        {
            if (columns?.Count() > 0)
            {
                var tmp = columns.Select(x => FilterInfo.In(x.Item1, tableName, x.Item2));
                FilterInfos.AddRange(tmp);
            }

            return this;
        }

        public Condition WhereNotIn(string column, string tableName = "", string variableName = "")
        {
            FilterInfos.Add(FilterInfo.NotIn(column, tableName, variableName));
            return this;
        }

        /// <summary>
        /// 例：Table1.Id not in @Ids
        /// </summary>
        public Condition WhereNotIn(IEnumerable<string> columns, string tableName = "")
        {
            if (columns?.Count() > 0)
            {
                var tmp = columns.Select(x => FilterInfo.NotIn(x, tableName, string.Empty));
                FilterInfos.AddRange(tmp);
            }

            return this;
        }

        /// <summary>
        /// 例：Table1.Id not in @Ids
        /// </summary>
        /// <param name="columns">(column,variable)</param>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public Condition WhereNotIn(IEnumerable<ValueTuple<string, string>> columns, string tableName = "")
        {
            if (columns?.Count() > 0)
            {
                var tmp = columns.Select(x => FilterInfo.NotIn(x.Item1, tableName, x.Item2));
                FilterInfos.AddRange(tmp);
            }

            return this;
        }

        public Condition WhereLike(string column, string tableName = "", string variableName = "", FuzzyPatternEnum fuzzyPattern = FuzzyPatternEnum.Backward)
        {
            FilterInfos.Add(FilterInfo.Like(column, tableName, variableName, fuzzyPattern));
            return this;
        }

        public Condition WhereBetween(string column, string variableName1, string variableName2, string tableName = "")
        {
            FilterInfos.Add(FilterInfo.BetweenAnd(column, variableName1, variableName2, tableName));
            return this;
        }

        internal string GetWhereString()
        {
            var conditions = FilterInfos.Select(x =>
            {
                string aliasName = MutiTable
                ? GetAliasTableNameByTableName(x.TableName)
                : string.Empty;

                return x.GetConditionString(aliasName);
            });

            return string.Join(" and ", conditions);
        }

        /// <summary>
        /// 过滤条件
        /// </summary>
        private List<FilterInfo> FilterInfos { get; set; }

        #endregion

        public Condition()
        {
            OrderColumns = new List<ValueTuple<string, string>>();
            SelectColumns = new List<ValueTuple<string, string, string>>();
            SelectExtraColumns = new List<(string, string, string)>();
            JoinTables = new List<JoinTable>();
            UpdateColumns = new List<ValueTuple<string, string>>();
            FilterInfos = new List<FilterInfo>();
        }

        internal virtual string GetKey<TIn>(OperationEnum operation, IEnumerable<string> insertColumns = null)
        {
            return operation switch
            {
                OperationEnum.INSERT => $"{operation}.{TableName}.{string.Join('.', insertColumns.Select(x => x))}",
                OperationEnum.DELETE => $"{operation}.{TableName}.{string.Join('.', FilterInfos.Select(x => $"{x.ColumnName} {x.FilterType} {x.VariableName} {x.FuzzyPattern}"))}",
                OperationEnum.UPDATE => $"{operation}.{string.Join('.', UpdateColumns.Select(x => $"{x.Item1}={x.Item2}"))}.{TableName}.{string.Join('.', FilterInfos.Select(x => $"{x.ColumnName} {x.FilterType} {x.VariableName} {x.FuzzyPattern}"))}",
                OperationEnum.EXIST => $"{operation}.*.{TableName}.{string.Join('.', FilterInfos.Select(x => $"{x.ColumnName} {x.FilterType} {x.VariableName} {x.FuzzyPattern}"))}",
                OperationEnum.QUERY => $"{operation}.{string.Join('.', SelectColumns.Select(x => $"{x.Item1}.{x.Item2} {x.Item3}"))}.{TableName}.{string.Join('.', JoinTables.Select(x => x.TableName))}.{string.Join('.', FilterInfos.Select(x => $"{x.ColumnName} {x.FilterType} {x.TableName}{x.VariableName} {x.FuzzyPattern}"))} {string.Join(" ", OrderColumns.Select(x => $"{x.Item1}.{x.Item2} {Order}"))}",
                OperationEnum.PAGE => $"{operation}.{string.Join('.', SelectColumns.Select(x => $"{x.Item1}.{x.Item2} {x.Item3}"))}.{TableName}.{string.Join('.', JoinTables.Select(x => x.TableName))}.{string.Join('.', FilterInfos.Select(x => $"{x.ColumnName} {x.FilterType} {x.TableName}{x.VariableName} {x.FuzzyPattern}"))} {string.Join(" ", OrderColumns.Select(x => $"{x.Item1}.{x.Item2}"))} {Order}",
                _ => null,
            };
        }

        private string GetAliasTableNameByTableName(string tableName)
        {
            if (string.IsNullOrEmpty(tableName) || tableName == TableName)
                return AliasTableName;

            var tmp = JoinTables.Where(x => x.TableName == tableName);
            if (tmp == null || tmp.Count() < 1)
                throw new ArgumentNullException($"Can't not find {tableName} from Join Tables.");

            return tmp.First().AliasTableName;
        }

        internal IEnumerable<FilterInfo> LikeFilter => FilterInfos.Where(x => x.FilterType == FilterTypeEnum.Like);

        bool MutiTable => JoinTables.Count > 0;

        const string AliasTableName = "t1";

        /// <summary>
        /// 事务句柄
        /// </summary>
        internal IDbTransaction transaction { get; private set; }

        private readonly static ConcurrentDictionary<string, string> _Cache = new ConcurrentDictionary<string, string>();
    }
}
