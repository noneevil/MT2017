using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;

/*
 * 示例:
 * new SnapWebSiteHelper("http://zx7.vicp.net/", Server.MapPath("/1.png"));
 */
namespace CommonUtils
{
    /// <summary>
    /// 生成网站快照
    /// </summary>
    public class SnapWebSiteHelper
    {
        /// <summary>
        /// 网址
        /// </summary>
        protected String Url { get; set; }
        /// <summary>
        /// 保存文件
        /// </summary>
        protected String SaveFile { get; set; }

        /// <summary>
        /// 生成网站缩略图
        /// </summary>
        /// <param name="url">网址</param>
        /// <param name="savefile">保存文件</param>
        public SnapWebSiteHelper(String url, String savefile)
        {
            this.Url = url;
            this.SaveFile = savefile;

            Thread t = new Thread(new ParameterizedThreadStart(StartRun));
            t.SetApartmentState(ApartmentState.STA);
            t.Start(this);
        }
        /// <summary>
        /// WebBrowser所开线程的启动入口函数
        /// </summary>
        /// <param name="obj"></param>
        protected void StartRun(Object obj)
        {
            SnapWebSiteHelper site = (SnapWebSiteHelper)obj;
            new Snapshot(site);
        }
        /// <summary>
        /// 生成处理类
        /// </summary>
        internal class Snapshot : IDisposable
        {
            WebBrowser web;
            SnapWebSiteHelper Site;
            Bitmap bmp;

            public Snapshot(SnapWebSiteHelper site)
            {
                Site = site;
                web = new WebBrowser();
                web.Size = new Size(1024, 768);//默认分辨率为1024x768
                web.ScrollBarsEnabled = false;
                web.ScriptErrorsSuppressed = false;
                web.NewWindow += new CancelEventHandler(CancelEventHandler);
                web.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(DocCompletedEventHandler);
                web.Navigate(Site.Url);

                while (web.ReadyState != WebBrowserReadyState.Complete)
                {
                    Application.DoEvents();
                }
                //web.Stop();
            }
            /// <summary>
            /// 页面加载完成事件
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void DocCompletedEventHandler(Object sender, EventArgs e)
            {
                int h = web.Document.Body.ScrollRectangle.Height;
                int w = web.Document.Body.ScrollRectangle.Width;
                web.Size = new Size(w, h);

                bmp = new Bitmap(web.Width, web.Height);
                web.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                bmp.Save(Site.SaveFile, ImageFormat.Png);
            }
            /// <summary>
            /// 防止弹窗
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void CancelEventHandler(Object sender, CancelEventArgs e)
            {
                e.Cancel = true;
            }

            #region IDisposable 成员

            public void Dispose()
            {
                bmp.Dispose();
                web.Dispose();
            }

            #endregion
        }
    }
}
