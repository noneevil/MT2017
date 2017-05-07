using System;
using System.Web;

namespace CommonUtils.Alipay
{
    /// <summary>
    /// 类名：Config
    /// 功能：基础配置类
    /// 详细：设置帐户有关信息及返回路径
    /// 版本：3.2
    /// 日期：2011-03-17
    /// 说明：
    /// 以下代码只是为了方便商户测试而提供的样例代码，商户可以根据自己网站的需要，按照技术文档编写,并非一定要使用该代码。
    /// 该代码仅供学习和研究支付宝接口使用，只是提供一个参考。
    /// 
    /// 如何获取安全校验码和合作身份者ID
    /// 1.用您的签约支付宝账号登录支付宝网站(www.alipay.com)
    /// 2.点击“商家服务”(https://b.alipay.com/order/myOrder.htm)
    /// 3.点击“查询合作者身份(PID)”、“查询安全校验码(Key)”
    /// </summary>
    internal class Config
    {
        static Config()
        {
            String strXmlFile = HttpContext.Current.Server.MapPath("/App_Data/Alipay.config");
            XmlControl XmlTool = new XmlControl(strXmlFile);
            Seller_email = XmlTool.GetText("Root/seller_email");                     //商家签约时的支付宝帐号，即收款的支付宝帐号
            Partner = XmlTool.GetText("Root/partner"); 		//partner合作伙伴id（必须填写）
            Key = XmlTool.GetText("Root/key"); //partner 的对应交易安全校验码（必须填写）
            XmlTool.Dispose();

            Return_url = "http://localhost/api/alipay/return_url.aspx";
            Notify_url = "http://localhost/api/alipay/notify_url.aspx";


            //↑↑↑↑↑↑↑↑↑↑请在这里配置您的基本信息↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑
            //字符编码格式 目前支持 gbk 或 utf-8
            Input_charset = "utf-8";
            //签名方式 不需修改
            Sign_type = "MD5";
            //访问模式,根据自己的服务器是否支持ssl访问，若支持请选择https；若不支持请选择http
            Transport = "https";
        }

        #region 属性
        /// <summary>
        /// 获取或设置合作者身份ID
        /// </summary>
        public static String Partner { get; set; }

        /// <summary>
        /// 获取或设置交易安全检验码
        /// </summary>
        public static String Key { get; set; }

        /// <summary>
        /// 获取或设置签约支付宝账号或卖家支付宝帐户
        /// </summary>
        public static String Seller_email { get; set; }

        /// <summary>
        /// 获取页面跳转同步通知页面路径
        /// </summary>
        public static String Return_url { get; set; }

        /// <summary>
        /// 获取服务器异步通知页面路径
        /// </summary>
        public static String Notify_url { get; set; }

        /// <summary>
        /// 获取字符编码格式
        /// </summary>
        public static String Input_charset { get; set; }

        /// <summary>
        /// 获取签名方式
        /// </summary>
        public static String Sign_type { get; set; }

        /// <summary>
        /// 获取访问模式
        /// </summary>
        public static String Transport { get; set; }

        #endregion
    }
}