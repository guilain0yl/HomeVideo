using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Lib.DataHelper
{
    public enum LockStateEnum : short
    {
        /// <summary>
        /// 查询全部
        /// </summary>
        All = -1,
        /// <summary>
        /// 冻结
        /// </summary>
        Locked = 1,
        /// <summary>
        /// 活动
        /// </summary>
        Unlocked = 0
    }
}
