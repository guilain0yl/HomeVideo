using System;
using System.Text;
using System.Linq;
using System.Collections.Concurrent;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Emit;

namespace Drapper.Core.SqlStringHelper
{
    public class SqlStringEngine
    {
        #region GenerateSQL

        internal static string GenerateInsertSql<TIn>(TIn @in, string tableName, out bool autoInc)
        {
            var type = typeof(TIn);
            string key = $"GenerateInsertSql.{type.FullName}";
            if (!SqlCache.TryGetValue(key, out var sql))
            {
                var classInfo = GetClassInfo(type);
                autoInc = classInfo.PrimaryKeyAutoInc;
                tableName = string.IsNullOrEmpty(tableName)
                    ? classInfo.ClassName
                    : tableName;
                string columnsString = string.Join(',', classInfo.PropertyNames);
                string variableString = string.Join(',', classInfo.PropertyNames.Select(x => $"@{x}"));
                sql = $"insert into {tableName} ({columnsString}) values ({variableString})";
                if (autoInc)
                {
                    sql = $"{sql};select @@IDENTITY";
                }

                SqlCache.TryAdd(key, sql);
            }

            autoInc = sql.EndsWith("select @@IDENTITY");
            Debug.WriteLine($"GenerateInsertSql:{sql}");

            return sql;
        }

        internal static string GenerateBatchInsertSql<TIn>(IEnumerable<TIn> @in, string tableName)
        {
            var type = typeof(TIn);
            string key = $"GenerateBatchInsertSql.{type.FullName}";
            if (!SqlCache.TryGetValue(key, out var sql))
            {
                var classInfo = GetClassInfo(type);
                tableName = string.IsNullOrEmpty(tableName)
                    ? classInfo.ClassName
                    : tableName;

                string columnsString = string.Join(',', classInfo.PropertyNames);
                string variableString = string.Join(',', classInfo.PropertyNames.Select(x => $"@{x}"));
                sql = $"insert into {tableName} ({columnsString}) values ({variableString})";

                SqlCache.TryAdd(key, sql);
            }

            Debug.WriteLine($"GenerateInsertSql:{sql}");

            return sql;
        }

        internal static string GenerateInsertIfNotExistSql<TIn>(TIn @in, out bool autoInc, Condition condition)
        {
            var type = typeof(TIn);
            string key = condition.GetKey<TIn>(OperationEnum.INSERTIFNOTEXIST);
            if (!SqlCache.TryGetValue(key, out var sql))
            {
                var classInfo = GetClassInfo(type);
                autoInc = classInfo.PrimaryKeyAutoInc;

                string columnsString = string.Join(',', classInfo.PropertyNames);
                string variableString = string.Join(',', classInfo.PropertyNames.Select(x => $"@{x}"));
                string whereString = condition.GetWhereString();
                if (!string.IsNullOrEmpty(whereString))
                    whereString = $"where {whereString}";

                sql = $"insert into {condition.TableName} ({columnsString}) select {variableString} where not exists (select * from {condition.TableName} {whereString})";

                if (autoInc)
                    sql = $"{sql};select @@IDENTITY";

                SqlCache.TryAdd(key, sql);
            }

            resetLikeStr(@in, condition.LikeFilter);

            autoInc = sql.EndsWith("select @@IDENTITY");
            Debug.WriteLine($"GenerateInsertIfNotExistSql:{sql}");

            return sql;
        }

        internal static string GenerateDeleteSql<TIn>(TIn @in, Condition condition)
        {
            string key = condition.GetKey<TIn>(OperationEnum.DELETE);
            if (!SqlCache.TryGetValue(key, out var sql))
            {
                string whereString = condition.GetWhereString();

                if (!string.IsNullOrEmpty(whereString))
                    whereString = $"where {whereString}";
                sql = $"delete from {condition.TableName} {whereString}";

                SqlCache.TryAdd(key, sql);
            }

            resetLikeStr(@in, condition.LikeFilter);

            Debug.WriteLine($"GenerateDeleteSql:{sql}");

            return sql;
        }

        internal static string GenerateUpdateSql<TIn>(TIn @in, Condition condition)
        {
            string key = condition.GetKey<TIn>(OperationEnum.UPDATE);
            if (!SqlCache.TryGetValue(key, out var sql))
            {
                var classInfo = GetClassInfo(condition.TableType);

                string whereString = condition.GetWhereString();
                if (!string.IsNullOrEmpty(whereString))
                    whereString = $"where {whereString}";

                sql = $"update {condition.TableName} set {condition.GetUpdateString(classInfo.PropertyNames)} {whereString}";

                SqlCache.TryAdd(key, sql);
            }

            resetLikeStr(@in, condition.LikeFilter);

            Debug.WriteLine($"GenerateUpdateSql:{sql}");

            return sql;
        }

        internal static string GenerateQuerySql<TIn>(TIn @in, Condition condition)
        {
            string key = condition.GetKey<TIn>(OperationEnum.QUERY);
            if (!SqlCache.TryGetValue(key, out var sql))
            {
                ClassInfo classInfo = GetClassInfo(condition.TableType);

                string selectString = condition.GetSelectString(classInfo.AllPropertyNames);
                string whereString = condition.GetWhereString();
                if (!string.IsNullOrEmpty(whereString))
                    whereString = $"where {whereString}";
                string leftJoinString = condition.GetLeftJoinString();
                string orderString = condition.GetOrderString(classInfo.PrimaryKeyProperty);

                sql = $"select {selectString} from {condition.TableName} {leftJoinString} {whereString} {orderString}";

                SqlCache.TryAdd(key, sql);
            }

            resetLikeStr(@in, condition.LikeFilter);

            Debug.WriteLine($"GenerateQuerySql:{sql}");

            return sql;
        }

        internal static string GenerateExistSql<TIn>(TIn @in, Condition condition)
        {
            string key = condition.GetKey<TIn>(OperationEnum.EXIST);
            if (!SqlCache.TryGetValue(key, out var sql))
            {
                string whereString = condition.GetWhereString();
                if (!string.IsNullOrEmpty(whereString))
                    whereString = $"where {whereString}";
                string leftJoinString = condition.GetLeftJoinString();

                sql = $"select count(1) from {condition.TableName} {leftJoinString} {whereString}";

                SqlCache.TryAdd(key, sql);
            }

            resetLikeStr(@in, condition.LikeFilter);

            Debug.WriteLine($"GenerateExistSql:{sql}");

            return sql;
        }

        internal static string GeneratePageSql<TIn>(TIn @in, PageCondition condition)
        {
            string key = condition.GetKey<TIn>(OperationEnum.PAGE);
            if (!SqlCache.TryGetValue(key, out var sql))
            {
                var classInfo = GetClassInfo(condition.TableType);

                string selectString = condition.GetSelectString(classInfo.AllPropertyNames);
                string whereString = condition.GetWhereString();
                if (!string.IsNullOrEmpty(whereString))
                    whereString = $"where {whereString}";
                string leftJoinString = condition.GetLeftJoinString();
                if (!string.IsNullOrEmpty(leftJoinString))
                    leftJoinString = $"t1 {leftJoinString}";
                string orderString = condition.GetOrderString(classInfo.PrimaryKeyProperty);

                string innerSql = $"select {selectString},ROW_NUMBER() over({orderString}) as row_num from {condition.TableName} {leftJoinString} {whereString}";

                string sqlTmp = $"select * from ({innerSql}) tmp where row_num>{(condition.PageIndex - 1) * condition.PageSize} and row_num<{condition.PageIndex * condition.PageSize + 1};";
                string sqlCount = $"select count(1) from {condition.TableName} {leftJoinString} {whereString}";

                sql = sqlTmp + sqlCount;

                SqlCache.TryAdd(key, sql);
            }

            resetLikeStr(@in, condition.LikeFilter);

            Debug.WriteLine($"GeneratePageSql:{sql}");

            return sql;
        }

        #endregion

        private static ClassInfo GetClassInfo(Type type)
        {
            if (!ClassCache.TryGetValue(type, out var classInfo))
            {
                var properties = type.GetProperties().ToArray();
                if (properties.Count() < 1)
                {
                    throw new ArgumentNullException($"The '{type.Name}' class has no property.");
                }

                classInfo = new ClassInfo();
                classInfo.ClassName = type.Name.Trim();

                bool func(PropertyInfo propertyInfo)
                {
                    var extraKey = propertyInfo.GetCustomAttribute(typeof(ExtraAttribute));
                    if (extraKey != null) return false;

                    var primaryKey = propertyInfo.GetCustomAttribute(typeof(PrimaryKeyAttribute));
                    if (primaryKey != null)
                    {
                        if (!string.IsNullOrEmpty(classInfo.PrimaryKeyProperty))
                        {
                            throw new Exception($"The '{type.Name}' class can only be one PrimaryKeyAttribute at most,but now,there are '{classInfo.PrimaryKeyProperty}' and '{propertyInfo.Name}'.");
                        }

                        classInfo.PrimaryKeyProperty = propertyInfo.Name;
                        classInfo.PrimaryKeyAutoInc = (primaryKey as PrimaryKeyAttribute).AutoIncreament;
                        return false;
                    }

                    return true;
                }

                classInfo.PropertyNames = properties.Where(x => func(x)).Select(x => x.Name)?.ToArray();

                ClassCache.TryAdd(type, classInfo);
            }

            return classInfo;
        }

        private static string GetClassName<T>(T @in)
            => (typeof(T).GetProperties().Count() < 1 ? @in.GetType().Name : typeof(T).Name).Trim();

        private static void resetLikeStr<T>(T @in, IEnumerable<FilterInfo> LikeFilter)
        {
            if (@in == null || LikeFilter == null || LikeFilter.Count() < 1) return;

            Type type = @in.GetType();
            IList<PropertyInfo> properties = null;

            foreach (var filter in LikeFilter)
            {
                var key = $"{type.FullName}.{filter.VariableName}";
                if (emitCache.TryGetValue(key, out var func))
                {
                    ((Action<T, FilterInfo>)func)(@in, filter);
                }
                else
                {
                    properties ??= type.GetProperties().ToList();
                    var prop = properties.Where(x => x.Name == filter.VariableName).FirstOrDefault();
                    if (prop == null)
                        throw new ArgumentOutOfRangeException($"Check like condition. Class ${type.Name} can not find prop ${filter.VariableName}.");
                    if (prop.PropertyType != typeof(string))
                        throw new ArgumentOutOfRangeException($"Check like condition. Class ${type.Name}.${filter.VariableName}'s Type is noe String.");

                    var func1 = EmitSetter<T>(key, filter.VariableName);
                    func1(@in, filter);
                    emitCache.TryAdd(key, func1);
                }
            }
        }

        private static Action<T, FilterInfo> EmitSetter<T>(string key, string propertyName)
        {
            var type = typeof(T);
            var dynamicMethod = new DynamicMethod(key, null, new[] { type, typeof(FilterInfo) }, typeof(FilterInfo).Module);
            var iLGenerator = dynamicMethod.GetILGenerator();
            if (iLGenerator != null)
            {
                var getCallMethod = type.GetMethod("get_" + propertyName, BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Public);
                var setCallMethod = type.GetMethod("set_" + propertyName, BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Public);

                if (getCallMethod != null && setCallMethod != null)
                {
                    iLGenerator.Emit(OpCodes.Ldarg_0);
                    iLGenerator.Emit(OpCodes.Ldarga, 1);
                    iLGenerator.Emit(OpCodes.Ldarg_0);
                    iLGenerator.EmitCall(OpCodes.Callvirt, getCallMethod, null);
                    iLGenerator.EmitCall(OpCodes.Call, typeof(FilterInfo).GetMethod(nameof(FilterInfo.GetLikeString), BindingFlags.NonPublic | BindingFlags.Instance), null);
                    iLGenerator.EmitCall(OpCodes.Callvirt, setCallMethod, null);
                    iLGenerator.Emit(OpCodes.Ret);
                }
            }

            return dynamicMethod.CreateDelegate(typeof(Action<T, FilterInfo>)) as Action<T, FilterInfo>;
        }

        private static readonly ConcurrentDictionary<Type, ClassInfo> ClassCache = new ConcurrentDictionary<Type, ClassInfo>();

        private static readonly ConcurrentDictionary<string, string> SqlCache = new ConcurrentDictionary<string, string>();

        private static readonly ConcurrentDictionary<string, object> emitCache = new ConcurrentDictionary<string, object>();
    }
}
