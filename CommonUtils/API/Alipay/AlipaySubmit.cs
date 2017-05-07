using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace CommonUtils.Alipay
{
    /// <summary>
    /// 类名：Submit
    /// 功能：支付宝各接口请求提交类
    /// 详细：构造支付宝各接口表单HTML文本，获取远程HTTP数据
    /// 版本：3.2
    /// 修改日期：2011-03-17
    /// 说明：
    /// 以下代码只是为了方便商户测试而提供的样例代码，商户可以根据自己网站的需要，按照技术文档编写,并非一定要使用该代码。
    /// 该代码仅供学习和研究支付宝接口使用，只是提供一个参考
    /// </summary>
    public class Submit
    {
        #region 字段
        //交易安全校验码
        private static String _key = "";
        //编码格式
        private static String _input_charset = "";
        //签名方式
        private static String _sign_type = "";
        #endregion

        static Submit()
        {
            _key = Config.Key.Trim().ToLower();
            _input_charset = Config.Input_charset.Trim().ToLower();
            _sign_type = Config.Sign_type.Trim().ToUpper();
        }

        /// <summary>
        /// 生成要请求给支付宝的参数数组
        /// </summary>
        /// <param name="sParaTemp">请求前的参数数组</param>
        /// <returns>要请求的参数数组</returns>
        private static Dictionary<String, String> BuildRequestPara(SortedDictionary<String, String> sParaTemp)
        {
            //待签名请求参数数组
            Dictionary<String, String> sPara = new Dictionary<String, String>();
            //签名结果
            String mysign = "";

            //过滤签名参数数组
            sPara = Core.FilterPara(sParaTemp);

            //获得签名结果
            mysign = Core.BuildMysign(sPara, _key, _sign_type, _input_charset);

            //签名结果与签名方式加入请求提交参数组中
            sPara.Add("sign", mysign);
            sPara.Add("sign_type", _sign_type);

            return sPara;
        }

        /// <summary>
        /// 生成要请求给支付宝的参数数组
        /// </summary>
        /// <param name="sParaTemp">请求前的参数数组</param>
        /// <returns>要请求的参数数组字符串</returns>
        private static String BuildRequestParaToString(SortedDictionary<String, String> sParaTemp)
        {
            //待签名请求参数数组
            Dictionary<String, String> sPara = new Dictionary<String, String>();
            sPara = BuildRequestPara(sParaTemp);

            //把参数组中所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
            String strRequestData = Core.CreateLinkString(sPara);

            return strRequestData;
        }

        /// <summary>
        /// 构造提交表单HTML数据
        /// </summary>
        /// <param name="sParaTemp">请求参数数组</param>
        /// <param name="gateway">网关地址</param>
        /// <param name="strMethod">提交方式。两个值可选：post、get</param>
        /// <param name="strButtonValue">确认按钮显示文字</param>
        /// <returns>提交表单HTML文本</returns>
        public static String BuildFormHtml(SortedDictionary<String, String> sParaTemp, String gateway, String strMethod, String strButtonValue)
        {
            //待请求参数数组
            Dictionary<String, String> dicPara = new Dictionary<String, String>();
            dicPara = BuildRequestPara(sParaTemp);

            StringBuilder sbHtml = new StringBuilder();

            sbHtml.Append("<form id='alipaysubmit' name='alipaysubmit' action='" + gateway + "_input_charset=" + _input_charset + "' method='" + strMethod.ToLower().Trim() + "'>");

            foreach (KeyValuePair<String, String> temp in dicPara)
            {
                sbHtml.Append("<input type='hidden' name='" + temp.Key + "' value='" + temp.Value + "'/>");
            }

            //submit按钮控件请不要含有name属性
            sbHtml.Append("<input type='submit' value='" + strButtonValue + "' style='display:none;'></form>");

            sbHtml.Append("<script>document.forms['alipaysubmit'].submit();</script>");

            return sbHtml.ToString();
        }

        /// <summary>
        /// 构造模拟远程HTTP的POST请求，获取支付宝的返回XML处理结果
        /// </summary>
        /// <param name="sParaTemp">请求参数数组</param>
        /// <param name="gateway">网关地址</param>
        /// <returns>支付宝返回XML处理结果</returns>
        public static XmlDocument SendPostInfo(SortedDictionary<String, String> sParaTemp, String gateway)
        {
            //待请求参数数组字符串
            String strRequestData = BuildRequestParaToString(sParaTemp);

            //把数组转换成流中所需字节数组类型
            Encoding code = Encoding.GetEncoding(_input_charset);
            byte[] bytesRequestData = code.GetBytes(strRequestData);

            //构造请求地址
            String strUrl = gateway + "_input_charset=" + _input_charset;

            //请求远程HTTP
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                //设置HttpWebRequest基本信息
                HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(strUrl);
                myReq.Method = "post";
                myReq.ContentType = "application/x-www-form-urlencoded";

                //填充POST数据
                myReq.ContentLength = bytesRequestData.Length;
                Stream requestStream = myReq.GetRequestStream();
                requestStream.Write(bytesRequestData, 0, bytesRequestData.Length);
                requestStream.Close();

                //发送POST数据请求服务器
                HttpWebResponse HttpWResp = (HttpWebResponse)myReq.GetResponse();
                Stream myStream = HttpWResp.GetResponseStream();

                //获取服务器返回信息
                XmlTextReader Reader = new XmlTextReader(myStream);
                xmlDoc.Load(Reader);
            }
            catch (Exception exp)
            {
                String strXmlError = "<error>" + exp.Message + "</error>";
                xmlDoc.LoadXml(strXmlError);
            }

            return xmlDoc;
        }

        /// <summary>
        /// 构造模拟远程HTTP的GET请求，获取支付宝的返回XML处理结果
        /// </summary>
        /// <param name="sParaTemp">请求参数数组</param>
        /// <param name="gateway">网关地址</param>
        /// <returns>支付宝返回XML处理结果</returns>
        public static XmlDocument SendGetInfo(SortedDictionary<String, String> sParaTemp, String gateway)
        {
            //待请求参数数组字符串
            String strRequestData = BuildRequestParaToString(sParaTemp);

            //构造请求地址
            String strUrl = gateway + strRequestData;

            //请求远程HTTP
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                //设置HttpWebRequest基本信息
                HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(strUrl);
                myReq.Method = "get";

                //发送POST数据请求服务器
                HttpWebResponse HttpWResp = (HttpWebResponse)myReq.GetResponse();
                Stream myStream = HttpWResp.GetResponseStream();

                //获取服务器返回信息
                XmlTextReader Reader = new XmlTextReader(myStream);
                xmlDoc.Load(Reader);
            }
            catch (Exception exp)
            {
                String strXmlError = "<error>" + exp.Message + "</error>";
                xmlDoc.LoadXml(strXmlError);
            }

            return xmlDoc;
        }
    }
}