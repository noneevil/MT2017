using System;
using System.Collections;
using System.Text;
using System.Web;

namespace CommonUtils.Tenpay
{
	/// <summary>
	/// RequestHandler ��ժҪ˵����
	/// </summary>
	public class RequestHandler
	{
		public RequestHandler(HttpContext httpContext)
		{
			parameters = new Hashtable();

			this.httpContext = httpContext;

			this.setGateUrl("https://www.tenpay.com/cgi-bin/v1.0/service_gate.cgi");
		}

		/** ����url��ַ */
		private String gateUrl;
		
		/** ��Կ */
		private String key;
		
		/** ����Ĳ��� */
		protected Hashtable parameters;
		
		/** debug��Ϣ */
		private String debugInfo;

		protected HttpContext httpContext;
		
		/** ��ʼ��������*/
		public virtual void init() 
		{
			//nothing to do
		}

		/** ��ȡ��ڵ�ַ,����������ֵ */
		public String getGateUrl() 
		{
			return gateUrl;
		}

		/** ������ڵ�ַ,����������ֵ */
		public void setGateUrl(String gateUrl) 
		{
			this.gateUrl = gateUrl;
		}

		/** ��ȡ��Կ */
		public String getKey() 
		{
			return key;
		}

		/** ������Կ */
		public void setKey(String key) 
		{
			this.key = key;
		}

		/** ��ȡ������������URL  @return String */
		public virtual String getRequestURL()
		{
			this.createSign();

			StringBuilder sb = new StringBuilder();
			ArrayList akeys=new ArrayList(parameters.Keys); 
			akeys.Sort();
			foreach(String k in akeys)
			{
				String v = (String)parameters[k];
				if(null != v && "key".CompareTo(k) != 0) 
				{
					sb.Append(k + "=" + TenpayUtil.UrlEncode(v, getCharset()) + "&");
				}
			}

			//ȥ�����һ��&
			if(sb.Length > 0)
			{
				sb.Remove(sb.Length-1, 1);
			}
							
			return this.getGateUrl() + "?" + sb.ToString();
		}

		/**
		* ����md5ժҪ,������:����������a-z����,������ֵ�Ĳ������μ�ǩ����
		*/
		protected virtual void createSign() 
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
			String sign = MD5Util.GetMD5(sb.ToString(), getCharset()).ToLower();
		
			this.setParameter("sign", sign);
		
			//debug��Ϣ
			this.setDebugInfo(sb.ToString() + " => sign:" + sign);		
		}

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

		public void doSend()
		{
			this.httpContext.Response.Redirect(this.getRequestURL());
		}
			
		/** ��ȡdebug��Ϣ */
		public String getDebugInfo() 
		{
			return debugInfo;
		}

		/** ����debug��Ϣ */
		public void setDebugInfo(String debugInfo) 
		{
			this.debugInfo = debugInfo;
		}

		public Hashtable getAllParameters()
		{
			return this.parameters;
		}

		protected virtual String getCharset()
		{
			return this.httpContext.Request.ContentEncoding.BodyName;
		}
	}
}
