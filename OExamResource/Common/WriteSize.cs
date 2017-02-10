using System;

namespace Common
{
	public class WriteSize
	{
		public WriteSize()
		{
		}

		public string WriteResourceSize(string str)
		{
			string str1;
			string temStr = "";
			if (!string.IsNullOrEmpty(str))
			{
				double i_size = double.Parse(str);
				if (i_size > 1073741824)
				{
					i_size = Math.Round(i_size / 1024 / 1024 / 1024, 2);
					temStr = string.Concat(i_size.ToString(), "G");
				}
				else if (i_size <= 1048576)
				{
					i_size = Math.Round(i_size / 1024, 2);
					temStr = string.Concat(i_size.ToString(), "K");
				}
				else
				{
					i_size = Math.Round(i_size / 1024 / 1024, 2);
					temStr = string.Concat(i_size.ToString(), "M");
				}
				str1 = temStr;
			}
			else
			{
				str1 = temStr;
			}
			return str1;
		}
	}
}