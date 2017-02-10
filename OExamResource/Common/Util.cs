using System;
using System.Text;

namespace Common
{
	public class Util
	{
		public Util()
		{
		}

		public static string GetEN(string strInput, string replace = null)
		{
			int iLen = strInput.Length;
			string strReVal = "";
			string temp = "";
			for (int i = 0; i < iLen; i++)
			{
				temp = Util.GetSpell(strInput.Substring(i, 1)).ToUpper();
				if (replace != null)
				{
					if ((Convert.ToChar(temp) < 'A' ? true : Convert.ToChar(temp) > 'Z'))
					{
						temp = replace;
					}
				}
				strReVal = string.Concat(strReVal, temp);
			}
			return strReVal;
		}

		public static string GetFirst(string str, string replace = null)
		{
			string eN;
			if (!string.IsNullOrEmpty(str))
			{
				eN = Util.GetEN(str.Substring(0, 1), replace);
			}
			else
			{
				eN = (replace == null ? "" : replace);
			}
			return eN;
		}

		private static string GetGbkX(string str)
		{
			string str1;
			if (str.CompareTo("吖") < 0)
			{
				str1 = str;
			}
			else if (str.CompareTo("八") < 0)
			{
				str1 = "A";
			}
			else if (str.CompareTo("嚓") < 0)
			{
				str1 = "B";
			}
			else if (str.CompareTo("咑") < 0)
			{
				str1 = "C";
			}
			else if (str.CompareTo("妸") < 0)
			{
				str1 = "D";
			}
			else if (str.CompareTo("发") < 0)
			{
				str1 = "E";
			}
			else if (str.CompareTo("旮") < 0)
			{
				str1 = "F";
			}
			else if (str.CompareTo("铪") < 0)
			{
				str1 = "G";
			}
			else if (str.CompareTo("讥") < 0)
			{
				str1 = "H";
			}
			else if (str.CompareTo("咔") < 0)
			{
				str1 = "J";
			}
			else if (str.CompareTo("垃") < 0)
			{
				str1 = "K";
			}
			else if (str.CompareTo("嘸") < 0)
			{
				str1 = "L";
			}
			else if (str.CompareTo("拏") < 0)
			{
				str1 = "M";
			}
			else if (str.CompareTo("噢") < 0)
			{
				str1 = "N";
			}
			else if (str.CompareTo("妑") < 0)
			{
				str1 = "O";
			}
			else if (str.CompareTo("七") < 0)
			{
				str1 = "P";
			}
			else if (str.CompareTo("亽") < 0)
			{
				str1 = "Q";
			}
			else if (str.CompareTo("仨") < 0)
			{
				str1 = "R";
			}
			else if (str.CompareTo("他") < 0)
			{
				str1 = "S";
			}
			else if (str.CompareTo("哇") < 0)
			{
				str1 = "T";
			}
			else if (str.CompareTo("夕") < 0)
			{
				str1 = "W";
			}
			else if (str.CompareTo("丫") < 0)
			{
				str1 = "X";
			}
			else if (str.CompareTo("帀") >= 0)
			{
				str1 = (str.CompareTo("咗") >= 0 ? str : "Z");
			}
			else
			{
				str1 = "Y";
			}
			return str1;
		}

		public static string GetSpell(string strNn)
		{
			string str;
			byte[] arrCN = Encoding.Default.GetBytes(strNn);
			if ((int)arrCN.Length <= 1)
			{
				str = (strNn.CompareTo("?") <= 0 ? strNn : Util.GetGbkX(strNn));
			}
			else
			{
				int strArea = arrCN[0];
				int iCode = (strArea << 8) + arrCN[1];
				int[] iAreacode = new int[] { 45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614, 48119, 48119, 49062, 49324, 49896, 50371, 50614, 50622, 50906, 51387, 51446, 52218, 52698, 52698, 52698, 52980, 53689, 54481 };
				int i = 0;
				while (i < 26)
				{
					int iMax = 55290;
					if (i != 25)
					{
						iMax = iAreacode[i + 1];
					}
					if ((iAreacode[i] > iCode ? true : iCode >= iMax))
					{
						i++;
					}
					else
					{
						Encoding @default = Encoding.Default;
						byte[] numArray = new byte[] { (byte)(65 + i) };
						str = @default.GetString(numArray);
						return str;
					}
				}
				str = (strNn.CompareTo("?") <= 0 ? "?" : Util.GetGbkX(strNn));
			}
			return str;
		}
	}
}