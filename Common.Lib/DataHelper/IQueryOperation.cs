using Drapper.Core.SqlStringHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Common.Lib.DataHelper
{
    public interface IQueryOperation<T>
    {
        /// <summary>
        /// 查询多行数据
        /// </summary>
        /// <typeparam name="TIn">筛选实例类型</typeparam>
        /// <param name="columns">查询字段</param>
        /// <param name="filter">筛选字段</param>
        /// <param name="filterInstance">筛选字段实例</param>
        /// <returns></returns>
        IEnumerable<T> Query<TIn>(IEnumerable<string> columns = null, IEnumerable<FilterInfo> filter = null, TIn filterInstance = null, IDbTransaction transaction = null)
            where TIn : class;
    }
}
