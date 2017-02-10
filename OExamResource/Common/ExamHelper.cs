using System;

namespace Common
{
    public class ExamHelper
	{
		public ExamHelper()
		{
		}

		public static string NumToLetter(int i)
		{
			return Convert.ToString((char)(i + 65));
		}

		public static string ShowAnswer(Enums.ExamOption option, string answer)
		{
			string str;
			if (option != Enums.ExamOption.Boolen)
			{
				str = answer;
			}
			else
			{
				str = (answer.Trim() == "A" ? "√" : "×");
			}
			return str;
		}

		public static string ToHtmlFile(string code, string k5url)
		{
			string mp3Start = string.Concat("<embed src='/Scripts/plugin/mp3Plugin/player01.swf?soundFile=", k5url);
			string mp3End = "&amp;autostart=no&amp;loop=no' width='290' height='24' type='application/x-shockwave-flash' wmode='transparent'>";
			string imgStart = string.Concat("<img src='", k5url);
			string imgEnd = "' />";
			string[] strArrays = new string[] { "}}" };
			string[] strs = code.Split(strArrays, StringSplitOptions.RemoveEmptyEntries);
			string html = "";
			string[] strArrays1 = strs;
			for (int i = 0; i < (int)strArrays1.Length; i++)
			{
				string item = strArrays1[i];
				if (item.Contains("{{"))
				{
					item = (!item.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase) ? string.Concat(item.Replace("{{", imgStart), imgEnd) : string.Concat(item.Replace("{{", mp3Start), mp3End));
				}
				else if (item.Contains("src="))
				{
					item = item.Replace("<IMG src='\"", imgStart);
				}
				html = string.Concat(html, item);
			}
			return html;
		}

		public static string ToHtmlOption(Enums.ExamOption option, int count, Guid id)
		{
			int i;
			object[] letter;
			object obj;
			string html = "";
			switch (option)
			{
				case Enums.ExamOption.Text:
				{
					html = "<input type='text' class='a_txt'/>";
					break;
				}
				case Enums.ExamOption.Boolen:
				{
					letter = new object[] { "<input type='radio' value='A' id='1_", id, "' name='", id, "' class='a_rd'/><label for='1_", id, "'>√</label><input type='radio' value='B' id='0_", id, "' name='", id, "' class='a_rd'/><label for='0_", id, "'>×</label>" };
					html = string.Concat(letter);
					break;
				}
				case Enums.ExamOption.Single:
				{
					for (i = 0; i < count; i++)
					{
						obj = html;
						letter = new object[] { obj, "<input type='radio' value='", ExamHelper.NumToLetter(i), "' id='", ExamHelper.NumToLetter(i), "_", id, "' name='", id, "' class='a_rd'/><label for='", ExamHelper.NumToLetter(i), "_", id, "'>", ExamHelper.NumToLetter(i), "</label>" };
						html = string.Concat(letter);
					}
					break;
				}
				case Enums.ExamOption.Multiple:
				{
					for (i = 0; i < count; i++)
					{
						obj = html;
						letter = new object[] { obj, "<input type='checkbox' value='", ExamHelper.NumToLetter(i), "' id='", ExamHelper.NumToLetter(i), "_", id, "' class='a_ck' /><label for='", ExamHelper.NumToLetter(i), "_", id, "'>", ExamHelper.NumToLetter(i), "</label>" };
						html = string.Concat(letter);
					}
					break;
				}
				case Enums.ExamOption.Select:
				{
					html = "<select class='a_ddl'>";
					for (i = 0; i < count; i++)
					{
						string str = html;
						string[] strArrays = new string[] { str, "<option value='", ExamHelper.NumToLetter(i), "'>", ExamHelper.NumToLetter(i), "</option>" };
						html = string.Concat(strArrays);
					}
					html = string.Concat(html, "</select>");
					break;
				}
			}
			return html;
		}
	}
}