using System;
using System.Text;
using System.Web;

namespace CommonUtils.Tenpay
{
	/// <summary>
	/// PayResponseHandler ��ժҪ˵����
	/// </summary>
	/**
	* ��ʱ����Ӧ����
	* ============================================================================
	* api˵����
	* getKey()/setKey(),��ȡ/������Կ
	* getParameter()/setParameter(),��ȡ/���ò���ֵ
	* getAllParameters(),��ȡ���в���
	* isTenpaySign(),�Ƿ�Ƹ�ͨǩ��,true:�� false:��
	* doShow(),��ʾ������
	* getDebugInfo(),��ȡdebug��Ϣ
	* 
	* ============================================================================
	*
	*/
	public class PayResponseHandler:ResponseHandler
	{
		public PayResponseHandler(HttpContext httpContext) : base(httpContext)
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}
		/**
	 * �Ƿ�Ƹ�ͨǩ��
	 * @Override
	 * @return boolean
	 */
		
		public override Boolean isTenpaySign() 
		{
		
			//��ȡ����
			String cmdno = getParameter("cmdno");
			String pay_result = getParameter("pay_result");
			String date = getParameter("date");
			String transaction_id = getParameter("transaction_id");
			String sp_billno = getParameter("sp_billno");
			String total_fee = getParameter("total_fee");		
			String fee_type = getParameter("fee_type");
			String attach = getParameter("attach");
			String tenpaySign = getParameter("sign").ToUpper();
			String key = this.getKey();
			
			//��֯ǩ����
			StringBuilder sb = new StringBuilder();
			sb.Append("cmdno=" + cmdno + "&");
			sb.Append("pay_result=" + pay_result + "&");
			sb.Append("date=" + date + "&");
			sb.Append("transaction_id=" + transaction_id + "&");
			sb.Append("sp_billno=" + sp_billno + "&");
			sb.Append("total_fee=" + total_fee + "&");
			sb.Append("fee_type=" + fee_type + "&");
			sb.Append("attach=" + attach + "&");
			sb.Append("key=" + key);
		
			//���ժҪ
			String sign = MD5Util.GetMD5(sb.ToString(),getCharset());	

			//debug��Ϣ
			setDebugInfo(sb.ToString() + " => sign:" + sign +
				" tenpaySign:" + tenpaySign);
		
			 return sign.Equals(tenpaySign);
		} 
	
	
	}
}
