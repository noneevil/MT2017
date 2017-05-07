using System;
using System.Text;
using System.Web;

namespace CommonUtils.Tenpay
{
	/// <summary>
	/// PayRequestHandler ��ժҪ˵����
	/// </summary>
	/**
	* ��ʱ����������
	* ============================================================================
	* api˵����
	* init(),��ʼ��������Ĭ�ϸ�һЩ������ֵ����cmdno,date�ȡ�
	* getGateURL()/setGateURL(),��ȡ/������ڵ�ַ,����������ֵ
	* getKey()/setKey(),��ȡ/������Կ
	* getParameter()/setParameter(),��ȡ/���ò���ֵ
	* getAllParameters(),��ȡ���в���
	* getRequestURL(),��ȡ������������URL
	* doSend(),�ض��򵽲Ƹ�֧ͨ��
	* getDebugInfo(),��ȡdebug��Ϣ
	* 
	* ============================================================================
	*
	*/
	public class PayRequestHandler:RequestHandler
	{
		public PayRequestHandler(HttpContext httpContext) : base(httpContext)
		{
			
			this.setGateUrl("https://www.tenpay.com/cgi-bin/v1.0/pay_gate.cgi");
		}


		/**
			* @Override
			* ��ʼ��������Ĭ�ϸ�һЩ������ֵ����cmdno,date�ȡ�
		*/
		public override void init() 
		{

			//�������
			this.setParameter("cmdno", "1");
		
			//����
			this.setParameter("date",DateTime.Now.ToString("yyyyMMdd"));
		
			//�̻���
			this.setParameter("bargainor_id", "");
		
			//�Ƹ�ͨ���׵���
			this.setParameter("transaction_id", "");
		
			//�̼Ҷ�����
			this.setParameter("sp_billno", "");
		
			//��Ʒ�۸��Է�Ϊ��λ
			this.setParameter("total_fee", "");
		
			//��������
			this.setParameter("fee_type",  "1");
		
			//����url
			this.setParameter("return_url",  "");
		
			//�Զ������
			this.setParameter("attach",  "");
		
			//�û�ip
			this.setParameter("spbill_create_ip",  "");
		
			//��Ʒ����
			this.setParameter("desc",  "");
		
			//���б���
			this.setParameter("bank_type",  "0");
		
			//�ַ�������
			this.setParameter("cs","utf-8");
		
			//ժҪ
			this.setParameter("sign", "");
		}



		/**
	 * @Override
	 * ����ǩ��
	 */
		protected override void createSign() 
		{
		
			//��ȡ����
			String cmdno = getParameter("cmdno");
			String date = getParameter("date");
			String bargainor_id = getParameter("bargainor_id");
			String transaction_id = getParameter("transaction_id");
			String sp_billno = getParameter("sp_billno");
			String total_fee = getParameter("total_fee");
			String fee_type = getParameter("fee_type");
			String return_url = getParameter("return_url");
			String attach = getParameter("attach");
			String spbill_create_ip = getParameter("spbill_create_ip");
			String key = getParameter("key");
		
			//��֯ǩ��
			StringBuilder sb = new StringBuilder();
			sb.Append("cmdno=" + cmdno + "&");
			sb.Append("date=" + date + "&");
			sb.Append("bargainor_id=" + bargainor_id + "&");
			sb.Append("transaction_id=" + transaction_id + "&");
			sb.Append("sp_billno=" + sp_billno + "&");
			sb.Append("total_fee=" + total_fee + "&");
			sb.Append("fee_type=" + fee_type + "&");
			sb.Append("return_url=" + return_url + "&");
			sb.Append("attach=" + attach + "&");
			if( !"".Equals(spbill_create_ip) ) 
			{
				sb.Append("spbill_create_ip=" + spbill_create_ip + "&");
			}
			sb.Append("key=" + getKey());
		
			//���ժҪ
			String sign = MD5Util.GetMD5(sb.ToString(),getCharset());
				
			setParameter("sign", sign);
	
			//debug��Ϣ
			setDebugInfo(sb.ToString() + " => sign:"  + sign);
		
		}

	}
}
