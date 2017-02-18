using Microsoft.CSharp.RuntimeBinder;
//using PetaPoco;
using Common;
using Models;
using Providers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ZipClass.Help;

namespace Controllers
{
	[ModuleFilter(Order=1)]
	[WebFilter(Order=0)]
	public class ResourceController : BaseController
	{
		private Common.Secret sc = new Common.Secret();

		public ResourceController()
		{
		}

		

		public ViewResult Index()
		{
			return base.View();
		}

        public JsonResult GetList()
        {
            bool flag = this.IsJX();
            List<ResourceCategory> list1 = ResourceCategoryProvider.Instance.GetList(Enums.ResourceType.Category);
            List<ResourceCategory> resourceCategories = ResourceCategoryProvider.Instance.GetList(Enums.ResourceType.Subject);
            var list =
                from p in list1
                select new
                {
                    Id = p.Id,
                    Name = p.Name,
                    Count = (flag ? p.Count_JX : p.Count),
                    Ico = p.Ico,
                    List = resourceCategories.Where<ResourceCategory>((ResourceCategory s) =>
                    {
                        Guid? parentId = s.ParentId;
                        Guid id = p.Id;
                        return (!parentId.HasValue ? false : parentId.GetValueOrDefault() == id);
                    }).Select((ResourceCategory s) => new { Id = s.Id, Name = s.Name, Count = (flag ? s.Count_JX : s.Count) }).ToList()
                };
            JsonResult jsonResult = base.Json(new { State = 1, Msg = "资源库大类列表", Result = list });
            return jsonResult;
        }
        private bool IsJX()
        {
            Guid sid = base.CurrentUser.SchoolId;
            Guid ModuleId = new Guid("F485F302-F6F5-4088-A366-18BB647EE05A");
            School_Module school = School_ModuleProvider.Instance.GetSchoolModuleType(sid, ModuleId);
            return school.TrialTag == 1;
        }

        [HttpGet]
        public ViewResult List(Guid? id, string key = "")
        {
            DB db = DB.GetInstance();
            db.OpenSharedConnection();
            bool isJX = this.IsJX();
            key = key.Trim();
            List<ResourceCategory> listSup = ResourceCategoryProvider.Instance.GetSuperList(id, 0);
            ViewBag.IsJX = isJX;
            ViewBag.Sup = listSup;
            List<ResourceType> listType = ResourceTypeProvider.Instance.GetList(id, key, isJX, true);
            //ViewBag.Url = this.sc.MD5Decrypt("TURNTECH", ConfigurationManager.AppSettings["Resource_Ico_Url"]);
            //ViewBag.Url = this.sc.MD5Decrypt("TURNTECH", "1EAC9FF13FD055F192796E085689882B6EFB80E078995A674BD4E9A226B9327A69C92C69188EDCF90FDFEE1C14BEC033");
            ViewBag.Url = ConfigurationManager.AppSettings["Resource_Ico_Url"];
            db.CloseSharedConnection();
            return base.View(listType);
        }

        [HttpPost]
        public JsonResult List(Guid? id, int baseType, Guid? typeId, int pageIndex, string key = "")
        {
            bool isJX = this.IsJX();
            key = key.Trim();
            IEnumerable<object> result = new List<object>();
            switch (baseType)
            {
                case 0:
                    {
                        List<ResourcePack> listPack = ResourcePackProvider.Instance.GetPackPage(isJX, key, id, typeId, true, pageIndex, 10);
                        string errorImg = this.GetErrorImg("pack");
                        result =
                            from p in listPack
                            select new { Id = p.Id, Title = p.Name ?? "", Ico = p.Ico, BaseType = 0, ErrorIco = errorImg, C = p.Count };
                        break;
                    }
                case 1:
                    {
                        List<ResourceMaterial> listMaterial = ResourcePackProvider.Instance.GetMaterialPage(isJX, key, id, typeId, true, pageIndex, 10);
                        result =
                            from p in listMaterial
                            select new { Id = p.Id, Title = p.Title ?? "", Ico = p.IcoFilepath, BaseType = 1, ErrorIco = this.GetErrorImg(p.FileExt), C = (p.Tag2 == "0" ? -1 : 1) };
                        break;
                    }
                default:
                    {
                        List<dynamic> listAll = ResourcePackProvider.Instance.GetAllPage(isJX, key, id, true, pageIndex, 10);
                        result = listAll.Select((dynamic p) => {
                            object obj = p.Id;
                            object obj1 = p.Name;
                            if (obj1 == null)
                            {
                                obj1 = "";
                            }
                            return new { Id = obj, Title = obj1, Ico = p.Ico, BaseType = p.BaseType, ErrorIco = this.GetErrorImg(p.FileExt), C = p.Count };
                        });
                        break;
                    }
            }
            JsonResult jsonResult = base.Json(new { State = 1, Result = result });
            return jsonResult;
        }

        private string GetErrorImg(string file_ext)
        {
            string str;
            file_ext = file_ext.ToLower().Trim();
            string fileExt = file_ext;
            if (fileExt != null)
            {
                switch (fileExt)
                {
                    case "bmp":
                    case "gif":
                    case "jpg":
                    case "wmf":
                    case "png":
                        {
                            str = "/Content/image/Default/TP03.jpg";
                            break;
                        }
                    case "flv":
                        {
                            str = "/Content/image/Default/SP01.jpg";
                            break;
                        }
                    case "mp3":
                        {
                            str = "/Content/image/Default/YP02.jpg";
                            break;
                        }
                    case "swf":
                        {
                            str = "/Content/image/Default/DH02.jpg";
                            break;
                        }
                    case "pack":
                        {
                            str = "/Content/image/Default/YS03.jpg";
                            break;
                        }
                    default:
                        {
                            str = "/Content/image/Default/NONE03.jpg";
                            return str;
                        }
                }
            }
            else
            {
                str = "/Content/image/Default/NONE03.jpg";
                return str;
            }
            return str;
        }

        private void Prelog(Guid hddate)
        {
            string ip = HttpContext.Request.UserHostAddress;
            object[] objArray = new object[] { "insert into  LogDetail values(newid(),'", hddate, "','", base.CurrentUser.Id, "','", base.CurrentUser.SchoolId, "','", ip, "',", 0, ",", 0, ",null,getdate(),0)" };
            string sql = string.Concat(objArray);
            SqlHelper.ExecuteNonQuery(sql, new SqlParameter[0]);
        }
        private List<SelectListItem> LanCourse()
        {
            List<LanCourse> listCourse = LanCourseProvider.Instance.GetList(base.CurrentUser.Id);
            List<SelectListItem> ddlCourse = new List<SelectListItem>();
            foreach (LanCourse item in listCourse)
            {
                SelectListItem selectListItem = new SelectListItem();
                selectListItem.Value= item.Id.ToString();
                selectListItem.Text=item.Name;
                ddlCourse.Add(selectListItem);
            }
            return ddlCourse;
        }
        private MvcHtmlString LanResourceType()
        {
            List<LanResourceType> types = LanResourceTypeProvider.Instance.GetList(base.CurrentUser.SchoolId);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<select id='ddlResourceType' name='typeId'>");
            stringBuilder.Append(string.Concat("<optgroup label='", Enums.LanResourceType.Base.GetName(), "'>"));
            types.Where<LanResourceType>((LanResourceType p) => {
                int? typeEnum = p.TypeEnum;
                return (typeEnum.GetValueOrDefault() != 0 ? false : typeEnum.HasValue);
            }).ToList<LanResourceType>().ForEach((LanResourceType p) => stringBuilder.Append(string.Concat(new object[] { "<option value='", p.Id, "'>", p.Name, "</option>" })));
            stringBuilder.Append("</optgroup>");
            stringBuilder.Append(string.Concat("<optgroup label='", Enums.LanResourceType.Ext.GetName(), "'>"));
            types.Where<LanResourceType>((LanResourceType p) => {
                int? typeEnum = p.TypeEnum;
                return (typeEnum.GetValueOrDefault() != 1 ? false : typeEnum.HasValue);
            }).ToList<LanResourceType>().ForEach((LanResourceType p) => stringBuilder.Append(string.Concat(new object[] { "<option value='", p.Id, "'>", p.Name, "</option>" })));
            stringBuilder.Append("</optgroup>");
            stringBuilder.Append(string.Concat("<optgroup label='", Enums.LanResourceType.Custom.GetName(), "'>"));
            types.Where<LanResourceType>((LanResourceType p) => {
                int? typeEnum = p.TypeEnum;
                return (typeEnum.GetValueOrDefault() != 2 ? false : typeEnum.HasValue);
            }).ToList<LanResourceType>().ForEach((LanResourceType p) => stringBuilder.Append(string.Concat(new object[] { "<option value='", p.Id, "'>", p.Name, "</option>" })));
            stringBuilder.Append("</optgroup>");
            stringBuilder.Append("</select>");
            return new MvcHtmlString(stringBuilder.ToString());
        }
        private string GetDownloadPath(string sourcepath, Guid? id = null)
        {
            string str;
            char[] chrArray;
            string str1;
            bool isJX = this.IsJX();
            if (string.IsNullOrWhiteSpace(sourcepath))
            {
                if (!id.HasValue)
                {
                    str = null;
                }
                else
                {
                    string sql = string.Concat("select FilePath from ResourcePack_Merge where PackId='", id, "'");
                    DataTable dt = SqlHelper.ExecuteDataTable(sql, new SqlParameter[0]);
                    string path = "";
                    foreach (DataRow dr in dt.Rows)
                    {
                        path = string.Concat(path, "|", dr[0]);
                    }
                    chrArray = new char[] { '|' };
                    str = path.Trim(chrArray);
                }
            }
            else if (!sourcepath.EndsWith("?"))
            {
                str = sourcepath;
            }
            else
            {
                if (isJX)
                {
                    chrArray = new char[] { '?' };
                    str1 = sourcepath.TrimEnd(chrArray);
                }
                else
                {
                    str1 = null;
                }
                str = str1;
            }
            return str;
        }
        public ViewResult Preview(Guid id, int t, int? index)
        {
            List<ResourceCategory> listSup;
            ViewResult viewResult;
            this.Prelog(id);
            bool isShare = (!base.IsTeacher() ? false : base.CurrentUser.Modules.Contains("lan"));
            ViewBag.Share = isShare;
            if (isShare)
            {
                ViewBag.slCourse = this.LanCourse();
                ViewBag.ResourceType = this.LanResourceType();
            }
            string url = ConfigurationManager.AppSettings["Resource_View_Url"];
            if (t != 0)
            {
                //ViewBag.DownloadUrl = this.sc.MD5Decrypt("TURNTECH", ConfigurationManager.AppSettings["Material_Down_Url"]);
                ViewBag.DownloadUrl = ConfigurationManager.AppSettings["Material_Down_Url"];
            }
            else
            {
                //ViewBag.DownloadUrl = this.sc.MD5Decrypt("TURNTECH", ConfigurationManager.AppSettings["Resource_Down_Url"]);
                ViewBag.DownloadUrl = ConfigurationManager.AppSettings["Resource_Down_Url"];
            }
            ViewBag.Url = ConfigurationManager.AppSettings["Resource_View_Url"];
            //ViewBag.Url = this.sc.MD5Decrypt("TURNTECH", ConfigurationManager.AppSettings["Resource_View_Url"]);
            ViewBag.Enable = true;
            ViewBag.baseType = t;
            if (t == 0)
            {
                ResourcePack pack = ResourcePackProvider.Instance.GetEntity(id);
                ResourcePack clickCount = pack;
                clickCount.ClickCount = clickCount.ClickCount + 1;
                ResourcePackProvider.Instance.Update(pack);
                pack.FilePath = this.GetDownloadPath(pack.FilePath, new Guid?(pack.Id));
                List<ResourceCourse> listCourse = ResourcePackProvider.Instance.GetCourse(pack.Id);
                listSup = ResourceCategoryProvider.Instance.GetSuperList(pack.CategoryId, 0);
                ViewBag.Sup = listSup;
                ViewBag.Pack = pack;
                ViewBag.List = listCourse;
                if (!index.HasValue)
                {
                    index = new int?(0);
                    int i = 0;
                    while (i < listCourse.Count)
                    {
                        if (listCourse[i].Tag1.Contains("无预览"))
                        {
                            i++;
                        }
                        else
                        {
                            index = new int?(i);
                            break;
                        }
                    }
                }
                index = new int?(Math.Min(Math.Max(0, index.Value), listCourse.Count - 1));
                ResourceCourse course = listCourse[index.Value];
                ViewBag.Enable = !course.Tag1.Contains("无预览");
                viewResult = base.View(course);
            }
            else if (t != 1)
            {
                viewResult = base.View();
            }
            else
            {
                ResourceMaterial material = ResourceMaterialProvider.Instance.GetEntity(id);
                ResourceMaterial resourceMaterial = material;
                resourceMaterial.ClickCount = resourceMaterial.ClickCount + 1;
                ResourceMaterialProvider.Instance.Update(material);
                material.DownloadFilepath = this.GetDownloadPath(material.DownloadFilepath, null);
                listSup = ResourceCategoryProvider.Instance.GetSuperList(material.CategoryId, 0);
                ViewBag.Sup = listSup;
                ViewBag.Enable = !material.Tag1.Contains("无预览");
                viewResult = base.View(material);
            }
            return viewResult;
        }
        public FileStreamResult StreamFile(string path, Guid Id)
        {
            string ip = HttpContext.Request.UserHostAddress;
            object[] id = new object[] { "insert into  LogDetail values(newid(),'", Id, "','", base.CurrentUser.Id, "','", base.CurrentUser.SchoolId, "','", ip, "',", 2, ",", 0, ",null,getdate(),0)" };
            string sql = string.Concat(id);
            SqlHelper.ExecuteNonQuery(sql, new SqlParameter[0]);
            Stream stream = (new WebClient()).OpenRead(path);
            string fileName = path.Substring(path.LastIndexOf("/") + 1);
            string ext = fileName.Substring(fileName.LastIndexOf(".") + 1);
            FileStreamResult fileStreamResult = this.File(stream, ext, Url.Encode(fileName));
            return fileStreamResult;
        }
        public FileStreamResult StreamFileDiskMaterial(string path, Guid Id)
        {
            FileStreamResult fileStreamResult;
            bool flag;
            if (base.CurrentUser.SchoolId == Guid.Empty)
            {
                flag = false;
            }
            else
            {
                Guid schoolId = base.CurrentUser.SchoolId;
                flag = 0 == 0;
            }
            if (flag)
            {
                string ip = HttpContext.Request.UserHostAddress;
                Guid id = base.CurrentUser.Id;
                if (base.CurrentUser.Id != Guid.Empty)
                {
                    if (UserInfoProvider.Instance.GetUserBySchool(base.CurrentUser.Id, base.CurrentUser.SchoolId) > 0)
                    {
                        fileStreamResult = this.StreamFile(path, Id);
                    }
                    else
                    {
                        fileStreamResult = null;
                    }
                }
                else if (!(base.CurrentUser.Id == Guid.Empty))
                {
                    fileStreamResult = null;
                }
                else if (IpProvider.Instance.GetEntity(IpHelper.ParseByte(ip), base.CurrentUser.SchoolId) > 0)
                {
                    fileStreamResult = this.StreamFile(path, Id);
                }
                else
                {
                    fileStreamResult = null;
                }
            }
            else
            {
                fileStreamResult = null;
            }
            return fileStreamResult;
        }

        public FileContentResult StreamFileDisk(string path, Guid Id)
        {
            FileContentResult fileContentResult;
            bool flag;
            if (base.CurrentUser.SchoolId == Guid.Empty)
            {
                flag = false;
            }
            else
            {
                Guid schoolId = base.CurrentUser.SchoolId;
                flag = 0 == 0;
            }
            if (flag)
            {
                string ip = HttpContext.Request.UserHostAddress;
                Guid id = base.CurrentUser.Id;
                if (base.CurrentUser.Id != Guid.Empty)
                {
                    if (UserInfoProvider.Instance.GetUserBySchool(base.CurrentUser.Id, base.CurrentUser.SchoolId) > 0)
                    {
                        fileContentResult = this.StreamTrue(path, Id);
                    }
                    else
                    {
                        fileContentResult = null;
                    }
                }
                else if (!(base.CurrentUser.Id == Guid.Empty))
                {
                    fileContentResult = null;
                }
                else if (IpProvider.Instance.GetEntity(IpHelper.ParseByte(ip), base.CurrentUser.SchoolId) > 0)
                {
                    fileContentResult = this.StreamTrue(path, Id);
                }
                else
                {
                    fileContentResult = null;
                }
            }
            else
            {
                fileContentResult = null;
            }
            return fileContentResult;
        }

        public FileContentResult StreamTrue(string path1, Guid Id)
        {
            string ip = HttpContext.Request.UserHostAddress;
            object[] id = new object[] { "insert into  LogDetail values(newid(),'", Id, "','", base.CurrentUser.Id, "','", base.CurrentUser.SchoolId, "','", ip, "',", 2, ",", 0, ",null,getdate(),0)" };
            string sql = string.Concat(id);
            SqlHelper.ExecuteNonQuery(sql, new SqlParameter[0]);
            string name = Path.GetFileName(path1);
            byte[] buffer = null;
            buffer = (new ZipFileClass()).UnZipFile(path1, "filepack");
            return File(buffer, "application/x-zip-compressed", name);
        }

        public PartialViewResult ListSub(bool isJX, Guid? id, string key = "")
        {
            DB.GetInstance().OpenSharedConnection();
            List<ResourceCategory> listSub = ResourceCategoryProvider.Instance.GetSubList(id, key, isJX, true);
            DB.GetInstance().CloseSharedConnection();
            return base.PartialView(listSub);
        }

        [HttpPost]
        public JsonResult shoucang(Guid id)
        {
            string message = "";
            string ip = HttpContext.Request.UserHostAddress;
            Guid teachid = new Guid();
            if (!base.IsLoginWeb())
            {
                message = "请登录！";
            }
            else
            {
                teachid = base.CurrentUser.Id;
                message = Resource_SubjectProvider.Instance.Getshoucang(id, teachid, base.CurrentUser.SchoolId, ip, 0);
            }
            JsonResult jsonResult = base.Json(new { State = 1, Msg = "收藏成功", Result = message });
            return jsonResult;
        }

        public JsonResult SetError(Guid id, int code, string remark)
        {
            ResourceError model = new ResourceError()
            {
                Id = Guid.NewGuid(),
                CreatedTime = DateTime.Now,
                IsDeleted = false,
                Code = code,
                ResourceId = id,
                Remark = remark
            };
            ResourceErrorProvider.Instance.Create(model);
            JsonResult jsonResult = base.Json(new { State = 1, Msg = "已提交" });
            return jsonResult;
        }


        public ViewResult Upload(string name, Guid? cateId, Guid? typeId, string course, string keyWord, string remark)
        {
            Guid? nullable;
            Guid? nullable1;
            if (Request.Files == null ? false : Request.Files.Count > 0)
            {
                int size = Request.Files[0].ContentLength;
                if (size > 0)
                {
                    ResourceUpload resourceUpload = new ResourceUpload()
                    {
                        Id = Guid.NewGuid(),
                        Name = name
                    };
                    ResourceUpload resourceUpload1 = resourceUpload;
                    if (base.IsLoginWeb())
                    {
                        nullable1 = new Guid?(base.CurrentUser.Id);
                    }
                    else
                    {
                        nullable = null;
                        nullable1 = nullable;
                    }
                    resourceUpload1.UserId = nullable1;
                    resourceUpload.CategoryId = cateId;
                    resourceUpload.TypeId = typeId;
                    resourceUpload.Size = new int?(size);
                    resourceUpload.Course = course;
                    resourceUpload.KeyWord = keyWord;
                    resourceUpload.Remark = remark;
                    resourceUpload.CreatedTime = DateTime.Now;
                    resourceUpload.IsDeleted = false;
                    ResourceUploadProvider.Instance.Create(resourceUpload);
                    ViewBag.Msg = "上传成功";
                }
            }
            nullable = null;
            List<ResourceCategory> cates = ResourceCategoryProvider.Instance.GetSubList(nullable, null, false, true);
            IOrderedEnumerable<ResourceType> types =
                from p in ResourceTypeProvider.Instance.GetList()
                orderby p.SortId
                select p;
            ViewBag.Cate = cates;
            ViewBag.Type = types;
            return base.View();
        }

        public JsonResult GetSubCate(Guid? id)
        {
            JsonResult jsonResult;
            if (id.HasValue)
            {
                List<ResourceCategory> list = ResourceCategoryProvider.Instance.GetSubList(id, null, false, true);
                jsonResult = base.Json(new { State = 1, Result = list });
            }
            else
            {
                jsonResult = base.Json(new { State = 0 });
            }
            return jsonResult;
        }

        ///////////////////////////////////////
        [HttpPost]
        public JsonResult GetShangchuan(string source_name, Guid slcategory, Guid slsubject, Guid slspeciality, string KeyWords, HttpPostedFileBase uploadfile, Guid sltype, string Coursename, string Fescribe)
        {
            JsonResult jsonResult;
            if (string.IsNullOrEmpty(source_name.Trim()))
            {
                jsonResult = base.Json(new { State = 0, Msg = "no", Result = "请填写资源名称!" });
            }
            else if (slcategory == Guid.Empty)
            {
                jsonResult = base.Json(new { State = 0, Msg = "no", Result = "请选择资源大类!" });
            }
            else if (slsubject == Guid.Empty)
            {
                jsonResult = base.Json(new { State = 0, Msg = "no", Result = "请选择资源学科!" });
            }
            else if (slspeciality == Guid.Empty)
            {
                jsonResult = base.Json(new { State = 0, Msg = "no", Result = "请选择资源专业!" });
            }
            else if (uploadfile == null)
            {
                jsonResult = base.Json(new { State = 0, Msg = "no", Result = "请选择资源文件!" });
            }
            else if (sltype == Guid.Empty)
            {
                jsonResult = base.Json(new { State = 0, Msg = "no", Result = "请选择资源类型!" });
            }
            else if (string.IsNullOrEmpty(Fescribe.Trim()))
            {
                jsonResult = base.Json(new { State = 0, Msg = "no", Result = "请填写资源描述信息!" });
            }
            else if (!base.IsTeacher())
            {
                jsonResult = base.Json(new { State = 0, Msg = "no", Result = "请先登录！" });
            }
            else
            {
                Resource res = new Resource()
                {
                    Id = Guid.NewGuid(),
                    Title = source_name,
                    Size = new int?(uploadfile.ContentLength),
                    Author = base.CurrentUser.Id.ToString(),
                    CategoryId = new Guid?(slcategory),
                    SpecialityId = new Guid?(slspeciality),
                    SubjectId = new Guid?(slsubject),
                    KeyWords = KeyWords,
                    TypeId = new Guid?(sltype),
                    CourseId = new Guid?(Resource_SubjectProvider.Instance.GetCourseId(Coursename, slspeciality)),
                    Fescribe = Fescribe,
                    SchoolId = new Guid?(base.CurrentUser.SchoolId),
                    FileExt = Path.GetExtension(uploadfile.FileName).Replace(".", "")
                };
                object[] id = new object[] { "/Images/source/file/", res.Id, ".", res.FileExt };
                res.Filepath = string.Concat(id);
                if (!Resource_SubjectProvider.Instance.GetShangchuan(res))
                {
                    jsonResult = base.Json(new { State = 0, Msg = "no", Result = "上传失败!" });
                }
                else
                {
                    FileHelper.UploadFile(uploadfile, Server.MapPath("/Images/source/file"), string.Concat(res.Id, ".", res.FileExt));
                    jsonResult = base.Json(new { State = 1, Msg = "ok", Result = "上传成功！" }, "text/html");
                }
            }
            return jsonResult;
        }

        [HttpPost]
        public JsonResult Getslsub(Guid cateid)
        {
            JsonResult jsonResult;
            if (!(cateid != Guid.Empty))
            {
                jsonResult = base.Json(new { State = 0, Msg = "ok", Result = "" });
            }
            else
            {
                List<Subject> subject = Resource_SubjectProvider.Instance.Getsub(cateid);
                jsonResult = base.Json(new { State = 1, Msg = "ok", Result = subject });
            }
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetSourceDetall(Guid resourceid, bool istop)
        {
            List<Resourcelist> list = PreviewProvider.Instance.GetResourceEntity(resourceid);
            WriteSize ws = new WriteSize();
            Resourcelist item = list[0];
            int size = list[0].Size;
            item.SizeStr = ws.WriteResourceSize(size.ToString());
            list[0].PreviewFilepath = this.Ex(list[0].PreviewFilepath);
            list[0].IsTop = istop;
            JsonResult jsonResult = base.Json(new { State = 1, Msg = "ok", Result = list });
            return jsonResult;
        }

        [HttpPost]
        public JsonResult Getspe(Guid subid)
        {
            JsonResult jsonResult;
            if (!(subid != Guid.Empty))
            {
                jsonResult = base.Json(new { State = 0, Msg = "ok", Result = "" });
            }
            else
            {
                List<Subject> spe = Resource_SubjectProvider.Instance.Getspe(subid);
                jsonResult = base.Json(new { State = 1, Msg = "ok", Result = spe });
            }
            return jsonResult;
        }



        [HttpPost]
        public JsonResult Gettypelist()
        {
            List<ResourceType> typelist = ResourceTypeProvider.Instance.GetList();
            JsonResult jsonResult = base.Json(new { State = 1, Msg = "ok", Result = typelist });
            return jsonResult;
        }



        [HttpPost]
        public JsonResult jiucuo(Guid id)
        {
            string message = "";
            Guid Gid = new Guid();
            Gid = (!base.IsLoginWeb() ? base.CurrentUser.SchoolId : base.CurrentUser.Id);
            message = (!Resource_SubjectProvider.Instance.Getjiucuo(id, Gid, 1) ? "系统错误请联系管理员！" : "感谢您提出宝贵的意见！");
            JsonResult jsonResult = base.Json(new { State = 1, Msg = "ok", Result = message });
            return jsonResult;
        }



        [HttpPost]
        public JsonResult Typepic(Guid typeid)
        {
            JsonResult jsonResult;
            if (!(typeid == Guid.Empty))
            {
                string typename = ResourceTypeProvider.Instance.GetEntity(typeid).Name;
                string resultstr = "";
                string str = typename;
                if (str != null)
                {
                    switch (str)
                    {
                        case "课件":
                            {
                                resultstr = "doc或ppt";
                                break;
                            }
                        case "图片":
                            {
                                resultstr = "Gif或jpg";
                                break;
                            }
                        case "动画":
                            {
                                resultstr = "swf";
                                break;
                            }
                        case "音频":
                            {
                                resultstr = "mp3";
                                break;
                            }
                        case "文档":
                            {
                                resultstr = "pdf";
                                break;
                            }
                        case "视频":
                            {
                                resultstr = "flv";
                                break;
                            }
                        default:
                            {
                                goto Label1;
                            }
                    }
                }
                else
                {
                }
                Label1:
                jsonResult = base.Json(new { State = 1, Msg = "ok", Result = string.Concat("(", resultstr, ")") });
            }
            else
            {
                jsonResult = base.Json(new { State = 1, Msg = "ok", Result = "*" });
            }
            return jsonResult;
        }



        [HttpGet]
        public ViewResult Uploading(Guid Id, int type, string guidlist, string searchname)
        {
            ViewBag.Id = Id;
            ViewBag.type = type;
            ViewBag.guidlist = guidlist;
            ViewBag.searchname = searchname;
            return base.View();
        }


        public string Ex(string filename)
        {
            if (!string.IsNullOrEmpty(filename))
            {
                string url = ConfigurationManager.AppSettings["K1kejian"];
                string filepath = string.Concat(url, filename);
                try
                {
                    HttpWebRequest req = (HttpWebRequest)WebRequest.CreateDefault(new Uri(filepath));
                    req.Method = "HEAD";
                    req.Timeout = 5000;
                    if (!(((HttpWebResponse)req.GetResponse()).StatusCode.ToString() == "OK"))
                    {
                        filename = "../../../images/source/file_zanwu_big.jpg";
                    }
                    else
                    {
                        filename = filepath;
                    }
                }
                catch (WebException webException)
                {
                    filename = "../../../images/source/file_zanwu_big.jpg";
                }
            }
            else
            {
                filename = "../../../images/source/file_zanwu_big.jpg";
            }
            return filename;
        }
    }
}