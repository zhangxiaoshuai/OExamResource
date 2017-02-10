using System;
using System.Security.Cryptography;
using System.Text;

namespace ZipClass.Help
{
	internal class md5
	{
		public md5()
		{
		}

		public static string encrypt(string str)
		{
			string str1 = str;
			string str2 = "";
			MD5 mD5 = MD5.Create();
			byte[] numArray = mD5.ComputeHash(Encoding.UTF8.GetBytes(str1));
			for (int i = 0; i < (int)numArray.Length; i++)
			{
				str2 = string.Concat(str2, numArray[i].ToString("X"));
			}
			return str2;
		}
	}
}