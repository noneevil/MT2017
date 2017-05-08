using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text.RegularExpressions;

public partial class Admin_login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {

    }
    /// <summary>
    /// 压缩HTML
    /// </summary>
    /// <param name="writer"></param>
    protected override void Render(HtmlTextWriter writer)
    {
        base.Render(writer);
        //using (StringWriter sw = new StringWriter())
        //{
        //    base.Render(new HtmlTextWriter(sw));
        //    String html = sw.ToString();
        //    html = Regex.Replace(html, @"\s+\s", "");
        //    writer.Write(html);
        //}
    }
}