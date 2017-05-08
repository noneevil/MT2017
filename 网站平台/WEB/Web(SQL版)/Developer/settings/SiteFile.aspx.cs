using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CommonUtils;
using Newtonsoft.Json;
using WebSite.BackgroundPages;
using WebSite.Interface;
using WebSite.Models;

namespace WebSite.Web
{
    public partial class SiteFile : BaseAdmin
    {
        /// <summary>
        /// 后退地址
        /// </summary>
        protected String BackUrl
        {
            get
            {
                return new Uri(DestFolder.FullName).MakeRelativeUri(RootPath).ToString();
            }
        }
        /// <summary>
        /// 相对站点地址
        /// </summary>
        protected String RelativeUrl
        {
            get
            {
                return "/" + RootPath.MakeRelativeUri(new Uri(DestFolder.FullName)).ToString();
            }
        }
        /// <summary>
        /// 当前文件夹
        /// </summary>
        protected DirectoryInfo DestFolder
        {
            get
            {
                String dir = Server.MapPath(String.IsNullOrEmpty(Request["dir"]) ? "/" : Request["dir"]);
                return new DirectoryInfo(dir);
            }
        }
        /// <summary>
        /// 显示模式
        /// </summary>
        protected DisplayMode displayMode
        {
            get
            {
                String val = CookieHelper.GetValue(ISessionKeys.cookie_displaymode);
                val = val ?? "1";
                return (DisplayMode)Enum.Parse(typeof(DisplayMode), val);
            }
        }
        /// <summary>
        /// 文件类型
        /// </summary>
        protected String FileType
        {
            get
            {
                String type = CookieHelper.GetValue(ISessionKeys.cookie_filetype);
                type = type ?? "*.*";
                return type;
            }
        }
        /// <summary>
        /// 站点根目录
        /// </summary>
        protected Uri RootPath
        {
            get
            {
                return new Uri(Server.MapPath("/"));
            }
        }
        /// <summary>
        /// 图标集合
        /// </summary>
        protected List<String> icons
        {
            get
            {
                Object obj = Cache[ISessionKeys.cache_filesicos];
                if (obj != null) return (List<String>)obj;
                List<String> list = new List<String>();
                foreach (FileInfo item in new DirectoryInfo(Server.MapPath("/Developer/skin/files")).GetFiles("*.*"))
                {
                    String name = item.Name.Substring(0, item.Name.IndexOf("_")).ToLower();
                    if (list.Contains(name)) continue;
                    list.Add(name);
                }
                Cache[ISessionKeys.cache_filesicos] = list;
                return list;
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                switch (Request["action"])
                {
                    case "cut":
                    case "copy":
                        CutOrCopyFiles();
                        break;
                    case "upload"://flash上传文件
                        MultiView1.ActiveViewIndex = 1;
                        FlashUpload1.QueryParameters = "dir=" + Server.UrlEncode(Request["dir"]);
                        break;
                    case "add"://新建文件夹
                        MultiView1.ActiveViewIndex = 2;
                        txtPath.Text = Request["dir"];
                        break;
                    case "unrar"://解压文件
                        MultiView1.ActiveViewIndex = 3;
                        txtToPath.Text = Request["dir"];
                        List<FileInfo> rarlist = new List<FileInfo>();
                        DirectoryInfo rardir = new DirectoryInfo(Server.MapPath(Request["dir"]));
                        foreach (FileInfo item in rardir.GetFiles("*.rar")) rarlist.Add(item);
                        foreach (FileInfo item in rardir.GetFiles("*.7z")) rarlist.Add(item);
                        foreach (FileInfo item in rardir.GetFiles("*.zip")) rarlist.Add(item);
                        if (rarlist.Count == 0)
                        {
                            norarfile.Visible = true;
                            rartable.Visible = false;
                        }
                        else
                        {
                            DropRar.DataSource = rarlist;
                            DropRar.DataTextField = "name";
                            DropRar.DataValueField = "FullName";
                            DropRar.DataBind();
                        }
                        break;
                    case "edit"://编辑文件
                        MultiView1.ActiveViewIndex = 4;
                        String editFile = Server.MapPath("/" + Server.UrlDecode(Request["file"]));
                        using (StreamReader sr = new StreamReader(editFile, Encoding.UTF8))
                        {
                            code.Value = sr.ReadToEnd().ToString();
                        }
                        break;
                    case "rename"://重命名文件
                        MultiView1.ActiveViewIndex = 5;
                        FileInfo _rename = new FileInfo(Server.UrlDecode(Request["file"]));
                        txtsourceFileName.Text = _rename.Name;
                        break;
                    case "filetype":
                        MultiView1.ActiveViewIndex = 6;
                        break;
                    default:
                        MultiView1.ActiveViewIndex = 0;
                        BindData();
                        break;
                }
            }
        }
        /// <summary>
        /// 数据绑定
        /// </summary>
        protected void BindData()
        {
            List<FileSystemInfo> list = new List<FileSystemInfo>();
            foreach (var item in DestFolder.GetDirectories())
            {
                list.Add(item);
            }
            foreach (String k in FileType.Split(';'))
            {
                foreach (var item in DestFolder.GetFiles(k))
                {
                    list.Add(item);
                }
            }

            //list.Sort(delegate(FileSystemInfo t1, FileSystemInfo t2)
            //{
            //    return t1.Attributes.CompareTo(t2.Attributes);
            //});
            if (displayMode == DisplayMode.列表)
            {
                Repeater1.DataSource = list;
                Repeater1.DataBind();
            }
            else
            {
                Repeater2.DataSource = list;
                Repeater2.DataBind();
            }
        }
        /// <summary>
        /// 删除选中文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCommand_Click(object sender, EventArgs e)
        {
            Int32 arguments = Convert.ToInt32(Request.Form["__EVENTARGUMENT"]);

            List<FileInfo> files = new List<FileInfo>();
            RepeaterItemCollection items = (displayMode == DisplayMode.列表) ? Repeater1.Items : Repeater2.Items;
            foreach (RepeaterItem item in items)
            {
                HtmlInputCheckBox chkid = item.FindControl("id") as HtmlInputCheckBox;
                if (chkid.Checked)
                {
                    files.Add(new FileInfo(Server.UrlDecode(chkid.Value)));
                    //FileInfo file = new FileInfo(Server.UrlDecode(chkid.Value));
                }
            }
            if (files.Count > 0)
            {
                if (arguments == -1)//批量删除
                {
                    foreach (FileInfo file in files)
                    {
                        if (file.Attributes == FileAttributes.Directory)
                        {
                            DirectoryInfo dir = new DirectoryInfo(file.FullName);
                            if (dir.Exists) dir.Delete(true);
                        }
                        else if (file.Exists)
                        {
                            file.Delete();
                        }
                    }
                }
            }
            BindData();
        }
        /// <summary>
        /// 重命名文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRename_Click(object sender, EventArgs e)
        {
            String oldfile = Server.MapPath(Server.UrlDecode(Request["file"]));
            FileInfo file = new FileInfo(oldfile);
            String dest = Path.Combine(file.DirectoryName, txtdestFileName.Text);
            try
            {
                if (file.Attributes == FileAttributes.Directory)
                {
                    Directory.Move(file.FullName, dest);
                }
                else
                {
                    File.Move(file.FullName, dest);
                }
                Alert(Label1, "保存成功！", "line1px_3");
            }
            catch (Exception ex)
            {
                Alert(Label1, ex.Message, "line1px_2");
            }
        }
        /// <summary>
        /// 新建文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddFolder_Click(object sender, EventArgs e)
        {
            String folder = Path.Combine(Server.MapPath(txtPath.Text), txtFolder.Text);
            try
            {
                Directory.CreateDirectory(folder);
                Alert(Label2, "创建成功！", "line1px_3");
            }
            catch (Exception ex)
            {
                Alert(Label2, ex.Message, "line1px_2");
            }
        }
        /// <summary>
        /// 解压文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUnRar_Click(object sender, EventArgs e)
        {
            String file = DropRar.SelectedValue;
            if (String.IsNullOrEmpty(file)) return;
            String unpath = Server.MapPath(txtToPath.Text);
            try
            {
                SevenZipSharpHelper.Decompress(file, unpath);
                Alert(Label3, "解压成功！", "line1px_3");
            }
            catch (Exception ex)
            {
                Alert(Label3, ex.Message, "line1px_2");
            }
            //if (!Directory.Exists(unpath)) Directory.CreateDirectory(unpath);

            //Process Process1 = new Process();
            //Process1.StartInfo.FileName = Server.MapPath("/App_Data/Rar.exe");
            //Process1.StartInfo.Arguments = "x -t -o+ -p- " + " " + file + " " + unpath;
            //Process1.Start();
            //while (!Process1.HasExited) { }
            //if (Process1.ExitCode == 0)
            //{
            //    Alert(Label2, "文件解压成功！", "line1px_3");
            //}
            //Process1.Close();
        }
        /// <summary>
        /// 编辑文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                String file = Server.MapPath("/" + Server.UrlDecode(Request["file"]));
                using (StreamWriter sw = new StreamWriter(file, false, Encoding.UTF8))
                {
                    sw.Write(code.Value);
                    sw.Flush();
                }
                Alert(Label4, "保存成功！", "line1px_3");
            }
            catch (Exception ex)
            {
                Alert(Label4, ex.Message, "line1px_2");
            }
        }
        /// <summary>
        /// 剪切或复制文件
        /// </summary>
        private void CutOrCopyFiles()
        {
            IJsonResult result = new IJsonResult { Text = "操作失败!", Ico = MessageICO.Failure };
            String dir = String.IsNullOrEmpty(Request["dir"]) ? "/" : Request["dir"];
            try
            {
                using (StreamReader sr = new StreamReader(Request.InputStream))
                {
                    String json = sr.ReadToEnd();
                    var data = JsonConvert.DeserializeAnonymousType(json, new { cmd = "", files = new String[] { } });

                    foreach (String s in data.files)
                    {
                        FileInfo file = new FileInfo(Server.UrlDecode(s));
                        String dest = Path.Combine(Server.MapPath(dir), file.Name);
                        if (file.Attributes == FileAttributes.Directory)
                        {
                            if (data.cmd == "cut")
                            {
                                Directory.Move(file.FullName, dest);
                            }
                            else if (data.cmd == "copy")
                            {
                                FileHelper.DirectoryCopy(file.FullName, dest, true);
                            }
                        }
                        else
                        {
                            if (data.cmd == "cut")
                            {
                                File.Move(file.FullName, dest);
                            }
                            else if (data.cmd == "copy")
                            {
                                File.Copy(file.FullName, dest);
                            }
                        }
                    }
                    result.Status = true;
                    result.Ico = MessageICO.Success;
                    result.Text = data.cmd == "copy" ? "所选文件复制成功!" : "所选文件移动成功!";
                }
            }
            catch (Exception ex)
            {
                result.Text = ex.Message;
            }
            Response.Write(JsonConvert.SerializeObject(result));
            Response.End();
        }
        /// <summary>
        /// 列表事件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            String value = Server.UrlDecode(((HtmlInputCheckBox)e.Item.FindControl("ID")).Value);
            FileInfo file = new FileInfo(value);
            switch (e.CommandName)
            {
                case "del":
                    if (file.Attributes == FileAttributes.Directory)
                    {
                        Directory.Delete(file.FullName, true);
                    }
                    else
                    {
                        file.Delete();
                    }
                    Thread.Sleep(100);
                    BindData();
                    break;
                case "save":
                    using (FileStream stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        Byte[] bytes = new Byte[(int)stream.Length];
                        stream.Read(bytes, 0, bytes.Length);
                        Response.Clear();
                        Response.ContentType = "application/octet-stream";
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlPathEncode(file.Name));
                        Response.BinaryWrite(bytes);
                    }
                    Response.Flush();
                    Response.End();
                    break;
            }
        }
        /// <summary>
        /// 列表视图控件生成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                FileSystemInfo file = e.Item.DataItem as FileSystemInfo;
                String filepath = "/" + RootPath.MakeRelativeUri(new Uri(file.FullName)).ToString();

                HtmlTableRow row = (HtmlTableRow)e.Item.FindControl("row");
                ImageButton del = (ImageButton)e.Item.FindControl("del");
                ImageButton edit = (ImageButton)e.Item.FindControl("edit");
                ImageButton save = (ImageButton)e.Item.FindControl("save");
                del.OnClientClick = String.Format("javascript:dialogConfirm({{el:this,text:'将删除 {0} 且无法恢复!确定要删除吗?'}});return false;", file.Name);
                edit.OnClientClick = String.Format("javascript:dialogIFrame({{ url: 'SiteFile.aspx?action=rename&file={1}', title: '重命名 - {0}', width: 350, height: 250 }});return false;", file.Name, Server.UrlEncode(filepath));

                HtmlTableCell td1 = (HtmlTableCell)row.Controls[1];
                Image ico = new Image();
                ico.Attributes.Add("align", "absmiddle");
                ico.Attributes.Add("style", "padding-right:5px;");
                td1.Controls.Add(ico);

                HyperLink link = new HyperLink();
                link.Text = file.Name;
                link.ToolTip = file.Name;
                td1.Controls.Add(link);

                if (file.Attributes == FileAttributes.Directory)
                {
                    link.NavigateUrl = "SiteFile.aspx?dir=" + filepath;
                    ico.ImageUrl = "../skin/files/folder_small.png";
                }
                else
                {
                    save.Visible = true;
                    FileInfo _file = file as FileInfo;
                    String ex = String.IsNullOrEmpty(_file.Extension) ? "none" : _file.Extension.Trim('.');

                    HtmlTableCell td4 = row.Controls[4] as HtmlTableCell;
                    long size = _file.Length;
                    td4.InnerText = FileHelper.FileSizeToStr((long)(size));

                    String src = String.Format("../skin/files/{0}_small.png", ex);
                    if (!icons.Contains(ex)) src = "../skin/files/none.png";
                    ico.ImageUrl = src;

                    switch (ex)
                    {
                        case "gif":
                        case "jpeg":
                        case "png":
                        case "bmp":
                        case "jpg":
                            link.NavigateUrl = filepath;
                            link.Attributes.Add("rel", "group1");
                            link.Attributes.Add("class", "lightbox");
                            Image exlink = new Image();
                            exlink.ImageAlign = ImageAlign.Baseline;
                            exlink.ToolTip = "查看大图";
                            exlink.Style.Add("margin-left", "5px");
                            exlink.Attributes.Add("align", "absmiddle");
                            exlink.ImageUrl = "../skin/icos/Link.png";
                            td1.Controls.Add(exlink);
                            break;
                        case "aspx":
                        case "asax":
                        case "ascx":
                        case "cs":
                        case "ashx":
                        case "config":
                        case "js":
                        case "css":
                        case "txt":
                        case "htm":
                        case "html":
                        case "xml":
                        case "asp":
                        case "shtml":
                        case "ini":
                        case "as":
                        case "md":
                        case "json":
                        case "yml":
                            link.NavigateUrl = "javascript:";
                            link.Attributes.Add("onclick", String.Format("javascript:dialogIFrame({{ url: 'SiteFile.aspx?action=edit&file={1}', title: '编辑 - {0}', width: 700, height: 500}});return false;", file.Name, Server.UrlEncode(filepath)));
                            break;
                        case "rar":
                        case "pdf":
                        case "ppt":
                        case "pptx":
                        case "doc":
                        case "docx":
                        case "mdb":
                        case "accdb":
                        case "xlsx":
                        case "xls":
                        case "psd":
                        case "wma":
                        case "chm":

                            break;
                    }
                }
            }
        }
        /// <summary>
        /// 图标视图控件生成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Repeater2_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                FileSystemInfo file = e.Item.DataItem as FileSystemInfo;
                String filepath = "/" + RootPath.MakeRelativeUri(new Uri(file.FullName)).ToString();

                HyperLink link = (HyperLink)e.Item.FindControl("Link");
                ImageButton del = (ImageButton)e.Item.FindControl("del");
                ImageButton edit = (ImageButton)e.Item.FindControl("edit");
                ImageButton save = (ImageButton)e.Item.FindControl("save");
                ImageButton cut = (ImageButton)e.Item.FindControl("cut");

                del.OnClientClick = String.Format("javascript:dialogConfirm({{el:this,text:'将删除 {0} 且无法恢复!确定要删除吗?'}});return false;", file.Name);
                edit.OnClientClick = String.Format("javascript:dialogIFrame({{ url: 'SiteFile.aspx?action=rename&file={1}', title: '重命名 - {0}', width: 350, height: 250 }});return false;", file.Name, Server.UrlEncode(filepath));

                Image ico = new Image();
                ico.ImageUrl = "../skin/transparent.gif";
                ico.Style.Add("background-position", "center center");

                link.Controls.Add(ico);

                if (file.Attributes == FileAttributes.Directory)
                {
                    link.NavigateUrl = "SiteFile.aspx?dir=" + filepath;
                    ico.Style.Add("background-image", "../skin/files/folder_large.png");
                }
                else
                {
                    save.Visible = true;

                    FileInfo _file = file as FileInfo;
                    String ex = String.IsNullOrEmpty(_file.Extension) ? "none" : _file.Extension.Substring(1).ToLower();

                    String src = String.Format("../skin/files/{0}_large.png", ex);
                    if (!icons.Contains(ex)) src = "../skin/files/none_large.png";
                    ico.Style.Add("background-image", src);

                    switch (ex)
                    {
                        case "gif":
                        case "jpeg":
                        case "png":
                        case "bmp":
                        case "jpg":
                            cut.Visible = true;
                            cut.OnClientClick = String.Format("javascript:dialogIFrame({{ url: '../Plugin-S/Kroppr/Kroppr.aspx?file={1}', title: '剪切 - {0}', width: 700, height: 480 }});return false;", file.Name, Server.UrlEncode(filepath));

                            link.NavigateUrl = filepath;
                            link.Attributes.Add("rel", "group1");
                            link.Attributes.Add("class", "lightbox");
                            ico.Style["background-image"] = "../Plugin-S/Thumbnail.aspx?w=110&h=146&file=" + filepath;
                            break;
                        case "aspx":
                        case "asax":
                        case "ascx":
                        case "cs":
                        case "ashx":
                        case "config":
                        case "js":
                        case "css":
                        case "txt":
                        case "htm":
                        case "html":
                        case "xml":
                        case "asp":
                        case "shtml":
                        case "ini":
                        case "as":
                        case "md":
                        case "json":
                        case "yml":
                            link.NavigateUrl = "javascript:";
                            link.Attributes.Add("onclick", String.Format("javascript:dialogIFrame({{ url: 'SiteFile.aspx?action=edit&file={1}', title: '编辑 - {0}', width: 700, height: 500}});return false;", file.Name, Server.UrlEncode(filepath)));
                            break;
                    }
                }
            }
        }
    }
}