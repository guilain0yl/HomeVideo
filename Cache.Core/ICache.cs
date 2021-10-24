using System;

namespace Cache.Core
{
    public interface ICache
    {
        /// <summary>
        /// 设置字符串值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间(秒)</param>
        /// <returns></returns>
        bool SetValue(string key, string value, int expiry = -1);


        /// <summary>
        /// 存储对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键值</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间(秒)</param>
        /// <returns></returns>
        bool SetValue<T>(string key, T value, int expiry = -1);

        /// <summary>
        /// 获取字符串值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetValue(string key);

        /// <summary>
        /// 获取指定类型的对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键值</param>
        /// <returns></returns>
        T GetValue<T>(string key);

        /// <summary>
        /// 键是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        bool KeyExists(string key);

        /// <summary>
        /// 设置键过期时间
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expiry">新的过期时间</param>
        /// <returns></returns>
        bool KeyExpiry(string key, int expiry = -1);

        /// <summary>
        /// 删除指定的键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool RemoveKey(string key);
    }
}
