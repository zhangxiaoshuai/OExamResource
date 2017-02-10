using Microsoft.CSharp.RuntimeBinder;
using Models;
using Providers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;

namespace Controllers
{
	[AttributeUsage(AttributeTargets.All, AllowMultiple=true)]
	public class ModuleFilter : ActionFilterAttribute
	{
		public static Exception Ex
		{
			get
			{
				Exception ex = new Exception("没有模块权限");
				ex.Data.Add("Code", 0);
				return ex;
			}
		}

		public ModuleFilter()
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
			UserIdentity user = filterContext.HttpContext.Session["user"] as UserIdentity;
			string controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower();
			List<Module> list = null;
			if (user.Modules == null)
			{
				list = ModuleProvider.Instance.GetList(user.SchoolId);
				List<string> strs = new List<string>();
				list.ForEach((Module p) => strs.Add(p.Code.ToLower()));
				user.Modules = strs;
				filterContext.HttpContext.Session["user"] = user;
			}
			if (!user.Modules.Contains(controller))
			{
				throw ModuleFilter.Ex;
			}
			if ((this.IsAjax(filterContext.HttpContext) || this.IsUpload(filterContext.HttpContext) ? false : controller != "lan"))
			{
				list = list ?? ModuleProvider.Instance.GetList(user.SchoolId);
				((dynamic)filterContext.Controller.ViewBag).LayModule = list;
			}
			base.OnActionExecuting(filterContext);
		}
	}
}