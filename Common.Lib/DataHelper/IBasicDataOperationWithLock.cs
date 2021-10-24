using Drapper.Core.SqlStringHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Common.Lib.DataHelper
{
    public interface IBasicDataOperationWithLock<T>
        : IBasicDataOperation<T>,
        ILogicDeleteDataOperation<T>
        where T : CommonInfo
    {
        /// <summary>
        /// 冻结数据
        /// </summary>
        /// <param name="data">数据实例</param>
        /// <param name="filter">筛选字段</param>
        /// <param name="transaction">事务实例</param>
        /// <returns></returns>
        bool LockData(T data, IEnumerable<FilterInfo> filter, IDbTransaction transaction = null);

        /// <summary>
        /// 解冻数据
        /// </summary>
        /// <param name="data">数据实例</param>
        /// <param name="filter">筛选字段</param>
        /// <param name="transaction">事务实例</param>
        /// <returns></returns>
        bool UnlockData(T data, IEnumerable<FilterInfo> filter, IDbTransaction transaction = null);
    }
}
