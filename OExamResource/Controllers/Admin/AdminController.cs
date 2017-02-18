using Common;
using Controllers;
using Models;
using Providers;
using System;
using System.Web.Mvc;

namespace Controllers.Admin
{
	public class AdminController : BaseController
	{
		public AdminController()
		{
		}

		public ViewResult Login()
		{
			return base.View();
		}

		public JsonResult LoginOut()
		{
			base.CurrentAdmin = null;
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = "" });
			return jsonResult;
		}

		public JsonResult ManagerLogin(string name, string pwd)
		{
			JsonResult jsonResult;
			bool flag;
			bool flag1;
			UserInfo admin = UserInfoProvider.Instance.GetEntity(name, Encrypt.GetMD5(pwd));
			if (admin == null)
			{
				jsonResult = base.Json(new { State = 0, Msg = "账号或密码错误！", Result = "" });
			}
			else
			{
				int? type = admin.Type;
				//if ((type.GetValueOrDefault() != -1 ? true : !type.HasValue))
				//{
				//	type = admin.Type;
				//	if ((type.GetValueOrDefault() != 3 ? true : !type.HasValue))
				//	{
				//		type = admin.Type;
				//		if ((type.GetValueOrDefault() != 5 ? false : type.HasValue))
				//		{
				//			goto Label1;
				//		}
				//		type = admin.Type;
						
    //                    flag = type == 6;
				//		goto Label0;
				//	}
				//}
    //            Label1:
    //            flag = false;
    //            Label0:

                if (type == -1 || type == 3 || type == 5 || type == 6)
                    flag = false;
                else
                    flag = true;
				if (flag)
				{
					jsonResult = base.Json(new { State = 0, Msg = "您无权限登陆！", Result = "" });
				}
				else
				{
					UserIdentity user = base.CurrentAdmin ?? new UserIdentity();
					if (admin.IsDeleted)
					{
						jsonResult = base.Json(new { State = 0, Msg = "您的帐号被停用！", Result = "" });
					}
					else
					{
						type = admin.Type;
						if ((type.GetValueOrDefault() != 5 ? false : type.HasValue))
						{
							flag1 = true;
						}
						else
						{
							type = admin.Type;
							
                            flag1 = type == 6;

                        }
						if (flag1)
						{
							user.Role = admin.Type;
						}
						else
						{
							User_Role ur = User_RoleProvider.Instance.GetEntityByUserId(admin.Id);
							type = RoleProvider.Instance.GetEntity(ur.RoleId).Type;
							user.Role = new int?(type.Value);
						}
						user.Id = admin.Id;
						user.Name = admin.Name;
						type = admin.Type;
						user.Type = new int?(type.Value);
						Guid schoolId = admin.SchoolId;
						user.SchoolId = admin.SchoolId;
						base.CurrentAdmin = user;
						jsonResult = base.Json(new { State = 1, Msg = "", Result = "/SystemManage/System" });
					}
				}
			}
			return jsonResult;
		}
	}
}