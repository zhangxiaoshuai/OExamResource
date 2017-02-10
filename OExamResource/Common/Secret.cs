using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
	public class Secret
	{
		public Secret()
		{
		}

		public string MD5Decrypt(string sKey, string sValue)
		{
			DESCryptoServiceProvider des = new DESCryptoServiceProvider();
			byte[] inputByteArray = new byte[sValue.Length / 2];
			for (int x = 0; x < sValue.Length / 2; x++)
			{
				int i = Convert.ToInt32(sValue.Substring(x * 2, 2), 16);
				inputByteArray[x] = (byte)i;
			}
			des.Key = Encoding.ASCII.GetBytes(sKey);
			des.IV = Encoding.ASCII.GetBytes(sKey);
			MemoryStream ms = new MemoryStream();
			CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
			cs.Write(inputByteArray, 0, (int)inputByteArray.Length);
			cs.FlushFinalBlock();
			StringBuilder ret = new StringBuilder();
			return Encoding.Default.GetString(ms.ToArray());
		}

		public string MD5Encrypt(string sKey, string sValue)
		{
			DESCryptoServiceProvider des = new DESCryptoServiceProvider();
			byte[] inputByteArray = Encoding.Default.GetBytes(sValue);
			des.Key = Encoding.ASCII.GetBytes(sKey);
			des.IV = Encoding.ASCII.GetBytes(sKey);
			MemoryStream ms = new MemoryStream();
			CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
			cs.Write(inputByteArray, 0, (int)inputByteArray.Length);
			cs.FlushFinalBlock();
			StringBuilder ret = new StringBuilder();
			byte[] array = ms.ToArray();
			for (int i = 0; i < (int)array.Length; i++)
			{
				ret.AppendFormat("{0:X2}", array[i]);
			}
			ret.ToString();
			return ret.ToString();
		}
	}
}