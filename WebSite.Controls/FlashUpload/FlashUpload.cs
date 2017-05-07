using System;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

[assembly: WebResource("WebSite.Controls.FlashUpload.Upload.swf", "application/x-shockwave-flash")]
[assembly: WebResource("WebSite.Controls.FlashUpload.Install.swf", "application/x-shockwave-flash")]
[assembly: WebResource("WebSite.Controls.FlashUpload.btn.gif", "image/gif")]
[assembly: WebResource("WebSite.Controls.FlashUpload.swfobject.js", "application/x-javascript")]
namespace WebSite.Controls
{
    public class FlashUpload : Control
    {
        protected override void OnLoad(EventArgs e)
        {
            if (!DesignMode)
            {
                if (Page.FindControl("swfobject") == null)
                {
                    HtmlGenericControl js = new HtmlGenericControl("script");
                    js.ID = "swfobject";
                    js.Attributes.Add("src", this.Page.ClientScript.GetWebResourceUrl(GetType(), "WebSite.Controls.FlashUpload.swfobject.js"));
                    js.Attributes.Add("type", "text/javascript");
                    Page.Header.Controls.Add(js);
                }
            }
            base.OnLoad(e);
        }
        protected override void Render(HtmlTextWriter writer)
        {
            if (DesignMode)
            {
                ////*******************************检查web.config配置***********************************************
                //IWebApplication webApp = (IWebApplication)Site.GetService(typeof(IWebApplication));
                //System.Configuration.Configuration config = webApp.OpenWebConfiguration(false);
                ////HttpModulesSection moduleSection = (HttpModulesSection)config.GetSection("system.web/httpModules");//添加接管请求接口
                ////HttpModuleAction ha = new HttpModuleAction("HttpModuleName", "Namespace.HttpModuleName");
                ////moduleSection.Modules.Add(ha); 
                //bool _chk0 = true;//, _chk1 = true;
                //HttpHandlersSection section = (HttpHandlersSection)config.GetSection("system.web/httpHandlers");
                //foreach (HttpHandlerAction action in section.Handlers)//节点检查
                //{
                //    if (action.Path == "Upload.axd" && action.Type == "Web.Controls.Upload")
                //    {
                //        _chk0 = false;
                //    }
                //}
                //if (_chk0)
                //{
                //    HttpHandlerAction hc = new HttpHandlerAction("Upload.axd", "Web.Controls.Upload", "*", false);//添加上传文件处理接口
                //    section.Handlers.Add(hc);
                //    config.Save();
                //}
                ////************************************************************************************************
                //String _str = Page.ClientScript.GetWebResourceUrl(GetType(), "WebSite.Controls.FlashUpload.ui.png");
                //writer.Write("<div style=\"width:64px;height:64px;background-image: url(" + _str + "); \"></div>");
            }
            else
            {
                String FlashUrl = Page.ClientScript.GetWebResourceUrl(GetType(), "WebSite.Controls.FlashUpload.Upload.swf");
                String InstallUrl = Page.ClientScript.GetWebResourceUrl(GetType(), "WebSite.Controls.FlashUpload.Install.swf");

                String uniqueid = this.UniqueID;
                if (String.IsNullOrEmpty(OnCancel)) OnCancel = "function(){}";
                if (String.IsNullOrEmpty(OnUploadComplete)) OnUploadComplete = "function(){}";

                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("<div id=\"{0}\"><a href=\"http://www.adobe.com/go/getflashplayer\">", uniqueid);
                sb.Append("<img src=\"http://www.adobe.com/images/shared/download_buttons/get_flash_player.gif\"/></a>");
                sb.Append("<script type=\"text/javascript\">");

                sb.Append("var flashvars = {};");
                sb.AppendFormat("flashvars.uploadPage = \"{0}?{1}\";", base.ResolveUrl(UploadPage), HttpUtility.UrlEncode(QueryParameters));
                sb.AppendFormat("flashvars.completeFunction = \"{0}\";", OnUploadComplete);
                sb.AppendFormat("flashvars.fileTypes = \"{0}\";", HttpUtility.UrlEncode(FileTypes));
                sb.AppendFormat("flashvars.fileTypeDescription = \"{0}\";", HttpUtility.UrlEncode(FileTypeDescription));
                sb.AppendFormat("flashvars.totalUploadSize = \"{0}\";", TotalUploadSizeLimit);
                sb.AppendFormat("flashvars.fileSizeLimit = \"{0}\";", UploadFileSizeLimit);
                sb.AppendFormat("flashvars.maxFileNumber = \"{0}\";", MaxFileNumber);
                sb.AppendFormat("flashvars.cancelFunction = \"{0}\";", OnCancel);

                sb.Append("var params = {};");
                sb.Append("params.quality = \"high\";");
                sb.Append("params.wmode = \"transparent\";");

                sb.Append("var attributes = {};");
                sb.AppendFormat("swfobject.embedSWF(\"{0}\", \"{1}\", \"500\", \"317\", \"10.0.0\",\"{2}\", flashvars, params, attributes);", FlashUrl, uniqueid, InstallUrl);
                sb.Append("</script>");
                sb.Append("</div>");

                writer.Write(sb.ToString());
            }
        }
        [Description("上传文件的类型描述 (如. Images (*.JPG;*.JPEG;*.JPE;*.GIF;*.PNG;))"), Category("上传属性"), DefaultValue("")]
        public String FileTypeDescription
        {
            get { return ViewState["FileTypeDescription"] == null ? "" : (String)ViewState["FileTypeDescription"]; }
            set { ViewState["FileTypeDescription"] = value; }
        }
        [Description("上传文件的类型(如. *.jpg; *.jpeg; *.jpe; *.gif; *.png;)"), DefaultValue(""), Category("上传属性")]
        public String FileTypes
        {
            get { return ViewState["FileTypes"] == null ? "" : (String)ViewState["FileTypes"]; }
            set { ViewState["FileTypes"] = value; }
        }
        [Category("上传属性"), Description("最大上传文件数 (0 无限制).")]
        public int MaxFileNumber
        {
            get { return ViewState["MaxFileNumber"] == null ? 0 : (int)ViewState["MaxFileNumber"]; }
            set { ViewState["MaxFileNumber"] = value; }
        }
        [Category("上传属性"), Description("取消上传时执行的Javascript."), DefaultValue("")]
        public String OnCancel
        {
            get { return ViewState["OnCancel"] == null ? "" : (String)ViewState["OnCancel"]; }
            set { ViewState["OnCancel"] = value; }
        }
        [Category("上传属性"), DefaultValue(""), Description("文件上传后JavaScript调用的设置参数的方法.")]
        public String OnUploadComplete
        {
            get { return ViewState["OnUploadComplete"] == null ? "" : (String)ViewState["OnUploadComplete"]; }
            set { ViewState["OnUploadComplete"] = value; }
        }
        [Description("上传页参数."), DefaultValue(""), Category("上传属性")]
        public String QueryParameters
        {
            get { return ViewState["QueryParameters"] == null ? "" : (String)ViewState["QueryParameters"]; }
            set { ViewState["QueryParameters"] = value; }
        }
        [Description("上传文件的总大小(单位:字节 0 无限制)."), Category("上传属性")]
        public decimal TotalUploadSizeLimit
        {
            get { return ViewState["TotalUploadSizeLimit"] == null ? 0M : (decimal)ViewState["TotalUploadSizeLimit"]; }
            set { ViewState["TotalUploadSizeLimit"] = value; }
        }
        [Description("设置允许上传的最大文件大小 (单位;字节 0 无限制)."), Category("上传属性")]
        public decimal UploadFileSizeLimit
        {
            get { return ViewState["UploadFileSizeLimit"] == null ? 0M : (decimal)ViewState["UploadFileSizeLimit"]; }
            set { ViewState["UploadFileSizeLimit"] = value; }
        }
        [Description("上传文件的ASP.NET处理页面."), Category("上传属性"), DefaultValue("Upload.axd")]
        public String UploadPage
        {
            get { return ViewState["UploadPage"] == null ? "Upload.axd" : (String)ViewState["UploadPage"]; }
            set { ViewState["UploadPage"] = value; }
        }
    }
}