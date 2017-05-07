using System;
using CommonUtils;
using System.Web.UI;
using System.IO;
using System.Net;
using System.Text;
using System.Globalization;

namespace MvcSite
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //int i = 0;
            //foreach (FileInfo file in new DirectoryInfo(Server.MapPath("/UpFiles/壁纸")).GetFiles())
            //{
            //    i++;
            //    file.MoveTo(Path.GetDirectoryName(file.FullName) + "/" + i.ToString("00") + Path.GetExtension(file.Name));
            //}

        }

        protected void 登录_Click(object sender, EventArgs e)
        {
            System.Net.WebClient web = new System.Net.WebClient();
            if (!String.IsNullOrEmpty(TextBox2.Text)) web.Headers.Add("Cookie", TextBox2.Text);
            Byte[] data = Encoding.UTF8.GetBytes("{UserName:'user',PassWord:'123456'}");
            Byte[] result = web.UploadData("http://localhost:4233/api/Login", "POST", data);
            TextBox1.Text = Encoding.UTF8.GetString(result);

            if (String.IsNullOrEmpty(TextBox2.Text))
            {
                String cookie = web.ResponseHeaders["Set-Cookie"];
                TextBox2.Text = cookie;
            }

            web.Dispose();
        }
    }
}