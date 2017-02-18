using Microsoft.CSharp.RuntimeBinder;
using Common;
using Models;
using Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;

namespace Controllers
{
	[AdminFilter]
	public class ResourceManageController : BaseController
	{
		public ResourceManageController()
		{
		}

		public JsonResult Audit(List<Guid> guidlist)
		{
			if (guidlist.Count > 0)
			{
				foreach (Guid id in guidlist)
				{
					ResourceMaterial resource = ResourceProvider.Instance.GetResourceMaterial(id);
					resource.ModifyTime = new DateTime?(DateTime.Now);
					resource.IsDeleted = false;
					ResourceMaterialProvider.Instance.Update(resource);
				}
			}
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = "" });
			return jsonResult;
		}

		public JsonResult Category(Guid? cateid, string flag, string name)
		{
			JsonResult jsonResult;
			if (string.IsNullOrEmpty(name))
			{
				jsonResult = base.Json(new { State = 0, Msg = "为空或未选中！", Result = "" });
			}
			else if (flag == "add")
			{
				if (CategoryProvider.Instance.GetNameBool(name))
				{
					jsonResult = base.Json(new { State = 0, Msg = "名称与其他项重复！", Result = "" });
				}
				else
				{
					Category newCate = new Category()
					{
						Id = Guid.NewGuid(),
						Name = name,
						CreateTime = DateTime.Now,
						IsDeleted = false
					};
					CategoryProvider.Instance.Create(newCate);
					jsonResult = base.Json(new { State = 1, Msg = "添加成功！", Result = newCate.Id });
				}
			}
			else if (flag == "edit")
			{
				if (CategoryProvider.Instance.GetNameBool(cateid.Value, name))
				{
					jsonResult = base.Json(new { State = 0, Msg = "名称与其他项重复！", Result = "" });
				}
				else
				{
					Category cate = CategoryProvider.Instance.GetEntity(cateid.Value);
					cate.Name = name;
					CategoryProvider.Instance.Update(cate);
					jsonResult = base.Json(new { State = 1, Msg = "修改成功！", Result = cate.Id });
				}
			}
			else if (!(flag == "del"))
			{
				jsonResult = base.Json(new { State = 0, Msg = "参数错误！", Result = "" });
			}
			else
			{
				int count = ResourceProvider.Instance.GetCountBySpeciality(cateid.Value);
				if (count > 0)
				{
					jsonResult = base.Json(new { State = 0, Msg = string.Concat("该专业下有“", count, "”条记录，删除失败！"), Result = "" });
				}
				else
				{
					CategoryProvider.Instance.Delete(cateid.Value);
					jsonResult = base.Json(new { State = 1, Msg = "删除成功！", Result = "" });
				}
			}
			return jsonResult;
		}

		public JsonResult Course_Audit()
		{
			List<object> list = new List<object>()
			{
				new { Count = ResourceProvider.Instance.GetCourceCount(Guid.Empty, Guid.Empty, Guid.Empty, "", 1), UserName = base.CurrentAdmin.Name, Cate = ResourceManageController.HtmlResCate(Guid.Empty), Spe = ResourceManageController.HtmlResSpe(Guid.Empty, Guid.Empty), Type = ResourceManageController.HtmlResType(Guid.Empty) }
			};
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "ok", Result = list });
			return jsonResult;
		}

		public JsonResult Course_Edit(Guid id)
		{
			Guid guid;
			Guid guid1;
			ResourceCourse resource = ResourceCourseProvider.Instance.GetEntity(id);
			ResourcePack rp = ResourcePackProvider.Instance.GetEntity(resource.PackId.Value);
			List<object> list = new List<object>();
			ResourceCategory spe = ResourceCategoryProvider.Instance.GetEntity((rp.CategoryId.HasValue ? rp.CategoryId.Value : Guid.Empty));
			ResourceCategory sub = ResourceCategoryProvider.Instance.GetEntity((spe.ParentId.HasValue ? spe.ParentId.Value : Guid.Empty));
			List<object> objs = list;
			Guid guid2 = resource.Id;
			string title = resource.Title;
			string keyWords = resource.KeyWords;
			string fescribe = resource.Fescribe;
			List<SelectListItem> selectListItems = ResourceManageController.HtmlResCate((sub.ParentId.HasValue ? sub.ParentId.Value : Guid.Empty));
			guid = (sub.ParentId.HasValue ? sub.ParentId.Value : Guid.Empty);
			Guid guid3 = sub.Id;
			List<SelectListItem> selectListItems1 = ResourceManageController.HtmlResSub(guid, sub.Id);
			guid1 = (spe.ParentId.HasValue ? spe.ParentId.Value : Guid.Empty);
			Guid guid4 = spe.Id;
			objs.Add(new { Id = guid2, Title = title, KeyWords = keyWords, Fescribe = fescribe, Cate = selectListItems, sub = selectListItems1, Spe = ResourceManageController.HtmlResSpe(guid1, spe.Id), Type = ResourceManageController.HtmlResType((rp.TypeId.HasValue ? rp.TypeId.Value : Guid.Empty)), UserName = base.CurrentAdmin.Name });
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "ok", Result = list });
			return jsonResult;
		}

		public JsonResult Course_EditRes(Guid resid, string name, Guid subid, Guid speid, Guid typeid, string fescribe, string keys)
		{
			JsonResult jsonResult;
			if (string.IsNullOrEmpty(name))
			{
				jsonResult = base.Json(new { State = 1, Msg = "名称不能为空!", Result = "" });
			}
			else
			{
				ResourceCourse res = ResourceCourseProvider.Instance.GetEntity(resid);
				ResourcePack rp = ResourcePackProvider.Instance.GetEntity(res.PackId.Value);
				res.Title = name;
				rp.CategoryId = new Guid?(speid);
				rp.TypeId = new Guid?(typeid);
				res.KeyWords = keys;
				res.Fescribe = fescribe;
				res.ModifyTime = new DateTime?(DateTime.Now);
				ResourceCourseProvider.Instance.Update(res);
				ResourcePackProvider.Instance.Update(rp);
				jsonResult = base.Json(new { State = 1, Msg = "修改成功!", Result = "" });
			}
			return jsonResult;
		}

		public JsonResult Course_list()
		{
			List<object> list = new List<object>()
			{
				new { Count = ResourceProvider.Instance.GetCourceCount(Guid.Empty, Guid.Empty, Guid.Empty, "", 0), UserName = base.CurrentAdmin.Name, Cate = ResourceManageController.HtmlResCate(Guid.Empty), Spe = ResourceManageController.HtmlResSpe(Guid.Empty, Guid.Empty), Type = ResourceManageController.HtmlResType(Guid.Empty) }
			};
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "ok", Result = list });
			return jsonResult;
		}

		public JsonResult Course_Stats()
		{
			List<object> list = new List<object>()
			{
				new { UserName = base.CurrentAdmin.Name, Count = ResourceCourseProvider.Instance.GetCount() }
			};
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "ok", Result = list });
			return jsonResult;
		}

		public JsonResult Course_Update()
		{
			List<object> list = new List<object>()
			{
				new { UserName = base.CurrentAdmin.Name }
			};
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "ok", Result = list });
			return jsonResult;
		}

		public JsonResult CourseAudit(List<Guid> guidlist)
		{
			if (guidlist.Count > 0)
			{
				foreach (Guid id in guidlist)
				{
					ResourceCourse Course = ResourceProvider.Instance.GetResourceCourse(id);
					Course.ModifyTime = new DateTime?(DateTime.Now);
					Course.IsDeleted = false;
					ResourceCourseProvider.Instance.Update(Course);
				}
			}
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = "" });
			return jsonResult;
		}

		//public JsonResult CoursePreview(Guid id)
		//{
			

		//}

		public JsonResult DeleteCourse(List<Guid> guidlist, string flag)
		{
			
			if (guidlist.Count > 0)
			{
				if (flag == "softdelete")
				{
					foreach (Guid guid in guidlist)
					{
						ResourceCourse res = ResourceCourseProvider.Instance.GetEntity(guid);
						res.CreateTime = DateTime.Now;
						res.IsDeleted = true;
						ResourceCourseProvider.Instance.Update(res);
					}
				}
				else if (flag == "delete")
				{
					foreach (Guid id in guidlist)
					{
						ResourceMaterialProvider.Instance.CourseDelete(id);
					}
				}
			}
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = "" });
			return jsonResult;
		}

		public JsonResult DeleteResource(List<Guid> guidlist, string flag)
		{
			
			if (guidlist.Count > 0)
			{
				if (flag == "softdelete")
				{
					foreach (Guid guid in guidlist)
					{
						ResourceMaterial res = ResourceMaterialProvider.Instance.GetEntity(guid);
						res.CreateTime = DateTime.Now;
						res.IsDeleted = true;
						ResourceMaterialProvider.Instance.Update(res);
					}
				}
				else if (flag == "delete")
				{
					foreach (Guid id in guidlist)
					{
						ResourceMaterialProvider.Instance.Delete(id);
					}
				}
			}
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = "" });
			return jsonResult;
		}

		public JsonResult EditRes(Guid resid, string name, Guid subid, Guid speid, Guid typeid, string fescribe, string keys)
		{
			JsonResult jsonResult;
			if (string.IsNullOrEmpty(name))
			{
				jsonResult = base.Json(new { State = 1, Msg = "名称不能为空!", Result = "" });
			}
			else
			{
				ResourceMaterial res = ResourceMaterialProvider.Instance.GetEntity(resid);
				res.Title = name;
				res.CategoryId = new Guid?(speid);
				res.TypeId = new Guid?(typeid);
				res.KeyWords = keys;
				res.Fescribe = fescribe;
				res.ModifyTime = new DateTime?(DateTime.Now);
				ResourceMaterialProvider.Instance.Update(res);
				jsonResult = base.Json(new { State = 1, Msg = "修改成功!", Result = "" });
			}
			return jsonResult;
		}

		private static List<SelectListItem> HtmlBoutiqueGrade(int val = -1)
		{
			List<SelectListItem> selectListItems = new List<SelectListItem>();
			SelectListItem selectListItem = new SelectListItem();
			selectListItem.Text=Enums.BoutiqueGrade.Simple.GetName();
			selectListItem.Value=0.ToString();
			selectListItems.Add(selectListItem);
			SelectListItem selectListItem1 = new SelectListItem();
			selectListItem1.Text=Enums.BoutiqueGrade.Default.GetName();
			selectListItem1.Value=1.ToString();
			selectListItems.Add(selectListItem1);
			SelectListItem selectListItem2 = new SelectListItem();
			selectListItem2.Text=Enums.BoutiqueGrade.VIP.GetName();
			selectListItem2.Value=2.ToString();
			selectListItems.Add(selectListItem2);
			SelectListItem selectListItem3 = new SelectListItem();
			selectListItem3.Text=Enums.BoutiqueGrade.Life.GetName();
			selectListItem3.Value=3.ToString();
			selectListItems.Add(selectListItem3);
			List<SelectListItem> ddlBoutiqueGrade = selectListItems;
			ddlBoutiqueGrade.ForEach((SelectListItem p) => p.Selected = (p.Value == val.ToString()));
			List<SelectListItem> list = (
				from p in ddlBoutiqueGrade
				orderby p.Selected
				select p).ToList<SelectListItem>();
			return list;
		}

		private static List<SelectListItem> HtmlResCate(Guid cate)
		{
			List<SelectListItem> selectListItems1 = new List<SelectListItem>();
			List<ResourceCategory> list = ResourceCategoryProvider.Instance.GetCategoryTier0();
			if ((cate == Guid.Empty ? true : list.Count == 0))
			{
				List<SelectListItem> selectListItems2 = selectListItems1;
				SelectListItem selectListItem1 = new SelectListItem();
				selectListItem1.Text="请选择";
				selectListItem1.Value=Guid.Empty.ToString();
				selectListItems2.Add(selectListItem1);
			}
			list.ForEach((ResourceCategory p) => {
				List<SelectListItem> selectListItems = selectListItems1;
				SelectListItem selectListItem = new SelectListItem();
				selectListItem.Text=p.Name;
				selectListItem.Value=p.Id.ToString();
				selectListItem.Selected = (cate == p.Id);
				selectListItems.Add(selectListItem);
			});
			List<SelectListItem> selectListItems3 = (
				from p in selectListItems1
				orderby p.Selected
				select p).ToList<SelectListItem>();
			return selectListItems3;
		}

		private static List<SelectListItem> HtmlResKnow(Guid spe, Guid know)
		{
			List<SelectListItem> selectListItems1 = new List<SelectListItem>();
			List<ResourceCategory> list = ResourceCategoryProvider.Instance.GetCategoryTier3(spe);
			List<SelectListItem> selectListItems2 = selectListItems1;
			SelectListItem selectListItem1 = new SelectListItem();
			selectListItem1.Text="请选择";
			selectListItem1.Value=Guid.Empty.ToString();
			selectListItems2.Add(selectListItem1);
			list.ForEach((ResourceCategory p) => {
				List<SelectListItem> selectListItems = selectListItems1;
				SelectListItem selectListItem = new SelectListItem();
				selectListItem.Text=p.Name;
				selectListItem.Value=p.Id.ToString();
				selectListItem.Selected=(spe == p.Id);
				selectListItems.Add(selectListItem);
			});
			List<SelectListItem> selectListItems3 = (
				from p in selectListItems1
				orderby p.Selected
				select p).ToList<SelectListItem>();
			return selectListItems3;
		}

		private static List<SelectListItem> HtmlResSpe(Guid sub, Guid spe)
		{
			List<SelectListItem> selectListItems1 = new List<SelectListItem>();
			List<ResourceCategory> list = ResourceCategoryProvider.Instance.GetCategoryTier2(sub);
			List<SelectListItem> selectListItems2 = selectListItems1;
			SelectListItem selectListItem1 = new SelectListItem();
			selectListItem1.Text="请选择";
			selectListItem1.Value=Guid.Empty.ToString();
			selectListItems2.Add(selectListItem1);
			list.ForEach((ResourceCategory p) => {
				List<SelectListItem> selectListItems = selectListItems1;
				SelectListItem selectListItem = new SelectListItem();
				selectListItem.Text=p.Name;
				selectListItem.Value=p.Id.ToString();
				selectListItem.Selected=(spe == p.Id);
				selectListItems.Add(selectListItem);
			});
			List<SelectListItem> selectListItems3 = (
				from p in selectListItems1
				orderby p.Selected
				select p).ToList<SelectListItem>();
			return selectListItems3;
		}

		private static List<SelectListItem> HtmlResSub(Guid cate, Guid sub)
		{
			List<SelectListItem> selectListItems1 = new List<SelectListItem>();
			List<ResourceCategory> list = ResourceCategoryProvider.Instance.GetCategoryTier1(cate);
			if (list.Count == 0)
			{
				List<SelectListItem> selectListItems2 = selectListItems1;
				SelectListItem selectListItem1 = new SelectListItem();
				selectListItem1.Text="请选择";
				selectListItem1.Value=Guid.Empty.ToString();
				selectListItems2.Add(selectListItem1);
			}
			list.ForEach((ResourceCategory p) => {
				List<SelectListItem> selectListItems = selectListItems1;
				SelectListItem selectListItem = new SelectListItem();
				selectListItem.Text=p.Name;
				selectListItem.Value=p.Id.ToString();
				selectListItem.Selected=(sub == p.Id);
				selectListItems.Add(selectListItem);
			});
			List<SelectListItem> selectListItems3 = (
				from p in selectListItems1
				orderby p.Selected
				select p).ToList<SelectListItem>();
			return selectListItems3;
		}

		private static List<SelectListItem> HtmlResType(Guid type)
		{
			List<SelectListItem> selectListItems1 = new List<SelectListItem>();
			if (type == Guid.Empty)
			{
				List<SelectListItem> selectListItems2 = selectListItems1;
				SelectListItem selectListItem1 = new SelectListItem();
				selectListItem1.Text="请选择";
				selectListItem1.Value=Guid.Empty.ToString();
				selectListItem1.Selected=(true);
				selectListItems2.Add(selectListItem1);
			}
			List<ResourceType> list = ResourceTypeProvider.Instance.GetList();
			list.ForEach((ResourceType p) => {
				List<SelectListItem> selectListItems = selectListItems1;
				SelectListItem selectListItem = new SelectListItem();
				selectListItem.Text=p.Name;
				selectListItem.Value=p.Id.ToString();
				selectListItem.Selected=(type == p.Id);
				selectListItems.Add(selectListItem);
			});
			List<SelectListItem> selectListItems3 = (
				from p in selectListItems1
				orderby p.Selected
				select p).ToList<SelectListItem>();
			return selectListItems3;
		}

		public int Import(string path, string xlsName)
		{
			DataTable dt = SqlHelper.ExcelToDataTable(path, xlsName);
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				ResourceMaterial rm = new ResourceMaterial()
				{
					Id = Guid.NewGuid(),
					Title = dt.Rows[i][0].ToString(),
					Size = new int?(int.Parse(dt.Rows[i][1].ToString())),
					Author = dt.Rows[i][2].ToString()
				};
				if (!string.IsNullOrEmpty(dt.Rows[i][3].ToString()))
				{
					rm.CategoryId = new Guid?(Guid.Parse(dt.Rows[i][3].ToString()));
				}
				if (dt.Rows[i][4].ToString() != "NULL")
				{
					if (!string.IsNullOrEmpty(dt.Rows[i][4].ToString()))
					{
						rm.SchoolId = new Guid?(Guid.Parse(dt.Rows[i][4].ToString()));
					}
				}
				if (!string.IsNullOrEmpty(dt.Rows[i][5].ToString()))
				{
					rm.TypeId = new Guid?(Guid.Parse(dt.Rows[i][5].ToString()));
				}
				rm.KeyWords = dt.Rows[i][6].ToString();
				rm.Fescribe = dt.Rows[i][7].ToString();
				rm.DownloadFilepath = dt.Rows[i][8].ToString();
				rm.IcoFilepath = dt.Rows[i][9].ToString();
				rm.PreviewFilepath = dt.Rows[i][10].ToString();
				rm.FileExt = dt.Rows[i][11].ToString();
				if (!string.IsNullOrEmpty(dt.Rows[i][12].ToString()))
				{
					rm.SortId = new int?(int.Parse(dt.Rows[i][12].ToString()));
				}
				rm.IsTop = new bool?((dt.Rows[i][13].ToString() == "1" ? true : false));
				rm.IssueTime = new DateTime?(DateTime.Now);
				rm.Is211 = new bool?(false);
				rm.ModifyTime = new DateTime?(DateTime.Now);
				rm.CreateTime = DateTime.Now;
				rm.IsDeleted = false;
				rm.Tag1_uptime = new DateTime?(DateTime.Now);
				ResourceMaterialProvider.Instance.Create(rm);
			}
			return dt.Rows.Count;
		}

		public int ImportCourse(string path, string xlsName)
		{
			DataTable dt = SqlHelper.ExcelToDataTable(path, xlsName);
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				ResourceCourse rc = new ResourceCourse()
				{
					Id = Guid.NewGuid(),
					Title = dt.Rows[i][0].ToString(),
					Size = new int?(int.Parse(dt.Rows[i][1].ToString())),
					Author = dt.Rows[i][2].ToString()
				};
				if (dt.Rows[i][3].ToString() != "NULL")
				{
					if (!string.IsNullOrEmpty(dt.Rows[i][3].ToString()))
					{
						rc.SchoolId = new Guid?(Guid.Parse(dt.Rows[i][3].ToString()));
					}
				}
				if (!string.IsNullOrEmpty(dt.Rows[i][4].ToString()))
				{
					rc.PackId = new Guid?(Guid.Parse(dt.Rows[i][4].ToString()));
				}
				rc.KeyWords = dt.Rows[i][5].ToString();
				rc.Fescribe = dt.Rows[i][6].ToString();
				rc.DownloadFilepath = dt.Rows[i][7].ToString();
				rc.IcoFilepath = dt.Rows[i][8].ToString();
				rc.PreviewFilepath = dt.Rows[i][9].ToString();
				rc.FileExt = dt.Rows[i][10].ToString();
				if (!string.IsNullOrEmpty(dt.Rows[i][11].ToString()))
				{
					rc.SortId = new int?(int.Parse(dt.Rows[i][11].ToString()));
				}
				rc.IsTop = new bool?((dt.Rows[i][12].ToString() == "1" ? true : false));
				rc.IssueTime = new DateTime?(DateTime.Now);
				rc.Is211 = new bool?(false);
				rc.ModifyTime = new DateTime?(DateTime.Now);
				rc.CreateTime = DateTime.Now;
				rc.IsDeleted = false;
				rc.Tag1_uptime = new DateTime?(DateTime.Now);
				ResourceCourseProvider.Instance.Create(rc);
			}
			return dt.Rows.Count;
		}

		public JsonResult Knowledge(Guid? speid, Guid? knowid, string flag, string name)
		{
			JsonResult jsonResult;
			if (string.IsNullOrEmpty(name))
			{
				jsonResult = base.Json(new { State = 0, Msg = "为空或未选中！", Result = "" });
			}
			else if (flag == "add")
			{
				if (KnowledgeProvider.Instance.GetNameBool(name))
				{
					jsonResult = base.Json(new { State = 0, Msg = "名称与其他项重复！", Result = "" });
				}
				else
				{
					Knowledge newKnow = new Knowledge()
					{
						Id = Guid.NewGuid(),
						Name = name,
						SpecialityId = speid.Value,
						CreateTime = DateTime.Now,
						IsDeleted = false
					};
					KnowledgeProvider.Instance.Create(newKnow);
					jsonResult = base.Json(new { State = 1, Msg = "添加成功！", Result = newKnow.Id });
				}
			}
			else if (flag == "edit")
			{
				if (KnowledgeProvider.Instance.GetNameBool(knowid.Value, name))
				{
					jsonResult = base.Json(new { State = 0, Msg = "名称与其他项重复！", Result = "" });
				}
				else
				{
					Knowledge know = KnowledgeProvider.Instance.GetEntity(knowid.Value);
					know.Name = name;
					KnowledgeProvider.Instance.Update(know);
					jsonResult = base.Json(new { State = 1, Msg = "修改成功！", Result = know.Id });
				}
			}
			else if (!(flag == "del"))
			{
				jsonResult = base.Json(new { State = 0, Msg = "参数错误！", Result = "" });
			}
			else
			{
				int count = ResourceProvider.Instance.GetCountByKnowledge(knowid.Value);
				if (count > 0)
				{
					jsonResult = base.Json(new { State = 0, Msg = string.Concat("该专业下有“", count, "”条记录，删除失败！"), Result = "" });
				}
				else
				{
					KnowledgeProvider.Instance.Delete(knowid.Value);
					jsonResult = base.Json(new { State = 1, Msg = "删除成功！", Result = "" });
				}
			}
			return jsonResult;
		}

		public ViewResult MassEdit()
		{
			ViewBag.BoutiqueGrade = ResourceManageController.HtmlBoutiqueGrade(-1);
			ViewBag.UserName = base.CurrentAdmin.Name;
			ViewBag.guidlist = Request["hdData"];
			ViewBag.Spe = ResourceManageController.HtmlResSpe(Guid.Empty, Guid.Empty);
			ViewBag.Know = ResourceManageController.HtmlResKnow(Guid.Empty, Guid.Empty);
			ViewBag.Type = ResourceManageController.HtmlResType(Guid.NewGuid());
			return base.View();
		}

		public JsonResult MassEditRes(List<Guid> guidlist, Guid cateid, Guid subid, Guid speid, Guid typeid, int jp, string fescribe, string keys)
		{
			JsonResult jsonResult;
			if ((guidlist.Count <= 0 ? false : guidlist != null))
			{
				foreach (Guid id in guidlist)
				{
					Resource res = ResourceProvider.Instance.GetEntity(id);
					res.CategoryId = new Guid?(cateid);
					res.SubjectId = new Guid?(subid);
					res.SpecialityId = new Guid?(speid);
					res.TypeId = new Guid?(typeid);
					res.BoutiqueGrade = new int?(jp);
					res.KeyWords = keys;
					res.Fescribe = fescribe;
					res.CreateTime = DateTime.Now;
					ResourceProvider.Instance.Update(res);
				}
				jsonResult = base.Json(new { State = 1, Msg = "修改成功！", Result = "" });
			}
			else
			{
				jsonResult = base.Json(new { State = 1, Msg = "没有选中资源！", Result = "" });
			}
			return jsonResult;
		}

		public JsonResult OperationCategory(Guid? parentId, Guid? id, string flag, string name, int tier)
		{
			ResourceCategory cate;
			JsonResult jsonResult;
			bool flag1;
			if (string.IsNullOrEmpty(name))
			{
				jsonResult = base.Json(new { State = 0, Msg = "为空或未选中！", Result = "" });
			}
			else if (flag == "add")
			{
				if (ResourceCategoryProvider.Instance.GetNameBool(new Guid?(Guid.Empty), name))
				{
					jsonResult = base.Json(new { State = 0, Msg = "名称与其他项重复！", Result = "" });
				}
				else
				{
					ResourceCategory newCate = new ResourceCategory()
					{
						Id = Guid.NewGuid(),
						Name = name,
						CreateTime = DateTime.Now,
						IsDeleted = false,
						Tier = tier
					};
					if ((!parentId.HasValue ? false : tier != 0))
					{
						newCate.ParentId = parentId;
					}
					ResourceCategoryProvider.Instance.Create(newCate);
					jsonResult = base.Json(new { State = 1, Msg = "添加成功！", Result = newCate.Id });
				}
			}
			else if (flag == "edit")
			{
				if (ResourceCategoryProvider.Instance.GetNameBool(new Guid?(id.Value), name))
				{
					jsonResult = base.Json(new { State = 0, Msg = "名称与其他项重复！", Result = "" });
				}
				else
				{
					cate = ResourceCategoryProvider.Instance.GetEntity(id.Value);
					cate.Name = name;
					ResourceCategoryProvider.Instance.Update(cate);
					jsonResult = base.Json(new { State = 1, Msg = "修改成功！", Result = cate.Id });
				}
			}
			else if (!(flag == "del"))
			{
				jsonResult = base.Json(new { State = 0, Msg = "参数错误！", Result = "" });
			}
			else
			{
				cate = ResourceCategoryProvider.Instance.GetEntity(id.Value);
				if (cate == null)
				{
					flag1 = true;
				}
				else
				{
					int? count = cate.Count;
					
                    if (count > 0)
                        flag1 = false;
                    else
                        flag1 = true;
				}
				if (flag1)
				{
					jsonResult = base.Json(new { State = 0, Msg = string.Concat("该专业下有“", cate.Count, "”条记录，删除失败！"), Result = "" });
				}
				else
				{
					ResourceCategoryProvider.Instance.Delete(id.Value);
					jsonResult = base.Json(new { State = 1, Msg = "删除成功！", Result = "" });
				}
			}
			return jsonResult;
		}

		//public JsonResult Preview(Guid id)
		//{
			

		//}

		public ViewResult Resource()
		{
			ViewBag.Count = ResourceProvider.Instance.GetCount(Guid.Empty, Guid.Empty, Guid.Empty, "", 0);
			ViewBag.UserName = base.CurrentAdmin.Name;
			ViewBag.Cate = ResourceManageController.HtmlResCate(Guid.Empty);
			ViewBag.Spe = ResourceManageController.HtmlResSpe(Guid.Empty, Guid.Empty);
			ViewBag.Type = ResourceManageController.HtmlResType(Guid.Empty);
			return base.View();
		}

		public JsonResult Resource_AuditResource()
		{
			List<object> list = new List<object>()
			{
				new { Count = ResourceProvider.Instance.GetCount(Guid.Empty, Guid.Empty, Guid.Empty, "", 1), UserName = base.CurrentAdmin.Name, Cate = ResourceManageController.HtmlResCate(Guid.Empty), Spe = ResourceManageController.HtmlResSpe(Guid.Empty, Guid.Empty), Type = ResourceManageController.HtmlResType(Guid.Empty) }
			};
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "ok", Result = list });
			return jsonResult;
		}

		public JsonResult Resource_EditResource(Guid id)
		{
			Guid guid;
			Guid guid1;
			ResourceMaterial resource = ResourceMaterialProvider.Instance.GetEntity(id);
			List<object> list = new List<object>();
			ResourceCategory spe = ResourceCategoryProvider.Instance.GetEntity((resource.CategoryId.HasValue ? resource.CategoryId.Value : Guid.Empty));
			ResourceCategory sub = ResourceCategoryProvider.Instance.GetEntity((spe.ParentId.HasValue ? spe.ParentId.Value : Guid.Empty));
			List<object> objs = list;
			Guid guid2 = resource.Id;
			string title = resource.Title;
			string keyWords = resource.KeyWords;
			string fescribe = resource.Fescribe;
			List<SelectListItem> selectListItems = ResourceManageController.HtmlResCate((sub.ParentId.HasValue ? sub.ParentId.Value : Guid.Empty));
			guid = (sub.ParentId.HasValue ? sub.ParentId.Value : Guid.Empty);
			Guid guid3 = sub.Id;
			List<SelectListItem> selectListItems1 = ResourceManageController.HtmlResSub(guid, sub.Id);
			guid1 = (spe.ParentId.HasValue ? spe.ParentId.Value : Guid.Empty);
			Guid guid4 = spe.Id;
			objs.Add(new { Id = guid2, Title = title, KeyWords = keyWords, Fescribe = fescribe, Cate = selectListItems, sub = selectListItems1, Spe = ResourceManageController.HtmlResSpe(guid1, spe.Id), Type = ResourceManageController.HtmlResType((resource.TypeId.HasValue ? resource.TypeId.Value : Guid.Empty)), UserName = base.CurrentAdmin.Name });
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "ok", Result = list });
			return jsonResult;
		}

		public JsonResult Resource_list()
		{
			List<object> list = new List<object>()
			{
				new { Count = ResourceProvider.Instance.GetCount(Guid.Empty, Guid.Empty, Guid.Empty, "", 0), UserName = base.CurrentAdmin.Name, Cate = ResourceManageController.HtmlResCate(Guid.Empty), Spe = ResourceManageController.HtmlResSpe(Guid.Empty, Guid.Empty), Type = ResourceManageController.HtmlResType(Guid.Empty) }
			};
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "ok", Result = list });
			return jsonResult;
		}

		public JsonResult Resource_SortManage()
		{
			List<object> list = new List<object>()
			{
				new { UserName = base.CurrentAdmin.Name }
			};
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "ok", Result = list });
			return jsonResult;
		}

		public JsonResult Resource_Stats()
		{
			List<object> list = new List<object>()
			{
				new { UserName = base.CurrentAdmin.Name, Count = ResourceMaterialProvider.Instance.GetCount() }
			};
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "ok", Result = list });
			return jsonResult;
		}

		public JsonResult resource_Update()
		{
			List<object> list = new List<object>()
			{
				new { UserName = base.CurrentAdmin.Name }
			};
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "ok", Result = list });
			return jsonResult;
		}

		public PartialViewResult ResourceMaterialsUpload(int? BaseType)
		{
			ViewBag.BaseType = BaseType;
			return base.PartialView("_Resource_MaterialUpload");
		}

		[HttpPost]
		public JsonResult ResourcePackUpload(int? BaseType)
		{
			Guid? nullable = null;
			ResourceCategoryProvider.Instance.GetSubList(nullable, null, false, true);
			List<object> list = new List<object>()
			{
				new { Cate = ResourceManageController.HtmlResCate(Guid.Empty), Spe = ResourceManageController.HtmlResSpe(Guid.Empty, Guid.Empty), Type = ResourceManageController.HtmlResType(Guid.Empty), BaseType = BaseType }
			};
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "ok", Result = list });
			return jsonResult;
		}

		public JsonResult Speciality(Guid? subid, Guid? speid, string flag, string name)
		{
			JsonResult jsonResult;
			if (string.IsNullOrEmpty(name))
			{
				jsonResult = base.Json(new { State = 0, Msg = "为空或未选中！", Result = "" });
			}
			else if (flag == "add")
			{
				if (SpecialityProvider.Instance.GetNameBool(name))
				{
					jsonResult = base.Json(new { State = 0, Msg = "名称与其他项重复！", Result = "" });
				}
				else
				{
					Speciality newSpe = new Speciality()
					{
						Id = Guid.NewGuid(),
						Name = name,
						SubjectId = subid.Value,
						CreateTime = DateTime.Now,
						IsDeleted = false
					};
					SpecialityProvider.Instance.Create(newSpe);
					jsonResult = base.Json(new { State = 1, Msg = "添加成功！", Result = newSpe.Id });
				}
			}
			else if (flag == "edit")
			{
				if (SpecialityProvider.Instance.GetNameBool(speid.Value, name))
				{
					jsonResult = base.Json(new { State = 0, Msg = "名称与其他项重复！", Result = "" });
				}
				else
				{
					Speciality spe = SpecialityProvider.Instance.GetEntity(speid.Value);
					spe.Name = name;
					SpecialityProvider.Instance.Update(spe);
					jsonResult = base.Json(new { State = 1, Msg = "修改成功！", Result = spe.Id });
				}
			}
			else if (!(flag == "del"))
			{
				jsonResult = base.Json(new { State = 0, Msg = "参数错误！", Result = "" });
			}
			else
			{
				int count = ResourceProvider.Instance.GetCountBySpeciality(speid.Value);
				if (count > 0)
				{
					jsonResult = base.Json(new { State = 0, Msg = string.Concat("该专业下有“", count, "”条记录，删除失败！"), Result = "" });
				}
				else
				{
					SpecialityProvider.Instance.Delete(speid.Value);
					jsonResult = base.Json(new { State = 1, Msg = "删除成功！", Result = "" });
				}
			}
			return jsonResult;
		}

		public JsonResult Subject(Guid? cateid, Guid? subid, string flag, string name)
		{
			JsonResult jsonResult;
			if (string.IsNullOrEmpty(name))
			{
				jsonResult = base.Json(new { State = 0, Msg = "为空或未选中！", Result = "" });
			}
			else if (flag == "add")
			{
				if (SubjectProvider.Instance.GetNameBool(name))
				{
					jsonResult = base.Json(new { State = 0, Msg = "名称与其他项重复！", Result = "" });
				}
				else
				{
					Subject newSub = new Subject()
					{
						Id = Guid.NewGuid(),
						Name = name,
						Count = new int?(0),
						CategoryId = cateid.Value,
						CreateTime = DateTime.Now,
						IsDeleted = false
					};
					SubjectProvider.Instance.Create(newSub);
					jsonResult = base.Json(new { State = 1, Msg = "添加成功！", Result = newSub.Id });
				}
			}
			else if (flag == "edit")
			{
				if (SubjectProvider.Instance.GetNameBool(subid.Value, name))
				{
					jsonResult = base.Json(new { State = 0, Msg = "名称与其他项重复！", Result = "" });
				}
				else
				{
					Subject sub = SubjectProvider.Instance.GetEntity(subid.Value);
					sub.Name = name;
					SubjectProvider.Instance.Update(sub);
					jsonResult = base.Json(new { State = 1, Msg = "修改成功！", Result = sub.Id });
				}
			}
			else if (!(flag == "del"))
			{
				jsonResult = base.Json(new { State = 0, Msg = "参数错误！", Result = "" });
			}
			else
			{
				int count = ResourceProvider.Instance.GetCountBySpeciality(subid.Value);
				if (count > 0)
				{
					jsonResult = base.Json(new { State = 0, Msg = string.Concat("该专业下有“", count, "”条记录，删除失败！"), Result = "" });
				}
				else
				{
					SubjectProvider.Instance.Delete(subid.Value);
					jsonResult = base.Json(new { State = 1, Msg = "删除成功！", Result = "" });
				}
			}
			return jsonResult;
		}

		[HttpPost]
		public JsonResult UpdateCourse(HttpPostedFileBase upXls)
		{
			JsonResult jsonResult;
			string filePath = Server.MapPath("~/Upload/Resource/");
			string xlsName = Request["txtSheet"];
			if (string.IsNullOrWhiteSpace(xlsName))
			{
				jsonResult = base.Json(new { State = 0, Msg = "请正确填写！", Result = "" }, "text/html");
			}
			else if ((upXls == null ? false : upXls.ContentLength != 0))
			{
				string fileExt = Path.GetExtension(upXls.FileName);
				if (!(fileExt != ".xls"))
				{
					DateTime now = DateTime.Now;
					string fileName = string.Concat("资源", now.ToString("yyyy-MM-dd-hh-mm-ss"), fileExt);
					string filePhysicalPath = string.Concat(filePath, fileName);
					if (!Directory.Exists(filePath))
					{
						Directory.CreateDirectory(filePath);
					}
					upXls.SaveAs(filePhysicalPath);
					if (System.IO.File.Exists(filePhysicalPath))
					{
						int num = this.ImportCourse(filePhysicalPath, xlsName);
						jsonResult = base.Json(new { State = 1, Msg = string.Concat("更新成功", num, "条"), Result = "" }, "text/html");
					}
					else
					{
						jsonResult = base.Json(new { State = 0, Msg = "文件上传出错！", Result = "" }, "text/html");
					}
				}
				else
				{
					jsonResult = base.Json(new { State = 0, Msg = "文件格式不正确！", Result = "" }, "text/html");
				}
			}
			else
			{
				jsonResult = base.Json(new { State = 0, Msg = "请正确选择文件！", Result = "" }, "text/html");
			}
			return jsonResult;
		}

		[HttpPost]
		public JsonResult UpdateResource(HttpPostedFileBase upXls)
		{
			JsonResult jsonResult;
			string filePath = Server.MapPath("~/Upload/Resource/");
			string xlsName = Request["txtSheet"];
			if (string.IsNullOrWhiteSpace(xlsName))
			{
				jsonResult = base.Json(new { State = 0, Msg = "请正确填写！", Result = "" }, "text/html");
			}
			else if ((upXls == null ? false : upXls.ContentLength != 0))
			{
				string fileExt = Path.GetExtension(upXls.FileName);
				if (!(fileExt != ".xls"))
				{
					DateTime now = DateTime.Now;
					string fileName = string.Concat("资源", now.ToString("yyyy-MM-dd-hh-mm-ss"), fileExt);
					string filePhysicalPath = string.Concat(filePath, fileName);
					if (!Directory.Exists(filePath))
					{
						Directory.CreateDirectory(filePath);
					}
					upXls.SaveAs(filePhysicalPath);
					if (System.IO.File.Exists(filePhysicalPath))
					{
						int num = this.Import(filePhysicalPath, xlsName);
						jsonResult = base.Json(new { State = 1, Msg = string.Concat("更新成功", num, "条"), Result = "" }, "text/html");
					}
					else
					{
						jsonResult = base.Json(new { State = 0, Msg = "文件上传出错！", Result = "" }, "text/html");
					}
				}
				else
				{
					jsonResult = base.Json(new { State = 0, Msg = "文件格式不正确！", Result = "" }, "text/html");
				}
			}
			else
			{
				jsonResult = base.Json(new { State = 0, Msg = "请正确选择文件！", Result = "" }, "text/html");
			}
			return jsonResult;
		}
	}
}