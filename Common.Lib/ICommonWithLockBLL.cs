using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Lib
{
    public interface ICommonWithLockBLL<TEntity, TToken>
        : ICommonBLL<TEntity, TToken>
    {
        /// <summary>
        /// 冻结信息
        /// </summary>
        /// <param name="data">数据实体</param>
        /// <param name="msg">额外信息</param>
        /// <param name="token">权限令牌</param>
        /// <returns></returns>
        bool LockInfo(TEntity data, out string msg, TToken token);

        /// <summary>
        /// 解冻信息
        /// </summary>
        /// <param name="data">数据实体</param>
        /// <param name="msg">额外信息</param>
        /// <param name="token">权限令牌</param>
        bool UnlockInfo(TEntity data, out string msg, TToken token);
    }
}
