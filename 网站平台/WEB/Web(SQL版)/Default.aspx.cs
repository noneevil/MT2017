using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebSite.Core.Table;
using MSSQLDB;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;
using WebSite.Models;
using System.Reflection;
using System.IO;
using System.Text;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Stopwatch watch1 = Stopwatch.StartNew();

        List<T_NewsEntity> data = db.ExecuteObject<List<T_NewsEntity>>("SELECT TOP 100000 * FROM [t_news]");


        watch1.Stop();

        Response.Write(watch1.Elapsed.ToString());
        Response.Write("<hr/>");
        //Response.Write(JsonConvert.SerializeObject(data));
        Response.End();
        return;
        //String s = "ABCDEFGHIJKLMNOPQRSTUVWXYZ ZYXWVUTSRQPONMLKJIHGFEDCBA    ";
        //Byte[] bytes = Encoding.UTF8.GetBytes(s);
        //Array.Reverse(bytes, 0, bytes.Length);
        //Response.Write(Encoding.UTF8.GetString(bytes));

        //去掉7z文件头信息
        foreach (FileInfo file in new DirectoryInfo(Server.MapPath("/")).GetFiles("*.7z"))
        {
            Response.Write(file.FullName + "<br>");
            using (FileStream zipfile = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
            {
                zipfile.Position = 6;
                Byte[] bytes = new Byte[zipfile.Length - 6];
                zipfile.Read(bytes, 0, bytes.Length);
                using (BinaryWriter bw = new BinaryWriter(new FileStream(Server.MapPath("/a.7z"), FileMode.OpenOrCreate, FileAccess.Write)))
                {
                    bw.Write(bytes);
                }
                //using (BinaryReader read = new BinaryReader(zipfile))
                //{
                //    Byte[] buffer = read.ReadBytes(6);
                //    for (int i = 0; i < buffer.Length; i++)
                //    {
                //        Response.Write("0x" + buffer[i].ToString("x") + ",");
                //    }
                //}
            }
            break;
        }
        //还原文件头信息
        Byte[] head = new Byte[] { 0x37, 0x7a, 0xbc, 0xaf, 0x27, 0x1c }; //37 7A BC AF 27 1C //55, 122, 188, 175, 39, 28
        using (FileStream zipfile = new FileStream(Server.MapPath("/a.7z"), FileMode.Open, FileAccess.ReadWrite))
        {
            Byte[] buffer = new Byte[zipfile.Length];
            zipfile.Read(buffer, 0, buffer.Length);
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(head, 0, head.Length);
                ms.Write(buffer, 0, buffer.Length);
                zipfile.Seek(0, SeekOrigin.Begin);
                zipfile.Write(ms.ToArray(), 0, head.Length + buffer.Length);
            }
        }

        //FileStream fs = new FileStream(Server.MapPath("/2.7z"), FileMode.Open, FileAccess.Read);
        //BinaryReader r = new BinaryReader(fs);
        //Byte[] byt = r.ReadBytes(6);
        //for (int i = 0; i < byt.Length; i++)
        //{
        //    Response.Write(byt[i].ToString("0x")+"\t");
        //}
        ////Response.Write(String.Join(",", byt.ToArray()));
        ////string bx = " ";
        ////byte buffer;
        ////try
        ////{
        ////    buffer = r.ReadByte();
        ////    bx = buffer.ToString();
        ////    buffer = r.ReadByte();
        ////    bx += buffer.ToString();
        ////}
        ////catch (Exception exc)
        ////{
        ////    Console.WriteLine(exc.Message);
        ////}
        ////Response.Write(bx);
        //r.Close();
        //fs.Close();



        //var Forms = TableForm.TableForms.Find(a => { return a.TableID == Guid.Parse("4ba15068-8693-4f0e-adef-6dd4d30b3552"); });
        //var frm = Forms.FromCollections.Find(a => { return a.ID == Guid.Parse("4b76337d-f047-48c8-ad88-bc90c08a5bf5"); });
        //String text = frm.Content;
        //Response.Write(text);

        //Server.Execute("virtual/from/test.aspx");
        //Server.Transfer("virtual/from/test.aspx");

        //Control u = Page.LoadControl("virtual/from/test.ascx");
        //PlaceHolder1.Controls.Add(u);

        //DataTable dt = new DataTable();
        //dt = db.ExecuteDataTable("SELECT TOP 10 id,title,pubdate,imageurl FROM t_news where imageurl<>''");

        //Repeater rep = new Repeater();
        //PlaceHolder1.Controls.Add(rep);
        //rep.ItemTemplate = Page.LoadTemplate("~/virtual/from/esdsa.ascx");
        //rep.DataSource = dt;
        //rep.DataBind();

        //Repeater1.ItemTemplate = Page.LoadTemplate("~/virtual/from/esds.ascx");
        //Repeater1.DataSource = dt;
        //Repeater1.DataBind();

        //PlaceHolder1.Controls.Add(Page.LoadControl("~/virtual/from/esdsa.ascx"));
        //Repeater ctl = PlaceHolder1.FindControl("Repeater1") as Repeater;
        //ctl.DataSource = dt;
        //ctl.DataBind();
    }
}