using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace Common
{
	public class IpHelper
	{
		public IpHelper()
		{
		}

		public static bool CheckIp(string ip)
		{
			return (new Regex("((?:(?:25[0-5]|2[0-4]\\d|((1\\d{2})|([1-9]?\\d)))\\.){3}(?:25[0-5]|2[0-4]\\d|((1\\d{2})|([1-9]?\\d))))")).IsMatch(ip);
		}

		public static List<string> IpToList(string ips)
		{
			char[] chrArray = new char[] { ',', '-' };
			return ips.Split(chrArray, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
		}

		public static byte[] ParseByte(string ip)
		{
			return IPAddress.Parse(ip).GetAddressBytes();
		}

		public static string ParseIp(byte[] b)
		{
			string str;
			if ((b == null ? false : (int)b.Length >= 0))
			{
				string ipstr = "";
				byte[] numArray = b;
				for (int i = 0; i < (int)numArray.Length; i++)
				{
					byte _b = numArray[i];
					ipstr = string.Concat(ipstr, _b.ToString(), ".");
				}
				str = ipstr.TrimEnd(new char[] { '.' });
			}
			else
			{
				str = "";
			}
			return str;
		}
	}
}