using Microsoft.CSharp.RuntimeBinder;
using Common;
using Models;
using Providers;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;

namespace Controllers
{
	[ModuleFilter(Order=1)]
	[WebFilter(Order=0)]
	public class QualityCourseController : BaseController
	{
		public QualityCourseController()
		{
		}

		[HttpGet]
		public ViewResult Detail(Guid Id)
		{
			ViewBag.Id = Id;
			ViewBag.Address = QualityCourseProvider.Instance.GetEntity(Id).Address;
			this.InsertLog(Id, base.CurrentUser.SchoolId, (base.IsLoginWeb() ? base.CurrentUser.Id : Guid.Empty), HttpContext.Request.UserHostAddress, 2, 2);
			return base.View();
		}

		public string ExList(string filename)
		{
			string str;
			if (!string.IsNullOrEmpty(filename))
			{
				try
				{
					filename = string.Concat("/k3/ico/", filename);
					if (!FileHelper.IsExists(Server.MapPath(filename)))
					{
						filename = "../../../Images/default.jpg";
					}
					else
					{
						str = filename;
						return str;
					}
				}
				catch (WebException webException)
				{
					filename = "../../../Images/default.jpg";
				}
			}
			else
			{
				filename = "../../../Images/default.jpg";
			}
			str = filename;
			return str;
		}

		[HttpPost]
		public JsonResult Get_list(string searchname, Guid id, int type, int pageindex, string jibie, string niandu)
		{
			if (searchname == null)
			{
				searchname = "";
			}
			if (jibie == null)
			{
				jibie = "";
			}
			if (niandu == null)
			{
				niandu = "";
			}
			List<QualityCourse> list = QualityCourseProvider.Instance.GetResource(searchname, id, type, pageindex, 10, jibie, niandu);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].CourceSort == "国家级精品课程")
				{
					list[i].CourceSort = "guo";
				}
				else if (list[i].CourceSort == "省级精品课程")
				{
					list[i].CourceSort = "sheng";
				}
				else if (list[i].CourceSort == "校级精品课程")
				{
					list[i].CourceSort = "xiao";
				}
				list[i].Tag2 = this.ExList(list[i].Tag2);
			}
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "ok", Result = list });
			return jsonResult;
		}

		[HttpPost]
		public JsonResult GetDetail(Guid id)
		{
			QualityDetail qd = QualityCourseProvider.Instance.GetDetail(id);
			qd.Tag2 = this.ExList(qd.Tag2);
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "ok", Result = qd });
			return jsonResult;
		}

		public JsonResult GetList()
		{
			List<QualitySubject> list = QualitySubjectProvider.Instance.GetList();
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "精品课程大类列表", Result = list });
			return jsonResult;
		}

		[HttpPost]
		public JsonResult Getnav(Guid Id)
		{
			QualityNav qn = QualityCourseProvider.Instance.GetNav(Id);
			List<object> list = new List<object>();
			if (qn.Subject != null)
			{
				list.Add(new { Id = qn.subid, Name = qn.Subject, Count = qn.count, type = 0 });
			}
			if (qn.Subject1 != null)
			{
				list.Add(new { Id = qn.subid1, Name = qn.Subject1, Count = qn.count1, type = 1 });
			}
			if (qn.Subject2 != null)
			{
				list.Add(new { Id = qn.subid2, Name = qn.Subject2, Count = qn.count2, type = 2 });
			}
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "ok", Result = list });
			return jsonResult;
		}

		[HttpPost]
		public JsonResult Getnavigation(string searchname, Guid id, int type, string jibie, string niandu)
		{
			if (string.IsNullOrEmpty(searchname))
			{
				searchname = "";
			}
			if (jibie == null)
			{
				jibie = "";
			}
			if (niandu == null)
			{
				niandu = "";
			}
			QualitySubject cate = null;
			QualitySubject1 sub = null;
			QualitySubject2 spe = null;
			List<object> list = new List<object>();
			Guid gid = new Guid();
			if (!(searchname != ""))
			{
				switch (type)
				{
					case 0:
					{
						cate = QualitySubjectProvider.Instance.GetEntity(id);
						break;
					}
					case 1:
					{
						sub = QualitySubject1Provider.Instance.GetEntity(id);
						cate = QualitySubjectProvider.Instance.GetEntity(sub.SubjectId);
						break;
					}
					case 2:
					{
						spe = QualitySubject2Provider.Instance.GetEntity(id);
						sub = QualitySubject1Provider.Instance.GetEntity(spe.Subject1id);
						cate = QualitySubjectProvider.Instance.GetEntity(sub.SubjectId);
						break;
					}
				}
				if (cate != null)
				{
					list.Add(new { Id = cate.Id, Name = (cate.Name.Substring(cate.Name.Length - 1, 1) == "类" ? cate.Name.Substring(0, cate.Name.Length - 1) : cate.Name), Count = cate.Count, Type = 0 });
				}
				if (sub != null)
				{
					list.Add(new { Id = sub.Id, Name = (sub.Name.Substring(sub.Name.Length - 1, 1) == "类" ? sub.Name.Substring(0, sub.Name.Length - 1) : sub.Name), Count = sub.Count, Type = 1 });
				}
				if (spe != null)
				{
					list.Add(new { Id = spe.Id, Name = (spe.Name.Substring(spe.Name.Length - 1, 1) == "类" ? spe.Name.Substring(0, spe.Name.Length - 1) : spe.Name), Count = spe.Count, Type = 2 });
				}
			}
			else
			{
				int count = QualityCourseProvider.Instance.GetResourceCount(searchname, id, 10, jibie, niandu);
				list.Add(new { Id = gid, Name = searchname, Count = count, Type = 10, searchname = searchname, jibie = jibie, niandu = niandu });
				switch (type)
				{
					case 1:
					{
						sub = QualityCourseProvider.Instance.GetSubject1(searchname, id, jibie, niandu);
						break;
					}
					case 2:
					{
						spe = QualityCourseProvider.Instance.GetSubject2(searchname, id, jibie, niandu);
						sub = QualityCourseProvider.Instance.GetSubject1(searchname, spe.Subject1id, jibie, niandu);
						break;
					}
				}
				if (sub != null)
				{
					list.Add(new { Id = sub.Id, Name = (sub.Name.Substring(sub.Name.Length - 1, 1) == "类" ? sub.Name.Substring(0, sub.Name.Length - 1) : sub.Name), Count = sub.Count, Type = 1, searchname = searchname, jibie = jibie, niandu = niandu });
				}
				if (spe != null)
				{
					list.Add(new { Id = spe.Id, Name = (spe.Name.Substring(spe.Name.Length - 1, 1) == "类" ? spe.Name.Substring(0, spe.Name.Length - 1) : spe.Name), Count = spe.Count, Type = 2, searchname = searchname, jibie = jibie, niandu = niandu });
				}
			}
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "ok", Result = list });
			return jsonResult;
		}

		[HttpPost]
		public JsonResult GetSubject(string searchname, Guid id, int type, string jibie, string niandu)
		{
			JsonResult jsonResult;
			if (string.IsNullOrEmpty(searchname))
			{
				searchname = "";
			}
			if (string.IsNullOrEmpty(jibie))
			{
				jibie = "选择级别";
			}
			if (string.IsNullOrEmpty(niandu))
			{
				niandu = "选择年度";
			}
			List<QualitySubject2> spe = null;
			List<QualitySubject1> sub = null;
			List<QualitySubject> cate = null;
			List<object> list = new List<object>();
			int num = type;
			switch (num)
			{
				case 0:
				{
					sub = QualityCourseProvider.Instance.GetSubject1list(searchname, id, jibie, niandu);
					break;
				}
				case 1:
				{
					spe = QualityCourseProvider.Instance.GetSubject2list(searchname, id, jibie, niandu);
					break;
				}
				case 2:
				{
					jsonResult = base.Json(new { State = 0, Msg = "no", Result = "" });
					return jsonResult;
				}
				default:
				{
					if (num == 10)
					{
						if (!(searchname != ""))
						{
							cate = QualitySubjectProvider.Instance.GetList();
						}
						else
						{
							sub = QualityCourseProvider.Instance.GetSubject1(searchname, jibie, niandu);
						}
						break;
					}
					else
					{
						break;
					}
				}
			}
			if (cate != null)
			{
				foreach (QualitySubject su in cate)
				{
					int? count = su.Count;
					//if ((count.GetValueOrDefault() <= 0 ? 0 : (int)count.HasValue) != 0)
					//{
					//	list.Add(new { Id = su.Id, Name = (su.Name.Substring(su.Name.Length - 1, 1) == "类" ? su.Name.Substring(0, su.Name.Length - 1) : su.Name), Count = su.Count, type = 0, searchname = searchname, jibie = jibie, niandu = niandu });
					//}
                    if(count != 0)
                    {
                        list.Add(new { Id = su.Id, Name = (su.Name.Substring(su.Name.Length - 1, 1) == "类" ? su.Name.Substring(0, su.Name.Length - 1) : su.Name), Count = su.Count, type = 0, searchname = searchname, jibie = jibie, niandu = niandu });
                    }
				}
			}
			if (sub != null)
			{
				foreach (QualitySubject1 su in sub)
				{
					if (su.Count > 0)
					{
						list.Add(new { Id = su.Id, Name = (su.Name.Substring(su.Name.Length - 1, 1) == "类" ? su.Name.Substring(0, su.Name.Length - 1) : su.Name), Count = su.Count, type = 1, searchname = searchname, jibie = jibie, niandu = niandu });
					}
				}
			}
			if (spe != null)
			{
				foreach (QualitySubject2 sp in spe)
				{
					if (sp.Count > 0)
					{
						string spname = sp.Name;
						int namecount = sp.Name.Length;
						spname = (spname.Substring(namecount - 1, 1) == "类" ? spname.Substring(0, namecount - 1) : spname);
						list.Add(new { Id = sp.Id, Name = spname, Count = sp.Count, type = 2, searchname = searchname, jibie = jibie, niandu = niandu });
					}
				}
			}
			jsonResult = base.Json(new { State = 1, Msg = "ok", Result = list });
			return jsonResult;
		}

		[HttpPost]
		public JsonResult GetUrlIsExists(string address)
		{
			JsonResult jsonResult;
			jsonResult = (!(new UrlIsExist()).CheckUrlVisit(address) ? base.Json(new { State = 0, Msg = "no", Result = 0 }) : base.Json(new { State = 1, Msg = "ok", Result = 1 }));
			return jsonResult;
		}

		private static List<SelectListItem> Htmljibie()
		{
			List<QualityCourceSort> list = QualityCourseProvider.Instance.Getjibie();
			List<SelectListItem> selectListItems = new List<SelectListItem>();
			SelectListItem selectListItem = new SelectListItem();
			selectListItem.Text =  "选择级别";
			selectListItem.Value = "选择级别";
			selectListItem.Selected = true;
			selectListItems.Add(selectListItem);
			List<SelectListItem> ddljibie = selectListItems;
			for (int i = 0; i < list.Count; i++)
			{
				SelectListItem selectListItem1 = new SelectListItem();
				selectListItem1.Text=list[i].CourceSort;
				selectListItem1.Value=list[i].CourceSort;
				ddljibie.Add(selectListItem1);
			}
			return ddljibie;
		}

		private static List<SelectListItem> Htmlniandu()
		{
			List<QualityAwardsDate> list = QualityCourseProvider.Instance.Getniandu();
			List<SelectListItem> selectListItems = new List<SelectListItem>();
			SelectListItem selectListItem = new SelectListItem();
			selectListItem.Text="选择年度";
			selectListItem.Value="选择年度";
			selectListItem.Selected=true;
			selectListItems.Add(selectListItem);
			List<SelectListItem> ddlniandu = selectListItems;
			for (int i = 0; i < list.Count; i++)
			{
				SelectListItem selectListItem1 = new SelectListItem();
				selectListItem1.Text=string.Concat(list[i].AwardsDate, "年");
				selectListItem1.Value=string.Concat(list[i].AwardsDate, "年");
				ddlniandu.Add(selectListItem1);
			}
			return ddlniandu;
		}

		public ViewResult Index()
		{
			dynamic model = new ExpandoObject();
			model.ddljibie = QualityCourseController.Htmljibie();
			model.ddlniandu = QualityCourseController.Htmlniandu();
			return (ViewResult)this.View(model);
		}

		public void InsertLog(Guid sourceId, Guid schoolid, Guid teacherId, string ip, int status, int ku)
		{
			LogDetail log = new LogDetail()
			{
				Id = Guid.NewGuid(),
				ResourceId = sourceId,
				SchoolId = new Guid?(schoolid),
				TeacherId = new Guid?(teacherId),
				Ip = ip,
				Status = status,
				Tag1 = ku.ToString(),
				CreateTime = DateTime.Now,
				IsDeleted = false
			};
			LogDetailProvider.Instance.Create(log);
		}

		[HttpGet]
		public ViewResult List(string searchname, Guid id, int type, string jibie, string niandu)
		{
			if (string.IsNullOrEmpty(searchname))
			{
				searchname = "";
			}
			if (string.IsNullOrEmpty(jibie))
			{
				jibie = "选择级别";
			}
			if (string.IsNullOrEmpty(niandu))
			{
				niandu = "选择年度";
			}
			ViewBag.searchname = searchname;
			ViewBag.jibie = jibie;
			ViewBag.niandu = niandu;
			ViewBag.id = id;
			ViewBag.type = type;
			dynamic model = new ExpandoObject();
			model.ddljibie = QualityCourseController.Htmljibie();
			model.ddlniandu = QualityCourseController.Htmlniandu();
			ViewBag.count = QualityCourseProvider.Instance.GetResourceCount(searchname, id, type, jibie, niandu);
			return (ViewResult)this.View(model);
		}
	}
}