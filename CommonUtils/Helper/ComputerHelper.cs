using System;
using System.Diagnostics;
using System.Management;

namespace CommonUtils
{
    ///   <summary>     
    ///   计算机信息     
    ///   </summary>     
    public class ComputerHelper
    {
        /// <summary>
        /// CPU序列号
        /// </summary>
        public String CpuID
        {
            get
            {
                try
                {
                    String cpuInfo = String.Empty;
                    ManagementClass mc = new ManagementClass("Win32_Processor");
                    ManagementObjectCollection moc = mc.GetInstances();
                    foreach (ManagementObject mo in moc)
                    {
                        cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                    }
                    moc.Dispose();
                    mc.Dispose();
                    return cpuInfo;
                }
                catch
                {
                    return "未工作";
                }
                finally
                {
                }
            }
        }
        /// <summary>
        /// 获取网卡硬件地址
        /// </summary>
        public String MacAddress
        {
            get
            {
                try
                {
                    String mac = String.Empty;
                    ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                    ManagementObjectCollection moc = mc.GetInstances();
                    foreach (ManagementObject mo in moc)
                    {
                        if ((Boolean)mo["IPEnabled"] == true)
                        {
                            mac = mo["MacAddress"].ToString();
                            break;
                        }
                    }
                    moc.Dispose();
                    mc.Dispose();
                    return mac;
                }
                catch
                {
                    return "未工作";
                }
                finally
                {
                }
            }
        }
        /// <summary>
        /// 获取IP地址
        /// </summary>
        public String IpAddress
        {
            get
            {
                try
                {
                    String st = String.Empty;
                    ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                    ManagementObjectCollection moc = mc.GetInstances();
                    foreach (ManagementObject mo in moc)
                    {
                        if ((Boolean)mo["IPEnabled"] == true)
                        {
                            //st=mo["IpAddress"].ToString();
                            Array ar = (Array)(mo.Properties["IpAddress"].Value);
                            st = ar.GetValue(0).ToString();
                            break;
                        }
                    }
                    moc.Dispose();
                    mc.Dispose();
                    return st;
                }
                catch
                {
                    return "未工作";
                }
                finally
                {
                }
            }
        }
        /// <summary>
        /// 硬盘序列号
        /// </summary>
        public String DiskID
        {
            get
            {
                try
                {
                    String HDid = String.Empty;
                    ManagementClass mc = new ManagementClass("Win32_DiskDrive");
                    ManagementObjectCollection moc = mc.GetInstances();
                    foreach (ManagementObject mo in moc)
                    {
                        HDid = (String)mo.Properties["Model"].Value;
                    }
                    moc.Dispose();
                    mc.Dispose();
                    return HDid;
                }
                catch
                {
                    return "未工作";
                }
                finally
                {
                }
            }
        }
        /// <summary>
        /// 登录用户名
        /// </summary>
        public String LoginUserName
        {
            get
            {
                return Environment.UserName;
            }
        }
        /// <summary>
        /// 操作系统信息
        /// </summary>
        public OperatingSystem OSVersion
        {
            get { return Environment.OSVersion; }
        }
        /// <summary>
        /// 计算机名
        /// </summary>
        public String ComputerName
        {
            get
            {
                return Environment.MachineName;
            }
        }
        /// <summary>
        /// 系统类型
        /// </summary>
        public String SystemType
        {
            get
            {
                try
                {
                    String st = String.Empty;
                    ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                    ManagementObjectCollection moc = mc.GetInstances();
                    foreach (ManagementObject mo in moc)
                    {
                        st = mo["SystemType"].ToString();
                    }
                    moc.Dispose();
                    mc.Dispose();
                    return st;
                }
                catch
                {
                    return "未工作";
                }
                finally
                {
                }
            }
        }
        /// <summary>
        /// 物理内存(单位:M)
        /// </summary>
        public String TotalPhysicalMemory
        {
            get
            {
                try
                {
                    String st = String.Empty;
                    ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                    ManagementObjectCollection moc = mc.GetInstances();
                    foreach (ManagementObject mo in moc)
                    {
                        st = mo["TotalPhysicalMemory"].ToString();
                    }
                    moc.Dispose();
                    mc.Dispose();
                    return st;
                }
                catch
                {
                    return "未工作";
                }
                finally
                {
                }
            }
        }
        /// <summary>
        /// 读取当前进程占用内存(MB)
        /// </summary>
        /// <returns></returns>
        public static double MemeryRead()
        {
            double mem = 0;
            using (PerformanceCounter pc = new PerformanceCounter("Process", "Private Bytes", Process.GetCurrentProcess().ProcessName))
            {
                mem = (pc.NextValue() / (1024 * 1024));
            }
            return mem;
        }
    }
}
