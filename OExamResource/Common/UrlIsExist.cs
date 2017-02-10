using System;
using System.Net;

namespace Common
{
	public class UrlIsExist
	{
		public UrlIsExist()
		{
		}

		public bool CheckUrlVisit(string url)
		{
			bool flag;
			try
			{
				HttpWebResponse resp = (HttpWebResponse)((HttpWebRequest)WebRequest.Create(url)).GetResponse();
				if (resp.StatusCode == HttpStatusCode.OK)
				{
					resp.Close();
					flag = true;
					return flag;
				}
			}
			catch (WebException webException)
			{
				flag = false;
				return flag;
			}
			flag = false;
			return flag;
		}

		public bool UrlIsExists(string url)
		{
			bool flag;
			Uri u = null;
			try
			{
				u = new Uri(url);
			}
			catch
			{
				flag = false;
				return flag;
			}
			bool isExist = false;
			HttpWebRequest r = WebRequest.Create(u) as HttpWebRequest;
			r.Method = "HEAD";
			r.Timeout = 3000;
			try
			{
				if ((r.GetResponse() as HttpWebResponse).StatusCode == HttpStatusCode.OK)
				{
					isExist = true;
				}
			}
			catch (WebException webException)
			{
				isExist = false;
			}
			flag = isExist;
			return flag;
		}
	}
}