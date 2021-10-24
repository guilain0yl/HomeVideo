using Drapper.Core;
using Drapper.Core.SqlStringHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Common.Lib.DataHelper
{
    public interface IBasicDataOperation<T>
        where T : class
    {
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="transaction">事务实例</param>
        /// <returns></returns>
        int InsertData(T data, IDbTransaction transaction = null);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="data">更新数据</param>
        /// <param name="columns">更新字段</param>
        /// <param name="filter">筛选字段</param>
        /// <param name="transaction">事务实例</param>
        /// <returns></returns>
        bool UpdateData(T data, IEnumerable<string> columns, IEnumerable<FilterInfo> filter, IDbTransaction transaction = null);

        /// <summary>
        /// 物理数据删除
        /// </summary>
        /// <param name="data">数据实例</param>
        /// <param name="filter">筛选字段</param>
        /// <param name="transaction">事务实例</param>
        /// <returns></returns>
        bool DeleteData(T data, IEnumerable<FilterInfo> filter, IDbTransaction transaction = null);

        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="total">总数据量</param>
        /// <param name="orderColumns">排序字段，默认使用逐渐排序</param>
        /// <param name="isDesc">是否倒叙</param>
        /// <param name="filter">筛选字段</param>
        /// <param name="filterInstance">筛选字段实例</param>
        /// <returns></returns>
        IEnumerable<T> Pages(int pageIndex, int pageSize, out long total, IEnumerable<FilterInfo> filter = null, T filterInstance = null, IEnumerable<string> orderColumns = null, bool isDesc = true);
    }
}
