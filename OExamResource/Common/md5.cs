using System;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
	internal class md5
	{
		public md5()
		{
		}

		public static string encrypt(string str)
		{
			string cl = str;
			string pwd = "";
			MD5 md5 = MD5.Create();
			byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
			for (int i = 0; i < (int)s.Length; i++)
			{
				pwd = string.Concat(pwd, s[i].ToString("X"));
			}
			return pwd;
		}
	}
}