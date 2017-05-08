using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebSite.BackgroundPages;

namespace WebSite.Web
{
    public partial class Default : BaseAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnExit_Click(object sender, EventArgs e)
        {
            Admin.SignOut();
        }
    }
}