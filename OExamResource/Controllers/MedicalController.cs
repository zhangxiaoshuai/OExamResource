using Microsoft.CSharp.RuntimeBinder;
using PetaPoco;
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
using System.Web;
using System.Web.Mvc;
using ZipClass.Help;

namespace Controllers
{
	[ModuleFilter(Order=1)]
	[WebFilter(Order=0)]
	public class MedicalController : BaseController
	{
		private Common.Secret sc = new Common.Secret();

		public MedicalController()
		{
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

		public JsonResult GetList()
		{
			Guid id = new Guid("429EC3B1-72FE-40DB-9A65-E5FEFA7A1600");
			List<ResourceCategory> mc = ResourceCategoryProvider.Instance.GetSubList(new Guid?(id), "", true, false);
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "医学库大类列表", Result = mc });
			return jsonResult;
		}

		public static string GetMacAddress()
		{
			string empty;
			string mac = string.Empty;
			try
			{
				foreach (ManagementBaseObject mo in (new ManagementClass("Win32_NetworkAdapterConfiguration")).GetInstances())
				{
					if ((bool)mo["IPEnabled"])
					{
						mac = mo["MacAddress"].ToString();
						break;
					}
				}
				empty = mac;
			}
			catch
			{
				empty = string.Empty;
			}
			return empty;
		}

		private string GetMainID()
		{
			string macAddress;
			string MainID = null;
			try
			{
				foreach (ManagementObject mgt in (new ManagementObjectSearcher("select * from Win32_baseboard")).Get())
				{
					MainID = mgt["SerialNumber"].ToString();
				}
				macAddress = (!string.Equals(MainID, "none", StringComparison.CurrentCultureIgnoreCase) ? MainID : MedicalController.GetMacAddress());
			}
			catch
			{
				macAddress = MedicalController.GetMacAddress();
			}
			return macAddress;
		}

		public ViewResult Index()
		{
			return base.View();
		}

		private bool IsJX()
		{
			Guid sid = base.CurrentUser.SchoolId;
			Guid ModuleId = new Guid("50D53DA0-BD5D-4462-84AF-4BDC1F75F32E");
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
			List<ResourceCategory> listSup = ResourceCategoryProvider.Instance.GetSuperList(id, 1);
			ViewBag.IsJX = isJX;
			ViewBag.Sup = listSup;
			List<ResourceType> listType = ResourceTypeProvider.Instance.GetList(id, key, isJX, false);
            //ViewBag.Url = this.sc.MD5Decrypt("TURNTECH", ConfigurationManager.AppSettings["Resource_Ico_Url"]);
            //ViewBag.UrlMedical = this.sc.MD5Decrypt("TURNTECH", ConfigurationManager.AppSettings["Medical_Ico_Url"]);
            ViewBag.Url = ConfigurationManager.AppSettings["Resource_Ico_Url"];
            ViewBag.UrlMedical = ConfigurationManager.AppSettings["Medical_Ico_Url"];
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
					List<ResourcePack> listPack = ResourcePackProvider.Instance.GetPackPage(isJX, key, id, typeId, false, pageIndex, 10);
					string errorImg = this.GetErrorImg("pack");
					result = 
						from p in listPack
						select new { Id = p.Id, Title = p.Name ?? "", Ico = p.Ico, BaseType = 0, ErrorIco = errorImg, C = p.Count, Medical_Tag = p.Medical_Tag };
					break;
				}
				case 1:
				{
					List<ResourceMaterial> listMaterial = ResourcePackProvider.Instance.GetMaterialPage(isJX, key, id, typeId, false, pageIndex, 10);
					result = 
						from p in listMaterial
						select new { Id = p.Id, Title = p.Title ?? "", Ico = p.IcoFilepath, BaseType = 1, ErrorIco = this.GetErrorImg(p.FileExt), C = (p.Tag2 == "0" ? -1 : 1), Medical_Tag = p.Medical_Tag };
					break;
				}
				default:
				{
					List<dynamic> listAll = ResourcePackProvider.Instance.GetAllPage(isJX, key, id, false, pageIndex, 10);
					result = listAll.Select((dynamic p) => {
						object obj = p.Id;
						object obj1 = p.Name;
						if (obj1 == null)
						{
							obj1 = "";
						}
						return new { Id = obj, Title = obj1, Ico = p.Ico, BaseType = p.BaseType, ErrorIco = this.GetErrorImg(p.FileExt), C = p.Count, Medical_Tag = p.Medical_Tag };
					});
					break;
				}
			}
			JsonResult jsonResult = base.Json(new { State = 1, Result = result });
			return jsonResult;
		}

		public PartialViewResult ListSub(bool isJX, Guid? id, string key = "")
		{
			DB.GetInstance().OpenSharedConnection();
			List<ResourceCategory> listSub = ResourceCategoryProvider.Instance.GetSubList(id, key, isJX, false);
			DB.GetInstance().CloseSharedConnection();
			return base.PartialView(listSub);
		}

		private void Prelog(Guid hddate)
		{
			string ip = HttpContext.Request.UserHostAddress;
			object[] objArray = new object[] { "insert into  LogDetail values(newid(),'", hddate, "','", base.CurrentUser.Id, "','", base.CurrentUser.SchoolId, "','", ip, "',", 0, ",", 1, ",null,getdate(),0)" };
			string sql = string.Concat(objArray);
			SqlHelper.ExecuteNonQuery(sql, new SqlParameter[0]);
		}

		public ViewResult Preview(Guid id, int t, int? index)
		{
			List<ResourceCategory> listSup;
			ViewResult viewResult;
			this.Prelog(id);
            //ViewBag.Url = this.sc.MD5Decrypt("TURNTECH", ConfigurationManager.AppSettings["Resource_View_Url"]);
            //ViewBag.UrlM = this.sc.MD5Decrypt("TURNTECH", ConfigurationManager.AppSettings["Medical_View_Url"]);
            //if (t != 0)
            //{
            //	ViewBag.DownloadUrl = this.sc.MD5Decrypt("TURNTECH", ConfigurationManager.AppSettings["Material_Down_Url"]);
            //	ViewBag.DownloadUrlM = this.sc.MD5Decrypt("TURNTECH", ConfigurationManager.AppSettings["Material_Medical_Down_Url"]);
            //}
            //else
            //{
            //	ViewBag.DownloadUrl = this.sc.MD5Decrypt("TURNTECH", ConfigurationManager.AppSettings["Resource_Down_Url"]);
            //	ViewBag.DownloadUrlM = this.sc.MD5Decrypt("TURNTECH", ConfigurationManager.AppSettings["Medical_Down_Url"]);
            //}

            ViewBag.Url = ConfigurationManager.AppSettings["Resource_View_Url"];
            ViewBag.UrlM = ConfigurationManager.AppSettings["Medical_View_Url"];
            if (t != 0)
            {
                ViewBag.DownloadUrl = ConfigurationManager.AppSettings["Material_Down_Url"];
                ViewBag.DownloadUrlM = ConfigurationManager.AppSettings["Material_Medical_Down_Url"];
            }
            else
            {
                ViewBag.DownloadUrl = ConfigurationManager.AppSettings["Resource_Down_Url"];
                ViewBag.DownloadUrlM = ConfigurationManager.AppSettings["Medical_Down_Url"];
            }

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
				listSup = ResourceCategoryProvider.Instance.GetSuperList(pack.CategoryId, 1);
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
				listSup = ResourceCategoryProvider.Instance.GetSuperList(material.CategoryId, 1);
				ViewBag.Sup = listSup;
				ViewBag.Enable = !material.Tag1.Contains("无预览");
				viewResult = base.View(material);
			}
			return viewResult;
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
				message = Resource_SubjectProvider.Instance.Getshoucang(id, teachid, base.CurrentUser.SchoolId, ip, 1);
			}
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "收藏成功", Result = message });
			return jsonResult;
		}

		public FileStreamResult StreamFile(string path, Guid Id)
		{
			string ip = HttpContext.Request.UserHostAddress;
			object[] id = new object[] { "insert into  LogDetail values(newid(),'", Id, "','", base.CurrentUser.Id, "','", base.CurrentUser.SchoolId, "','", ip, "',", 2, ",", 1, ",null,getdate(),0)" };
			string sql = string.Concat(id);
			SqlHelper.ExecuteNonQuery(sql, new SqlParameter[0]);
			Stream stream = (new WebClient()).OpenRead(path);
			string fileName = path.Substring(path.LastIndexOf("/") + 1);
			string ext = fileName.Substring(fileName.LastIndexOf(".") + 1);
			FileStreamResult fileStreamResult = this.File(stream, ext, Url.Encode(fileName));
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

		public FileContentResult StreamTrue(string path1, Guid Id)
		{
			string ip = HttpContext.Request.UserHostAddress;
			object[] id = new object[] { "insert into  LogDetail values(newid(),'", Id, "','", base.CurrentUser.Id, "','", base.CurrentUser.SchoolId, "','", ip, "',", 2, ",", 1, ",null,getdate(),0)" };
			string sql = string.Concat(id);
			SqlHelper.ExecuteNonQuery(sql, new SqlParameter[0]);
			string name = Path.GetFileName(path1);
			byte[] buffer = null;
			buffer = (new ZipFileClass()).UnZipFile(path1, "downfile");
			return this.File(buffer, "application/x-zip-compressed", name);
		}
	}
}