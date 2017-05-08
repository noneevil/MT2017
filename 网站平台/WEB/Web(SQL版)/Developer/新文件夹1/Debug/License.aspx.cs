using System;
using CommonUtils;
using WebSite.BackgroundPages;
using WebSite.Interface;
using WebSite.Plugins;

namespace WebSite.Web.Debug
{
    public partial class License : BaseAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Random rand = new Random();
                guid.Text = Guid.NewGuid().ToString();
                Count.Text = rand.Next(10, 255).ToString();
                EndTime.Text = DateTime.Now.AddYears(1).ToString("yyyy-MM-dd");
                DomainName.Text = Request.Url.Host;
            }
        }
        /// <summary>
        /// 创建授权文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            DateTime time = Convert.ToDateTime(EndTime.Text);
            ILicenseData data = new ILicenseData
            {
                CompanyName = CompanyName.Text,
                DomainName = DomainName.Text,
                Guid = Guid.Parse(guid.Text),
                Count = Convert.ToInt32(Count.Text),
                UnixEndTime = DateHelper.DateTimeToUnix(time)
            };
            LicenseHelper.CreateLicense(data);
        }
        /// <summary>
        /// 查看授权文件信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnView_Click(object sender, EventArgs e)
        {
            Alert(Label3, "", "");
            if (FileUpload1.HasFile && FileUpload1.PostedFile.ContentLength > 0)
            {
                try
                {
                    ILicenseData data = LicenseHelper.DecryptLicense(FileUpload1.PostedFile.InputStream);
                    this.SetFormValue(data);
                }
                catch
                {
                    Alert(Label3, "文件无效!", "line1px_2");
                }
            }
            else
            {
                Alert(Label3, "未选择文件!", "line1px_2");
            }
        }
    }
}