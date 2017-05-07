using System;
using System.Security.Cryptography;  
using System.Text;
namespace CommonUtils
{
	public class DEncrypt
	{
		#region 使用 缺省密钥字符串 加密/解密String

		/// <summary>
		/// 使用缺省密钥字符串加密String
		/// </summary>
		/// <param name="original">明文</param>
		/// <returns>密文</returns>
		public static String Encrypt(String original)
		{
			return Encrypt(original,"123456789");
		}
		/// <summary>
		/// 使用缺省密钥字符串解密String
		/// </summary>
		/// <param name="original">密文</param>
		/// <returns>明文</returns>
		public static String Decrypt(String original)
		{
            return Decrypt(original, "123456789", Encoding.Default);
		}

		#endregion

		#region 使用 给定密钥字符串 加密/解密String
		/// <summary>
		/// 使用给定密钥字符串加密String
		/// </summary>
		/// <param name="original">原始文字</param>
		/// <param name="key">密钥</param>
		/// <param name="encoding">字符编码方案</param>
		/// <returns>密文</returns>
		public static String Encrypt(String original, String key)  
		{  
			Byte[] buff = Encoding.Default.GetBytes(original);  
			Byte[] kb = Encoding.Default.GetBytes(key);
			return Convert.ToBase64String(Encrypt(buff,kb));      
		}
		/// <summary>
		/// 使用给定密钥字符串解密String
		/// </summary>
		/// <param name="original">密文</param>
		/// <param name="key">密钥</param>
		/// <returns>明文</returns>
		public static String Decrypt(String original, String key)
		{
			return Decrypt(original,key,Encoding.Default);
		}

		/// <summary>
		/// 使用给定密钥字符串解密String,返回指定编码方式明文
		/// </summary>
		/// <param name="encrypted">密文</param>
		/// <param name="key">密钥</param>
		/// <param name="encoding">字符编码方案</param>
		/// <returns>明文</returns>
		public static String Decrypt(String encrypted, String key,Encoding encoding)  
		{       
			Byte[] buff = Convert.FromBase64String(encrypted);  
			Byte[] kb = Encoding.Default.GetBytes(key);
			return encoding.GetString(Decrypt(buff,kb));      
		}  
		#endregion

		#region 使用 缺省密钥字符串 加密/解密/Byte[]
		/// <summary>
		/// 使用缺省密钥字符串解密Byte[]
		/// </summary>
		/// <param name="encrypted">密文</param>
		/// <param name="key">密钥</param>
		/// <returns>明文</returns>
		public static Byte[] Decrypt(Byte[] encrypted)  
		{  
			Byte[] key = Encoding.Default.GetBytes("MATICSOFT"); 
			return Decrypt(encrypted,key);     
		}
		/// <summary>
		/// 使用缺省密钥字符串加密
		/// </summary>
		/// <param name="original">原始数据</param>
		/// <param name="key">密钥</param>
		/// <returns>密文</returns>
		public static Byte[] Encrypt(Byte[] original)  
		{  
			Byte[] key = Encoding.Default.GetBytes("MATICSOFT"); 
			return Encrypt(original,key);     
		}  
		#endregion

		#region  使用 给定密钥 加密/解密/Byte[]

		/// <summary>
		/// 生成MD5摘要
		/// </summary>
		/// <param name="original">数据源</param>
		/// <returns>摘要</returns>
		public static Byte[] MakeMD5(Byte[] original)
		{
			MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();   
			Byte[] keyhash = hashmd5.ComputeHash(original);       
			hashmd5 = null;  
			return keyhash;
		}


		/// <summary>
		/// 使用给定密钥加密
		/// </summary>
		/// <param name="original">明文</param>
		/// <param name="key">密钥</param>
		/// <returns>密文</returns>
		public static Byte[] Encrypt(Byte[] original, Byte[] key)  
		{  
			TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();       
			des.Key =  MakeMD5(key);
			des.Mode = CipherMode.ECB;  
     
			return des.CreateEncryptor().TransformFinalBlock(original, 0, original.Length);     
		}  

		/// <summary>
		/// 使用给定密钥解密数据
		/// </summary>
		/// <param name="encrypted">密文</param>
		/// <param name="key">密钥</param>
		/// <returns>明文</returns>
		public static Byte[] Decrypt(Byte[] encrypted, Byte[] key)  
		{  
			TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();  
			des.Key =  MakeMD5(key);    
			des.Mode = CipherMode.ECB;  

			return des.CreateDecryptor().TransformFinalBlock(encrypted, 0, encrypted.Length);
		}  
  
		#endregion

		

		
	}
}
