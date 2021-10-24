using Drapper.Core.SqlStringHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Util;

namespace Common.Lib.DataHelper
{
    public abstract class SqlOperationWithLogicDeleteAdapter<TClass>
        : SqlOperationAdapter<TClass>,
        ILogicDeleteDataOperation<TClass>
        where TClass : class, ILogicDeleteInfo
    {
        public bool DeleteDataLogic(TClass data, IEnumerable<FilterInfo> filter, IDbTransaction transaction = null)
        {
            if (data == null) throw new ArgumentNullException($"When Delete Logical Data,the data instacne is null!");

            try
            {
                data.IsDeleted = true;
                Condition condition = new Condition();
                condition.SetTransaction(transaction)
                    .Update(new string[] {
                        nameof(ILogicDeleteInfo.IsDeleted)
                    })
                    .Where(filter);


                return Update(data, condition) > 0;
            }
            catch (Exception ex)
            {
                LoggerHelper.GlobalLogger.Error($"逻辑删除数据失败，类名：{typeof(TClass).Name}，数据：{data.ToJson()}，筛选条件字段：{filter.ToJson()}", ex);
                return false;
            }
        }
    }
}
