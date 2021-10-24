using System;
using System.Management;
using System.Linq;
using Util;




#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 类 名 称 ：HardwareInfo
* 类 描 述 ：获取服务器硬件信息
* 所在的域 ：guilain_cp
* 命名空间 ：Frame.Util.SystemInfo
* 机器名称 ：GUILAIN_CP 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：guilain
* 创建时间 ：2018/6/9 11:44:56
* 更新时间 ：2018/6/9 11:44:56
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ guilain 2018. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion



namespace Util
{
    public static class HardwareInfo
    {
        private static string _cpuid = string.Empty;
        /// <summary>
        /// 获取CPU序列号
        /// </summary>
        public static string CPUID { get { if (_cpuid.IsNullOrEmpty()) _cpuid = GetCPUID(); return _cpuid; } }

        private static string _mac = string.Empty;
        /// <summary>
        /// MAC地址
        /// </summary>
        public static string MacAddress { get { if (_mac.IsNullOrEmpty()) _mac = GetMacAddress(); return _mac; } }

        private static string _hdid = string.Empty;
        /// <summary>
        /// 硬盘地址
        /// </summary>
        public static string HardDiskID { get { if (_hdid.IsNullOrEmpty()) _hdid = GetHardDiskId(); return _hdid; } }

        private static string _ip = string.Empty;
        /// <summary>
        /// IP地址
        /// </summary>
        public static string IP { get { if (_ip.IsNullOrEmpty()) _ip = GetIPAddress(); return _ip; } }

        private static string _lun = string.Empty;
        /// <summary>
        /// 登陆用户名
        /// </summary>
        public static string LoginUserName { get { if (_lun.IsNullOrEmpty()) _lun = GetLoginUserName(); return _lun; } }

        private static string _sn = string.Empty;
        /// <summary>
        /// 系统名称
        /// </summary>
        public static string SystemName { get { if (_sn.IsNullOrEmpty()) _sn = GetSystemName(); return _sn; } }

        private static string _st = string.Empty;
        /// <summary>
        /// 系统类型
        /// </summary>
        public static string SystemType { get { if (_st.IsNullOrEmpty()) _st = GetSystemType(); return _st; } }

        private static string _tpm = string.Empty;
        /// <summary>
        /// 物理内存大小
        /// </summary>
        public static string TotalPhysicalMemory { get { if (_tpm.IsNullOrEmpty()) _tpm = GetTotalPhysicalMemory(); return _tpm; } }


        private static string GetCPUID()
        {
            try
            {
                string cpuInfo = string.Empty;
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                }
                mc.Dispose(); ; moc.Dispose();
                return cpuInfo;
            }
            catch
            {
                return "unknow";
            }
            finally { }
        }

        private static string GetMacAddress()
        {
            try
            {
                string mac = string.Empty;
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        mac = mo["MacAddress"].ToString();
                        break;
                    }
                }
                mc.Dispose(); ; moc.Dispose();
                return mac;
            }
            catch
            {
                return "unknow";
            }
            finally { }
        }

        private static string GetHardDiskId()
        {
            try
            {
                string hdId = string.Empty;
                ManagementClass mc = new ManagementClass("Win32_DiskDrive");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    hdId = (string)mo.Properties["Model"].Value;
                }
                mc.Dispose(); ; moc.Dispose();
                return hdId;
            }
            catch
            {
                return "unknow";
            }
            finally { }
        }


        private static string GetIPAddress()
        {
            try
            {
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        //st=mo["IpAddress"].ToString(); 
                        Array ar;
                        ar = (Array)(mo.Properties["IpAddress"].Value);
                        st = ar.GetValue(0).ToString();
                        break;
                    }
                }
                mc.Dispose(); ; moc.Dispose();
                return st;
            }
            catch
            {
                return "unknow";
            }
            finally { }
        }

        private static string GetLoginUserName() => Environment.UserName;

        private static string GetSystemName() => Environment.MachineName;


        ///7 PC类型 
        private static string GetSystemType()
        {
            try
            {
                string st = string.Empty;
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    st = mo["SystemType"].ToString();
                }
                mc.Dispose(); ; moc.Dispose();
                return st;
            }
            catch
            {
                return "unknow";
            }
            finally { }
        }


        private static string GetTotalPhysicalMemory()
        {
            try
            {
                string st = string.Empty;
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    st = mo["TotalPhysicalMemory"].ToString();
                }
                mc.Dispose(); ; moc.Dispose();
                return st;
            }
            catch
            {
                return "unknow";
            }
            finally { }
        }
    }
}
