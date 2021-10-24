using Drapper.Core;
using Drapper.Core.DBOperation;
using Drapper.Core.SqlStringHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Util;

namespace Common.Lib.DataHelper
{
    public abstract class SqlOperationAdapter<TClass>
        : SqlOperation<TClass>,
        IBasicDataOperation<TClass>,
        ICountOperation<TClass>,
        IQueryOperation<TClass>,
        ISingleColumnsOperation<TClass>
        where TClass : class
    {
        #region BasicDataOperation
        int IBasicDataOperation<TClass>.InsertData(TClass data, IDbTransaction transaction)
        {
            if (data == null) throw new ArgumentNullException($"When Insert Data,the data instacne is null!");

            try
            {
                return Insert(data, transaction);
            }
            catch (Exception ex)
            {
                LoggerHelper.GlobalLogger.Error($"插入数据失败，类名：{typeof(TClass).Name}，数据：{data.ToJson()}", ex);
                return -1;
            }
        }

        bool IBasicDataOperation<TClass>.UpdateData(TClass data, IEnumerable<string> columns, IEnumerable<FilterInfo> filter, IDbTransaction transaction)
        {
            if (data == null) throw new ArgumentNullException($"When Update Data,the data instacne is null!");

            try
            {
                Condition condition = new Condition();
                condition.SetTransaction(transaction)
                    .Update(columns)
                    .Where(filter);

                return Update(data, condition) > 0;
            }
            catch (Exception ex)
            {
                LoggerHelper.GlobalLogger.Error($"更新数据失败，类名：{typeof(TClass).Name}，数据：{data.ToJson()}，筛选条件字段：{filter.ToJson()}", ex);
                return false;
            }
        }

        bool IBasicDataOperation<TClass>.DeleteData(TClass data, IEnumerable<FilterInfo> filter, IDbTransaction transaction)
        {
            if (data == null) throw new ArgumentNullException($"When Delete Data,the data instacne is null!");

            try
            {
                Condition condition = new Condition();
                condition.SetTransaction(transaction)
                    .Where(filter);

                return Delete(data, condition);
            }
            catch (Exception ex)
            {
                LoggerHelper.GlobalLogger.Error($"删除数据失败，类名：{typeof(TClass).Name}，数据：{data.ToJson()}，筛选条件字段：{filter.ToJson()}", ex);
                return false;
            }
        }

        IEnumerable<TClass> IBasicDataOperation<TClass>.Pages(int pageIndex, int pageSize, out long total, IEnumerable<FilterInfo> filter, TClass filterInstance, IEnumerable<string> orderColumns, bool isDesc)
        {
            total = 0;

            try
            {
                PageCondition pageCondition = new PageCondition();
                pageCondition.SetPage(pageIndex, pageSize)
                    .OrderBy(orderColumns?.Count() > 0 ? orderColumns : null)
                    .Where(filter)
                    .SetOrder(isDesc ? OrderEnum.desc : OrderEnum.asc);

                return Page(filterInstance, pageCondition, out total);
            }
            catch (Exception ex)
            {
                LoggerHelper.GlobalLogger.Error($"删除数据失败，类名：{typeof(TClass).Name}，数据：{filterInstance.ToJson()}，筛选条件字段：{filter.ToJson()}，页码：{pageIndex}，页数：{pageSize}", ex);
                return null;
            }
        }

        #endregion

        int ICountOperation<TClass>.Count(IEnumerable<FilterInfo> filter, TClass filterInstance, IDbTransaction transaction)
        {
            try
            {
                Condition condition = new Condition();
                condition.SetTransaction(transaction)
                    .Select("count(1)")
                    .Where(filter);

                return Single<TClass, int>(filterInstance, condition);
            }
            catch (Exception ex)
            {
                LoggerHelper.GlobalLogger.Error($"计算数据数量失败，类名：{typeof(TClass).Name}，数据：{filterInstance.ToJson()}，筛选条件字段：{filter.ToJson()}", ex);
                return -1;
            }
        }

        IEnumerable<TClass> IQueryOperation<TClass>.Query<TIn>(IEnumerable<string> columns, IEnumerable<FilterInfo> filter, TIn filterInstance, IDbTransaction transaction)
        {
            try
            {
                if (filter?.Count() > 0 && filterInstance == null)
                {
                    throw new Exception("The filterInstance is null,but filter columns is not null!");
                }

                Condition condition = new Condition();
                condition.SetTransaction(transaction)
                    .Select(columns)
                    .Where(filter);

                return Query(filterInstance, condition);
            }
            catch (Exception ex)
            {
                LoggerHelper.GlobalLogger.Error($"查询数据失败，类名：{typeof(TIn).Name}，数据：{filterInstance.ToJson()}，筛选条件字段：{filter.ToJson()}", ex);
                return null;
            }
        }

        TClass ISingleColumnsOperation<TClass>.SingleColumns<TIn>(IEnumerable<string> columns, IEnumerable<FilterInfo> filter, TIn filterInstance, IDbTransaction transaction)
        {
            try
            {
                if (filter?.Count() > 0 && filterInstance == null)
                {
                    throw new Exception("The filterInstance is null,but filter columns is not null!");
                }

                Condition condition = new Condition();
                condition.SetTransaction(transaction)
                    .Select(columns)
                    .Where(filter);

                return Single(filterInstance, condition);
            }
            catch (Exception ex)
            {
                LoggerHelper.GlobalLogger.Error($"查询单条数据失败，类名：{typeof(TIn).Name}，数据：{filterInstance.ToJson()}，筛选条件字段：{filter.ToJson()}", ex);
                return null;
            }
        }
    }
}
