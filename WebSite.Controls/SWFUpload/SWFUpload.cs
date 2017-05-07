using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

[assembly: WebResource("WebSite.Controls.SWFUpload.swfupload.swf", "application/x-shockwave-flash")]
[assembly: WebResource("WebSite.Controls.SWFUpload.SWFUpload.js", "application/x-javascript", PerformSubstitution = true)]
[assembly: WebResource("WebSite.Controls.SWFUpload.upload.gif", "image/gif")]
[assembly: WebResource("WebSite.Controls.SWFUpload.uploadBtn.png", "image/png")]
[assembly: WebResource("WebSite.Controls.SWFUpload.ProgressBar.gif", "image/gif")]
namespace WebSite.Controls
{
    /// <summary>
    /// 上传控件
    /// </summary>
    public class SWFUpload : System.Web.UI.WebControls.TextBox
    {
        protected override void OnInit(EventArgs e)
        {
            if (!DesignMode)
            {
                if (Page.FindControl("swfupload") == null)
                {
                    var src = Page.ClientScript.GetWebResourceUrl(GetType(), "WebSite.Controls.SWFUpload.SWFUpload.js");
                    if (Page.Header != null)
                    {
                        HtmlGenericControl js = new HtmlGenericControl("script");
                        js.ID = "swfupload";
                        js.Attributes["type"] = "text/javascript";
                        js.Attributes["src"] = src;
                        Page.Header.Controls.Add(js);
                    }
                    else// if (!this.Page.ClientScript.IsClientScriptIncludeRegistered("swfupload"))
                    {
                        this.Page.ClientScript.RegisterClientScriptInclude("swfupload", src);
                    }
                }
            }
            base.OnInit(e);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (DesignMode)
            {
                this.Text = this.ClientID;
                this.Style.Add("border", "1px solid #ADD2DA");
                this.Style.Add("background", "url(" + Page.ClientScript.GetWebResourceUrl(GetType(), "WebSite.Controls.SWFUpload.upload.gif") + ") no-repeat right");
                base.Render(writer);
            }
            else
            {
                this.Attributes.Add("onfocus", "this.blur();");
                this.ReadOnly = true;
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
                flashvars.Add("fileTypes", this.FileTypes);
                flashvars.Add("fileTypesDescription", this.FileTypesDescription);
                flashvars.Add("fileSizeLimit", this.FileSizeLimit);
                flashvars.Add("fileUploadLimit", this.FileUploadLimit);
                flashvars.Add("fileQueueLimit", this.FileQueueLimit);
                #endregion

                #region 按钮设置
                flashvars.Add("debugEnabled", this.debugEnabled);
                flashvars.Add("buttonImageURL", this.buttonImage);
                flashvars.Add("buttonWidth", "61");
                flashvars.Add("buttonHeight", "22");
                flashvars.Add("buttonText", this.buttonText);
                flashvars.Add("buttonTextTopPadding", this.buttonTextTopPadding);
                flashvars.Add("buttonTextLeftPadding", this.buttonTextLeftPadding);
                flashvars.Add("buttonTextStyle", this.buttonTextStyle);
                flashvars.Add("buttonAction", (int)this.buttonAction);
                flashvars.Add("buttonDisabled", this.buttonDisabled);
                flashvars.Add("buttonCursor", this.buttonCursor);
                #endregion

                List<string> vars = new List<string>();
                foreach (string s in flashvars.Keys)
                {
                    vars.Add(string.Format("{0} : '{1}'", s, HttpUtility.UrlEncode(flashvars[s].ToString())));
                }

                writer.WriteBeginTag("table");
                writer.WriteAttribute("border", "0");
                writer.WriteAttribute("cellpadding", "0");
                writer.WriteAttribute("cellspacing", "0");
                //writer.WriteAttribute("width", "100%");
                writer.Write(">");
                writer.WriteFullBeginTag("tr");

                writer.WriteFullBeginTag("td");
                writer.Write("<div style=\"position:relative;z-index:0;float:left;\">");
                base.Render(writer);
                writer.Write("</div>");
                writer.WriteEndTag("td");

                writer.WriteBeginTag("td");
                writer.WriteAttribute("style", "width:35px;text-align:right;");
                writer.Write(">");
                //writer.WriteBeginTag("span");
                //writer.WriteAttribute("style", "width:35px;text-align:center;position:relative;display:inline-block;");
                //writer.Write(">");

                //writer.WriteBeginTag("div");
                //writer.WriteAttribute("style", "height:20px;position:absolute;z-index:1;left:0;top:2px;");
                //writer.Write(">");style=\"width:60px; height:30px; display:block;\" 
                writer.Write(string.Format("<div id=\"{0}_target\">", this.ClientID));
                writer.Write("<script type=\"text/javascript\">");
                writer.Write("window.addEvent('domready', function() {");
                writer.Write(string.Format("new SWFUpload('{0}?r=0.{1}', {{ id:'{2}', width:'30',height:'19',container:'{2}_target',properties:{{'class':'swfupload'}}, vars:{{ {3} }} }});",
                new object[] {
                        Page.ClientScript.GetWebResourceUrl(GetType(), "WebSite.Controls.SWFUpload.swfupload.swf"),
                        DateTime.Now.Ticks,
                        this.ClientID,
                        string.Join(",", vars.ToArray())
                }));
                writer.Write("});");
                writer.WriteEndTag("script");
                writer.WriteEndTag("div");
                //writer.WriteEndTag("div");

                //writer.Write("上传");
                //writer.WriteEndTag("span");
                writer.WriteEndTag("td");

                //if (SelectButton)
                //{
                //    writer.WriteBeginTag("td");
                //    writer.WriteAttribute("valign", "middle");
                //    writer.Write(">");
                //    writer.WriteBeginTag("a");
                //    //writer.WriteAttribute("rel", "popup");
                //    //writer.WriteAttribute("title", "选择文件");
                //    writer.WriteAttribute("href", String.Format("javascript:BrowseFile('{0}','{1}','{2}');", HttpContext.Current.Server.UrlEncode(this.FileTypes), (Int32)this.buttonAction, this.ClientID));
                //    //string.Format("SiteFile.aspx?clientid={0}&TB_iframe=true&width=520&height=350", this.ClientID));
                //    writer.Write(">");
                //    writer.Write("选择");
                //    writer.WriteEndTag("a");
                //    writer.WriteEndTag("td");
                //}
                writer.WriteEndTag("tr");
                writer.WriteEndTag("table");
            }
        }
        //protected override HtmlTextWriterTag TagKey
        //{
        //    get
        //    {
        //        return HtmlTextWriterTag.Object;
        //    }
        //}
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override bool ReadOnly
        {
            get
            {
                return base.ReadOnly;
            }
            set
            {
                base.ReadOnly = value;
            }
        }
        /// <summary>
        /// 文件类型
        /// </summary>
        [Category("控件参数"), Description("文件类型(*.*)."), DefaultValue("")]
        public string FileTypes
        {
            get { return ViewState["FileTypes"] == null ? "*.jpg;*.jpeg;*.png;*.bmp;*.gif" : (String)ViewState["FileTypes"]; }
            set { ViewState["FileTypes"] = value; }
        }
        /// <summary>
        /// 文件类型描述
        /// </summary>
        [Category("控件参数"), Description("文件类型描述."), DefaultValue("")]
        public string FileTypesDescription
        {
            get { return ViewState["FileTypesDescription"] == null ? "图片文件" : (String)ViewState["FileTypesDescription"]; }
            set { ViewState["FileTypesDescription"] = value; }
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
        /// 上传文件数量限制
        /// </summary>
        [Category("控件参数"), Description("上传文件数量限制."), DefaultValue(0)]
        public int FileUploadLimit
        {
            get { return ViewState["FileUploadLimit"] == null ? 0 : (int)ViewState["FileUploadLimit"]; }
            set { ViewState["FileUploadLimit"] = value; }
        }
        /// <summary>
        /// 文件队列限制
        /// </summary>
        [Category("控件参数"), Description("文件队列限制."), DefaultValue(0)]
        public int FileQueueLimit
        {
            get { return ViewState["FileQueueLimit"] == null ? 0 : (int)ViewState["FileQueueLimit"]; }
            set { ViewState["FileQueueLimit"] = value; }
        }
        /// <summary>
        /// 按钮图片地址
        /// </summary>
        [Category("按钮参数"), Description("按钮图片地址."), DefaultValue("")]
        public string buttonImage
        {
            get
            {
                //string img = Page.ClientScript.GetWebResourceUrl(GetType(), "WebSite.Controls.SWFUpload.uploadBtn.png");
                return ViewState["buttonImage"] == null ? "" : (String)ViewState["buttonImage"];
            }
            set { ViewState["buttonImage"] = value; }
        }
        /// <summary>
        /// 按钮文本
        /// </summary>
        [Category("按钮参数"), Description("按钮文本."), DefaultValue("")]
        public string buttonText
        {
            get { return ViewState["buttonText"] == null ? "上传" : (String)ViewState["buttonText"]; }
            set { ViewState["buttonText"] = value; }
        }
        /// <summary>
        /// 按钮上边距
        /// </summary>
        [Category("按钮参数"), Description("按钮上边距."), DefaultValue(0)]
        public int buttonTextTopPadding
        {
            get { return ViewState["buttonTextTopPadding"] == null ? 0 : (int)ViewState["buttonTextTopPadding"]; }
            set { ViewState["buttonTextTopPadding"] = value; }
        }
        /// <summary>
        /// 按钮左边距
        /// </summary>
        [Category("按钮参数"), Description("按钮左边距."), DefaultValue(0)]
        public int buttonTextLeftPadding
        {
            get { return ViewState["buttonTextLeftPadding"] == null ? 0 : (int)ViewState["buttonTextLeftPadding"]; }
            set { ViewState["buttonTextLeftPadding"] = value; }
        }
        /// <summary>
        /// 按钮样式设置
        /// </summary>
        [Category("按钮参数"), Description("按钮样式设置."), DefaultValue("")]
        public string buttonTextStyle
        {
            get { return ViewState["buttonTextStyle"] == null ? "font-size: 16pt;" : (String)ViewState["buttonTextStyle"]; }
            set { ViewState["buttonTextStyle"] = value; }
        }
        /// <summary>
        /// 按钮事件
        /// </summary>       
        [Category("按钮参数"), Description("按钮事件."), DefaultValue(0)]
        public ButtonAction buttonAction
        {
            get
            {
                object obj = ViewState["buttonAction"];
                if (obj != null) return (ButtonAction)obj;
                return ButtonAction.SELECT_FILE;
            }
            set { ViewState["buttonAction"] = value; }
        }
        /// <summary>
        /// 鼠标指针样式
        /// </summary>
        [Category("按钮参数"), Description("鼠标指针样式."), DefaultValue("")]
        public string buttonCursor
        {
            get { return ViewState["buttonCursor"] == null ? "-2" : (String)ViewState["buttonCursor"]; }
            set { ViewState["buttonCursor"] = value; }
        }
        /// <summary>
        /// 禁用按钮
        /// </summary>
        [Category("按钮参数"), Description("禁用按钮."), DefaultValue(false)]
        public bool buttonDisabled
        {
            get { return ViewState["buttonDisabled"] == null ? false : (Boolean)ViewState["buttonDisabled"]; }
            set { ViewState["buttonDisabled"] = value; }
        }
        /// <summary>
        /// 选择按钮
        /// </summary>
        [Category("按钮参数"), Description("显示选择文件."), DefaultValue(false)]
        public bool SelectButton
        {
            get { return ViewState["SelectButton"] == null ? false : (Boolean)ViewState["SelectButton"]; }
            set { ViewState["SelectButton"] = value; }
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
        [Category("上传参数"), Description("上传文件接收关键字."), DefaultValue("")]
        public string FilePostName
        {
            get { return ViewState["FilePostName"] == null ? "Filedata" : (String)ViewState["FilePostName"]; }
            set { ViewState["FilePostName"] = value; }
        }
        /// <summary>
        /// 禁用调试
        /// </summary>
        [Category("调试设置"), Description("禁用调试."), DefaultValue(false)]
        public bool debugEnabled
        {
            get { return ViewState["debugEnabled"] == null ? false : (Boolean)ViewState["debugEnabled"]; }
            set { ViewState["debugEnabled"] = value; }
        }
    }
}
