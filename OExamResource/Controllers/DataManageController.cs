using Common;
using Models;
using Providers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web.Mvc;

namespace Controllers
{
	[AdminFilter]
	public class DataManageController : BaseController
	{
		public DataManageController()
		{
		}

		public JsonResult GetCategory()
		{
			JsonResult jsonResult;
			List<ResourceCategory> list = ResourceCategoryProvider.Instance.GetCategoryTier0();
			jsonResult = (list.Count <= 0 ? base.Json(new { State = 0, Msg = "", Result = "" }) : base.Json(new { State = 1, Msg = "", Result = list }));
			return jsonResult;
		}

		public JsonResult GetCategorySubject()
		{
			
			List<ResourceCategory> list1 = ResourceCategoryProvider.Instance.GetList();
			var list = (
				from p in list1
				where p.Tier == 0
				select p).Select((ResourceCategory p) => {
				Guid guid = p.Id;
				string name = p.Name;
				int? count = p.Count;
				string ico = p.Ico;
				IEnumerable<ResourceCategory> resourceCategories = list1.Where<ResourceCategory>((ResourceCategory s) => {
					Guid? parentId = s.ParentId;
					Guid id = p.Id;
					return (!parentId.HasValue ? false : parentId.GetValueOrDefault() == id);
				});
				//if (func == null)
				//{
				//	func = (ResourceCategory s) => new { Id = s.Id, Name = s.Name, Count = s.Count };
				//}
                var listfunc = resourceCategories.Select((ResourceCategory s) => new { Id = s.Id, Name = s.Name, Count = s.Count }).ToList();

                return new { Id = guid, Name = name, Count = count, Ico = ico, List = listfunc };
			});
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = list });
			return jsonResult;
		}

		public JsonResult GetCourseList(Guid csid, Guid specialityid, Guid typeid, string name, int pageIndex)
		{
			Guid schoolid = Guid.Empty;
			UserInfoProvider.Instance.GetEntity(base.CurrentAdmin.Id);
			List<Resourcelist> CourseList = ResourceProvider.Instance.GetResourceCourseList(schoolid, csid, specialityid, typeid, name, pageIndex, 50, 0);
			var list = CourseList.Select((Resourcelist p) => {
				string str;
				string str1;
				string str2;
				string str3;
				string str4;
				string str5;
				string str6;
				Guid id = p.Id;
				string title = p.Title;
				Guid packid = p.Packid;
				str = (p.Packname == null ? "无" : p.Packname);
				str1 = (p.Title.Length >= 15 ? string.Concat(p.Title.Substring(0, 15), ".") : p.Title);
				str2 = (p.CategoryName == null ? "无" : p.CategoryName);
				str3 = (p.SubjectName == null ? "无" : p.SubjectName);
				str4 = (p.SpecialityName == null ? "无" : p.SpecialityName);
				str5 = (p.KnowledgeName == null ? "无" : p.KnowledgeName);
				str6 = (p.TypeName == null ? "无" : p.TypeName);
				DateTime createTime = p.CreateTime;
				return new { Id = id, Title = title, Packid = packid, Packname = str, Name = str1, CategoryName = str2, SubjectName = str3, SpecialityName = str4, KnowledgeName = str5, TypeName = str6, CreateTime = p.CreateTime.ToString("yyyy-MM-dd"), Size = (new WriteSize()).WriteResourceSize(p.Size.ToString()) };
			});
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = list });
			return jsonResult;
		}

		public JsonResult GetCourseListCount(Guid csid, Guid specialityid, Guid typeid, string name)
		{
			Guid schoolid = Guid.Empty;
			UserInfoProvider.Instance.GetEntity(base.CurrentAdmin.Id);
			int count = ResourceProvider.Instance.GetCourceCount(schoolid, specialityid, typeid, name, 0);
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = count });
			return jsonResult;
		}

		public JsonResult GetCourseStatistics()
		{
			
			List<Resourcelist> listCate = ResourceProvider.Instance.GetCourseCateCountList();
			List<Resourcelist> subCountList = ResourceProvider.Instance.GetSubCountList();
			var list = listCate.Select((Resourcelist p) => {
				Guid id = p.Id;
				string categoryName = p.CategoryName;
				int size = p.Size;
				string str = (new WriteSize()).WriteResourceSize(p.Tag1);
				IEnumerable<Resourcelist> resourcelists = 
					from q in subCountList
					where (q.CategoryName != p.CategoryName ? false : q.Size != 0)
					select q;
				//if (func == null)
				//{
				//	func = (Resourcelist q) => new { Id = q.Id, Name = q.SubjectName, Count = q.Size, Size = (new WriteSize()).WriteResourceSize(q.Tag1) };
				//}
                var listfunc = resourcelists.Select((Resourcelist q) => new { Id = q.Id, Name = q.SubjectName, Count = q.Size, Size = (new WriteSize()).WriteResourceSize(q.Tag1) });

                return new { Id = id, Name = categoryName, Count = size, Size = str, List = listfunc };
			});
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = 
				from p in list
				orderby p.List.Count() descending
				select p });
			return jsonResult;
		}

		public JsonResult GetDelCourseList(Guid csid, Guid specialityid, Guid typeid, string name, int pageIndex)
		{
			Guid schoolid = Guid.Empty;
			UserInfoProvider.Instance.GetEntity(base.CurrentAdmin.Id);
			List<Resourcelist> CourseList = ResourceProvider.Instance.GetResourceCourseList(schoolid, csid, specialityid, typeid, name, pageIndex, 50, 1);
			var list = CourseList.Select((Resourcelist p) => {
				string str;
				string str1;
				string str2;
				string str3;
				string str4;
				string str5;
				string str6;
				Guid id = p.Id;
				string title = p.Title;
				Guid packid = p.Packid;
				str = (p.Packname == null ? "无" : p.Packname);
				str1 = (p.Title.Length >= 15 ? string.Concat(p.Title.Substring(0, 15), ".") : p.Title);
				str2 = (p.CategoryName == null ? "无" : p.CategoryName);
				str3 = (p.SubjectName == null ? "无" : p.SubjectName);
				str4 = (p.SpecialityName == null ? "无" : p.SpecialityName);
				str5 = (p.KnowledgeName == null ? "无" : p.KnowledgeName);
				str6 = (p.TypeName == null ? "无" : p.TypeName);
				DateTime createTime = p.CreateTime;
				return new { Id = id, Title = title, Packid = packid, Packname = str, Name = str1, CategoryName = str2, SubjectName = str3, SpecialityName = str4, KnowledgeName = str5, TypeName = str6, CreateTime = p.CreateTime.ToString("yyyy-MM-dd"), Size = (new WriteSize()).WriteResourceSize(p.Size.ToString()) };
			});
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = list });
			return jsonResult;
		}

		public JsonResult GetDelCourseListCount(Guid csid, Guid specialityid, Guid typeid, string name)
		{
			Guid schoolid = Guid.Empty;
			UserInfoProvider.Instance.GetEntity(base.CurrentAdmin.Id);
			int count = ResourceProvider.Instance.GetCourceCount(schoolid, specialityid, typeid, name, 1);
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = count });
			return jsonResult;
		}

		public JsonResult GetDelResourceList(Guid csid, Guid specialityid, Guid typeid, string name, int pageIndex)
		{
			Guid schoolid = Guid.Empty;
			UserInfoProvider.Instance.GetEntity(base.CurrentAdmin.Id);
			List<Resourcelist> resourceList = ResourceProvider.Instance.GetList(schoolid, csid, specialityid, typeid, name, pageIndex, 50, 1);
			var list = resourceList.Select((Resourcelist p) => {
				string str;
				string str1;
				string str2;
				string str3;
				string str4;
				string str5;
				Guid id = p.Id;
				string title = p.Title;
				str = (p.Title.Length >= 15 ? string.Concat(p.Title.Substring(0, 15), ".") : p.Title);
				str1 = (p.CategoryName == null ? "无" : p.CategoryName);
				str2 = (p.SubjectName == null ? "无" : p.SubjectName);
				str3 = (p.SpecialityName == null ? "无" : p.SpecialityName);
				str4 = (p.KnowledgeName == null ? "无" : p.KnowledgeName);
				str5 = (p.TypeName == null ? "无" : p.TypeName);
				DateTime createTime = p.CreateTime;
				return new { Id = id, Title = title, Name = str, CategoryName = str1, SubjectName = str2, SpecialityName = str3, KnowledgeName = str4, TypeName = str5, CreateTime = p.CreateTime.ToString("yyyy-MM-dd"), Size = (new WriteSize()).WriteResourceSize(p.Size.ToString()) };
			});
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = list });
			return jsonResult;
		}

		public JsonResult GetDelResourceListCount(Guid csid, Guid specialityid, Guid typeid, string name)
		{
			Guid schoolid = Guid.Empty;
			UserInfoProvider.Instance.GetEntity(base.CurrentAdmin.Id);
			int count = ResourceProvider.Instance.GetCount(schoolid, specialityid, typeid, name, 1);
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = count });
			return jsonResult;
		}

		public JsonResult GetDepartment(Guid facid)
		{
			JsonResult jsonResult;
			List<School> departent = SchoolProvider.Instance.GetListSub(facid);
			jsonResult = (departent.Count <= 0 ? base.Json(new { State = 0, Msg = "", Result = "" }) : base.Json(new { State = 1, Msg = "", Result = departent }));
			return jsonResult;
		}

		public JsonResult GetDepList(Guid schoolid, string name, int pageIndex)
		{
	
			List<School> depList = SchoolProvider.Instance.GetFacDepList(schoolid, name.Trim(), 1, pageIndex, 10);
			List<School> list1 = SchoolProvider.Instance.GetList();
			var list = depList.Select((School p) => {
				Guid guid = p.Id;
				string str = p.Name;
				Guid? nullable = p.ParentId;
				int num = list1.Count<School>((School a) => {
					Guid? parentId = a.ParentId;
					Guid id = p.Id;
					return (!parentId.HasValue ? false : parentId.GetValueOrDefault() == id);
				});
				IEnumerable<School> schools = list1.Where<School>((School d) => {
					Guid? parentId = d.ParentId;
					Guid id = p.Id;
					return (!parentId.HasValue ? false : parentId.GetValueOrDefault() == id);
				});
                //if (func == null)
                //{
                //	func = (School d) => new { Id = d.Id, Name = d.Name };
                //}
                var listfunc = schools.Select((School d) => new { Id = d.Id, Name = d.Name });

                return new { Id = guid, Name = str, ParentId = nullable, Length = num, List = listfunc };
			});
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = list });
			return jsonResult;
		}

		public JsonResult GetDepListCount(Guid schoolid, string name)
		{
			int count = SchoolProvider.Instance.GetFacDepCount(schoolid, name, 1);
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = count });
			return jsonResult;
		}

		public JsonResult GetDownSchoolIpList(string name, string ip, string teacherName, string Startime, string Endtime, int pageIndex)
		{
			List<SchoolLogDetail> schoolList = SchoolProvider.Instance.GetDownSchoolDetail(name, Startime, teacherName, Endtime, ip, pageIndex, 10);
			var list = 
				from p in schoolList
				select new { DownLoadCount = p.DownLoadCount, SchoolName = p.Name, CreateTime = p.CreateTime, ip = p.ip, teachername = p.Tag1, sellname = p.Tag2, Id = p.Id };
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = list, Identity = base.CurrentAdmin.Role });
			return jsonResult;
		}

		public JsonResult GetDownSchoolIpListCount(string name, string ip, string teacherName, string Startime, string Endtime)
		{
			int count = SchoolProvider.Instance.GetDownSchoolDetailCount(name, Startime, Endtime, ip, teacherName);
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = count });
			return jsonResult;
		}

		public JsonResult GetExamType2(Guid id)
		{
			ExamPaperType ept = ExamPaperTypeProvider.Instance.GetEntity(id);
			List<ExamPaperType> list2 = ExamPaperTypeProvider.Instance.GetList(ept.ParentId, ept.Tier);
			var list = 
				from p in list2
				select new { Id = p.Id, Title = p.Name, Name = (p.Name.Length >= 10 ? p.Name.Substring(0, 10) : p.Name), Count = p.PaperCount };
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = list });
			return jsonResult;
		}

		public JsonResult GetExamType3(Guid id)
		{
			ExamPaperTypeProvider.Instance.GetEntity(id);
			var list = 
				from p in ExamPaperTypeProvider.Instance.GetList(id, 2)
				select new { Id = p.Id, Title = p.Name, Name = (p.Name.Length >= 10 ? p.Name.Substring(0, 10) : p.Name), Count = p.PaperCount };
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = list });
			return jsonResult;
		}

		public JsonResult GetExamTypeList()
		{
			
			List<ExamPaperType> listAdmin = ExamPaperTypeProvider.Instance.GetListAdmin();
			List<ExamPaperType> list0 = ExamPaperTypeProvider.Instance.GetListsAdmin(Enums.PaperTier.Level1);
			var list = list0.Select((ExamPaperType p) => {
				string str;
				Guid id = p.Id;
				string name = p.Name;
				bool isDeleted = p.IsDeleted;
				str = (p.Name.Length >= 10 ? p.Name.Substring(0, 10) : p.Name);
				int? paperCount = p.PaperCount;
				IEnumerable<ExamPaperType> parentId = 
					from s in listAdmin
					where s.ParentId == p.Id
					select s;
				//if (func == null)
				//{
				//	func = (ExamPaperType s) => new { Id = s.Id, Title = s.Name, isdeleted = s.IsDeleted, Name = (s.Name.Length >= 10 ? s.Name.Substring(0, 10) : s.Name), Count = s.PaperCount };
				//}
                var listfunc = parentId.Select((ExamPaperType s) => new { Id = s.Id, Title = s.Name, isdeleted = s.IsDeleted, Name = (s.Name.Length >= 10 ? s.Name.Substring(0, 10) : s.Name), Count = s.PaperCount }).ToList();

                return new { id = id, Title = name, isdeleted = isDeleted, Name = str, Count = paperCount, List = listfunc };
			});
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = list });
			return jsonResult;
		}

		public JsonResult GetFacDep(Guid schoolid)
		{
			JsonResult jsonResult;
			List<School> schoolList = SchoolProvider.Instance.GetListSub(schoolid);
			if (schoolList.Count > 0)
			{
				var list = 
					from p in schoolList
					select new { Id = p.Id, Name = p.Name };
				jsonResult = base.Json(new { State = 1, Msg = "", Result = list });
			}
			else
			{
				jsonResult = base.Json(new { State = 0, Msg = "", Result = "" });
			}
			return jsonResult;
		}

		public JsonResult GetFaculty(Guid schoolid)
		{
			JsonResult jsonResult;
			List<School> faculty = SchoolProvider.Instance.GetListSub(schoolid);
			jsonResult = (faculty.Count <= 0 ? base.Json(new { State = 0, Msg = "", Result = "" }) : base.Json(new { State = 1, Msg = "", Result = faculty }));
			return jsonResult;
		}

		public JsonResult GetHeadMenu()
		{
			JsonResult jsonResult;
			Guid userId = base.CurrentAdmin.Id;
			UserInfo user = UserInfoProvider.Instance.GetUser(userId);
			if ((user == null ? true : user.IsDeleted))
			{
				jsonResult = base.Json(new { State = 1, Msg = "", Result = "" });
			}
			else
			{
				User_RoleProvider.Instance.GetEntityByUserId(user.Id);
				int r = base.CurrentAdmin.Role.Value;
				var commonList = 
					from p in MenuProvider.Instance.GetRoleMenu()
					where !p.ModuleId.HasValue
					orderby p.CreateTime
					select new { Id = p.Id, Title = p.Title, Url = p.Url };
				if ((r == 3 ? false : r != 5))
				{
					var moduleList = 
						from p in MenuProvider.Instance.GetModulesBySchoolId(user.SchoolId)
						orderby p.CreateTime
						select new { Id = p.Id, Title = p.Title, Url = p.Url };
					var list = commonList.Union(moduleList);
					jsonResult = base.Json(new { State = 1, Msg = "", Result = list });
				}
				else
				{
					jsonResult = base.Json(new { State = 1, Msg = "", Result = commonList });
				}
			}
			return jsonResult;
		}

		public JsonResult GetIpList(Guid schoolid)
		{
			List<Ip> ips = IpProvider.Instance.GetList(schoolid);
			StringBuilder sb = new StringBuilder();
			if (ips.Count > 0)
			{
				foreach (Ip ip in ips)
				{
					if (!(IpHelper.ParseIp(ip.IpStart) == IpHelper.ParseIp(ip.IpEnd)))
					{
						sb.AppendLine(string.Concat(IpHelper.ParseIp(ip.IpStart), "-", IpHelper.ParseIp(ip.IpEnd), ","));
					}
					else
					{
						sb.AppendLine(string.Concat(IpHelper.ParseIp(ip.IpStart), ","));
					}
				}
			}
			string str = sb.ToString();
			char[] chrArray = new char[] { ',' };
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = str.TrimEnd(chrArray) });
			return jsonResult;
		}

		public JsonResult GetJob()
		{
			JsonResult jsonResult;
			List<Job> job = JobProvider.Instance.GetList();
			jsonResult = (job.Count <= 0 ? base.Json(new { State = 0, Msg = "", Result = "" }) : base.Json(new { State = 1, Msg = "", Result = job }));
			return jsonResult;
		}

		public JsonResult GetJobDetail(Guid id)
		{
			List<Job> job = JobProvider.Instance.GetList();
			List<UserInfo> listBySchoolId = UserInfoProvider.Instance.GetListBySchoolId(id);
			var list = 
				from p in job
				select new { Name = p.Name, Count = listBySchoolId.Count<UserInfo>((UserInfo u) => {
					Guid? jobId = u.JobId;
					Guid guid = p.Id;
					return (!jobId.HasValue ? false : jobId.GetValueOrDefault() == guid);
				}) };
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = list });
			return jsonResult;
		}

		public JsonResult GetKnowledge(Guid speid)
		{
			JsonResult jsonResult;
			List<Knowledge> list = KnowledgeProvider.Instance.GetList(speid);
			jsonResult = (list.Count <= 0 ? base.Json(new { State = 0, Msg = "", Result = "" }) : base.Json(new { State = 1, Msg = "", Result = list }));
			return jsonResult;
		}

		public JsonResult GetLanResourceList(Guid schoolId, Guid courseId, Guid typeId, string key, int pageIndex)
		{
			List<LanResource> resourceList = LanResourceProvider.Instance.GetLanResource(pageIndex, 50, schoolId, courseId, typeId, key.Trim());
			var list = resourceList.Select((LanResource p) => {
				string str;
				string str1;
				string str2;
				Guid id = p.Id;
				string name = p.Name;
				str = (p.Name.Length >= 15 ? string.Concat(p.Name.Substring(0, 15), ".") : p.Name);
				string viewUrl = p.ViewUrl;
				str1 = (p.Tag2 == null ? "无" : p.Tag2);
				str2 = (p.Tag1 == null ? "无" : p.Tag1);
				Guid guid = schoolId;
				DateTime createTime = p.CreateTime;
				return new { Id = id, Title = name, Name = str, Teacher = viewUrl, CourseName = str1, ResourceTypeName = str2, SchoolId = guid, CreateTime = p.CreateTime.ToString("yyyy-MM-dd") };
			});
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = list });
			return jsonResult;
		}

		public JsonResult GetLanResourceListCount(Guid schoolId, Guid courseId, Guid typeId, string key)
		{
			int count = LanResourceProvider.Instance.GetCount(schoolId, courseId, typeId, key.Trim());
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = count });
			return jsonResult;
		}

		public JsonResult GetLeftMenu(Guid Id)
		{
			//DataManageController.<>c__DisplayClass12 variable = null;
			JsonResult jsonResult;
			Func<Menu, DateTime> func = null;
			
			Guid userId = base.CurrentAdmin.Id;
			UserInfo user = UserInfoProvider.Instance.GetUser(userId);
			string isschool = ConfigurationManager.AppSettings["isschool"];
			if ((user == null ? true : user.IsDeleted))
			{
				jsonResult = base.Json(new { State = 1, Msg = "", Result = "" });
			}
			else
			{
				int r = base.CurrentAdmin.Role.Value;
				List<Menu> menuList = MenuProvider.Instance.GetMenuList();
				var comlist = menuList.Where<Menu>((Menu p) => {
					bool flag;
					bool flag1;
					bool flag2;
					Guid? parentId = p.ParentId;
					Guid id = Id;
					if ((!parentId.HasValue ? false : parentId.GetValueOrDefault() == id))
					{
						int? type = user.Type;
						if ((type.GetValueOrDefault() != 6 ? true : !type.HasValue))
						{
							flag2 = true;
						}
						else
						{
							type = p.new_tag;
							flag2 = (type.GetValueOrDefault() != 1 ? false : type.HasValue);
						}
						if (!flag2)
						{
							flag1 = false;
							flag = flag1;
							return flag;
						}
						type = user.Type;
						flag1 = ((type.GetValueOrDefault() != 5 ? false : type.HasValue) ? p.Id == new Guid("0FEB0F27-DC13-4A60-830D-D5274D4104FC") : true);
						flag = flag1;
						return flag;
					}
					flag1 = false;
					flag = flag1;
					return flag;
				}).OrderBy<Menu, DateTime>((Menu p) => p.CreateTime).Select((Menu p) => {
					//DataManageController.<>c__DisplayClass12 cSu0024u003cu003e8_locals13 = variable;
                        
                    Guid guid = p.Id;
					string title = p.Title;
					IEnumerable<Menu> menus = menuList.Where<Menu>((Menu q) => {
						bool flag;
						bool flag1;
						bool flag2;
						Guid? parentId = q.ParentId;
						Guid id = p.Id;
						if ((!parentId.HasValue ? false : parentId.GetValueOrDefault() == id))
						{
                            //int? type = cSu0024u003cu003e8_locals13.user.Type;
                            int? type = user.Type;
                            if ((type.GetValueOrDefault() != 6 ? true : !type.HasValue))
							{
								flag2 = true;
							}
							else
							{
								type = q.new_tag;
								flag2 = (type.GetValueOrDefault() != 1 ? false : type.HasValue);
							}
							if (!flag2)
							{
								flag1 = false;
								flag = flag1;
								return flag;
							}
                            //type = cSu0024u003cu003e8_locals13.user.Type;
                            type = user.Type;
                            flag1 = ((type.GetValueOrDefault() != 5 ? false : type.HasValue) ? q.Id == new Guid("4B50ADCF-BFAA-4A48-B55F-9FCA4AFE0E42") : true);
							flag = flag1;
							return flag;
						}
						flag1 = false;
						flag = flag1;
						return flag;
					});
					if (func == null)
					{
						func = (Menu q) => q.CreateTime;
					}
					IOrderedEnumerable<Menu> menus1 = menus.OrderBy<Menu, DateTime>(func);
					//if (func1 == null)
					//{
					//	func1 = (Menu q) => new { Id = q.Id, Title = q.Title, Url = q.Url };
					//}
                    var listfunc = menus1.Select((Menu q) => new { Id = q.Id, Title = q.Title, Url = q.Url });

                    return new { Id = guid, Title = title, List = listfunc };
				});
				jsonResult = base.Json(new { State = 1, Msg = "", Result = comlist });
			}
			return jsonResult;
		}

		public JsonResult GetManagerList(int state, string name, int pageIndex)
		{
			bool flag;
			UserInfo user = null;
			Guid schoolId = Guid.Empty;
			Guid facultyId = Guid.Empty;
			if (base.CurrentAdmin != null)
			{
				user = UserInfoProvider.Instance.GetEntity(base.CurrentAdmin.Id);
				int? role = base.CurrentAdmin.Role;
				if ((role.GetValueOrDefault() != 0 ? false : role.HasValue))
				{
					flag = false;
				}
				else
				{
					role = base.CurrentAdmin.Role;
					flag = role != 3 ;
				}
				if (flag)
				{
					role = base.CurrentAdmin.Role;
					if (role != 1)
					{
						role = base.CurrentAdmin.Role;
						if (role == 2 )
						{
							schoolId = user.SchoolId;
							facultyId = (!user.FacultyId.HasValue ? Guid.Empty : user.FacultyId.Value);
						}
					}
					else
					{
						schoolId = user.SchoolId;
					}
				}
				else
				{
					schoolId = Guid.Empty;
				}
			}
			List<UserInfoList> userList = UserInfoProvider.Instance.GetUserList(schoolId, facultyId, state, name, "-1", pageIndex, 30);
			var list = 
				from p in userList
				select new { Id = p.Id, LoginName = p.LoginName, Name = p.Name, SchoolName = (p.SchoolName == null ? "无" : p.SchoolName), FacultyName = (p.FacultyName == null ? "无" : p.FacultyName), DepartmentName = (p.DepartmentName == null ? "无" : p.DepartmentName), UserType = (p.UserType), RoleName = p.RoleName, State = (!p.State ? "已启用" : "已停用") };
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = list });
			return jsonResult;
		}

		public JsonResult GetManagerListCount(int state, string name)
		{
			bool flag;
			UserInfo user = null;
			Guid schoolId = Guid.Empty;
			Guid facultyId = Guid.Empty;
			if (base.CurrentAdmin != null)
			{
				user = UserInfoProvider.Instance.GetEntity(base.CurrentAdmin.Id);
				int? role = base.CurrentAdmin.Role;
				if ((role.GetValueOrDefault() != 0 ? false : role.HasValue))
				{
					flag = false;
				}
				else
				{
					role = base.CurrentAdmin.Role;
					flag = role != 3 ;
				}
				if (flag)
				{
					role = base.CurrentAdmin.Role;
					if (role != 1 )
					{
						schoolId = user.SchoolId;
						facultyId = (!user.FacultyId.HasValue ? Guid.Empty : user.FacultyId.Value);
					}
				}
				else
				{
					schoolId = Guid.Empty;
				}
			}
			int count = UserInfoProvider.Instance.GetUserListCount(schoolId, facultyId, state, "-1", name);
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = count });
			return jsonResult;
		}

		public JsonResult GetMedicalDelResourceList(Guid categoryid, Guid specialityid, Guid typeid, string name, int pageIndex)
		{
			List<MedicalList> resourceList = MedicalProvider.Instance.GetList(categoryid, specialityid, typeid, Guid.Empty, name, pageIndex, 50, 1);
			var list = resourceList.Select((MedicalList p) => {
				string str;
				string str1;
				string str2;
				Guid id = p.Id;
				string title = p.Title;
				str = (p.CategoryName == null ? "无" : p.CategoryName);
				str1 = (p.SpecialityName == null ? "无" : p.SpecialityName);
				str2 = (p.TypeName == null ? "无" : p.TypeName);
				DateTime createTime = p.CreateTime;
				return new { Id = id, Name = title, CategoryName = str, SpecialityName = str1, TypeName = str2, CreateTime = p.CreateTime.ToString("yyyy-MM-dd"), Size = (new WriteSize()).WriteResourceSize(p.Size.ToString()) };
			});
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = list });
			return jsonResult;
		}

		public JsonResult GetMedicalDelResourceListCount(Guid categoryid, Guid specialityid, Guid typeid, string name)
		{
			int count = MedicalProvider.Instance.GetCount(categoryid, specialityid, typeid, Guid.Empty, name, 1);
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = count });
			return jsonResult;
		}

		public JsonResult GetMedicalKnowledge(Guid speid)
		{
			JsonResult jsonResult;
			List<MedicalKnowledge> list = MedicalKnowledgeProvider.Instance.GetKnowledgeList(speid);
			jsonResult = (list.Count <= 0 ? base.Json(new { State = 0, Msg = "", Result = "" }) : base.Json(new { State = 1, Msg = "", Result = list }));
			return jsonResult;
		}

		public JsonResult GetMedicalResourceList(Guid categoryid, Guid specialityid, Guid typeid, string name, int pageIndex)
		{
			Guid schoolid = Guid.Empty;
			UserInfoProvider.Instance.GetEntity(base.CurrentAdmin.Id);
			List<MedicalList> resourceList = MedicalProvider.Instance.GetList(categoryid, specialityid, typeid, Guid.Empty, name, pageIndex, 50, 0);
			var list = resourceList.Select((MedicalList p) => {
				string str;
				string str1;
				string str2;
				Guid id = p.Id;
				string title = p.Title;
				str = (p.CategoryName == null ? "无" : p.CategoryName);
				str1 = (p.SpecialityName == null ? "无" : p.SpecialityName);
				str2 = (p.TypeName == null ? "无" : p.TypeName);
				DateTime createTime = p.CreateTime;
				return new { Id = id, Name = title, CategoryName = str, SpecialityName = str1, TypeName = str2, CreateTime = p.CreateTime.ToString("yyyy-MM-dd"), Size = (new WriteSize()).WriteResourceSize(p.Size.ToString()) };
			});
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = list });
			return jsonResult;
		}

		public JsonResult GetMedicalResourceListCount(Guid categoryid, Guid specialityid, Guid typeid, string name)
		{
			int count = MedicalProvider.Instance.GetCount(categoryid, specialityid, typeid, Guid.Empty, name, 0);
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = count });
			return jsonResult;
		}

		public JsonResult GetMedicalSpeciality(Guid cateid)
		{
			JsonResult jsonResult;
			List<MedicalSpeciality> list = MedicalSpecialityProvider.Instance.GetSpecialityList(cateid);
			jsonResult = (list.Count <= 0 ? base.Json(new { State = 0, Msg = "", Result = "" }) : base.Json(new { State = 1, Msg = "", Result = list }));
			return jsonResult;
		}

		public JsonResult GetMedicalStatistics()
		{
			
			List<Resourcelist> listCate = MedicalProvider.Instance.GetCateCountList();
			List<Resourcelist> speCountList = MedicalProvider.Instance.GetSpeCountList();
			var list = listCate.Select((Resourcelist p) => {
				Guid id = p.Id;
				string categoryName = p.CategoryName;
				int size = p.Size;
				string str = (new WriteSize()).WriteResourceSize(p.Tag1);
				IEnumerable<Resourcelist> resourcelists = 
					from q in speCountList
					where q.CategoryName == p.CategoryName
					select q;
				//if (func == null)
				//{
				//	func = (Resourcelist q) => new { Id = q.Id, Name = q.SpecialityName, Count = q.Size, Size = (new WriteSize()).WriteResourceSize(q.Tag1) };
				//}
                var listfunc = resourcelists.Select((Resourcelist q) => new { Id = q.Id, Name = q.SpecialityName, Count = q.Size, Size = (new WriteSize()).WriteResourceSize(q.Tag1) });

                return new { Id = id, Name = categoryName, Count = size, Size = str, List = listfunc };
			});
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = 
				from p in list
				orderby p.List.Count() descending
				select p });
			return jsonResult;
		}

		public JsonResult GetMenuList()
		{
			//var func = null;
			//var func1 = null;
			List<Menu> menuList = MenuProvider.Instance.GetMenuList();
			var commonList = menuList.Where<Menu>((Menu p) => {
				bool flag;
				if (p.ParentId.HasValue)
				{
					flag = false;
				}
				else
				{
					int? type = p.Type;
					if ((type.GetValueOrDefault() != -1 ? false : type.HasValue))
					{
						flag = true;
					}
					else
					{
						type = p.Type;
						flag = (type.GetValueOrDefault() != 1 ? false : type.HasValue);
					}
				}
				return flag;
			}).Select((Menu p) => {
				string str;
				string str1;
				string str2;
				string str3;
				Guid guid = p.Id;
				string title = p.Title;
				str = (p.IsDeleted ? "(已隐藏)" : "");
				str1 = (p.IsDeleted ? "0" : "1");
				str2 = (p.IsDeleted ? "显示" : "隐藏");
				int? type = p.Type;
				str3 = (!p.ModuleId.HasValue ? "" : p.ModuleId.ToString());
				IEnumerable<Menu> menus = menuList.Where<Menu>((Menu q) => {
					Guid? parentId = q.ParentId;
					Guid id = p.Id;
					return (!parentId.HasValue ? false : parentId.GetValueOrDefault() == id);
				});
				//if (func == null)
				//{
				//	func = (Menu q) => new { Id = q.Id, Title = q.Title, Flag = (q.IsDeleted ? "(已隐藏)" : ""), IsDeleted = (q.IsDeleted ? "0" : "1"), Stats = (q.IsDeleted ? "显示" : "隐藏"), Url = q.Url };
				//}
                var listfunc = menus.Select((Menu q) => new { Id = q.Id, Title = q.Title, Flag = (q.IsDeleted ? "(已隐藏)" : ""), IsDeleted = (q.IsDeleted ? "0" : "1"), Stats = (q.IsDeleted ? "显示" : "隐藏"), Url = q.Url });

                return new { Id = guid, Title = title, Flag = str, IsDeleted = str1, Stats = str2, Level = type, Module = str3, List = listfunc };
			});
			var moduleList = menuList.Where<Menu>((Menu p) => {
				bool flag;
				if (p.ParentId.HasValue)
				{
					flag = false;
				}
				else
				{
					int? type = p.Type;
					flag = (type.GetValueOrDefault() != 0 ? false : type.HasValue);
				}
				return flag;
			}).Select((Menu p) => {
				string str;
				string str1;
				string str2;
				string str3;
				Guid guid = p.Id;
				string title = p.Title;
				str = (p.IsDeleted ? "(已隐藏)" : "");
				str1 = (p.IsDeleted ? "0" : "1");
				str2 = (p.IsDeleted ? "显示" : "隐藏");
				int? type = p.Type;
				str3 = (!p.ModuleId.HasValue ? "" : p.ModuleId.ToString());
				IEnumerable<Menu> menus = menuList.Where<Menu>((Menu q) => {
					Guid? parentId = q.ParentId;
					Guid id = p.Id;
					return (!parentId.HasValue ? false : parentId.GetValueOrDefault() == id);
				});
				//if (func1 == null)
				//{
				//	func1 = (Menu q) => new { Id = q.Id, Title = q.Title, Flag = (q.IsDeleted ? "(已隐藏)" : ""), IsDeleted = (q.IsDeleted ? "0" : "1"), Stats = (q.IsDeleted ? "显示" : "隐藏"), Url = q.Url };
				//}
                var listfuncl = menus.Select((Menu q) => new { Id = q.Id, Title = q.Title, Flag = (q.IsDeleted ? "(已隐藏)" : ""), IsDeleted = (q.IsDeleted ? "0" : "1"), Stats = (q.IsDeleted ? "显示" : "隐藏"), Url = q.Url });

                return new { Id = guid, Title = title, Flag = str, IsDeleted = str1, Stats = str2, Level = type, Module = str3, List = listfuncl };
			});
			var list = commonList.Union(moduleList);
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = list });
			return jsonResult;
		}

		public JsonResult GetModuleList()
		{
			var list = 
				from p in ModuleProvider.Instance.GetModuleList()
				select new { Id = p.Id, Name = p.Name, Checked = "" };
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = list });
			return jsonResult;
		}

		public JsonResult GetModules(Guid id)
		{
			List<School_Module> schoolModules = School_ModuleProvider.Instance.GetList(id);
			List<Module> moduleList = ModuleProvider.Instance.GetList();
			var list = 
				from p in moduleList
				select new { Id = p.Id, Name = p.Name, Checked = (schoolModules.Count<School_Module>((School_Module s) => s.ModuleId == p.Id) > 0 ? "checked" : ""), trialTag = (schoolModules.Count<School_Module>((School_Module s) => s.ModuleId == p.Id) > 0 ? schoolModules.FirstOrDefault<School_Module>((School_Module s) => s.ModuleId == p.Id).TrialTag.ToString() : "") };
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = list });
			return jsonResult;
		}

		[HttpPost]
		public JsonResult GetPaper(Guid id)
		{
			
			List<ExamItemPart> Parts = ExamItemPartProvider.Instance.GetListByPaper(id);
			List<ExamItem> listByPaper = ExamItemProvider.Instance.GetListByPaper(id);
			var list = Parts.Select((ExamItemPart p) => {
				ExamItemPart examItemPart = p;
				IEnumerable<ExamItem> examItems = listByPaper.Where<ExamItem>((ExamItem i) => {
					Guid? partId = i.PartId;
					Guid guid = p.Id;
					return (!partId.HasValue ? false : partId.GetValueOrDefault() == guid);
				});
				//if (func == null)
				//{
				//	func = (ExamItem s) => new { Id = s.Id, SortId = s.SortId, Question = s.Question, Answer = s.Answer, Explain = s.Explain };
				//}
                var listfunc = examItems.Select((ExamItem s) => new { Id = s.Id, SortId = s.SortId, Question = s.Question, Answer = s.Answer, Explain = s.Explain }).ToList();

                return new { Part = examItemPart, List = listfunc };
			});
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "试卷内容", Result = list });
			return jsonResult;
		}

		public JsonResult GetPaperList2(Guid id, string name, int pageindex)
		{
			List<ExamPaperType> examPaperTypes = ExamPaperTypeProvider.Instance.GetList();
			List<ExamPaper> paperlist = ExamPaperProvider.Instance.GetListByTypeId2(id, name, pageindex, 20);
			var list = 
				from p in paperlist
				select new { Id = p.Id, Title = p.Name, Name = (p.Name.Length >= 15 ? p.Name.Substring(0, 15) : p.Name), Type1 = examPaperTypes.Single<ExamPaperType>((ExamPaperType a) => a.Id == examPaperTypes.Single<ExamPaperType>((ExamPaperType b) => p.TypeId == b.Id).ParentId).Name, Type2 = examPaperTypes.Single<ExamPaperType>((ExamPaperType b) => p.TypeId == b.Id).Name, Time = p.TimeOut, YearMonth = string.Concat(p.Year, ".", p.Month).ToString() };
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = list });
			return jsonResult;
		}

		public JsonResult GetPaperList3(Guid id, string name, int pageindex)
		{
			List<ExamPaperType> examPaperTypes = ExamPaperTypeProvider.Instance.GetList();
			List<ExamPaper> paperlist = ExamPaperProvider.Instance.GetListByTypeId3(id, name, pageindex, 20);
			var list = 
				from p in paperlist
				select new { Id = p.Id, Title = p.Name, Name = (p.Name.Length >= 15 ? p.Name.Substring(0, 15) : p.Name), Type1 = examPaperTypes.Single<ExamPaperType>((ExamPaperType a) => a.Id == examPaperTypes.Single<ExamPaperType>((ExamPaperType b) => p.TypeId == b.Id).ParentId).Name, Type2 = examPaperTypes.Single<ExamPaperType>((ExamPaperType b) => p.TypeId == b.Id).Name, Time = p.TimeOut, YearMonth = string.Concat(p.Year, ".", p.Month).ToString() };
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = list });
			return jsonResult;
		}

		public JsonResult GetPaperListCount2(Guid id, string name)
		{
			int count = ExamPaperProvider.Instance.GetListCountByTypeId2(id, name.Trim());
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = count });
			return jsonResult;
		}

		public JsonResult GetPaperListCount3(Guid id, string name)
		{
			int count = ExamPaperProvider.Instance.GetListCountByTypeId3(id, name.Trim());
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = count });
			return jsonResult;
		}

		public JsonResult GetPost()
		{
			JsonResult jsonResult;
			List<Post> post = PostProvider.Instance.GetList();
			jsonResult = (post.Count <= 0 ? base.Json(new { State = 0, Msg = "", Result = "" }) : base.Json(new { State = 1, Msg = "", Result = post }));
			return jsonResult;
		}

		public JsonResult GetPostDetail(Guid id)
		{
			List<Post> post = PostProvider.Instance.GetList();
			List<UserInfo> listBySchoolId = UserInfoProvider.Instance.GetListBySchoolId(id);
			var list = 
				from p in post
				select new { Name = p.Name, Count = listBySchoolId.Count<UserInfo>((UserInfo u) => {
					Guid? postId = u.PostId;
					Guid guid = p.Id;
					return (!postId.HasValue ? false : postId.GetValueOrDefault() == guid);
				}) };
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = list });
			return jsonResult;
		}

		public JsonResult GetPro()
		{
			JsonResult jsonResult;
			List<Profession> pro = ProfessionProvider.Instance.GetList();
			jsonResult = (pro.Count <= 0 ? base.Json(new { State = 0, Msg = "", Result = "" }) : base.Json(new { State = 1, Msg = "", Result = pro }));
			return jsonResult;
		}

		public JsonResult GetProfessionDetail(Guid id)
		{
			List<Profession> profession = ProfessionProvider.Instance.GetList();
			List<UserInfo> listBySchoolId = UserInfoProvider.Instance.GetListBySchoolId(id);
			var list = 
				from p in profession
				select new { Name = p.Name, Count = listBySchoolId.Count<UserInfo>((UserInfo u) => {
					Guid? professionId = u.ProfessionId;
					Guid guid = p.Id;
					return (!professionId.HasValue ? false : professionId.GetValueOrDefault() == guid);
				}) };
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = list });
			return jsonResult;
		}

		public JsonResult GetQualityList(string sort, string year, int conn, string name, int pageIndex)
		{
			List<QualityCourse> courseList = QualityCourseProvider.Instance.GetCourseList(sort, year, conn, name, pageIndex, 50);
			var list = courseList.Select((QualityCourse p) => {
				string str;
				Guid id = p.Id;
				str = (p.Name.Length >= 15 ? string.Concat(p.Name.Substring(0, 15), ".") : p.Name);
				string str2 = p.Name;
				string schoolName = p.SchoolName;
				string author = p.Author;
				int? awardsDate = p.AwardsDate;
				string str1 = p.CourceSort.Replace("精品课程", "");
				int? connected = p.Connected;
				return new { Id = id, Name = str, Title = str2, SchoolName = schoolName, Author = author, AwardsDate = awardsDate, CourceSort = str1, Connected = ((connected.GetValueOrDefault() != 0 ? false : connected.HasValue) ? "正常" : "错误"), Province = p.Province };
			});
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = list });
			return jsonResult;
		}

		public JsonResult GetQualityListCount(string sort, string year, int conn, string name)
		{
			int count = QualityCourseProvider.Instance.GetCourseCount(sort, year, conn, name);
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = count });
			return jsonResult;
		}

		public JsonResult GetResCategorySubject(Guid resourceid)
		{
			List<Category> list1 = CategoryProvider.Instance.GetList();
			List<Subject> subjects = SubjectProvider.Instance.GetList();
			Resource entity = ResourceProvider.Instance.GetEntity(resourceid);
			var list = 
				from p in list1
				select new { Id = p.Id, Name = p.Name, Count = p.Count, Ico = p.Ico, List = (
					from s in subjects
					where s.CategoryId == p.Id
					select s).Select((Subject s) => {
					Guid id = s.Id;
					string name = s.Name;
					int? count = s.Count;
					Guid? subjectId = entity.SubjectId;
					Guid guid = s.Id;
					return new { Id = id, Name = name, Count = count, Checked = ((!subjectId.HasValue ? false : subjectId.GetValueOrDefault() == guid) ? "selected='selected'" : "") };
				}).ToList() };
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = list });
			return jsonResult;
		}

		public JsonResult GetResourceList(Guid csid, Guid specialityid, Guid typeid, string name, int pageIndex)
		{
			Guid schoolid = Guid.Empty;
			UserInfoProvider.Instance.GetEntity(base.CurrentAdmin.Id);
			List<Resourcelist> resourceList = ResourceProvider.Instance.GetResourceMaterialList(schoolid, csid, specialityid, typeid, name, pageIndex, 50, 0);
			var list = resourceList.Select((Resourcelist p) => {
				string str;
				string str1;
				string str2;
				string str3;
				string str4;
				string str5;
				Guid id = p.Id;
				string title = p.Title;
				str = (p.Title.Length >= 15 ? string.Concat(p.Title.Substring(0, 15), ".") : p.Title);
				str1 = (p.CategoryName == null ? "无" : p.CategoryName);
				str2 = (p.SubjectName == null ? "无" : p.SubjectName);
				str3 = (p.SpecialityName == null ? "无" : p.SpecialityName);
				str4 = (p.KnowledgeName == null ? "无" : p.KnowledgeName);
				str5 = (p.TypeName == null ? "无" : p.TypeName);
				DateTime createTime = p.CreateTime;
				return new { Id = id, Title = title, Name = str, CategoryName = str1, SubjectName = str2, SpecialityName = str3, KnowledgeName = str4, TypeName = str5, CreateTime = p.CreateTime.ToString("yyyy-MM-dd"), Size = (new WriteSize()).WriteResourceSize(p.Size.ToString()) };
			});
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = list });
			return jsonResult;
		}

		public JsonResult GetResourceListCount(Guid csid, Guid specialityid, Guid typeid, string name)
		{
			Guid schoolid = Guid.Empty;
			UserInfoProvider.Instance.GetEntity(base.CurrentAdmin.Id);
			int count = ResourceProvider.Instance.GetCount(schoolid, specialityid, typeid, name, 0);
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = count });
			return jsonResult;
		}

		public JsonResult GetRole(Guid schoolid)
		{
			JsonResult jsonResult;
			List<Role> role = RoleProvider.Instance.GetListBySchoolId(schoolid);
			jsonResult = (role.Count <= 0 ? base.Json(new { State = 0, Msg = "", Result = "" }) : base.Json(new { State = 1, Msg = "", Result = role }));
			return jsonResult;
		}

		public JsonResult GetRType()
		{
			JsonResult jsonResult;
			List<ResourceType> list = Resource_SubjectProvider.Instance.GetList();
			jsonResult = (list.Count <= 0 ? base.Json(new { State = 0, Msg = "", Result = "" }) : base.Json(new { State = 1, Msg = "", Result = list }));
			return jsonResult;
		}

		public JsonResult GetSchool()
		{
			JsonResult jsonResult;
			bool flag;
			UserInfo admin = UserInfoProvider.Instance.GetEntity(base.CurrentAdmin.Id);
			int? role = base.CurrentAdmin.Role;
			if ((role.GetValueOrDefault() != 0 ? false : role.HasValue))
			{
				flag = false;
			}
			else
			{
				role = base.CurrentAdmin.Role;
				flag = role != 3;
			}
			if (flag)
			{
				School school = SchoolProvider.Instance.GetEntity(admin.SchoolId);
				jsonResult = base.Json(new { State = 1, Msg = "", Result = school });
			}
			else
			{
				List<School> school = (
					from p in SchoolProvider.Instance.GetList()
					orderby p.Name
					select p).ToList<School>();
				jsonResult = base.Json(new { State = 1, Msg = "", Result = school });
			}
			return jsonResult;
		}

		public JsonResult GetSchoolInfoList(Guid sellerid, Guid privinceID, string name, string TrialTag, string Startime, string Endtime, int pageIndex)
		{
			bool flag;
			Guid schoolid = Guid.Empty;
			int? role = base.CurrentAdmin.Role;
			if ((role.GetValueOrDefault() != 5 ? false : role.HasValue))
			{
				flag = false;
			}
			else
			{
				role = base.CurrentAdmin.Role;
				flag = role != 6 ;
			}
			if (!flag)
			{
				schoolid = base.CurrentAdmin.SchoolId;
			}
			List<SchoolLogDetail> schoolList = SchoolProvider.Instance.GetListLogDetail(sellerid, privinceID, name.Trim(), TrialTag, Startime, Endtime, 0, schoolid, pageIndex, 10);
			string item = ConfigurationManager.AppSettings["isschool"];
			var list = schoolList.Select((SchoolLogDetail p) => {
				DateTime value;
				string str;
				string str1;
				string str2;
				Guid id = p.Id;
				string str3 = p.Name;
				if (string.IsNullOrEmpty(p.Tag2))
				{
					str = "无";
				}
				else
				{
					string tag2 = p.Tag2;
					int? trialType = p.TrialType;
					str = string.Concat(tag2, ((trialType.GetValueOrDefault() != 0 ? false : trialType.HasValue) ? "(试用)" : "(镜像)"));
				}
				if (!p.StartTime.HasValue)
				{
					str1 = "";
				}
				else
				{
					value = p.StartTime.Value;
					str1 = value.ToString("yyyy-MM-dd");
				}
				if (!p.EndTime.HasValue)
				{
					str2 = "";
				}
				else
				{
					value = p.EndTime.Value;
					str2 = value.ToString("yyyy-MM-dd");
				}
				return new { Id = id, Name = str3, Tag2 = str, StartTime = str1, EndTime = str2, Tag1 = (string.IsNullOrEmpty(p.Tag1) ? "" : p.Tag1), PressCount = p.PressCount, ProvinceName = p.ProvinceName, DownLoadCount = p.DownLoadCount, isschool = item };
			});
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = list, Identity = base.CurrentAdmin.Role });
			return jsonResult;
		}

		public JsonResult GetSchoolInfoListCount(Guid sellerid, Guid privinceID, string name, string TrialTag, string Startime, string Endtime)
		{
			bool flag;
			Guid schoolid = Guid.Empty;
			int? role = base.CurrentAdmin.Role;
			if ((role.GetValueOrDefault() != 5 ? false : role.HasValue))
			{
				flag = false;
			}
			else
			{
				role = base.CurrentAdmin.Role;
                flag = role.GetValueOrDefault() != 6;
			}
			if (!flag)
			{
				schoolid = base.CurrentAdmin.SchoolId;
			}
			int count = SchoolProvider.Instance.GetCount(sellerid, privinceID, name, 0, TrialTag, Startime, Endtime, schoolid);
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = count });
			return jsonResult;
		}

		public JsonResult GetSpeciality(Guid subid)
		{
			JsonResult jsonResult;
			List<ResourceCategory> list = ResourceCategoryProvider.Instance.GetCategoryTier2(subid);
			jsonResult = (list.Count <= 0 ? base.Json(new { State = 0, Msg = "", Result = "" }) : base.Json(new { State = 1, Msg = "", Result = list }));
			return jsonResult;
		}

		public JsonResult GetStatistics()
		{
			
			List<Resourcelist> listCate = ResourceProvider.Instance.GetCateCountList();
			List<Resourcelist> subCountList = ResourceProvider.Instance.GetSubCountList();
			var list = listCate.Select((Resourcelist p) => {
				Guid id = p.Id;
				string categoryName = p.CategoryName;
				int size = p.Size;
				string str = (new WriteSize()).WriteResourceSize(p.Tag1);
				IEnumerable<Resourcelist> resourcelists = 
					from q in subCountList
					where (q.CategoryName != p.CategoryName ? false : q.Size != 0)
					select q;
				//if (func == null)
				//{
				//	func = (Resourcelist q) => new { Id = q.Id, Name = q.SubjectName, Count = q.Size, Size = (new WriteSize()).WriteResourceSize(q.Tag1) };
				//}

                var listfunc = resourcelists.Select((Resourcelist q) => new { Id = q.Id, Name = q.SubjectName, Count = q.Size, Size = (new WriteSize()).WriteResourceSize(q.Tag1) });

                return new { Id = id, Name = categoryName, Count = size, Size = str, List = listfunc };
			});
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = 
				from p in list
				orderby p.List.Count() descending
				select p });
			return jsonResult;
		}

		public JsonResult GetSub2(Guid sub1)
		{
			List<QualitySubject2> list = QualitySubject2Provider.Instance.GetListBySub1(sub1);
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = list });
			return jsonResult;
		}

		public JsonResult GetSubject(Guid cateid)
		{
			JsonResult jsonResult;
			List<ResourceCategory> list = ResourceCategoryProvider.Instance.GetCategoryTier1(cateid);
			jsonResult = (list.Count <= 0 ? base.Json(new { State = 0, Msg = "", Result = "" }) : base.Json(new { State = 1, Msg = "", Result = list }));
			return jsonResult;
		}

		public JsonResult GetSysModules(Guid id)
		{
			List<School_Module> schoolModules = School_ModuleProvider.Instance.GetList(id);
			List<Module> moduleList = (
				from p in ModuleProvider.Instance.GetList()
				orderby p.CreateTime
				select p).ToList<Module>();
			var list = 
				from p in moduleList
				select new { Id = p.Id, Name = p.Name, Checked = (schoolModules.Count<School_Module>((School_Module s) => (s.ModuleId != p.Id ? false : !s.IsDeleted)) > 0 ? "checked='checked'" : ""), Disabled = (schoolModules.Count<School_Module>((School_Module s) => s.ModuleId == p.Id) == 0 ? "disabled='disabled'" : "disabled='disabled'") };
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = list });
			return jsonResult;
		}

		public JsonResult GetUserList(int state, string name, int pageIndex)
		{
			UserInfo user = null;
			Guid schoolId = Guid.Empty;
			Guid facultyId = Guid.Empty;
			if (base.CurrentAdmin != null)
			{
				user = UserInfoProvider.Instance.GetEntity(base.CurrentAdmin.Id);
				int? role = base.CurrentAdmin.Role;
				if (role.GetValueOrDefault() != 1)
				{
					role = base.CurrentAdmin.Role;
					if (role.GetValueOrDefault() != 2)
					{
						schoolId = user.SchoolId;
						facultyId = (!user.FacultyId.HasValue ? Guid.Empty : user.FacultyId.Value);
					}
				}
				else
				{
					schoolId = user.SchoolId;
				}
			}
			List<UserInfoList> userList = UserInfoProvider.Instance.GetUserList(schoolId, facultyId, state, name, "0,1,5", pageIndex, 30);
			var list = 
				from p in userList
				select new { Id = p.Id, LoginName = p.LoginName, Name = p.Name, SchoolName = (p.SchoolName == null ? "无" : p.SchoolName), FacultyName = (p.FacultyName == null ? "无" : p.FacultyName), DepartmentName = (p.DepartmentName == null ? "无" : p.DepartmentName), UserType = p.UserType, Type = p.UserType, State = (!p.State ? "已启用" : "已停用") };
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = list });
			return jsonResult;
		}

		public JsonResult GetUserListCount(int state, string name)
		{
			UserInfo user = null;
			Guid schoolId = Guid.Empty;
			Guid facultyId = Guid.Empty;
			if (base.CurrentAdmin != null)
			{
				user = UserInfoProvider.Instance.GetEntity(base.CurrentAdmin.Id);
				int? role = base.CurrentAdmin.Role;
				if (role != 1 )
				{
					role = base.CurrentAdmin.Role;
					if (role.GetValueOrDefault() != 2)
					{
						schoolId = user.SchoolId;
						facultyId = (!user.FacultyId.HasValue ? Guid.Empty : user.FacultyId.Value);
					}
				}
				else
				{
					schoolId = user.SchoolId;
				}
			}
			int count = UserInfoProvider.Instance.GetUserListCount(schoolId, facultyId, state, "0,1,5", name);
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = count });
			return jsonResult;
		}

		//public JsonResult GetUsers(Guid id)
		//{
			
		//}

		public JsonResult Teachers(string guidlist)
		{
			JsonResult jsonResult;
			if (guidlist.Length > 0)
			{
				List<Guid> idlist = JsonHelper.Deserialize<List<Guid>>(guidlist);
				List<UserInfo> list = new List<UserInfo>();
				for (int i = 0; i < idlist.Count; i++)
				{
					UserInfo user = UserInfoProvider.Instance.GetUser(idlist[i]);
					list.Add(user);
				}
				jsonResult = base.Json(new { State = 1, Msg = "", Result = list });
			}
			else
			{
				jsonResult = base.Json(new { State = 0, Msg = "", Result = "" });
			}
			return jsonResult;
		}

	}
}