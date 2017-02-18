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

        [UserFilter(Order = 1)]
        [WebFilter(Order = 0)]
        public PartialViewResult AccInfo()
        {
            UserView model = UserInfoProvider.Instance.GetViewEntity(base.CurrentUser.Id);
            return base.PartialView(model);
        }

        [UserFilter(Order = 1)]
        [WebFilter(Order = 0)]
        public PartialViewResult EditPwd()
        {
            return base.PartialView();
        }

        [UserFilter(Order = 1)]
        [WebFilter(Order = 0)]
        public JsonResult RePwd(string opwd, string npwd)
        {
            JsonResult jsonResult;
            UserInfo model = UserInfoProvider.Instance.GetEntity(base.CurrentUser.Id);
            if (!(model.LoginPwd == Encrypt.GetMD5(opwd)))
            {
                jsonResult = base.Json(new { State = 0, Msg = "对不起，原密码错误。", Result = "" });
            }
            else
            {
                model.LoginPwd = Encrypt.GetMD5(npwd);
                UserInfoProvider.Instance.Update(model);
                jsonResult = base.Json(new { State = 1, Msg = "修改密码成功", Result = "" });
            }
            return jsonResult;
        }

        [UserFilter(Order = 1)]
        [WebFilter(Order = 0)]
        public ViewResult Collect()
        {
            bool k1 = base.CurrentUser.Modules.Contains("resource");
            bool k2 = base.CurrentUser.Modules.Contains("medical");
            bool k4 = base.CurrentUser.Modules.Contains("exam");
            bool k5 = base.CurrentUser.Modules.Contains("lan");
            List<int> list = new List<int>();
            list.Insert(0, (k1 ? Resource_SubjectProvider.Instance.CollectCount(base.CurrentUser.Id, 0) : -1));
            list.Insert(1, (k2 ? Resource_SubjectProvider.Instance.CollectCount(base.CurrentUser.Id, 1) : -1));
            list.Insert(2, (k4 ? ExamCollectProvider.Instance.GetCounByUser(base.CurrentUser.Id) : -1));
            list.Insert(3, (k5 ? LanCollectProvider.Instance.GetCount(base.CurrentUser.Id) : -1));
            return base.View(list);
        }

        [UserFilter(Order = 1)]
        [WebFilter(Order = 0)]
        public JsonResult CollectlistK1(Guid? id, int medicalTag, int pageIndex, int pageCount)
        {
            id = new Guid?(base.CurrentUser.Id);
            string down = WebConfigurationManager.AppSettings["Resource_Down_Url"];
            List<CollectSource> rlist = Resource_SubjectProvider.Instance.Collect(id, medicalTag, pageIndex, pageCount);
            for (int i = 0; i < rlist.Count; i++)
            {
                rlist[i].Icofilepath = string.Concat("/k1", rlist[i].Icofilepath);
                rlist[i].Previewfilepath = string.Concat("/k1", rlist[i].Previewfilepath);
                rlist[i].DownloadFilepath = string.Concat(down, rlist[i].DownloadFilepath);
            }
            JsonResult jsonResult = base.Json(new { State = 1, Msg = "ok", Result = rlist });
            return jsonResult;
        }
    }
}