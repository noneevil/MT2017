using System;
using MSSQLDB;
using Newtonsoft.Json;
using WebSite.Interface;

public partial class validate_navigation : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        IJsonResult msg = new IJsonResult { info = "该导航菜单ID不可为空！", status = "n" };
        if (navname == old_name)
        {
            msg.status = "y";
            msg.info = "该导航菜单ID可使用";
        }
        else if (navname.ToLower().StartsWith("channel_"))
        {
            msg.info = "该导航菜单ID系统保留，请更换！";
        }
        else
        {
            ExecuteObject obj = new ExecuteObject();
            obj.cmdtype = CmdType.SELECT;
            obj.tableName = "B_Navigation";
            obj.terms.Add("name", navname);//查询条件
            obj.cells.Add("id", null);
          
            Object data = db.ExecuteScalar(obj);
           
            if (Convert.ToInt32(data) > 0)
            {
                msg.info = "该导航菜单ID系统保留，请更换！";
            }
            else
            {
                msg.status = "y";
                msg.info = "该导航菜单ID可使用";
            }
        }
        String json = JsonConvert.SerializeObject(msg);
        Response.Write(json);
    }
    /// <summary>
    /// 新的名称
    /// </summary>
    [RequestField(Name = "param")]
    protected String navname { get; set; }
    /// <summary>
    /// 旧的名称
    /// </summary>
    [RequestField(Name = "old_name")]
    protected String old_name { get; set; }
}