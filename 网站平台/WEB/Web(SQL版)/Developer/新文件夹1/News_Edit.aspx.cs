using System;
using System.Collections.Generic;
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
using WebSite.Core;
using WebSite.Models;

namespace WebSite.Web
{
    public partial class News_Edit : BaseAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                #region 权限读取

                List<Int32> nodes = new List<Int32>();
                var list = new List<T_AccessControlEntity>();
                if (IsEdit)
                {
                    list = Access.FindAll(a => { return Admin.IsSuper | ActionTypeHelper.IsCreat(a.ActionType) | ActionTypeHelper.IsEdit(a.ActionType); });
                }
                else
                {
                    list = Access.FindAll(a => { return Admin.IsSuper | ActionTypeHelper.IsCreat(a.ActionType); });
                }
                foreach (var item in list)
                {
                    nodes.Add(item.Node);
                }

                #endregion

                #region 绑定分类数据

                ListItem root = new ListItem("选择分类", "0");
                root.Selected = true;
                GroupId.Items.Add(root);
                List<T_GroupEntity> treenodes = T_GroupHelper.Groups.FindAll(a => { return a.TableName == "t_news" & (Admin.IsSuper || nodes.Contains(a.ID)); });
                foreach (var item in treenodes)
                {
                    ListItem el = new ListItem(item.GroupName, item.ID.ToString());
                    el.Attributes.Add("pid", item.ParentID.ToString());
                    GroupId.Items.Add(el);
                }

                #endregion

                if (IsEdit)
                {
                    LoadData();
                }
                else
                {
                    //通过IE右键收藏网页数据
                    if (Request["by"] == "ie")
                    {
                        @title.Text = HttpUtility.UrlDecode(Request["title"]);
                        @content.Text = HttpUtility.UrlDecode(Request["content"]);
                        @Url.Value = HttpUtility.UrlDecode(Request["url"]);
                    }
                }
            }
        }
        /// <summary>
        /// 分类权限表
        /// </summary>
        private List<T_AccessControlEntity> Access
        {
            get
            {
                return Admin.AccessControl.FindAll(a => { return a.TableName == "t_group"; });
            }
        }
        /// <summary>
        /// 是否为编辑模式
        /// </summary>
        protected Boolean IsEdit
        {
            get { return EditID > 0; }
        }
        /// <summary>
        /// 编辑ID
        /// </summary>
        protected Int32 EditID
        {
            get
            {
                Int32 id = 0;
                Int32.TryParse(Request["id"], out id);
                return id;
            }
        }
        /// <summary>
        /// 加载编辑数据
        /// </summary>
        protected void LoadData()
        {
            String sql = String.Format("SELECT a.*,b.groupname FROM [T_News] as a LEFT JOIN [T_Group] as b ON a.groupid = b.ID WHERE a.ID={0}", EditID);
            T_NewsEntity data = db.ExecuteObject<T_NewsEntity>(sql);
            ViewState["data"] = data;
            this.SetFormValue(data);

            T_AccessControlEntity acl = Access.Find(a => { return a.Node == data.GroupId; });
            if (!Admin.IsSuper && !ActionTypeHelper.IsSetting(acl.ActionType))
            {
                nominate.Enabled = hotspot.Enabled = focus.Enabled = stick.Enabled = status.Enabled = false;
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            T_NewsEntity data = (T_NewsEntity)ViewState["data"];
            data = this.GetFormValue<T_NewsEntity>(data);
            CmdType cmd = IsEdit ? CmdType.UPDATE : CmdType.INSERT;
            data.EditDate = DateTime.Now;
            if (!IsEdit) data.PubDate = DateTime.Now;

            T_AccessControlEntity acl = Access.Find(a => { return a.Node == data.GroupId; });
            if (ActionTypeHelper.IsCreat(acl.ActionType) || (IsEdit && ActionTypeHelper.IsEdit(acl.ActionType)))
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