using System;
using MSSQLDB;
using Newtonsoft.Json;
using WebSite.Interface;

public partial class validate_user : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        IJsonResult result = new IJsonResult { Text = " " };

        ExecuteObject obj = new ExecuteObject();
        obj.tableName = "T_User";
        obj.terms.Add("username", UserName);
        obj.cells.Add("id", null);
        Object val = db.ExecuteScalar(obj);
        Int32 id = Convert.ToInt32(val);

        result.Status = id == 0;

        Response.Write(JsonConvert.SerializeObject(result));
    }
    /// <summary>
    /// 注册用户名称
    /// </summary>
    [RequestField(Name = "param")]
    protected String UserName { get; set; }
}