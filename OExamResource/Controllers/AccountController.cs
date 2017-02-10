using Microsoft.CSharp.RuntimeBinder;
using Common;
using Models;
using Providers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Controllers
{
	public class AccountController : BaseController
	{
		public AccountController()
		{
		}

        public JsonResult Login(string name, string pwd, string path)
        {
            JsonResult jsonResult;
            int? type;
            bool flag = false;
            UserInfo model = UserInfoProvider.Instance.GetEntity(name, Encrypt.GetMD5(pwd));
            if (model == null)
            {
                flag = true;
            }
            
            if (flag)
            {
                jsonResult = base.Json(new { State = 0, Msg = "登录失败", Result = "" });
            }
            else
            {
                School school = SchoolProvider.Instance.GetEntity(model.SchoolId);
                if (school == null)
                {
                    jsonResult = base.Json(new { State = 0, Msg = "登录失败", Result = "" });
                }
                else if (DateTime.Parse(school.EndTime.ToString()).CompareTo(DateTime.Parse(DateTime.Now.ToString())) >= 0)
                {
                    UserIdentity userIdentity = new UserIdentity()
                    {
                        Id = model.Id,
                        Name = (string.IsNullOrWhiteSpace(model.Name) ? model.LoginName : model.Name)
                    };
                    UserIdentity nullable = userIdentity;
                    type = model.Type;
                    nullable.Type = new int?((type.HasValue ? type.GetValueOrDefault() : 1));
                    userIdentity.SchoolId = model.SchoolId;
                    UserIdentity user = userIdentity;
                    List<Module> list = ModuleProvider.Instance.GetList(user.SchoolId);
                    List<string> strs = new List<string>();
                    list.ForEach((Module p) => strs.Add(p.Code.ToLower()));
                    user.Modules = strs;
                    base.CurrentUser = user;
                    path = path.TrimStart(new char[] { '/' });
                    if (string.IsNullOrEmpty(path))
                    {
                        path = "Resource";
                    }
                    if (user.Modules.Count > 0)
                    {
                        jsonResult = (!user.Modules.Contains(path.ToLower()) ? base.Json(new { State = 2, Msg = "登陆成功，跳转", Result = string.Concat("/", user.Modules[0]) }) : base.Json(new { State = 1, Msg = "登录成功", Result = user }));
                    }
                    else
                    {
                        jsonResult = base.Json(new { State = 2, Msg = "登陆成功，但没有权限", Result = "/Account" });
                    }
                }
                else
                {
                    ViewBag.SchoolEnd = true;
                    ViewBag.Name = school.Name;
                    jsonResult = base.Json(new { State = 2, Msg = "登陆成功，但该学校到期", Result = "/Account" });
                }
            }
            return jsonResult;
        }

        public JsonResult LogOut()
        {
            base.CurrentUser = null;
            JsonResult jsonResult = base.Json(new { State = 1, Msg = "退出登录", Result = "" });
            return jsonResult;
        }

        [UserFilter(Order = 1)]
        [WebFilter(Order = 0)]
        public ViewResult Manage()
        {
            ViewBag.LayModule = ModuleProvider.Instance.GetList(base.CurrentUser.SchoolId);
            return base.View(base.CurrentUser);
        }

        public ViewResult Index()
		{
			ViewResult viewResult;
			base.CurrentUser = null;
			string ip = HttpContext.Request.UserHostAddress;
			Ip model = IpProvider.Instance.GetEntity(IpHelper.ParseByte(ip));
			if (model != null)
			{
				School school = SchoolProvider.Instance.GetEntity(model.SchoolId);
				if (school == null)
				{
					viewResult = base.View();
					return viewResult;
				}
				if (DateTime.Parse(school.EndTime.ToString()).CompareTo(DateTime.Parse(DateTime.Now.ToString())) >= 0)
				{
					List<Module> list = ModuleProvider.Instance.GetList(model.SchoolId);
					if (list.Count > 0)
					{
						ViewBag.Name = school.Name;
						ViewBag.Url = string.Concat("/", list[0].Code);
					}
				}
				else
				{
					ViewBag.SchoolEnd = true;
					ViewBag.Name = school.Name;
				}
			}
			viewResult = base.View();
			return viewResult;
		}

        public JsonResult GetIdentity()
        {
            JsonResult jsonResult;
            jsonResult = (!base.IsLoginWeb() ? base.Json(new { State = 0, Msg = "登录信息", Result = "" }) : base.Json(new { State = 1, Msg = "登录信息", Result = base.CurrentUser }));
            return jsonResult;
        }
    }
}