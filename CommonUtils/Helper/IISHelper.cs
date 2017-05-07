using System;
using System.Diagnostics;
using System.ServiceProcess;

namespace CommonUtils
{
    public class IISHelper
    {
        #region IIS操作辅助类

        ServiceController sc = new ServiceController("iisadmin");
        /// <summary>
        /// 停止IIS服务
        /// </summary>
        /// <returns>返回TRUE和FALSE</returns>
        public Boolean StopIIS()
        {
            try
            {
                sc.Stop();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 启动IIS服务
        /// </summary>
        /// <returns>返回TRUE和FALSE</returns>
        public Boolean StartIIS()
        {
            try
            {
                sc.Start();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        ///  重启IIS服务
        /// </summary>
        /// <returns>返回TRUE和FALSE</returns>
        public Boolean ResetIIS()
        {
            try
            {
                Process.Start("iisreset");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}
