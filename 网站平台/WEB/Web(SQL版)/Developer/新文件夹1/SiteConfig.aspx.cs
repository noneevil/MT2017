using System;
using System.Collections.Generic;
using System.Web.UI;
using CommonUtils;
using MSSQLDB;
using Newtonsoft.Json;
using WebSite.BackgroundPages;
using WebSite.Core;
using WebSite.Models;

namespace WebSite.Web
{
    public partial class SiteConfig : BaseAdmin
    {
        protected void Page_Load(Object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                @DataType.DataSource = Enum.GetValues(typeof(DatabaseType));
                @DataType.DataBind();
                @ViewStateMode.DataSource = Enum.GetValues(typeof(ViewStateType));
                @ViewStateMode.DataBind();

                language_SelectedIndexChanged(sender, e);
                @SiteURL.Text = Request.Url.Host;
            }
        }
        /// <summary>
        /// 语言选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void language_SelectedIndexChanged(Object sender, EventArgs e)
        {
            String value = Language.SelectedValue;
            if (sender is Page)
            {
                String cookie = CookieHelper.GetValue("Language");
                if (!String.IsNullOrEmpty(cookie))
                {
                    value = cookie;
                    Language.SelectedValue = value;
                }
            }
            CookieHelper.AddCookie("Language", value);

            var config = SiteParameter.Config;
            var lang = config.Languages.Find(a => { return a.language == value; });
            if (lang == null) lang = new Language();
            this.SetFormValue(lang);
            this.SetFormValue(config);

            //BadKeywords.Text = String.Join(",", config.BadKeywords);
            BadKeywords.Text = "";
            ViewState["keywords"] = JsonConvert.SerializeObject(config.BadKeywords);
            ViewState["options"] = JsonConvert.SerializeObject(config.DataTypeOptions);
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(Object sender, EventArgs e)
        {
            #region 保存更改

            var config = SiteParameter.Config;
            var lang = config.Languages.Find(a => { return a.language == Language.SelectedValue; });
            if (lang == null)
            {
                lang = new Language();
                config.Languages.Add(lang);
            }
            lang = this.GetFormValue<Language>(lang);
            config = this.GetFormValue<SiteParameter>(config);

            config.DataTypeOptions.ForEach(a => { a.Selected = false; });
            var datatype = config.DataTypeOptions.Find(a => { return a.Name == @DataType.SelectedValue; });
            if (datatype != null)
            {
                datatype.Selected = true;
                datatype.ConnectionString = ConnectionString.Text;
            }

            SiteParameter.SaveConfig();

            #endregion

            Alert("保存成功!", "success");
        }
    }
}