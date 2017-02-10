using Common;
using Models;
using Providers;
using System;
using System.Collections;
using System.Web;
using System.Web.Mvc;

namespace Controllers
{
	[AttributeUsage(AttributeTargets.All, AllowMultiple=true)]
	public class WebFilter : ActionFilterAttribute, IExceptionFilter
	{
		public static Exception Ex
		{
			get
			{
				Exception ex = new Exception("未登录或没有权限");
				ex.Data.Add("Code", 0);
				return ex;
			}
		}

		public WebFilter()
		{
		}

		private bool IsAjax(HttpContextBase filterContext)
		{
			return AjaxRequestExtensions.IsAjaxRequest(filterContext.Request);
		}

		private bool IsUpload(HttpContextBase filterContext)
		{
			return filterContext.Request.Files.Count > 0;
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			filterContext.HttpContext.Request.Url.ToString();
			if (!(filterContext.HttpContext.Session["user"] is UserIdentity))
			{
				string ip = filterContext.HttpContext.Request.UserHostAddress;
				Ip model = IpProvider.Instance.GetEntity(IpHelper.ParseByte(ip));
				if (model == null)
				{
					throw WebFilter.Ex;
				}
				if (DateTime.Parse(SchoolProvider.Instance.GetEntity(model.SchoolId).EndTime.ToString()).CompareTo(DateTime.Parse(DateTime.Now.ToString())) < 0)
				{
					model = null;
				}
				if (model == null)
				{
					throw WebFilter.Ex;
				}
				UserIdentity userIdentity = new UserIdentity()
				{
					SchoolId = model.SchoolId
				};
				filterContext.HttpContext.Session["user"] = userIdentity;
			}
			base.OnActionExecuting(filterContext);
		}

		public void OnException(ExceptionContext filterContext)
		{
			if ((!filterContext.Exception.Data.Contains("Code") ? false : (int)filterContext.Exception.Data["Code"] == 0))
			{
				filterContext.HttpContext.Response.Clear();
				filterContext.HttpContext.Response.StatusCode = 500;
				filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
				filterContext.ExceptionHandled = true;
				this.ReturnUrl(filterContext.HttpContext, "/Account");
			}
		}

		private void ReturnUrl(HttpContextBase filterContext, string url)
		{
			if ((this.IsUpload(filterContext) ? false : !this.IsAjax(filterContext)))
			{
				filterContext.Response.Redirect(url, true);
			}
			else
			{
				HttpContext.Current.Response.Write(string.Concat("<script>window.location.href='", url, "';</script>"));
			}
			filterContext.Response.End();
		}
	}
}