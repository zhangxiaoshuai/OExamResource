using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
	public class Encrypt
	{
		public Encrypt()
		{
		}

		public static string GetMD5(string sDataIn)
		{
			MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
			byte[] bytValue = Encoding.UTF8.GetBytes(sDataIn);
			byte[] bytHash = md5.ComputeHash(bytValue);
			md5.Clear();
			string sTemp = "";
			for (int i = 0; i < (int)bytHash.Length; i++)
			{
				sTemp = string.Concat(sTemp, bytHash[i].ToString("x").PadLeft(2, '0'));
			}
			return sTemp;
		}

		public static string MD5Value(string filepath)
		{
			byte[] md5ch;
			MD5 md5 = new MD5CryptoServiceProvider();
			FileStream fs = File.OpenRead(filepath);
			try
			{
				md5ch = md5.ComputeHash(fs);
			}
			finally
			{
				if (fs != null)
				{
					((IDisposable)fs).Dispose();
				}
			}
			md5.Clear();
			string strMd5 = "";
			for (int i = 0; i < (int)md5ch.Length - 1; i++)
			{
				strMd5 = string.Concat(strMd5, md5ch[i].ToString("x").PadLeft(2, '0'));
			}
			return strMd5;
		}
	}
}