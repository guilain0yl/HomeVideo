using System;
using System.Collections.Generic;
using System.Text;



#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：Pay.Util
* 项目描述 ：
* 类 名 称 ：AppSetting
* 类 描 述 ：
* 所在的域 ：GUILAIN
* 命名空间 ：Pay.Util
* 机器名称 ：GUILAIN 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：guilain
* 创建时间 ：2019/10/28 14:22:40
* 更新时间 ：2019/10/28 14:22:40
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ guilain 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion

namespace HomeVideo.Util
{
    public static class AppSetting
    {
        /// <summary>
        /// 登录信息过期时间
        /// </summary>
        public static int ExpireIn { get; set; }

        /// <summary>
        /// 加密串
        /// </summary>
        public static string SessionKey { get; set; }

        public static string VideoPath { get; set; }

        public static string ImagePath { get; set; }

        public static string Password { get; set; }
    }
}
