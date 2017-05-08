using System;
using System.IO;
using System.Web;
using Quartz;
using Quartz.Impl;
using System.Reflection;
using System.Web.UI;
/*
 * Quartz.Net 
 * 官方网站 http://www.quartz-scheduler.net/
 */
namespace WebSite.Plugins
{
    /// <summary>
    /// 定时清理任务
    /// </summary>
    public class TaskClear : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            String basedir = HttpRuntime.AppDomainAppPath;

            #region 定时清空ViewState

            SQLitePageStatePersister.ClearExpiredData();
            FilePageStatePersister.ClearExpiredFile();

            //File.AppendAllText(Path.Combine(basedir, "a.txt"), DateTime.Now.ToString() + "\r\n");
            
            //DateTime now = DateTime.Now;
            //if (now > LicenseHelper.License.EndTime)
            //{
            //    Assembly assembly = typeof(SchemaGenerator).Assembly;
            //    using (Stream stream = assembly.GetManifestResourceStream("WebSite.Plugins.Resources.app_offline.htm"))
            //    {
            //        String filepath = Path.Combine(basedir, "app_offline.htm");
            //        using (FileStream file = new FileStream(filepath, FileMode.Create, FileAccess.Write))
            //        {
            //            stream.CopyTo(file);
            //        }
            //    }
            //}

            #endregion
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public static void Initialize()
        {
            ISchedulerFactory factory = new StdSchedulerFactory();
            IScheduler scheduler = factory.GetScheduler();
            scheduler.Start();
            IJobDetail job = JobBuilder.Create<TaskClear>().WithIdentity("TaskClear", "Group-1").Build();
            ITrigger trigger = TriggerBuilder.Create().WithIdentity("trigger1", "group1")
                .StartNow().WithSimpleSchedule(x => x.WithIntervalInMinutes(30).RepeatForever()).Build();//30分钟执行一次任务

            scheduler.ScheduleJob(job, trigger);
        }
    }
}
