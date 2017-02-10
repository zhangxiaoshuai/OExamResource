using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ZipClass.Help
{
	public class Secret
	{
		public Secret()
		{
		}

		public string MD5Decrypt(string sKey, string sValue)
		{
			DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
			byte[] numArray = new byte[sValue.Length / 2];
			for (int i = 0; i < sValue.Length / 2; i++)
			{
				int num = Convert.ToInt32(sValue.Substring(i * 2, 2), 16);
				numArray[i] = (byte)num;
			}
			dESCryptoServiceProvider.Key = Encoding.ASCII.GetBytes(sKey);
			dESCryptoServiceProvider.IV = Encoding.ASCII.GetBytes(sKey);
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateDecryptor(), CryptoStreamMode.Write);
			cryptoStream.Write(numArray, 0, (int)numArray.Length);
			cryptoStream.FlushFinalBlock();
			StringBuilder stringBuilder = new StringBuilder();
			return Encoding.Default.GetString(memoryStream.ToArray());
		}

		public string MD5Encrypt(string sKey, string sValue)
		{
			DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
			byte[] bytes = Encoding.Default.GetBytes(sValue);
			dESCryptoServiceProvider.Key = Encoding.ASCII.GetBytes(sKey);
			dESCryptoServiceProvider.IV = Encoding.ASCII.GetBytes(sKey);
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateEncryptor(), CryptoStreamMode.Write);
			cryptoStream.Write(bytes, 0, (int)bytes.Length);
			cryptoStream.FlushFinalBlock();
			StringBuilder stringBuilder = new StringBuilder();
			byte[] array = memoryStream.ToArray();
			for (int i = 0; i < (int)array.Length; i++)
			{
				stringBuilder.AppendFormat("{0:X2}", array[i]);
			}
			stringBuilder.ToString();
			return stringBuilder.ToString();
		}
	}
}