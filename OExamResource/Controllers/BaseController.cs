using Common;
using Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Controllers
{
	public class BaseController : Controller
	{
        public const string COOKNAME = "resourcecook";
		public UserIdentity CurrentAdmin
		{
			get
			{
				return HttpContext.Session["admin"] as UserIdentity;
			}
			set
			{
				HttpContext.Session["admin"] = value;
			}
		}

		public UserIdentity CurrentUser
		{
			get
			{
				return HttpContext.Session["user"] as UserIdentity;
			}
			set
			{
				HttpContext.Session["user"] = value;
			}
		}

		public BaseController()
		{
		}

		public List<ExamCollect> GetPaperCollect()
		{
			HttpCookie cookie = Request.Cookies.Get(COOKNAME);
			List<ExamCollect> list = new List<ExamCollect>();
			if ((cookie == null ? false : !string.IsNullOrEmpty(cookie["CollectPaper"])))
			{
				list = JsonHelper.Deserialize<List<ExamCollect>>(cookie["CollectPaper"]);
			}
			return list;
		}

		public bool IsLoginAdmin()
		{
			bool flag;
			if (this.CurrentAdmin == null)
			{
				flag = false;
			}
			else
			{
				int? type = this.CurrentAdmin.Type;
				flag = (type.GetValueOrDefault() != -1 ? false : type.HasValue);
			}
			return flag;
		}

		public bool IsLoginWeb()
		{
			return (this.CurrentUser == null ? false : !(this.CurrentUser.Id == Guid.Empty));
		}

		public bool IsTeacher()
		{
			bool flag;
			if (this.CurrentUser == null)
			{
				flag = false;
			}
			else
			{
				int? type = this.CurrentUser.Type;
				flag = (type.GetValueOrDefault() != 0 ? false : type.HasValue);
			}
			return flag;
		}

		public void SetPaperCollect(List<ExamCollect> list)
		{
			HttpCookie cookie = Request.Cookies.Get(COOKNAME) ?? new HttpCookie(COOKNAME);
			if (list.Count > 10)
			{
				list.RemoveRange(0, list.Count - 10);
			}
			cookie["CollectPaper"] = JsonHelper.Serialize(list);
			cookie.Expires = DateTime.Now.AddMonths(1);
			HttpContext.Response.Cookies.Set(cookie);
		}
	}
}