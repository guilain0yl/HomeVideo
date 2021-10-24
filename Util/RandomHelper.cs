using System;
using System.Collections.Generic;
using System.Text;



#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：Util
* 项目描述 ：
* 类 名 称 ：RandomHelper
* 类 描 述 ：
* 所在的域 ：GUILAIN
* 命名空间 ：Util
* 机器名称 ：GUILAIN 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：guilain
* 创建时间 ：2019/10/24 14:11:21
* 更新时间 ：2019/10/24 14:11:21
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ guilain 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion

namespace Util
{
    public class RandomHelper
    {
        public static string RandomString(int length, RandomMod randomMod = RandomMod.Number)
        {
            List<char> pool = new List<char>();
            if (((int)randomMod & (int)RandomMod.Number) != 0)
            {
                for (int i = 0; i < 10; i++)
                    pool.Add(letter[i]);
            }
            if (((int)randomMod & (int)RandomMod.Upper) != 0)
            {
                for (int i = 10; i < 36; i++)
                    pool.Add(letter[i]);
            }
            if (((int)randomMod & (int)RandomMod.Lower) != 0)
            {
                for (int i = 36; i < letter.Length; i++)
                    pool.Add(letter[i]);
            }

            Random random = new Random(DateTime.Now.GetUnixTimestamp().ToInt());
            char[] chars = pool.ToArray();
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append(chars[random.Next() % chars.Length]);
            }
            return stringBuilder.ToString();
        }

        private static readonly char[] letter = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'S', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 's', 'y', 'z' };

        /// <summary>
        /// hash散列
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int BKDRHash(string str)
        {
            int seed = 131;
            int hash = 0;

            foreach (var c in str)
            {
                int i = c;
                hash = hash * seed + i++;
            }

            return (hash & 0x7FFFFFFF);
        }
    }

    [Flags]
    public enum RandomMod
    {
        /// <summary>
        /// 纯数字
        /// </summary>
        Number = 0x1,
        /// <summary>
        /// 大写字母
        /// </summary>
        Upper = 0x2,
        /// <summary>
        /// 小写字母
        /// </summary>
        Lower = 0x4
    }
}
