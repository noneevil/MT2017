using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using CommonUtils;
using HtmlAgilityPack;
using MSSQLDB;
using ScrapySharp.Extensions;
using WebSite.BackgroundPages;
using WebSite.Models;

namespace WebSite.Web
{
    public partial class News_Edit : BaseAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();

                if (IsEdit)
                {
                    LoadData();
                }
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        protected void BindData()
        {
            //绑定阅读权限
            readaccess.DataSource = Enum.GetValues(typeof(MemberType));
            readaccess.DataBind();

            #region 读取分类

            String strSql = String.Format("SELECT a.id,a.groupname,a.parentid,b.actiontype FROM [T_Group] a INNER JOIN [T_AccessControl] b ON a.id = b.node WHERE a.tablename='t_news' AND b.tablename = 't_group' AND b.actiontype<>0 AND b.role={0}", Admin.RoleID);
            DataTable dt = db.ExecuteDataTable(strSql);
            dt.PrimaryKey = new DataColumn[] { dt.Columns["id"] };
            DataTable ds = dt.Clone();
            foreach (DataRow dr in dt.Rows)
            {
                if (!Admin.IsSuper)
                {
                    if (!chkActionType(dr, dt)) continue;
                    //父节点权限检查
                    Boolean flag = false;
                    Int32 parentid = (Int32)dr["parentid"];
                    while (true)
                    {
                        if (parentid == 0) break;
                        DataRow row = dt.Rows.Find(parentid);
                        if (row == null || !chkActionType(row, dt))
                        {
                            //无权限
                            flag = true;
                            break;
                        }
                        parentid = (Int32)row["parentid"];
                    }
                    if (flag) continue;
                }
                ds.ImportRow(dr);
            }
            //绑定分类
            ListItem root = new ListItem("选择分类", "0");
            root.Selected = true;
            groupid.Items.Add(root);

            foreach (DataRow dr in ds.Rows)
            {
                ListItem node = new ListItem(dr["groupname"].ToString(), dr["id"].ToString());
                node.Attributes.Add("pid", dr["parentid"].ToString());
                groupid.Items.Add(node);
            }

            #endregion

            //通过IE右键收藏网页数据
            if (Request["by"] == "ie")
            {
                @title.Text = HttpUtility.UrlDecode(Request["title"]);
                @content.Text = HttpUtility.UrlDecode(Request["content"]);
                @Url.Value = HttpUtility.UrlDecode(Request["url"]);
            }
        }
        /// <summary>
        /// 加载编辑数据
        /// </summary>
        protected void LoadData()
        {
            String strSql = String.Format("SELECT a.*,b.groupname FROM [T_News] as a LEFT JOIN [T_Group] as b ON a.groupid = b.ID WHERE a.ID={0}", EditID);
            T_NewsEntity data = db.ExecuteObject<T_NewsEntity>(strSql);
            ViewState["data"] = data;
            this.SetFormValue(data);

            T_AccessControlEntity acl = GetGroupAction(data.GroupId);
            if (acl == null || !acl.ActionType.HasFlag(ActionType.Edit))
            {
                form1.Visible = false;
                btnSave.Visible = false;
            }
            if (acl != null && !acl.ActionType.HasFlag(ActionType.Setting))
            {
                isnominate.Enabled = ishotspot.Enabled = isslide.Enabled = isstick.Enabled = isenable.Enabled = isaudit.Enabled = iscomments.Enabled = readaccess.Enabled = false;
            }
        }
        /// <summary>
        /// 节点权限检查
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        private Boolean chkActionType(DataRow dr, DataTable dt)
        {
            ActionType value = (ActionType)Enum.Parse(typeof(ActionType), dr["actiontype"].ToString());
            DataRow[] childs = dt.Select("parentid=" + dr["id"].ToString());
            if (childs.Length > 0)
            {
                return value.HasFlag(ActionType.View);
            }

            if (IsEdit)
            {
                return value.HasFlag(ActionType.Create) | value.HasFlag(ActionType.Edit);
            }
            else
            {
                return value.HasFlag(ActionType.Create);
            }
        }
        /// <summary>
        /// 获取分类权限
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        private T_AccessControlEntity GetGroupAction(Int32 groupid)
        {
            String strSql = String.Format("SELECT * FROM [T_AccessControl] WHERE tablename='t_group' AND node={0} AND role={1}", groupid, Admin.RoleID);
            return db.ExecuteObject<T_AccessControlEntity>(strSql);
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (readaccess.SelectedIndex == -1) readaccess.SelectedIndex = 0;

            T_NewsEntity data = (T_NewsEntity)ViewState["data"];
            data = this.GetFormValue<T_NewsEntity>(data);
            CmdType cmd = IsEdit ? CmdType.UPDATE : CmdType.INSERT;
            data.EditDate = DateTime.Now;
            if (!IsEdit) data.PubDate = DateTime.Now;

            T_AccessControlEntity acl = GetGroupAction(data.GroupId);
            if (acl.ActionType.HasFlag(ActionType.Create) || acl.ActionType.HasFlag(ActionType.Edit))
            {
                #region 网络图片采集

                HtmlDocument xml = new HtmlDocument();
                xml.LoadHtml(data.Content);

                var html = xml.DocumentNode;
                var nodes = html.CssSelect("img[src^='http:']");
                foreach (HtmlNode n in nodes)
                {
                    HtmlAttribute src = n.Attributes["src"];
                    src.Value = DownloadImage(src.Value);
                }
                data.Content = xml.DocumentNode.WriteTo();
                content.Text = data.Content;

                #endregion

                if (db.ExecuteCommand<T_NewsEntity>(data, cmd))
                {
                    if (!IsEdit) this.ClearFromValue();
                    Alert(Label1, "保存成功！", "line1px_3");
                }
            }
        }
        /// <summary>
        /// 采集图片
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        protected String DownloadImage(String src)
        {
            if (String.IsNullOrEmpty(src)) return "";
            Uri url = new Uri(src);
            String outfile = String.Empty;

            System.Net.WebClient web = new System.Net.WebClient();
            web.Headers.Add("Accept", "image/png, image/svg+xml, image/*;q=0.8, */*;q=0.5");
            web.Headers.Add("Referer", String.IsNullOrEmpty(@Url.Value) ? url.Host : @Url.Value);
            web.Headers.Add("User-Agent", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/6.0)");
            try
            {
                Byte[] data = web.DownloadData(url);
                using (MemoryStream ms = new MemoryStream(data))
                {
                    Bitmap img = new Bitmap(ms);
                    ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
                    ImageCodecInfo format = encoders[1];//0-bmp 1-jpg 2-gif 3-tif 4-png
                    EncoderParameters ep = new EncoderParameters(1);
                    ep.Param[0] = new EncoderParameter(Encoder.Quality, 80L);

                    String Folder = Path.Combine(Server.MapPath("/UpFiles/"), DateTime.Now.ToString("yyyy-MM-dd"));
                    FileHelper.CreatePath(Folder);
                    Int32 count = new DirectoryInfo(Folder).GetFiles().Length + 1;
                    String filename = Path.Combine(Folder, DateTime.Now.ToString("yyyyMMdd") + count.ToString("00000") + ".jpg");

                    img.Save(filename, format, ep);
                    img.Dispose();

                    outfile = "/" + new Uri(Server.MapPath("/")).MakeRelativeUri(new Uri(filename)).ToString();
                }
            }
            catch { }
            finally
            {
                web.Dispose();
            }
            return outfile;
        }
    }
}