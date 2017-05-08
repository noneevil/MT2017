using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Caching;
using MSSQLDB;
using Newtonsoft.Json;
using Novacode;

namespace WebSite.Models
{
    /// <summary>
    /// 名称预审核表
    /// </summary>
    [Table("T_Preaudit")]
    [Serializable]
    public partial class T_PreauditEntity
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Field("ID", IsPrimaryKey = true)]
        public long ID { get; set; }
        /// <summary>
        /// 会员编号
        /// </summary>
        [Field("UserID")]
        public long UserID { get; set; }
        /// <summary>
        /// 申请企业名称
        /// </summary>
        [Field("CompanyName0")]
        public String CompanyName0 { get; set; }
        /// <summary>
        /// 备选名称1
        /// </summary>
        [Field("CompanyName1")]
        public String CompanyName1 { get; set; }
        /// <summary>
        /// 备选名称2
        /// </summary>
        [Field("CompanyName2")]
        public String CompanyName2 { get; set; }
        /// <summary>
        /// 备选名称3
        /// </summary>
        [Field("CompanyName3")]
        public String CompanyName3 { get; set; }
        /// <summary>
        /// 许可经营范围
        /// </summary>
        [Field("Business0")]
        public String Business0 { get; set; }
        /// <summary>
        /// 一般经营范围
        /// </summary>
        [Field("Business1")]
        public String Business1 { get; set; }
        /// <summary>
        /// 注册资金
        /// </summary>
        [Field("Capital")]
        public Int32? Capital { get; set; }
        /// <summary>
        /// 企业类型
        /// </summary>
        [Field("CompanyType")]
        public String CompanyType { get; set; }
        /// <summary>
        /// 住所所在地
        /// </summary>
        [Field("Address")]
        public String Address { get; set; }
        /// <summary>
        /// 指定代表或者委托代理人
        /// </summary>
        [Field("Agent")]
        public String Agent { get; set; }
        /// <summary>
        /// (指定代表或委托代理人的权限) 是否同意核对登记材料中的复印件并签署核对意见
        /// </summary>
        [Field("Power1")]
        public bool Power1 { get; set; }
        /// <summary>
        /// (指定代表或委托代理人的权限) 是否同意修改有关表格的填写错误
        /// </summary>
        [Field("Power2")]
        public bool Power2 { get; set; }
        /// <summary>
        /// (指定代表或委托代理人的权限) 是否同意领取《企业名称预先核准通知书》
        /// </summary>
        [Field("Power3")]
        public bool Power3 { get; set; }
        /// <summary>
        /// (指定或者委托的有效期限开始日期)
        /// </summary>
        [Field("StartTime")]
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// (指定或者委托的有效期限结束日期)
        /// </summary>
        [Field("EndTime")]
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 投资者集合
        /// </summary>
        [Field("Investors")]
        public String Investors
        {
            get
            {
                return JsonConvert.SerializeObject(InvestorItems);
            }
            set
            {
                InvestorItems = JsonConvert.DeserializeObject<List<Investor>>(value);
            }
        }
        /// <summary>
        /// 投资者集合
        /// </summary>
        [JsonIgnore]
        [Field(IsIgnore = true)]
        public List<Investor> InvestorItems { get; set; }
        /// <summary>
        /// 填表日期
        /// </summary>
        [Field("FilledDate")]
        public DateTime FilledDate { get; set; }
        /// <summary>
        /// 经办人名字
        /// </summary>
        [Field("Transactor")]
        public String Transactor { get; set; }
        /// <summary>
        /// 经办人坐机
        /// </summary>
        [Field("TransactorTel")]
        public String TransactorTel { get; set; }
        /// <summary>
        /// 经办人手机
        /// </summary>
        [Field("TransactorTelephone")]
        public String TransactorTelephone { get; set; }
        /// <summary>
        /// 指定代表或委托代理人、具体经办人身份证明复印件
        /// </summary>
        [Field("TransactorCard")]
        public String TransactorCard { get; set; }
        /// <summary>
        /// 处理状态
        /// </summary>
        [Field("Status")]
        public AuditState Status { get; set; }
        /// <summary>
        /// 回复
        /// </summary>
        [Field("Reply")]
        public String Reply { get; set; }
        /// <summary>
        /// 回复时间
        /// </summary>
        [Field("ReplyTime")]
        public DateTime? ReplyTime { get; set; }
        /// <summary>
        /// 处理日期
        /// </summary>
        [Field("ProcessDate")]
        public DateTime? ProcessDate { get; set; }



        /// <summary>
        /// 缓存模板文件
        /// </summary>
        private static MemoryStream Template
        {
            get
            {
                MemoryStream data = new MemoryStream();
                Cache Cache = HttpRuntime.Cache;
                if (Cache["T_Preaudit_ExportTemplate"] == null)
                {
                    String doc = HttpContext.Current.Server.MapPath("/App_Data/Template.docx");
                    using (FileStream file = new FileStream(doc, FileMode.OpenOrCreate, FileAccess.Read))
                    {
                        file.CopyTo(data);
                    }
                    Cache.Insert("T_Preaudit_ExportTemplate", data, new CacheDependency(doc));
                }
                else
                {
                    data = (MemoryStream)Cache["T_Preaudit_ExportTemplate"];
                }
                return data;
            }
        }
        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="data"></param>
        public static void ExportData(T_PreauditEntity data, MemoryStream ms)
        {
            HttpServerUtility Server = HttpContext.Current.Server;
            DocX document = DocX.Load(Template);

            document.ReplaceText("#CompanyName0#", data.CompanyName0);
            document.ReplaceText("#CompanyName1#", data.CompanyName1);
            document.ReplaceText("#CompanyName2#", data.CompanyName2);
            document.ReplaceText("#CompanyName3#", data.CompanyName3);
            document.ReplaceText("#Business0#", data.Business0);
            document.ReplaceText("#Business1#", data.Business1);
            document.ReplaceText("#Capital#", Convert.ToString(data.Capital));
            document.ReplaceText("#CompanyType#", data.CompanyType);
            document.ReplaceText("#StartTime#", data.StartTime == null ? "    年    月    日" : data.StartTime.Value.ToString("yyyy 年 MM 月 dd 日"));
            document.ReplaceText("#EndTime#", data.StartTime == null ? "    年    月    日" : data.EndTime.Value.ToString("yyyy 年 MM 月 dd 日"));
            document.ReplaceText("#Address#", data.Address);
            document.ReplaceText("#Agent#", data.Agent);

            document.ReplaceText("#Power1#", data.Power1 ? "同意☑不同意☐" : "同意☐不同意☑");
            document.ReplaceText("#Power2#", data.Power2 ? "同意☑不同意☐" : "同意☐不同意☑");
            document.ReplaceText("#Power3#", data.Power3 ? "同意☑不同意☐" : "同意☐不同意☑");

            //var tmp = data.InvestorItems[0];
            //for (int i = 0; i < 10; i++)
            //{
            //    data.InvestorItems.Add(tmp);
            //}

            Table tab = document.Tables[1];
            foreach (var item in data.InvestorItems)
            {
                Row row = tab.InsertRow();
                row.Height = 30;

                for (int i = 0; i < row.Cells.Count; i++)
                {
                    row.Cells[i].VerticalAlignment = VerticalAlignment.Center;
                }

                row.Cells[0].Paragraphs[0].Append(item.name);
                row.Cells[1].Paragraphs[0].Append(item.card);
                row.Cells[2].Paragraphs[0].Append(Convert.ToString(item.Amounts));
                row.Cells[3].Paragraphs[0].Append(Convert.ToString(item.Percentage));

                row.Cells[2].Paragraphs[0].Alignment = Alignment.center;
                row.Cells[3].Paragraphs[0].Alignment = Alignment.center;
            }
            document.ReplaceText("#FilledDate#", data.FilledDate.ToString("yyyy 年 MM 月 dd 日"));
            document.ReplaceText("#Transactor#", data.Transactor);
            document.ReplaceText("#TransactorTel#", data.TransactorTel);
            document.ReplaceText("#TransactorTelephone#", data.TransactorTelephone);

            document.SaveAs(ms);
        }
    }
    /// <summary>
    /// 审核状态
    /// </summary>
    public enum AuditState
    {
        未查看 = 0,
        未处理 = 1,
        未通过 = 2,
        已通过 = 3,
        超时 = 4,
        已删除 = -1
    }
    /// <summary>
    /// 投资都
    /// </summary>
    [Serializable]
    public class Investor
    {
        /// <summary>
        /// 投资人的姓名或名称
        /// </summary>
        public String name { get; set; }
        /// <summary>
        /// 证照号码
        /// </summary>
        public String card { get; set; }
        /// <summary>
        /// 投资金额
        /// </summary>
        public Decimal? Amounts { get; set; }
        /// <summary>
        /// 投资比例
        /// </summary>
        public String Percentage { get; set; }
        /// <summary>
        /// 证件照片
        /// </summary>
        public String photo { get; set; }
    }
}
