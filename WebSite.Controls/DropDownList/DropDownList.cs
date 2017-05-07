using System;
using System.ComponentModel;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MSSQLDB;
using NetServ.Net.Json;

[assembly: WebResource("WebSite.Controls.DropDownList.DropList.js", "application/x-javascript", PerformSubstitution = true)]
[assembly: WebResource("WebSite.Controls.DropDownList.DropDownList.gif", "image/gif")]
namespace WebSite.Controls
{
    public class DropDownList : System.Web.UI.WebControls.TextBox
    {
        protected override void OnInit(EventArgs e)
        {
            if (!DesignMode)
            {
                if (Page.Items["__DropDownList"] == null)
                {
                    HtmlGenericControl js = new HtmlGenericControl("script");
                    js.Attributes["type"] = "text/javascript";
                    js.Attributes["src"] = Page.ClientScript.GetWebResourceUrl(GetType(), "WebSite.Controls.DropDownList.DropList.js");
                    Page.Header.Controls.Add(js);
                    Page.Items["__DropDownList"] = true;
                }
            }
            base.OnInit(e);
        }
        protected override void Render(HtmlTextWriter writer)
        {
            if (!DesignMode)
            {
                writer.Write("<div style=\"position:relative;z-index:9;float:left;\">");
                if (!string.IsNullOrEmpty(Sql))
                {
                    DataTable ds = db.ExecuteDataTable(Sql);
                    JsonArray ja = new JsonArray();
                    AddNode(ds, ja, "0");

                    JsonObject j = new JsonObject();
                    j.Add("id", 0);
                    j.Add("open", "true");
                    j.Add("name", "顶级栏目");
                    j.Add("children", ja);

                    JsonArray _ja = new JsonArray();
                    _ja.Add(j);

                    JsonWriter jw = new JsonWriter();
                    _ja.Write(jw);
                    this.Attributes.Add("json", jw.ToString());
                }
                this.ReadOnly = true;
                this.Style.Add("display", "none");
                base.Render(writer);

                TextBox txt = new TextBox();
                txt.Text = this.TextValue;
                txt.ReadOnly = true;
                txt.Width = this.Width;
                txt.Height = this.Height;
                txt.CssClass = this.CssClass;
                txt.ID = this.ClientID + "_txt";
                txt.Attributes["style"] = this.Attributes["style"];
                txt.Attributes.Add("onfocus", "this.blur();");
                txt.Style.Remove("display");
                txt.RenderControl(writer);

                writer.Write("<script type=\"text/javascript\">");
                writer.Write("window.addEvent('domready', function() {");
                writer.Write(string.Format("var {0}=new DropList({{container:'{0}',multiple:{1},parent:{2},autopostback:{3}}});",
                    this.ClientID,
                    Convert.ToString(Multiple).ToLower(),
                    Convert.ToString(ChkParent).ToLower(),
                    this.AutoPostBack.ToString().ToLower()));
                writer.Write("});");
                writer.Write("</script>");
                writer.Write("</div>");
            }
            else
            {
                this.Text = this.ClientID;
                this.Style.Add("border", "1px solid #ADD2DA");
                this.Style.Add("background", "url(" + Page.ClientScript.GetWebResourceUrl(GetType(), "WebSite.Controls.DropDownList.DropDownList.gif") + ") no-repeat right");
                base.Render(writer);
            }
        }
        /// <summary>
        /// 数据值
        /// </summary>
        public string TextValue
        {
            get
            {
                Object obj = ViewState["TextValue"];
                if (obj != null) return (String)obj;
                return string.Empty;
            }
            set { ViewState["TextValue"] = value; }
        }
        /// <summary>
        /// 是否充许多选
        /// </summary>
        [Category("控件参数"), Description("是否充许多选"), DefaultValue(false)]
        public Boolean Multiple
        {
            get
            {
                Object obj = ViewState["Multiple"];
                if (obj != null) return (Boolean)obj;
                return false;
            }
            set { ViewState["Multiple"] = value; }
        }
        /// <summary>
        /// 是否允许选择父节点
        /// </summary>
        [Category("控件参数"), Description("是否允许选择父节点"), DefaultValue(false)]
        public bool ChkParent
        {
            get
            {
                Object obj = ViewState["ChkParent"];
                if (obj != null) return (Boolean)obj;
                return false;
            }
            set { ViewState["ChkParent"] = value; }
        }
        /// <summary>
        /// 数据源SQL语句
        /// </summary>
        [Category("控件参数"), Description("数据源SQL语句"), DefaultValue("")]
        public string Sql
        {
            get
            {
                Object obj = ViewState["Sql"];
                if (obj != null) return (String)obj;
                return string.Empty;
            }
            set { ViewState["Sql"] = value; }
        }
        /// <summary>
        /// 绑定文本字段
        /// </summary>
        [Category("控件参数"), Description("绑定文本字段"), DefaultValue("")]
        public string DataTextField
        {
            get
            {
                Object obj = ViewState["DataTextField"];
                if (obj != null) return (String)obj;
                return string.Empty;
            }
            set { ViewState["DataTextField"] = value; }
        }
        /// <summary>
        /// 绑定值字段
        /// </summary>
        [Category("控件参数"), Description("绑定值字段"), DefaultValue("")]
        public string DataValueField
        {
            get
            {
                Object obj = ViewState["DataValueField"];
                if (obj != null) return (String)obj;
                return string.Empty;
            }
            set { ViewState["DataValueField"] = value; }
        }
        /// <summary>
        /// 树型JSON结构处理
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="json"></param>
        /// <param name="ID"></param>
        private void AddNode(DataTable ds, JsonArray json, string ID)
        {
            DataRow[] rows = ds.Select("pid=" + ID);
            foreach (DataRow dr in rows)
            {
                string _id = dr["id"].ToString();
                JsonObject j = new JsonObject();
                j.Add("id", _id);
                j.Add("name", dr["name"].ToString());
                JsonArray arr = new JsonArray();
                AddNode(ds, arr, _id);
                if (arr.Count > 0)
                {
                    j.Add("open", "true");
                    j.Add("children", arr);
                }
                json.Add(j);
            }
        }
    }
}
