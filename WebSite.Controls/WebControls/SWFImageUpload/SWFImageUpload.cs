using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

[assembly: WebResource("WebSite.Controls.WebControls.SWFImageUpload.SWFImageUpload.js", "application/x-javascript", PerformSubstitution = true)]
namespace WebSite.Controls
{
    /// <summary>
    /// SWFUpload上传图片控件
    /// </summary>
    [ToolboxData("<{0}:SWFImageUpload runat=server />")]
    public class SWFImageUpload : WebSite.Controls.HiddenField
    {
        protected override void OnInit(EventArgs e)
        {
            if (!DesignMode)
            {
                if (Page.FindControl("swfimageupload") == null)
                {
                    var src = Page.ClientScript.GetWebResourceUrl(GetType(), "WebSite.Controls.WebControls.SWFImageUpload.SWFImageUpload.js");
                    if (Page.Header != null)
                    {
                        HtmlGenericControl js = new HtmlGenericControl("script");
                        js.ID = "swfimageupload";
                        js.Attributes["type"] = "text/javascript";
                        js.Attributes["src"] = src;
                        Page.Header.Controls.Add(js);
                    }
                    else// if (!this.Page.ClientScript.IsClientScriptIncludeRegistered("swfupload"))
                    {
                        Page.ClientScript.RegisterClientScriptInclude("swfimageupload", src);
                    }
                }
            }
            base.OnInit(e);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (DesignMode)
            {
                //this.Text = this.ClientID;
                //this.Style.Add("border", "1px solid #ADD2DA");
                //this.Style.Add("background", "url(" + Page.ClientScript.GetWebResourceUrl(GetType(), "WebSite.Controls.SWFUpload.upload.gif") + ") no-repeat right");
                base.Render(writer);
            }
            else
            {
                Hashtable flashvars = new Hashtable();
                #region 上传后台设置
                flashvars.Add("uploadURL", this.uploadURL);
                flashvars.Add("useQueryString", "false");
                flashvars.Add("requeueOnError", "false");
                flashvars.Add("httpSuccess", "");
                flashvars.Add("assumeSuccessTimeout", "0");
                flashvars.Add("params", this.param);
                flashvars.Add("filePostName", this.FilePostName);
                #endregion

                #region 文件设置
                flashvars.Add("fileTypes", "*.jpg;*.jpeg;*.png;*.bmp;*.gif");
                flashvars.Add("fileTypesDescription", "图片文件");
                flashvars.Add("fileSizeLimit", this.FileSizeLimit);
                flashvars.Add("fileUploadLimit", 0);
                flashvars.Add("fileQueueLimit", 0);
                #endregion

                #region 按钮设置
                flashvars.Add("debugEnabled", false);
                flashvars.Add("buttonImageURL", "");
                flashvars.Add("buttonWidth", "61");
                flashvars.Add("buttonHeight", "22");
                flashvars.Add("buttonText", "");
                flashvars.Add("buttonTextTopPadding", 0);
                flashvars.Add("buttonTextLeftPadding", 0);
                flashvars.Add("buttonTextStyle", "");
                flashvars.Add("buttonAction", (int)ButtonAction.SELECT_FILE);
                flashvars.Add("buttonDisabled", false);
                flashvars.Add("buttonCursor", -2);
                #endregion

                List<string> vars = new List<string>();
                foreach (string s in flashvars.Keys)
                {
                    vars.Add(string.Format("{0} : '{1}'", s, HttpUtility.UrlEncode(flashvars[s].ToString())));
                }

                Image img = new Image();
                img.ImageUrl = "/developer/skin/transparent.gif";
                String url = "about:blank";
                if (!String.IsNullOrEmpty(this.Value))
                {
                    url = String.Format("/Developer/Plugin-S/Thumbnail.aspx?w={0}&h={1}&file={2}", this.Width.Value, this.Height.Value, this.Value);
                }
                img.Style.Add("background", "url(" + url + ") no-repeat center center;");

                writer.WriteBeginTag("div");
                writer.WriteAttribute("id", this.ClientID + "_holder");
                writer.WriteAttribute("style", string.Format("width:{0};height:{1};position:relative;overflow:hidden;", Width, Height));
                writer.Write(">");

                img.RenderControl(writer);
                base.Render(writer);

                writer.Write("<script type=\"text/javascript\">");
                writer.Write("window.addEvent('domready', function() {");
                writer.Write(string.Format("new SWFImageUpload('{0}?r=0.{1}', {{ id:'{2}',vars:{{ {3} }} }});",
                new object[] {
                        Page.ClientScript.GetWebResourceUrl(GetType(), "WebSite.Controls.SWFUpload.swfupload.swf"),
                        DateTime.Now.Ticks,
                        this.ClientID,
                        string.Join(",", vars.ToArray())
                }));
                writer.Write("});");
                writer.WriteEndTag("script");

                writer.WriteEndTag("div");
            }
        }

        public Unit Width
        {
            get { return ViewState["Width"] == null ? 100 : (Unit)ViewState["Width"]; }
            set { ViewState["Width"] = value; }
        }
        public Unit Height
        {
            get { return ViewState["Height"] == null ? 100 : (Unit)ViewState["Height"]; }
            set { ViewState["Height"] = value; }
        }

        /// <summary>
        /// 文件大小限制
        /// </summary>
        [Category("控件参数"), Description("文件大小限制(默认1MB)."), DefaultValue("")]
        public string FileSizeLimit
        {
            get { return ViewState["FileSizeLimit"] == null ? "1M" : (String)ViewState["FileSizeLimit"]; }
            set { ViewState["FileSizeLimit"] = value; }
        }
        /// <summary>
        /// 上传文件处理接口
        /// </summary>
        [Category("上传参数"), UrlProperty, Description("上传文件处理接口."), DefaultValue("")]
        public string uploadURL
        {
            get { return ViewState["uploadURL"] == null ? "/Upload.axd" : (String)ViewState["uploadURL"]; }
            set { ViewState["uploadURL"] = value; }
        }
        /// <summary>
        /// 上传时传递参数信息
        /// </summary>
        [Category("上传参数"), Description("上传时传递参数信息."), DefaultValue("")]
        public string param
        {
            get { return ViewState["param"] == null ? "" : (String)ViewState["param"]; }
            set { ViewState["param"] = value; }
        }
        /// <summary>
        /// 上传文件接收关键字
        /// </summary>
        [Category("上传参数名称"), Description("上传文件接收关键字."), DefaultValue("")]
        public string FilePostName
        {
            get { return ViewState["FilePostName"] == null ? "Filedata" : (String)ViewState["FilePostName"]; }
            set { ViewState["FilePostName"] = value; }
        }
    }
}
