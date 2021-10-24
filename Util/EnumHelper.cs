using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;



#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：Util
* 项目描述 ：
* 类 名 称 ：EnumHelper
* 类 描 述 ：
* 所在的域 ：GUILAIN
* 命名空间 ：Util
* 机器名称 ：GUILAIN 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：guilain
* 创建时间 ：2019/10/29 11:27:39
* 更新时间 ：2019/10/29 11:27:39
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ guilain 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion

namespace Util
{
    public static class EnumHelper
    {
        /// <summary>
        /// 返回枚举描述信息
        /// </summary>
        /// <param name="en"></param>
        /// <returns></returns>
        public static string GetEnumDesc(this Enum en)
        {
            MemberInfo[] memberInfo = en.GetType().GetMember(en.ToString());
            if (memberInfo == null && memberInfo.Length <= 0) return en.ToString();
            var attr = memberInfo[0].GetCustomAttribute(typeof(DescriptionAttribute), false);
            return attr == null ? en.ToString() : ((DescriptionAttribute)attr).Description;
        }

        /// <summary>
        /// 返回枚举项和描述信息
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetEnumItemDesc(this Type enumType)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            FieldInfo[] fieldInfo = enumType.GetFields();
            fieldInfo.AsParallel().ForAll(x =>
            {
                if (x.FieldType.IsEnum)
                {
                    var attr = x.GetCustomAttribute(typeof(DescriptionAttribute), false);
                    dic.Add(x.Name, attr != null ? ((DescriptionAttribute)attr).Description : string.Empty);
                }
            });
            return dic;
        }
    }
}
