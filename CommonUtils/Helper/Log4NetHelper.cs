using System;
using log4net;

namespace CommonUtils
{
    /// <summary>
    /// log4net实例辅助类
    /// </summary>
    public class Log4NetHelper
    {
        private ILog _log4net;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public Log4NetHelper(Type type)
        {
            _log4net = log4net.LogManager.GetLogger(type);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public Log4NetHelper(String name)
        {
            _log4net = log4net.LogManager.GetLogger(name);
        }

        #region Debug

        /// <summary>
        /// 使用log4net Debug级别记录日志信息
        /// </summary>
        /// <param name="message"></param>
        public void Debug(Object message)
        {
            _log4net.Debug(message);
        }
        /// <summary>
        /// 使用log4net Debug级别记录日志信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Debug(Object message, Exception exception)
        {
            _log4net.Debug(message, exception);
        }
        /// <summary>
        /// 使用log4net Debug级别记录日志信息
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void DebugFormat(String format, params Object[] args)
        {
            _log4net.DebugFormat(format, args);
        }

        #endregion

        #region Info

        /// <summary>
        /// 使用log4net Info级别记录日志信息
        /// </summary>
        /// <param name="message"></param>
        public void Info(Object message)
        {
            _log4net.Info(message);
        }
        /// <summary>
        /// 使用log4net Info级别记录日志信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Info(Object message, Exception exception)
        {
            _log4net.Info(message, exception);
        }
        /// <summary>
        /// 使用log4net Info级别记录日志信息
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void InfoFormat(String format, params Object[] args)
        {
            _log4net.InfoFormat(format, args);
        }

        #endregion

        #region Warn

        /// <summary>
        /// 使用log4net Warn级别记录日志信息
        /// </summary>
        /// <param name="message"></param>
        public void Warn(Object message)
        {
            _log4net.Warn(message);
        }
        /// <summary>
        /// 使用log4net Warn级别记录日志信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Warn(Object message, Exception exception)
        {
            _log4net.Warn(message, exception);
        }
        /// <summary>
        /// 使用log4net Warn级别记录日志信息
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void WarnFormat(String format, params Object[] args)
        {
            _log4net.WarnFormat(format, args);
        }

        #endregion

        #region Error

        /// <summary>
        /// 使用log4net Error级别记录日志信息
        /// </summary>
        /// <param name="message"></param>
        public void Error(Object message)
        {
            _log4net.Error(message);
        }
        /// <summary>
        /// 使用log4net Error级别记录日志信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Error(Object message, Exception exception)
        {
            _log4net.Error(message, exception);
        }
        /// <summary>
        /// 使用log4net Error级别记录日志信息
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void ErrorFormat(String format, params Object[] args)
        {
            _log4net.ErrorFormat(format, args);
        }

        #endregion

        #region Fatal

        /// <summary>
        /// 使用log4net Fatal级别记录日志信息
        /// </summary>
        /// <param name="message"></param>
        public void Fatal(Object message)
        {
            _log4net.Fatal(message);
        }
        /// <summary>
        /// 使用log4net Fatal级别记录日志信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Fatal(Object message, Exception exception)
        {
            _log4net.Fatal(message, exception);
        }
        /// <summary>
        /// 使用log4net Fatal级别记录日志信息
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void FatalFormat(String format, params Object[] args)
        {
            _log4net.FatalFormat(format, args);
        }

        #endregion

        /// <summary>
        /// 返回Logger名称
        /// </summary>
        /// <returns></returns>
        public String GetLoggerName()
        {
            return _log4net.Logger.Name;
        }
    }
}
