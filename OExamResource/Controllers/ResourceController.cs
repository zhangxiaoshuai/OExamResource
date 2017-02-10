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
            buffer = (new ZipFileClass()).UnZipFile(path1, "syncpack");
            return this.File(buffer, "application/x-zip-compressed", name);
        }

        public PartialViewResult ListSub(bool isJX, Guid? id, string key = "")
        {
            DB.GetInstance().OpenSharedConnection();
            List<ResourceCategory> listSub = ResourceCategoryProvider.Instance.GetSubList(id, key, isJX, true);
            DB.GetInstance().CloseSharedConnection();
            return base.PartialView(listSub);
        }
    }
}