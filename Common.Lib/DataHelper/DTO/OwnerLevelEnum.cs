using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Lib.DataHelper
{
    public enum OwnerLevelEnum
    {
        /// <summary>
        /// 所有
        /// </summary>
        ALL = 0,
        /// <summary>
        /// 大后台
        /// </summary>
        Manager = 1,
        /// <summary>
        /// 代理商
        /// </summary>
        Agent = 2,
        /// <summary>
        /// 商户
        /// </summary>
        Merchant = 4
    }
}
