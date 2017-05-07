using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace CommonUtils.Tenpay
{
	/// <summary>
	/// ResponseHandler 的摘要说明。
	/// </summary>
	public class ResponseHandler
	{
		/** 密钥 */
		private String key;
		
		/** 应答的参数 */
		protected Hashtable parameters;
		
		/** debug信息 */
		private String debugInfo;

		protected HttpContext httpContext;

		//获取服务器通知数据方式，进行参数获取
		public ResponseHandler(HttpContext httpContext)
		{
			parameters = new Hashtable();

			this.httpContext = httpContext;
			NameValueCollection collection;
			if(this.httpContext.Request.HttpMethod == "POST")
			{
				collection = this.httpContext.Request.Form;
			}
			else
			{
				collection = this.httpContext.Request.QueryString;
			}

			foreach(String k in collection)
			{
				String v = (String)collection[k];
				this.setParameter(k, v);
			}
		}	

		/** 获取密钥 */
		public String getKey() 
		{ return key;}

		/** 设置密钥 */
		public void setKey(String key) 
		{ this.key = key;}

		/** 获取参数值 */
		public String getParameter(String parameter) 
		{
			String s = (String)parameters[parameter];
			return (null == s) ? "" : s;
		}

		/** 设置参数值 */
		public void setParameter(String parameter,String parameterValue) 
		{
			if(parameter != null && parameter != "")
			{
				if(parameters.Contains(parameter))
				{
					parameters.Remove(parameter);
				}
	
				parameters.Add(parameter,parameterValue);		
			}
		}

		/** 是否财付通签名,规则是:按参数名称a-z排序,遇到空值的参数不参加签名。 
		 * @return boolean */
		public virtual Boolean isTenpaySign() 
		{
			StringBuilder sb = new StringBuilder();

			ArrayList akeys=new ArrayList(parameters.Keys); 
			akeys.Sort();

			foreach(String k in akeys)
			{
				String v = (String)parameters[k];
				if(null != v && "".CompareTo(v) != 0
					&& "sign".CompareTo(k) != 0 && "key".CompareTo(k) != 0) 
				{
					sb.Append(k + "=" + v + "&");
				}
			}

			sb.Append("key=" + this.getKey());
			String sign = MD5Util.GetMD5(sb.ToString(),getCharset()).ToLower();
			
			//debug信息
			this.setDebugInfo(sb.ToString() + " => sign:" + sign);
			return getParameter("sign").ToLower().Equals(sign); 
		}

		/**
		* 显示处理结果。
		* @param show_url 显示处理结果的url地址,绝对url地址的形式(http://www.xxx.com/xxx.aspx)。
		* @throws IOException 
		*/
		public void doShow(String show_url) 
		{
			String strHtml = "<html><head>\r\n" +
				"<meta name=\"TENCENT_ONLINE_PAYMENT\" content=\"China TENCENT\">\r\n" +
				"<script language=\"javascript\">\r\n" +
				"window.location.href='" + show_url + "';\r\n" +
				"</script>\r\n" +
				"</head><body></body></html>";

			this.httpContext.Response.Write(strHtml);

			this.httpContext.Response.End();		
		}

		/** 获取debug信息 */
		public String getDebugInfo() 
		{ return debugInfo;}
				
		/** 设置debug信息 */
		protected void setDebugInfo(String debugInfo)
		{ this.debugInfo = debugInfo;}

		protected virtual String getCharset()
		{
			return this.httpContext.Request.ContentEncoding.BodyName;
			
		}

		/** 是否财付通签名,规则是:按参数名称a-z排序,遇到空值的参数不参加签名。 
		 * @return boolean */
		public virtual Boolean _isTenpaySign(ArrayList akeys) 
		{
			StringBuilder sb = new StringBuilder();

			foreach(String k in akeys)
			{
				String v = (String)parameters[k];
				if(null != v && "".CompareTo(v) != 0
					&& "sign".CompareTo(k) != 0 && "key".CompareTo(k) != 0) 
				{
					sb.Append(k + "=" + v + "&");
				}
			}

			sb.Append("key=" + this.getKey());
			String sign = MD5Util.GetMD5(sb.ToString(),getCharset()).ToLower();
			
			//debug信息
			this.setDebugInfo(sb.ToString() + " => sign:" + sign);
			return getParameter("sign").ToLower().Equals(sign); 
		}
	}
}
