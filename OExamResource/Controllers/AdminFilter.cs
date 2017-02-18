using Models;
using System;
using System.Collections;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Controllers
{
	[AttributeUsage(AttributeTargets.All, AllowMultiple=true)]
	public class AdminFilter : ActionFilterAttribute, IExceptionFilter
	{
		public static Exception Ex
		{
			get
			{
				Exception ex = new Exception("超时或无权限");
				ex.Data.Add("Code", 0);
				return ex;
			}
		}

		public AdminFilter()
		{
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			bool flag = false;
			UserIdentity user = HttpContext.Current.Session["admin"] as UserIdentity;

            if (user == null)
                flag = false;
            else
            {
                if (user.Type != -1 && user.Type != 3 )
                {
                    if (user.Type == 5 || user.Type == 6)
                        flag = true;
                }
                   

            }

			if (!flag)
			{
				throw AdminFilter.Ex;
			}
			filterContext.HttpContext.Session["admin"] = user;
			base.OnActionExecuting(filterContext);
		}

		public void OnException(ExceptionContext filterContext)
		{
			if ((!filterContext.Exception.Data.Contains("Code") ? false : (int)filterContext.Exception.Data["Code"] == 0))
			{
				filterContext.HttpContext.Response.Clear();
				filterContext.HttpContext.Response.StatusCode = 500;
				filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
				filterContext.ExceptionHandled= true;
				this.ReturnUrl(filterContext.HttpContext, "/Admin/Login");
			}
		}

		private void ReturnUrl(HttpContextBase filterContext, string url)
		{
			if ((AjaxRequestExtensions.IsAjaxRequest(filterContext.Request) ? false : filterContext.Request.Files.Count <= 0))
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