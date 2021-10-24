using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;



#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：Util
* 项目描述 ：
* 类 名 称 ：StringHelper
* 类 描 述 ：
* 所在的域 ：GUILAIN
* 命名空间 ：Util
* 机器名称 ：GUILAIN 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：guilain
* 创建时间 ：2019/10/29 11:30:45
* 更新时间 ：2019/10/29 11:30:45
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ guilain 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion

namespace Util
{
    public static class StringHelper
    {
        public static bool IsNullOrEmpty(this string str)
            => string.IsNullOrEmpty(str);

        public static bool IsZNCH(this string str) 
            => Regex.IsMatch(str, @"[\u4e00-\u9fbb]");

        public static bool IsPhone(string phone) 
            => Regex.IsMatch(phone, "(\\d{11})|^((\\d{7,8})|(\\d{4}|\\d{3})-(\\d{7,8})|(\\d{4}|\\d{3})-(\\d{7,8})-(\\d{4}|\\d{3}|\\d{2}|\\d{1})|(\\d{7,8})-(\\d{4}|\\d{3}|\\d{2}|\\d{1}))$");

        /// <summary>
        /// 将枚举转化成间隔符隔开的字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumer"></param>
        /// <param name="separator">间隔符</param>
        /// <returns></returns>
        public static string Join<T>(this IEnumerable<T> enumer, string separator = ",") 
            => string.Join(separator, enumer);

        /// <summary>
        /// 将字符串以制定分隔符隔开，并返回制定类型的枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <param name="Cast">转换表达式</param>
        /// <param name="separator">分隔符</param>
        /// <returns></returns>
        public static IEnumerable<T> StrToEnumerable<T>(this string str, Expression<Func<string, T>> Cast = null, char separator = ',') 
            => str.Split(separator, StringSplitOptions.RemoveEmptyEntries).Select(x => Cast.Compile()(x));

        /// <summary>
        /// 将字符串按数量分组
        /// </summary>
        /// <param name="str"></param>
        /// <param name="splitCount">每组字符串的数量</param>
        /// <returns></returns>
        public static IEnumerable<string> Split(this string str, int charCount = 1)
        {
            if (charCount < 0) return null;
            if (charCount == 1) return str.Select(x => x.ToString());
            List<string> result = new List<string>();
            for (int index = 0; index < str.Length; index += charCount)
                result.Add(str.Substring(index, (index + charCount > str.Length) ? (str.Length - index) : charCount));
            return result;
        }

        /// <summary>
        /// 将字符串按数量分组 逆序
        /// </summary>
        /// <param name="str"></param>
        /// <param name="charCount">每组字符串的数量</param>
        /// <returns></returns>
        public static IEnumerable<string> SplitDesc(this string str, int charCount = 1)
        {
            if (charCount < 0) return null;
            if (charCount == 1) return str.Select(x => x.ToString());
            List<string> result = new List<string>();
            for (int index = str.Length; index > 0; index -= charCount)
                result.Add(str.Substring(index < charCount ? 0 : (index - charCount), index < charCount ? index : charCount));
            return result;
        }

        /// <summary>
        /// base64字符串转string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Base64ToString(this string str)
            => Encoding.Default.GetString(Convert.FromBase64String(str));

        /// <summary>
        /// base64编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string StringToBase64(this string str) 
            => Convert.ToBase64String(Encoding.ASCII.GetBytes(str));

        /// <summary>
        /// url编码
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string UrlEncode(this string str, Encoding encoding = null)
            => System.Web.HttpUtility.UrlEncode(str, encoding ?? Encoding.Default);

        /// <summary>
        /// url解码
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string UrlDecode(this string str, Encoding encoding = null) 
            => System.Web.HttpUtility.UrlDecode(str, encoding ?? Encoding.Default);

        public static string ToUnicode(this string str)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(str);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i += 2)
            {
                stringBuilder.AppendFormat(@"\u{0}{1}", bytes[i + 1].ToString("x").PadLeft(2, '0'), bytes[i].ToString("x").PadLeft(2, '0'));
            }
            return stringBuilder.ToString();
        }
    }
}
