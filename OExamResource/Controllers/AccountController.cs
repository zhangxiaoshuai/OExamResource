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
        [WebFilter(Order = 0), UserFilter(Order = 1)]
        public PartialViewResult AccInfo()
        {
            UserView viewEntity = UserInfoProvider.Instance.GetViewEntity(base.CurrentUser.Id);
            return base.PartialView(viewEntity);
        }

        [TeacherFilter(Order = 1), WebFilter(Order = 0)]
        public JsonResult AddCourse(string txt)
        {
            UserInfo entity = UserInfoProvider.Instance.GetEntity(base.CurrentUser.Id);
            if (LanCourseProvider.Instance.GetCourseList(entity.SchoolId).Count >= 5)
            {
                return base.Json(new
                {
                    State = 0,
                    Msg = "对不起，本系统只能创建5门课程，如有更多需求请联系管理员。"
                });
            }
            LanCourse lanCourse = new LanCourse
            {
                Name = txt.Trim(),
                ClickCount = 0,
                CreateTime = DateTime.Now,
                DepartmentId = entity.DepartmentId,
                FacultyId = entity.FacultyId,
                IsDeleted = false,
                ResourceCount = 0,
                SchoolId = new Guid?(entity.SchoolId),
                UserId = new Guid?(entity.Id)
            };
            lanCourse = LanCourseProvider.Instance.Create(lanCourse);
            School school = SchoolProvider.Instance.GetEntity(base.CurrentUser.SchoolId);
            LanCourseProvider.Instance.UpdateLanCourseCount(base.CurrentUser.SchoolId, 1);
            return base.Json(new
            {
                State = 1,
                Msg = "添加成功",
                Result = lanCourse.Id
            });
        }

        [WebFilter(Order = 0), TeacherFilter(Order = 1)]
        public JsonResult AddResource(Guid hdId, string name, Guid typeId, HttpPostedFileBase mini, HttpPostedFileBase view, HttpPostedFileBase file)
        {
            string extension;
            if (string.IsNullOrWhiteSpace(name))
            {
                return base.Json(new
                {
                    State = 0,
                    Msg = "请填写资源名称！",
                    Result = ""
                }, "text/html");
            }
            if ((mini == null) || (mini.ContentLength <= 0))
            {
                return base.Json(new
                {
                    State = 0,
                    Msg = "请您上传缩略图片",
                    Result = ""
                }, "text/html");
            }
            LanResource lanResource = new LanResource
            {
                Id = Guid.NewGuid(),
                CourseId = new Guid?(hdId),
                TypeId = new Guid?(typeId),
                Name = name,
                IsShare = false,
                CreateTime = DateTime.Now,
                IsDeleted = false
            };
            string path = "/Images/Lan/LanResource/" + lanResource.Id;
            string[] source = new string[] { ".jpg", ".png", ".jpeg", "gif" };
            string str2 = Path.GetExtension(mini.FileName).ToLower();
            if (!source.Contains<string>(str2))
            {
                return base.Json(new
                {
                    State = 0,
                    Msg = "缩略图片格式不正确！",
                    Result = ""
                }, "text/html");
            }
            string str3 = path + "/mini" + str2;
            lanResource.ImageUrl = str3;
            FileHelper.ResizeImage(mini, 130, 100, Server.MapPath(path), "mini" + str2);
            if ((view != null) && (view.ContentLength > 0))
            {
                extension = Path.GetExtension(view.FileName);
                string str5 = path + "/view" + extension;
                lanResource.ViewUrl = str5;
                FileHelper.UploadFile(view, Server.MapPath(path), "view" + extension);
            }
            if ((file != null) && (file.ContentLength > 0))
            {
                extension = Path.GetExtension(file.FileName);
                string str6 = path + "/file" + extension;
                lanResource.DownloadUrl = str6;
                lanResource.NameEx = extension;
                FileHelper.UploadFile(file, Server.MapPath(path), "file" + extension);
            }
            LanResourceProvider.Instance.Create(lanResource);
            if ((lanResource.CourseId != Guid.Empty) && lanResource.CourseId.HasValue)
            {
                LanCourseProvider.Instance.AddResourceCount(lanResource.CourseId.Value);
            }
            LanCourseProvider.Instance.UpdateLanResourceCount(base.CurrentUser.SchoolId, 1);
            return base.Json(new
            {
                State = 1,
                Msg = "添加成功",
                Result = ""
            }, "text/html");
        }

        [WebFilter(Order = 0), UserFilter(Order = 1)]
        public ViewResult Collect()
        {
            bool flag = base.CurrentUser.Modules.Contains("resource");
            bool flag2 = base.CurrentUser.Modules.Contains("medical");
            bool flag3 = base.CurrentUser.Modules.Contains("exam");
            bool flag4 = base.CurrentUser.Modules.Contains("lan");
            List<int> list = new List<int>();
            list.Insert(0, flag ? Resource_SubjectProvider.Instance.CollectCount(base.CurrentUser.Id, 0) : -1);
            list.Insert(1, flag2 ? Resource_SubjectProvider.Instance.CollectCount(base.CurrentUser.Id, 1) : -1);
            list.Insert(2, flag3 ? ExamCollectProvider.Instance.GetCounByUser(base.CurrentUser.Id) : -1);
            list.Insert(3, flag4 ? LanCollectProvider.Instance.GetCount(base.CurrentUser.Id) : -1);
            return base.View(list);
        }

        [UserFilter(Order = 1), WebFilter(Order = 0)]
        public JsonResult CollectDeletedk1(Guid resourceid)
        {
            bool flag = Resource_SubjectProvider.Instance.Collectdeleted(base.CurrentUser.Id, resourceid);
            if (flag)
            {
                return base.Json(new
                {
                    State = 1,
                    Msg = "删除成功",
                    Result = flag
                });
            }
            return base.Json(new
            {
                State = 0,
                Msg = "删除失败",
                Result = flag
            });
        }

        [UserFilter(Order = 1), WebFilter(Order = 0)]
        public JsonResult CollectDeletedk2(Guid resourceid)
        {
            bool flag = MedicalProvider.Instance.Collectdeleted(base.CurrentUser.Id, resourceid);
            if (flag)
            {
                return base.Json(new
                {
                    State = 1,
                    Msg = "删除成功",
                    Result = flag
                });
            }
            return base.Json(new
            {
                State = 0,
                Msg = "删除失败",
                Result = flag
            });
        }

        [UserFilter(Order = 1), WebFilter(Order = 0)]
        public JsonResult CollectlistK1(Guid? id, int medicalTag, int pageIndex, int pageCount)
        {
            id = new Guid?(base.CurrentUser.Id);
            string str = WebConfigurationManager.AppSettings["Resource_Down_Url"];
            List<CollectSource> list = Resource_SubjectProvider.Instance.Collect(id, medicalTag, pageIndex, pageCount);
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Icofilepath = "/k1" + list[i].Icofilepath;
                list[i].Previewfilepath = "/k1" + list[i].Previewfilepath;
                list[i].DownloadFilepath = str + list[i].DownloadFilepath;
            }
            return base.Json(new
            {
                State = 1,
                Msg = "ok",
                Result = list
            });
        }

        [UserFilter(Order = 1), WebFilter(Order = 0)]
        public JsonResult Collectlistk1_To_preview(Guid hdid)
        {
            Models.Resource entity = ResourceProvider.Instance.GetEntity(hdid);
            return base.Json(new
            {
                State = 1,
                Msg = "ok",
                Result = entity
            });
        }

        [WebFilter(Order = 0), UserFilter(Order = 1)]
        public JsonResult CollectlistK2(Guid? id, int pageIndex, int pageCount)
        {
            id = new Guid?(base.CurrentUser.Id);
            List<CollectSource> list = MedicalProvider.Instance.Collect(id, pageIndex, pageCount);
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Icofilepath = "/k2" + list[i].Icofilepath;
                list[i].Previewfilepath = "/k2" + list[i].Previewfilepath;
                list[i].DownloadFilepath = "/k2" + list[i].DownloadFilepath;
            }
            return base.Json(new
            {
                State = 1,
                Msg = "ok",
                Result = list
            });
        }

        [WebFilter(Order = 0), UserFilter(Order = 1)]
        public JsonResult Collectlistk2_To_preview(Guid hdid)
        {
            Medical entity = MedicalProvider.Instance.GetEntity(hdid);
            return base.Json(new
            {
                State = 1,
                Msg = "ok",
                Result = entity
            });
        }

        [TeacherFilter(Order = 1), WebFilter(Order = 0)]
        public PartialViewResult Course(Guid? id)
        {
            List<LanCourse> list = LanCourseProvider.Instance.GetList(base.CurrentUser.Id);
            List<SelectListItem> list2 = new List<SelectListItem>();
            foreach (LanCourse course in list)
            {
                SelectListItem item = new SelectListItem();
                item.Value=course.Id.ToString();
                item.Text=course.Name;
                item.Selected=course.Id == id;
                list2.Add(item);
            }
            if (list2.Count == 0)
            {
                SelectListItem item3 = new SelectListItem();
                item3.Text="--暂无课程--";
                list2.Add(item3);
            }
            ViewBag.slCourse = list2;
            bool flag = base.CurrentUser.Modules.Contains("test");
            if (flag)
            {
                List<Test> list3 = TestProvider.Instance.GetList(0);
                List<SelectListItem> list4 = new List<SelectListItem>();
                foreach (Test test in list3)
                {
                    SelectListItem item2 = new SelectListItem();
                    item2.Value=test.Id.ToString();
                    item2.Text=test.Name;
                    list4.Add(item2);
                }
                ViewBag.ddlTest = list4;
            }
            ViewBag.HasTest = flag;
            LanCourse entityAndTest = new LanCourse();
            if (list.Count > 0)
            {
                if (!id.HasValue)
                {
                    entityAndTest = LanCourseProvider.Instance.GetEntityAndTest(list[0].Id);
                }
                else
                {
                    entityAndTest = LanCourseProvider.Instance.GetEntityAndTest(id.Value);
                }
            }
            return base.PartialView(entityAndTest);
        }

        [WebFilter(Order = 0), TeacherFilter(Order = 1)]
        public JsonResult DelCourse(Guid id)
        {
            int num = LanResourceProvider.Instance.GetCount(Guid.Empty, id, Guid.Empty, null);
            if (num > 0)
            {
                return base.Json(new
                {
                    State = 0,
                    Msg = "该课程下有" + num + "项资源，不能直接删除"
                });
            }
            LanCourseProvider.Instance.SoftDelete(id);
            LanCourseProvider.Instance.UpdateLanCourseCount(base.CurrentUser.SchoolId, -1);
            return base.Json(new
            {
                State = 1,
                Msg = "删除成功",
                Result = ""
            });
        }

        [TeacherFilter(Order = 1), WebFilter(Order = 0)]
        public JsonResult DelForTest(Guid id)
        {
            LanCourse entity = LanCourseProvider.Instance.GetEntity(id);
            entity.K4Id = null;
            LanCourseProvider.Instance.Update(entity);
            return base.Json(new
            {
                State = 1,
                Msg = "取消关联",
                Result = ""
            });
        }

        [TeacherFilter(Order = 1), WebFilter(Order = 0)]
        public JsonResult DelImg(Guid id, string url)
        {
            LanCourse entity = LanCourseProvider.Instance.GetEntity(id);
            List<string> values = new List<string>();
            if (!string.IsNullOrWhiteSpace(entity.ImageUrl))
            {
                values.AddRange(entity.ImageUrl.Split(new char[] { ',' }));
                if (values.Contains(url))
                {
                    values.Remove(url);
                    entity.ImageUrl = string.Join(",", values);
                    LanCourseProvider.Instance.Update(entity);
                    string str = Server.MapPath(url);
                    return base.Json(new
                    {
                        State = 1,
                        Msg = "删除成功",
                        Result = ""
                    });
                }
                return base.Json(new
                {
                    State = 0,
                    Msg = "删除错误",
                    Result = ""
                });
            }
            return base.Json(new
            {
                State = 0,
                Msg = "删除错误",
                Result = ""
            });
        }

        public JsonResult DelK4Collect(Guid id)
        {
            ExamCollectProvider.Instance.Delete(id);
            return base.Json(new
            {
                State = 1,
                Msg = "取消收藏",
                Result = ""
            });
        }

        public JsonResult DelK5Collect(Guid id)
        {
            LanCollectProvider.Instance.Delete(id);
            return base.Json(new
            {
                State = 1,
                Msg = "取消收藏",
                Result = ""
            });
        }

        [WebFilter(Order = 0), TeacherFilter(Order = 1)]
        public JsonResult DelResource(Guid courseId, Guid resourceId)
        {
            LanCourseProvider.Instance.SubtractResourceCount(courseId);
            LanCollectProvider.Instance.Delete(base.CurrentUser.Id, resourceId);
            LanResourceProvider.Instance.SoftDelete(resourceId);
            LanCourseProvider.Instance.UpdateLanResourceCount(base.CurrentUser.SchoolId, -1);
            return base.Json(new
            {
                State = 1,
                Msg = "删除成功",
                Result = ""
            });
        }

        [WebFilter(Order = 0), TeacherFilter(Order = 1)]
        public JsonResult EditCourse(Guid id, string txt)
        {
            LanCourse entity = LanCourseProvider.Instance.GetEntity(id);
            entity.Name = txt;
            LanCourseProvider.Instance.Update(entity);
            return base.Json(new
            {
                State = 1,
                Msg = "修改成功",
                Result = entity.Name
            });
        }

        [TeacherFilter(Order = 1), WebFilter(Order = 0)]
        public JsonResult EditForTest(Guid courseId, Guid testId)
        {
            LanCourse entity = LanCourseProvider.Instance.GetEntity(courseId);
            entity.K4Id = new Guid?(testId);
            LanCourseProvider.Instance.Update(entity);
            Test test = TestProvider.Instance.GetEntity(testId);
            return base.Json(new
            {
                State = 1,
                Msg = "修改成功",
                Result = test.Name
            });
        }

        [UserFilter(Order = 1), WebFilter(Order = 0)]
        public PartialViewResult EditPwd() =>
            base.PartialView();

        [WebFilter(Order = 0), TeacherFilter(Order = 1)]
        public JsonResult EditRemark(Guid id, string txt)
        {
            LanCourse entity = LanCourseProvider.Instance.GetEntity(id);
            entity.Remark = txt;
            LanCourseProvider.Instance.Update(entity);
            return base.Json(new
            {
                State = 1,
                Msg = "修改成功",
                Result = ""
            });
        }

        [WebFilter(Order = 0), TeacherFilter(Order = 1)]
        public JsonResult EditResource(Guid hdId, string name, Guid typeId, HttpPostedFileBase mini, HttpPostedFileBase view, HttpPostedFileBase file)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return base.Json(new
                {
                    State = 0,
                    Msg = "请输入资源名称",
                    Result = ""
                }, "text/html");
            }
            LanResource entity = LanResourceProvider.Instance.GetEntity(hdId);
            entity.TypeId = new Guid?(typeId);
            entity.Name = name;
            if (entity.IsShare != true)
            {
                string extension;
                string path = "/Images/Lan/LanResource/" + entity.Id;
                if ((mini != null) && (mini.ContentLength > 0))
                {
                    string[] source = new string[] { ".jpg", ".png", ".jpeg", "gif" };
                    extension = Path.GetExtension(mini.FileName).ToLower();
                    if (!source.Contains<string>(extension))
                    {
                        return base.Json(new
                        {
                            State = 0,
                            Msg = "缩略图片格式不正确！",
                            Result = ""
                        }, "text/html");
                    }
                    string str3 = path + "/mini" + extension;
                    entity.ImageUrl = str3;
                    FileHelper.DeleteFile(Server.MapPath(str3));
                    FileHelper.ResizeImage(mini, 130, 100, Server.MapPath(path), "mini" + extension);
                }
                if ((view != null) && (view.ContentLength > 0))
                {
                    extension = Path.GetExtension(view.FileName);
                    string str4 = path + "/view" + extension;
                    entity.ViewUrl = str4;
                    FileHelper.DeleteFile(Server.MapPath(str4));
                    FileHelper.UploadFile(view, Server.MapPath(path), "view" + extension);
                }
                if ((file != null) && (file.ContentLength > 0))
                {
                    extension = Path.GetExtension(file.FileName);
                    string str5 = path + "/file" + extension;
                    entity.DownloadUrl = str5;
                    entity.NameEx = extension;
                    FileHelper.DeleteFile(Server.MapPath(str5));
                    FileHelper.UploadFile(file, Server.MapPath(path), "file" + extension);
                }
            }
            LanResourceProvider.Instance.Update(entity);
            return base.Json(new
            {
                State = 1,
                Msg = "修改成功",
                Result = ""
            }, "text/html");
        }

        public JsonResult GetIdentity()
        {
            if (base.IsLoginWeb())
            {
                return base.Json(new
                {
                    State = 1,
                    Msg = "登录信息",
                    Result = base.CurrentUser
                });
            }
            return base.Json(new
            {
                State = 0,
                Msg = "登录信息",
                Result = ""
            });
        }

        [WebFilter(Order = 0), TeacherFilter(Order = 1)]
        public JsonResult GetTest(Guid parentId)
        {
            List<Test> listByParentId = TestProvider.Instance.GetListByParentId(parentId);
            int num = 0;
            if (listByParentId.Count > 0)
            {
                int? tier = listByParentId[0].Tier;
                num = tier.HasValue ? tier.GetValueOrDefault() : 0;
            }
            return base.Json(new
            {
                State = 1,
                Msg = "",
                Result = new
                {
                    Tier = num,
                    List = from p in listByParentId
                           select new
                           {
                               Id = p.Id,
                               Name = p.Name
                           }
                }
            });
        }

        public ViewResult Index()
        {
            base.CurrentUser = null;
            string userHostAddress = HttpContext.Request.UserHostAddress;
            Ip entity = IpProvider.Instance.GetEntity(IpHelper.ParseByte(userHostAddress));
            if (entity != null)
            {
                School school = SchoolProvider.Instance.GetEntity(entity.SchoolId);
                if (school == null)
                {
                    return base.View();
                }
                DateTime time = DateTime.Parse(school.EndTime.ToString());
                DateTime time2 = DateTime.Parse(DateTime.Now.ToString());
                if (time.CompareTo(time2) < 0)
                {
                    ViewBag.SchoolEnd = true;
                    ViewBag.Name = school.Name;
                }
                else
                {
                    List<Module> list = ModuleProvider.Instance.GetList(entity.SchoolId);
                    if (list.Count > 0)
                    {
                        ViewBag.Name = school.Name;
                        ViewBag.Url = "/" + list[0].Code;
                    }
                }
            }
            return base.View();
        }

        [WebFilter(Order = 0), UserFilter(Order = 1)]
        public JsonResult K4Collect(int pageIndex, int pageCount)
        {
            List<ExamCollectInfo> list = ExamCollectProvider.Instance.GetPageInfo(base.CurrentUser.Id, pageIndex, pageCount);
            return base.Json(new
            {
                State = 1,
                Msg = "k5收藏分页",
                Result = new { List = list }
            });
        }

        [WebFilter(Order = 0), UserFilter(Order = 1)]
        public JsonResult K5Collect(int pageIndex, int pageCount)
        {
            List<LanCollectInfo> list = LanCollectProvider.Instance.GetPageInfo(base.CurrentUser.Id, pageIndex, pageCount);
            string str = WebConfigurationManager.AppSettings["Resource_Down_Url"];
            string str2 = WebConfigurationManager.AppSettings["Resource_Ico_Url"];
            foreach (LanCollectInfo info in list)
            {
                if (info.IsShare == true)
                {
                    info.ImageUrl = str2 + "/k1" + info.ImageUrl;
                    info.ViewUrl = str + "/k1" + info.ViewUrl;
                }
            }
            return base.Json(new
            {
                State = 1,
                Msg = "k5收藏分页",
                Result = new
                {
                    SchoolId = base.CurrentUser.SchoolId,
                    List = list
                }
            });
        }

        public JsonResult Login(string name, string pwd, string path)
        {
            int? type = null;
            UserInfo entity = UserInfoProvider.Instance.GetEntity(name, Encrypt.GetMD5(pwd));
            if (entity != null)
            {
                type = entity.Type;
            }
            
            if (((type != -1 || !type.HasValue) && !entity.IsDeleted) 
                    && (type != 6 || !type.HasValue))
            {
                School school = SchoolProvider.Instance.GetEntity(entity.SchoolId);
                if (school == null)
                {
                    return base.Json(new
                    {
                        State = 0,
                        Msg = "登录失败",
                        Result = ""
                    });
                }
                DateTime time = DateTime.Parse(school.EndTime.ToString());
                DateTime time2 = DateTime.Parse(DateTime.Now.ToString());
                if (time.CompareTo(time2) < 0)
                {
                    ViewBag.SchoolEnd = true;
                    ViewBag.Name = school.Name;
                    return base.Json(new
                    {
                        State = 2,
                        Msg = "登陆成功，但该学校到期",
                        Result = "/Account"
                    });
                }
                UserIdentity identity2 = new UserIdentity
                {
                    Id = entity.Id,
                    Name = string.IsNullOrWhiteSpace(entity.Name) ? entity.LoginName : entity.Name
                };
                type = entity.Type;
                identity2.Type = new int?(type.HasValue ? type.GetValueOrDefault() : 1);
                identity2.SchoolId = entity.SchoolId;
                UserIdentity identity = identity2;
                List<Module> list = ModuleProvider.Instance.GetList(identity.SchoolId);
                List<string> modules = new List<string>();
                list.ForEach(delegate (Module p) {
                    modules.Add(p.Code.ToLower());
                });
                identity.Modules = modules;
                base.CurrentUser = identity;
                path = path.TrimStart(new char[] { '/' });
                if (string.IsNullOrEmpty(path))
                {
                    path = "Resource";
                }
                if (identity.Modules.Count <= 0)
                {
                    return base.Json(new
                    {
                        State = 2,
                        Msg = "登陆成功，但没有权限",
                        Result = "/Account"
                    });
                }
                if (identity.Modules.Contains(path.ToLower()))
                {
                    return base.Json(new
                    {
                        State = 1,
                        Msg = "登录成功",
                        Result = identity
                    });
                }
                return base.Json(new
                {
                    State = 2,
                    Msg = "登陆成功，跳转",
                    Result = "/" + identity.Modules[0]
                });
            }
            return base.Json(new
            {
                State = 0,
                Msg = "登录失败",
                Result = ""
            });
        }

        public JsonResult LogOut()
        {
            base.CurrentUser = null;
            return base.Json(new
            {
                State = 1,
                Msg = "退出登录",
                Result = ""
            });
        }

        [WebFilter(Order = 0), UserFilter(Order = 1)]
        public ViewResult Manage()
        {
            ViewBag.LayModule = ModuleProvider.Instance.GetList(base.CurrentUser.SchoolId);
            return base.View(base.CurrentUser);
        }

        public JsonResult RegInit()
        {
            bool? nullable;
            List<SystemInfo> list = SystemInfoProvider.Instance.GetList();
            if (!((list.Count <= 0) ? false : ((nullable = list[0].IsReg).HasValue ? nullable.GetValueOrDefault() : false)))
            {
                return base.Json(new
                {
                    State = 0,
                    Msg = "系统未开启注册，请联系管理员",
                    Result = ""
                });
            }
            List<Job> list2 = JobProvider.Instance.GetList();
            List<Post> list3 = PostProvider.Instance.GetList();
            List<Profession> list4 = ProfessionProvider.Instance.GetList();
            List<School> list5 = SchoolProvider.Instance.GetList();
            IEnumerable<School> enumerable = from p in list5
                                             where p.Tier == 0
                                             select p;
            IEnumerable<School> list1 = from p in list5
                                        where p.Tier == 1
                                        select p;
            IEnumerable<School> list52 = from p in list5
                                        where p.Tier == 2
                                        select p;
            var enumerable2 = from p0 in enumerable
                              select new
                              {
                                  Id = p0.Id,
                                  Name = p0.Name,
                                  List = from p1 in list1
                                         where p1.ParentId == p0.Id
                                         select new
                                         {
                                             Id = p1.Id,
                                             Name = p1.Name,
                                             List = from p2 in list52
                                                    where p2.ParentId == p1.Id
                                                    select new
                                                    {
                                                        Id = p2.Id,
                                                        Name = p2.Name
                                                    }
                                         }
                              };
            return base.Json(new
            {
                State = 1,
                Msg = "注册初始化数据",
                Result = new
                {
                    ZC = list2,
                    ZW = list3,
                    ZY = list4,
                    School = enumerable2
                }
            });
        }

        public JsonResult RegPost(UserInfo user)
        {
            bool? nullable;
            List<SystemInfo> list = SystemInfoProvider.Instance.GetList();
            if (!((list.Count <= 0) ? false : ((nullable = list[0].IsReg).HasValue ? nullable.GetValueOrDefault() : false)))
            {
                return base.Json(new
                {
                    State = 0,
                    Msg = "系统未开启注册，请联系管理员",
                    Result = ""
                });
            }
            if (((string.IsNullOrWhiteSpace(user.LoginName) || string.IsNullOrWhiteSpace(user.LoginPwd)) || string.IsNullOrWhiteSpace(user.Name)) || (user.SchoolId == Guid.Empty))
            {
                return base.Json(new
                {
                    State = 0,
                    Msg = "注册信息错误",
                    Result = ""
                });
            }
            if (UserInfoProvider.Instance.GetEntity(user.LoginName) != null)
            {
                return base.Json(new
                {
                    State = 0,
                    Msg = "该账号已经被注册",
                    Result = ""
                });
            }
            user.LoginPwd = Encrypt.GetMD5(user.LoginPwd);
            user.CreateTime = DateTime.Now;
            if (!((user.Type == 0) ? false : !((nullable = list[0].IsValid).HasValue ? nullable.GetValueOrDefault() : false)))
            {
                user.IsDeleted = true;
                UserInfoProvider.Instance.Create(user);
                return base.Json(new
                {
                    State = 1,
                    Msg = "注册成功，请等待审核",
                    Result = ""
                });
            }
            user.IsDeleted = false;
            UserInfoProvider.Instance.Create(user);
            return base.Json(new
            {
                State = 1,
                Msg = "注册成功",
                Result = ""
            });
        }

        [WebFilter(Order = 0), UserFilter(Order = 1)]
        public JsonResult RePwd(string opwd, string npwd)
        {
            UserInfo entity = UserInfoProvider.Instance.GetEntity(base.CurrentUser.Id);
            if (entity.LoginPwd == Encrypt.GetMD5(opwd))
            {
                entity.LoginPwd = Encrypt.GetMD5(npwd);
                UserInfoProvider.Instance.Update(entity);
                return base.Json(new
                {
                    State = 1,
                    Msg = "修改密码成功",
                    Result = ""
                });
            }
            return base.Json(new
            {
                State = 0,
                Msg = "对不起，原密码错误。",
                Result = ""
            });
        }

        [TeacherFilter(Order = 1), WebFilter(Order = 0)]
        public ActionResult Resource(Guid? id)
        {
            List<LanCourse> list = LanCourseProvider.Instance.GetList(base.CurrentUser.Id);
            if (list.Count > 0)
            {
                Guid? nullable = id;
                id = new Guid?(nullable.HasValue ? nullable.GetValueOrDefault() : list[0].Id);
            }
            else
            {
                return base.RedirectToAction("Course", new { id = id });
            }
            List<SelectListItem> list2 = new List<SelectListItem>();
            foreach (LanCourse course in list)
            {
                SelectListItem item = new SelectListItem();
                item.Value=course.Id.ToString();
                item.Text=course.Name;
                item.Selected=course.Id == id;
                list2.Add(item);
            }
            ViewBag.slCourse = list2;
            List<Models.LanResourceType> list3 = LanResourceTypeProvider.Instance.GetList(base.CurrentUser.SchoolId);
            StringBuilder sb = new StringBuilder();
            sb.Append("<select id='ddlResourceType' name='typeId'>");
            sb.Append("<optgroup label='" + Enums.LanResourceType.Base.GetName() + "'>");
            (from p in list3
             where p.TypeEnum == 0
             select p).ToList<Models.LanResourceType>().ForEach(delegate (Models.LanResourceType p) {
                 sb.Append(string.Concat(new object[] { "<option value='", p.Id, "'>", p.Name, "</option>" }));
             });
            sb.Append("</optgroup>");
            sb.Append("<optgroup label='" + Enums.LanResourceType.Ext.GetName() + "'>");
            (from p in list3
             where p.TypeEnum == 1
             select p).ToList<Models.LanResourceType>().ForEach(delegate (Models.LanResourceType p) {
                 sb.Append(string.Concat(new object[] { "<option value='", p.Id, "'>", p.Name, "</option>" }));
             });
            sb.Append("</optgroup>");
            sb.Append("<optgroup label='" + Enums.LanResourceType.Custom.GetName() + "'>");
            (from p in list3
             where p.TypeEnum == 2
             select p).ToList<Models.LanResourceType>().ForEach(delegate (Models.LanResourceType p) {
                 sb.Append(string.Concat(new object[] { "<option value='", p.Id, "'>", p.Name, "</option>" }));
             });
            sb.Append("</optgroup>");
            sb.Append("</select>");
            ViewBag.ResourceType = new MvcHtmlString(sb.ToString());
            ViewBag.UserInfo = base.CurrentUser;
            List<LanResource> listAndType = LanResourceProvider.Instance.GetListAndType(id.Value);
            string str = WebConfigurationManager.AppSettings["Resource_Down_Url"].TrimEnd(new char[] { '/' });
            string str2 = WebConfigurationManager.AppSettings["Resource_Ico_Url"].TrimEnd(new char[] { '/' }) + "/k1";
            IEnumerable<LanResource> enumerable = from p in listAndType
                                                  where p.IsShare == true
                                                  select p;
            foreach (LanResource resource in enumerable)
            {
                resource.ViewUrl = str + "/k1" + resource.ViewUrl;
                resource.DownloadUrl = str + "?path=/k1" + resource.DownloadUrl;
                resource.ImageUrl = str2 + resource.ImageUrl;
            }
            return base.PartialView(listAndType);
        }

        [TeacherFilter(Order = 1), WebFilter(Order = 0)]
        public JsonResult Upload(HttpPostedFileBase upImg, Guid hdId)
        {
            LanCourse entity = LanCourseProvider.Instance.GetEntity(hdId);
            string str = "/Images/Lan/LanCourse/" + entity.Id;
            if ((upImg == null) || (upImg.ContentLength == 0))
            {
                return base.Json(new
                {
                    State = 0,
                    Msg = "请正确选择图片！",
                    Result = ""
                }, "text/html");
            }
            string[] source = new string[] { ".jpg", ".png", ".jpeg", "gif" };
            string str2 = Path.GetExtension(upImg.FileName).ToLower();
            if (!source.Contains<string>(str2))
            {
                return base.Json(new
                {
                    State = 0,
                    Msg = "图片格式不正确！",
                    Result = ""
                }, "text/html");
            }
            try
            {
                string fileName = Guid.NewGuid() + str2;
                FileHelper.ResizeImage(upImg, 540, 180, Server.MapPath("~" + str), fileName);
                str = str + "/" + fileName;
                if (string.IsNullOrWhiteSpace(entity.ImageUrl))
                {
                    entity.ImageUrl = str;
                }
                else
                {
                    entity.ImageUrl = entity.ImageUrl + "," + str;
                }
                LanCourseProvider.Instance.Update(entity);
                return base.Json(new
                {
                    State = 1,
                    Msg = "上传成功",
                    Result = str
                }, "text/html");
            }
            catch (Exception exception)
            {
                return base.Json(new
                {
                    State = 0,
                    Msg = exception.Message,
                    Result = ""
                }, "text/html");
            }
        }
    }
}