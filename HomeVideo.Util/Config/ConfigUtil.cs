using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Xml;
using Util;



#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：Pay.Util
* 项目描述 ：
* 类 名 称 ：ConfigUtil
* 类 描 述 ：
* 所在的域 ：GUILAIN
* 命名空间 ：Pay.Util
* 机器名称 ：GUILAIN 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：guilain
* 创建时间 ：2019/10/28 13:45:29
* 更新时间 ：2019/10/28 13:45:29
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ guilain 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion

namespace HomeVideo.Util
{
    public static class ConfigUtil
    {
        public static void InitGlobalConfig(this IServiceCollection services, IConfiguration configuration)
        {
            string configPath = LoadConfigPath(services, configuration);

            InitGlobalConfig(configPath);
        }

        public static void InitGlobalConfig(string configPath)
        {
            LoadConfig(configPath);

            InitAppSetting();
            InitConnStrings();
        }

        public static string GetConnectionString(string key) => _connStrings.ContainsKey(key) ? _connStrings[key] : string.Empty;

        public static string GetAppSetting(string key) => _appSettings.ContainsKey(key) ? _appSettings[key] : string.Empty;

        private static string LoadConfigPath(IServiceCollection services, IConfiguration configuration)
        {
            string configPath = string.Empty;

            string DefaultPath = Path.Combine("Common", "Common.config");

            if (!configuration.GetSection(CommonConifgFilePath).Value.IsNullOrEmpty())
            {
                configPath = configuration[CommonConifgFilePath];
                configPath = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName, configPath);
                //string relativePath = configuration[CommonConifgFilePath];

                //int depth = 10;
                //string path = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName;
                //while (depth > 0 && relativePath.StartsWith(RelativeString))
                //{
                //    path = Directory.GetParent(path).FullName;
                //    --depth;
                //    relativePath = relativePath.Substring(RelativeString.Length, relativePath.Length - RelativeString.Length);
                //}
                //configPath = Path.Combine(relativePath, DefaultPath);
            }

            if (configPath?.Length < 1)
            {
                configPath = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName, DefaultPath);
            }

            if (!File.Exists(configPath))
            {
                throw new FileNotFoundException($"can't find file. File Path:{configPath}");
            }

            return configPath;
        }

        private static void LoadConfig(string configPath)
        {
            XmlDocument document = new XmlDocument();
            document.Load(configPath);

            XmlNodeList nodes = document.DocumentElement.SelectNodes("connectionStrings/add");
            foreach (XmlNode node in nodes)
            {
                XmlElement element = node as XmlElement;
                if (element != null)
                {
                    string name = element.GetAttribute("name");
                    string connectionString = element.GetAttribute("connectionString");
                    _connStrings[name] = connectionString;
                }
            }

            nodes = document.DocumentElement.SelectNodes("appSettings/add");
            foreach (XmlNode node in nodes)
            {
                XmlElement element = node as XmlElement;
                if (element != null)
                {
                    string key = element.GetAttribute("key");
                    string value = element.GetAttribute("value");
                    _appSettings[key] = value;
                }
            }
        }

        private static void InitAppSetting()
        {
            foreach (var t in typeof(AppSetting).GetProperties())
            {
                if (_appSettings.TryGetValue(t.Name, out string value))
                {
                    t.SetValue(null, value.ToTargetType(t.PropertyType));
                }
                else
                {
                    throw new ArgumentException($"can't find {t.Name} from common.config");
                }
            }
        }

        private static void InitConnStrings()
        {
            foreach (var t in typeof(ConnectionStrings).GetProperties())
            {
                if (_connStrings.TryGetValue(t.Name, out string value))
                {
                    t.SetValue(null, value);
                }
                else
                {
                    throw new ArgumentException($"can't find {t.Name} from common.config");
                }
            }
        }

        private static ConcurrentDictionary<string, string> _connStrings = new ConcurrentDictionary<string, string>();

        private static ConcurrentDictionary<string, string> _appSettings = new ConcurrentDictionary<string, string>();

        private const string RelativeString = "../";

        private const string CommonConifgFilePath = nameof(CommonConifgFilePath);
    }
}
