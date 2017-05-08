using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;

namespace WebSite.Plugins
{
    /// <summary>
    /// 文件存储 VIEWSTATE
    /// </summary>
    public class FilePageStatePersister : PageStatePersister
    {
        private readonly String STATEKEY = "___VIEWSTATE";

        public FilePageStatePersister(Page page) : base(page) { }

        public override void Load()
        {
            String viewstateid = Page.Request.Form[STATEKEY];
            if (!String.IsNullOrEmpty(viewstateid))
            {
                String filename = Path.Combine(HttpRuntime.AppDomainAppPath, "App_Data/ViewState", viewstateid);
                using (FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    BinaryFormatter format = new BinaryFormatter();

                    Pair data = (Pair)format.Deserialize(file);
                    ViewState = data.First;
                    ControlState = data.Second;
                }
            }
        }

        public override void Save()
        {
            if (ViewState != null || ControlState != null)
            {
                Pair data = new Pair(ViewState, ControlState);

                Byte[] buffer = new Byte[8];
                using (RNGCryptoServiceProvider rand = new RNGCryptoServiceProvider())
                {
                    rand.GetNonZeroBytes(buffer);
                }

                String postbackstate = Page.Request.Form[STATEKEY];
                String viewstateid = postbackstate ?? Page.Request.Url.GetHashCode().ToString("X2") + DateTime.Now.Ticks.ToString("X2") + String.Concat(buffer);
                String filename = Path.Combine(HttpRuntime.AppDomainAppPath, "App_Data/ViewState", viewstateid);

                using (FileStream file = new FileStream(filename, FileMode.Create, FileAccess.Write))
                {
                    BinaryFormatter format = new BinaryFormatter();
                    format.Serialize(file, data);
                }

                Page.ClientScript.RegisterHiddenField(STATEKEY, viewstateid);
            }
        }

        /// <summary>
        /// 清理过期VIEWSTATE会话文件
        /// </summary>
        public static void ClearExpiredFile()
        {
            DirectoryInfo folder = new DirectoryInfo(HttpRuntime.AppDomainAppPath + "App_Data/ViewState/");
            //if (!folder.Exists) folder.Create();
            DateTime time = DateTime.Now.AddMinutes(30);
            foreach (FileInfo file in folder.GetFiles())
            {
                if (file.LastWriteTime < time) file.Delete();
            }
        }
        /// <summary>
        /// 删除所有VIEWSTATE会话文件
        /// </summary>
        public static void ClearFile()
        {
            DirectoryInfo folder = new DirectoryInfo(HttpRuntime.AppDomainAppPath + "App_Data/ViewState/");
            if (!folder.Exists) folder.Create();
            foreach (FileInfo file in folder.GetFiles())
            {
                file.Delete();
            }
        }
    }
}
