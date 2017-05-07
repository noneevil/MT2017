using System;
using System.Collections;
using System.Configuration.Install;
using System.ServiceProcess;

namespace CommonUtils
{
    /// <summary>
    /// windows 服务安装和卸载
    /// </summary>
    public class ServiceHelper
    {
        /// <summary>
        /// 检查服务是否存在
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        protected static Boolean ServiceExist(String serviceName)
        {
            serviceName = serviceName.ToLower();
            String _servicename = String.Empty;
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController service in services)
            {
                _servicename = service.ServiceName;
                if (_servicename == serviceName) return true;
            }
            return false;
        }
        /// <summary>
        /// 安装服务
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="serviceName"></param>
        public static void InstallService(String filepath, String serviceName)
        {
            ServiceController service = new ServiceController(serviceName);
            if (!ServiceExist(serviceName))
            {
                AssemblyInstaller ass = new AssemblyInstaller();
                ass.UseNewContext = true;
                ass.Path = filepath;
                Hashtable stateSaver = new Hashtable();
                stateSaver.Clear();
                ass.Install(stateSaver);
                ass.Commit(stateSaver);
                ass.Dispose();
                service.Start();
            }
            else
            {
                if (service.Status != ServiceControllerStatus.Running && service.Status != ServiceControllerStatus.StartPending)
                {
                    service.Start();
                }
            }
        }
        /// <summary>
        /// 卸载服务
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="serviceName"></param>
        public static void UnInstallService(String filepath, String serviceName)
        {
            if (ServiceExist(serviceName))
            {
                AssemblyInstaller ass = new AssemblyInstaller();
                ass.UseNewContext = true;
                ass.Path = filepath;
                ass.Uninstall(null);
                ass.Dispose();
            }
        }
        /// <summary>
        /// 停止服务
        /// </summary>
        /// <param name="servicename"></param>
        /// <returns></returns>
        private Boolean StopService(String servicename)
        {
            ServiceController service = new ServiceController(servicename);
            try
            {
                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 开启服务
        /// </summary>
        private Boolean StartService(String servicename)
        {
            ServiceController service = new ServiceController(servicename);
            try
            {
                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
