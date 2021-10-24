using Drapper.Core.SqlStringHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Common.Lib.DataHelper
{
    public interface ILogicDeleteDataOperation<T>
        where T : ILogicDeleteInfo
    {
        /// <summary>
        /// 物理删除数据
        /// </summary>
        /// <param name="data">数据实例</param>
        /// <param name="filter">筛选字段</param>
        /// <param name="transaction">事务实例</param>
        /// <returns></returns>
        bool DeleteDataLogic(T data, IEnumerable<FilterInfo> filter, IDbTransaction transaction = null);
    }
}
