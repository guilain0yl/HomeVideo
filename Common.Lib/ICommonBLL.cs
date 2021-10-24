using System.Collections.Generic;

namespace Common.Lib
{
    public interface ICommonBLL<TEntity, TToken>
    {
        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="data">数据实体</param>
        /// <param name="msg">额外信息</param>
        /// <param name="token">权限令牌</param>
        /// <returns></returns>
        bool AddInfo(TEntity data, out string msg, TToken token);

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <param name="data">数据实体</param>
        /// <param name="msg">额外信息</param>
        /// <param name="token">权限令牌</param>
        /// <returns></returns>
        bool UpdateInfo(TEntity data, out string msg, TToken token);

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="data">数据实体</param>
        /// <param name="msg">额外信息</param>
        /// <param name="token">权限令牌</param>
        bool DeleteInfo(TEntity data, out string msg, TToken token);

        /// <summary>
        /// 分页信息
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="total">总数据</param>
        /// <param name="data">筛选实体</param>
        /// <param name="token">权限令牌</param>
        /// <returns></returns>
        IEnumerable<TEntity> Page(int pageIndex, int pageSize, out long total, TEntity data, TToken token);
    }
}
