using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace CommonUtils.Tenpay
{
	/// <summary>
	/// ResponseHandler ��ժҪ˵����
	/// </summary>
	public class ResponseHandler
	{
		/** ��Կ */
		private String key;
		
		/** Ӧ��Ĳ��� */
		protected Hashtable parameters;
		
		/** debug��Ϣ */
		private String debugInfo;

		protected HttpContext httpContext;

		//��ȡ������֪ͨ���ݷ�ʽ�����в�����ȡ
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

		/** ��ȡ��Կ */
		public String getKey() 
		{ return key;}

		/** ������Կ */
		public void setKey(String key) 
		{ this.key = key;}

		/** ��ȡ����ֵ */
		public String getParameter(String parameter) 
		{
			String s = (String)parameters[parameter];
			return (null == s) ? "" : s;
		}

		/** ���ò���ֵ */
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

		/** �Ƿ�Ƹ�ͨǩ��,������:����������a-z����,������ֵ�Ĳ������μ�ǩ���� 
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
			
			//debug��Ϣ
			this.setDebugInfo(sb.ToString() + " => sign:" + sign);
			return getParameter("sign").ToLower().Equals(sign); 
		}

		/**
		* ��ʾ��������
		* @param show_url ��ʾ��������url��ַ,����url��ַ����ʽ(http://www.xxx.com/xxx.aspx)��
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

		/** ��ȡdebug��Ϣ */
		public String getDebugInfo() 
		{ return debugInfo;}
				
		/** ����debug��Ϣ */
		protected void setDebugInfo(String debugInfo)
		{ this.debugInfo = debugInfo;}

		protected virtual String getCharset()
		{
			return this.httpContext.Request.ContentEncoding.BodyName;
			
		}

		/** �Ƿ�Ƹ�ͨǩ��,������:����������a-z����,������ֵ�Ĳ������μ�ǩ���� 
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
			
			//debug��Ϣ
			this.setDebugInfo(sb.ToString() + " => sign:" + sign);
			return getParameter("sign").ToLower().Equals(sign); 
		}
	}
}
