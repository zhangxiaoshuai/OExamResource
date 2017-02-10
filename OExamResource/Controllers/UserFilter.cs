using Models;
using System;
using System.Collections;
using System.Web;
using System.Web.Mvc;

namespace Controllers
{
	[AttributeUsage(AttributeTargets.All, AllowMultiple=true)]
	public class UserFilter : ActionFilterAttribute
	{
		public static Exception Ex
		{
			get
			{
				Exception ex = new Exception("没有登录账号");
				ex.Data.Add("Code", 0);
				return ex;
			}
		}

		public UserFilter()
		{
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if ((filterContext.HttpContext.Session["user"] as UserIdentity).Id == Guid.Empty)
			{
				throw UserFilter.Ex;
			}
			base.OnActionExecuting(filterContext);
		}
	}
}