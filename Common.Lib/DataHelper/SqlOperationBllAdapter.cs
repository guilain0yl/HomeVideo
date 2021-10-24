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
    public abstract class SqlOperationBllAdapter<TClass>
        : SqlOperationAdapter<TClass>,
        IBasicDataOperationWithLock<TClass>,
        ICountOperation<TClass>
        where TClass : CommonInfo, new()
    {
        #region BasicDataOperation

        bool IBasicDataOperation<TClass>.UpdateData(TClass data, IEnumerable<string> columns, IEnumerable<FilterInfo> filter, IDbTransaction transaction)
        {
            if (data == null) throw new ArgumentNullException($"When Update Data,the data instacne is null!");

            try
            {
                columns = CheckColumns(columns);

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

        bool IBasicDataOperationWithLock<TClass>.LockData(TClass data, IEnumerable<FilterInfo> filter, IDbTransaction transaction)
        {
            data.State = LockStateEnum.Locked;

            return LockOrUnlockData(data, filter, transaction);
        }

        private bool LockOrUnlockData(TClass data, IEnumerable<FilterInfo> filter, IDbTransaction transaction)
        {
            if (data == null) throw new ArgumentNullException($"When {(data.State == LockStateEnum.Locked ? "Lock" : "Unlock")} Data,the data instacne is null!");

            try
            {
                Condition condition = new Condition();
                condition.SetTransaction(transaction)
                    .Update(new string[] {
                        nameof(CommonInfo.State),
                        nameof(CommonInfo.ModifierId),
                        nameof(CommonInfo.ModifierName),
                        nameof(CommonInfo.ModifiyTime)})
                    .Where(filter);

                return Update(data, condition) > 0;
            }
            catch (Exception ex)
            {
                LoggerHelper.GlobalLogger.Error($"{(data.State == LockStateEnum.Locked ? "冻结" : "解冻")}数据失败，类名：{typeof(TClass).Name}，数据：{data.ToJson()}，筛选条件字段：{filter.ToJson()}", ex);
                return false;
            }
        }

        bool IBasicDataOperationWithLock<TClass>.UnlockData(TClass data, IEnumerable<FilterInfo> filter, IDbTransaction transaction)
        {
            data.State = LockStateEnum.Unlocked;

            return LockOrUnlockData(data, filter, transaction);
        }

        bool ILogicDeleteDataOperation<TClass>.DeleteDataLogic(TClass data, IEnumerable<FilterInfo> filter, IDbTransaction transaction)
        {
            if (data == null) throw new ArgumentNullException($"When Delete Logical Data,the data instacne is null!");

            try
            {
                data.IsDeleted = true;
                Condition condition = new Condition();
                condition.SetTransaction(transaction)
                    .Update(new string[] {
                        nameof(ILogicDeleteInfo.IsDeleted),
                        nameof(CommonInfo.ModifierId),
                        nameof(CommonInfo.ModifierName),
                        nameof(CommonInfo.ModifiyTime)})
                    .Where(filter);

                return Update(data, condition) > 0;
            }
            catch (Exception ex)
            {
                LoggerHelper.GlobalLogger.Error($"逻辑删除数据失败，类名：{typeof(TClass).Name}，数据：{data.ToJson()}，筛选条件字段：{filter.ToJson()}", ex);
                return false;
            }
        }

        IEnumerable<TClass> IBasicDataOperation<TClass>.Pages(int pageIndex, int pageSize, out long total, IEnumerable<FilterInfo> filter, TClass filterInstance, IEnumerable<string> orderColumns, bool isDesc)
        {
            total = 0;

            try
            {
                filter = (filter ?? new List<FilterInfo>()).Append(FilterInfo.Equal(nameof(CommonInfo.IsDeleted)));
                filterInstance ??= new TClass();
                filterInstance.IsDeleted = false;

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
                filter = (filter ?? new List<FilterInfo>()).Append(FilterInfo.Equal(nameof(CommonInfo.IsDeleted)));
                filterInstance ??= new TClass();
                filterInstance.IsDeleted = false;

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

        private IEnumerable<string> CheckColumns(IEnumerable<string> columns)
        {
            if (columns?.Count() > 0)
            {
                var t = columns.Select(x => x.Trim()).ToList();

                if (!columns.Contains(nameof(CommonInfo.ModifierId)))
                    t.Add(nameof(CommonInfo.ModifierId));
                if (!columns.Contains(nameof(CommonInfo.ModifierName)))
                    t.Add(nameof(CommonInfo.ModifierName));
                if (!columns.Contains(nameof(CommonInfo.ModifiyTime)))
                    t.Add(nameof(CommonInfo.ModifiyTime));

                return t;
            }

            return null;
        }
    }
}
