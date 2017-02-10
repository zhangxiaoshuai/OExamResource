using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Common
{
	public static class EnumCH
	{
		public static string GetName(this Enum en)
		{
			string description;
			MemberInfo[] memberInfos = en.GetType().GetMember(en.ToString());
			if ((memberInfos == null ? false : (int)memberInfos.Length > 0))
			{
				object[] objs = memberInfos[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
				if ((objs == null ? false : (int)objs.Length > 0))
				{
					description = ((DescriptionAttribute)objs[0]).Description;
					return description;
				}
			}
			description = en.ToString();
			return description;
		}
	}
}