using System;
using System.IO;
using CommonUtils;
using WebSite.BackgroundPages;

namespace WebSite.Web
{
    /// <summary>
    /// ckeditor Word转换插件
    /// </summary>
    public partial class Word : BaseAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Label1.Visible = false;
            txt.Value = "";
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            Label1.Visible = true;
            if (!FileUpload1.HasFile)
            {
                Alert(Label1, "未选择文件!", "line1px_2");
            }
            else
            {
                String filename = FileUpload1.FileName.ToLower();
                String ex = Path.GetExtension(filename);
                if (ex != ".doc" && ex != ".docx")
                {
                    Alert(Label1, "只能转换Word文件!", "line1px_2");
                }
                else
                {
                    DirectoryInfo dir = new DirectoryInfo(Server.MapPath("/UpFiles/temp/"));
                    if (!dir.Exists) dir.Create();
                    Int32 count = dir.GetFiles("*.*").Length + 1;
                    String filepath = dir.FullName + "/" + count.ToString("D10") + ex;
                    FileUpload1.SaveAs(filepath);

                    WordHelper word = new WordHelper();
                    word.InputFile = filepath;
                    String html = word.ConvertToString();
                    txt.Value = html;
                    Alert(Label1, "转换成功,点击确定完成!", "line1px_3");
                }
            }
        }
    }
}