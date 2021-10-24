using Drapper.Core.SqlStringHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Common.Lib.DataHelper
{
    public interface ICountOperation<T>
        where T : class
    {
        /// <summary>
        /// 统计数量
        /// </summary>
        /// <typeparam name="TIn">筛选实例类型</typeparam>
        /// <param name="filter">筛选字段</param>
        /// <param name="filterInstance">筛选字段实例</param>
        /// <returns></returns>
        int Count(IEnumerable<FilterInfo> filter = null, T filterInstance = null, IDbTransaction transaction = null);
    }
}
