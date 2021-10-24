using System;
using System.Collections.Generic;
using System.Text;
using HomeVideo.Util;
using Util;

namespace HomeVideo.Util
{
    public class TokenInfo
    {

        /// <summary>
        /// Token原文
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 过期时间 unix time
        /// </summary>
        public long ExpiresIn { get; set; }

        public static TokenInfo GenerateToken(string key)
        {
            TokenInfo tokenInfo = new TokenInfo();
            tokenInfo.Name = key?.Trim();
            tokenInfo.ExpiresIn = DateTime.Now.AddSeconds(AppSetting.ExpireIn + 10).GetUnixTimestamp();
            tokenInfo.Token = $"Bearer {AesHelper.AESEncrypt($"{key}@{tokenInfo.ExpiresIn}", AppSetting.SessionKey)}";

            return tokenInfo;
        }

        public static bool DescToken(string token, out string key, out long expiresIn)
        {
            key = string.Empty;
            expiresIn = 0;

            try
            {
                if (token.IsNullOrEmpty())
                    return false;

                var cipherText = token.Replace("Bearer ", "").Trim();

                var text = AesHelper.AESDecrypt(cipherText, AppSetting.SessionKey)?.Trim();

                var content = text.Split('@');
                if (content == null || content.Length != 2)
                    return false;

                key = content[0]?.Trim();
                long.TryParse(content[1], out expiresIn);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
