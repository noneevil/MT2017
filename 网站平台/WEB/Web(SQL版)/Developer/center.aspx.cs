using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebSite.BackgroundPages;
using Newtonsoft.Json;
using WebSite.Core;

public partial class Developer_center : BaseAdmin
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Response.Write(JsonConvert.SerializeObject(T_AccessControlHelper.GetRole(Admin.RoleID)).Replace("},", "},\r\n"));
        //Response.End();
    }
}