using System;
using System.ComponentModel;
using System.IO;
using System.Web;
using System.Web.UI;
using WebSite.Interface;

namespace WebSite.Controls
{
    public class FileUpload : System.Web.UI.WebControls.FileUpload, IControls
    {
        /// <summary>
        /// 添加value属性标记
        /// </summary>
        /// <param name="writer"></param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Value, this.Value);
            base.AddAttributesToRender(writer);
        }
        /// <summary>
        /// 数据值
        /// </summary>
        [Bindable(true), Category("Expand"), DefaultValue("")]
        public String Value
        {
            get
            {
                return Convert.ToString(ViewState["value"]);
            }
            set
            {
                ViewState["value"] = value;
            }
        }

        [Browsable(false)]
        public String FormatString { get; set; }

        public void SetValue(Object value)
        {
            this.Value = Convert.ToString(value);
        }

        public Object GetValue()
        {
            String result = this.Value;
            if (this.HasFile)
            {
                HttpPostedFile file = this.PostedFile;
                if (file.ContentLength > 0)
                {
                    HttpServerUtility Server = HttpContext.Current.Server;
                    String ex = Path.GetExtension(file.FileName).ToLower();
                    String SaveFolder = Path.Combine(Server.MapPath("/UpFiles/"), DateTime.Now.ToString("yyyy-MM-dd"));

                    Int32 count = Directory.CreateDirectory(SaveFolder).GetFiles().Length + 1;
                    String SavePath = Path.Combine(SaveFolder, DateTime.Now.ToString("yyyyMMdd") + count.ToString("D5") + ex);
                    file.SaveAs(SavePath);

                    result = "/" + new Uri(Server.MapPath("/")).MakeRelativeUri(new Uri(SavePath)).ToString();
                    this.Value = result;
                }
            }
            return result;
        }
    }
}
