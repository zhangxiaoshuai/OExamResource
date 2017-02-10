using System;
using System.Web.Script.Serialization;

namespace Common
{
	public class JsonHelper
	{
		public JsonHelper()
		{
		}

		public static T Deserialize<T>(string input)
		{
			return (new JavaScriptSerializer()).Deserialize<T>(input);
		}

		public static string Serialize(object obj)
		{
			return (new JavaScriptSerializer()).Serialize(obj);
		}
	}
}