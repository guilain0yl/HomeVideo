using System;
using System.Collections.Generic;
using System.Text;



#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：Util
* 项目描述 ：
* 类 名 称 ：TimeHelper
* 类 描 述 ：
* 所在的域 ：GUILAIN
* 命名空间 ：Util
* 机器名称 ：GUILAIN 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：guilain
* 创建时间 ：2019/10/24 13:46:51
* 更新时间 ：2019/10/24 13:46:51
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ guilain 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion

namespace Util
{
    public static class TimeHelper
    {
        public static long GetUnixTimestamp() => (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;

        public static long GetUnixTimestamp(this DateTime dateTime) => (dateTime.Ticks - 621355968000000000) / 10000000;

        public static DateTime GetStartDate() => DateTime.Parse("1946-02-14 00:00:00");

        public static DateTime GetLastWeek(DateTime dateTime)
        {
            bool flag = false;
            DateTime date = dateTime;

            while (date.DayOfWeek != DayOfWeek.Monday || !flag)
            {
                if (!flag)
                {
                    flag = date.DayOfWeek == DayOfWeek.Monday;
                }

                date = date.AddDays(-1);
            }

            return date;
        }

        /// <summary>
        /// 常用格式yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="T">是否是yyyy-MM-ddTHH:mm:ss</param>
        /// <returns></returns>
        public static string ToCommonDateTime(this DateTime dateTime, bool T = false) => dateTime.ToString($"yyyy-MM-dd{(T ? "T" : " ")}HH:mm:ss");

        /// <summary>
        /// 常用格式yyyy-MM-dd
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToCommonDate(this DateTime dateTime) => dateTime.ToString("yyyy-MM-dd");

        /// <summary>
        /// 是否大于
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="compareDatetime">被比较时间</param>
        /// <returns></returns>
        public static bool IsGreater(this DateTime dateTime, DateTime compareDatetime) => DateTime.Compare(dateTime, compareDatetime) > 0;

        /// <summary>
        /// 是否大于等于
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="compareDatetime">被比较时间</param>
        /// <returns></returns>
        public static bool IsGreaterAndEqual(this DateTime dateTime, DateTime compareDatetime) => DateTime.Compare(dateTime, compareDatetime) >= 0;

        /// <summary>
        /// 是否小于
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="compareDatetime">被比较时间</param>
        /// <returns></returns>
        public static bool IsLess(this DateTime dateTime, DateTime compareDatetime) => DateTime.Compare(dateTime, compareDatetime) < 0;

        /// <summary>
        /// 是否小于等于
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="compareDatetime">被比较时间</param>
        /// <returns></returns>
        public static bool IsLessEqual(this DateTime dateTime, DateTime compareDatetime) => DateTime.Compare(dateTime, compareDatetime) <= 0;
    }
}
