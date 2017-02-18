using Microsoft.CSharp.RuntimeBinder;
using Novacode;
using Common;
using Models;
using Providers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Controllers
{
	[AdminFilter]
	public class SystemManageController : BaseController
	{
		public int num = 3;

		public int numExam = 6;

		public SystemManageController()
		{
		}

		public JsonResult AddChild(Guid parentid, string title, string url, int type)
		{
			JsonResult jsonResult;
			if (string.IsNullOrEmpty(title))
			{
				jsonResult = base.Json(new { State = 0, Msg = "节点标题不能为空！", Result = "" });
			}
			else if (!string.IsNullOrEmpty(url))
			{
				Menu child = new Menu()
				{
					Title = title,
					ParentId = new Guid?(parentid),
					Url = url,
					Type = new int?(type),
					Tier = new int?(1),
					IsDeleted = false,
					CreateTime = DateTime.Now
				};
				MenuProvider.Instance.Create(child);
				jsonResult = base.Json(new { State = 1, Msg = "添加成功！", Result = "" });
			}
			else
			{
				jsonResult = base.Json(new { State = 0, Msg = "页面地址不能为空！", Result = "" });
			}
			return jsonResult;
		}

		public JsonResult AddDep(Guid facultyid, string name)
		{
			JsonResult jsonResult;
			if (string.IsNullOrEmpty(name))
			{
				jsonResult = base.Json(new { State = 0, Msg = "系别名称不能为空！", Result = "" });
			}
			else if (SchoolProvider.Instance.GetEntityDep(name, facultyid) == null)
			{
				School school = new School()
				{
					Name = name,
					ParentId = new Guid?(facultyid),
					Tier = 2,
					SellId = base.CurrentAdmin.Id,
					StartTime = new DateTime?(DateTime.Now)
				};
				DateTime now = DateTime.Now;
				school.EndTime = new DateTime?(now.AddYears(100));
				school.CreateTime = DateTime.Now;
				school.IsDeleted = false;
				SchoolProvider.Instance.Create(school);
				jsonResult = base.Json(new { State = 1, Msg = "添加成功！", Result = "" });
			}
			else
			{
				jsonResult = base.Json(new { State = 1, Msg = "该系别名称已存在！", Result = "" });
			}
			return jsonResult;
		}

		public JsonResult AddFac(Guid schoolid, string name)
		{
			JsonResult jsonResult;
			if (string.IsNullOrEmpty(name))
			{
				jsonResult = base.Json(new { State = 0, Msg = "学院名称不能为空！", Result = "" });
			}
			else if (SchoolProvider.Instance.GetEntityFac(name, schoolid) == null)
			{
				School school = new School()
				{
					Name = name,
					ParentId = new Guid?(schoolid),
					Tier = 1,
					SellId = base.CurrentAdmin.Id,
					StartTime = new DateTime?(DateTime.Now)
				};
				DateTime now = DateTime.Now;
				school.EndTime = new DateTime?(now.AddYears(100));
				school.CreateTime = DateTime.Now;
				school.IsDeleted = false;
				SchoolProvider.Instance.Create(school);
				jsonResult = base.Json(new { State = 1, Msg = "添加成功！", Result = "" });
			}
			else
			{
				jsonResult = base.Json(new { State = 1, Msg = "该学院名称已存在！", Result = "" });
			}
			return jsonResult;
		}

		public JsonResult AddManager(Guid schoolid, Guid facultyid, Guid departmentid, string name, string loginName, string loginPwd, Guid role)
		{
			JsonResult jsonResult;
			if (UserInfoProvider.Instance.GetEntity(loginName) != null)
			{
				jsonResult = base.Json(new { State = 1, Msg = "用户名已存在！", Result = "" });
			}
			else if ((string.IsNullOrEmpty(loginName) ? true : string.IsNullOrEmpty(loginPwd)))
			{
				jsonResult = base.Json(new { State = 1, Msg = "用户名或密码不能为空！", Result = "" });
			}
			else
			{
				UserInfoProvider.Instance.GetEntity(base.CurrentAdmin.Id);
				UserInfo manager = new UserInfo()
				{
					Id = Guid.NewGuid(),
					Name = name,
					LoginName = loginName,
					LoginPwd = Encrypt.GetMD5(loginPwd),
					SchoolId = schoolid
				};
				if (facultyid != Guid.Empty)
				{
					manager.FacultyId = new Guid?(facultyid);
				}
				if (departmentid != Guid.Empty)
				{
					manager.DepartmentId = new Guid?(departmentid);
				}
				if (!(role != Guid.Empty))
				{
					jsonResult = base.Json(new { State = 1, Msg = "请选择或添加角色！", Result = "" });
				}
				else
				{
					User_Role ur = new User_Role()
					{
						UserId = manager.Id,
						RoleId = role,
						CreateTime = DateTime.Now,
						IsCustom = false,
						IsDeleted = false
					};
					manager.Type = new int?(-1);
					manager.CreateTime = DateTime.Now;
					manager.IsDeleted = false;
					User_RoleProvider.Instance.Create(ur);
					UserInfoProvider.Instance.Create(manager);
					jsonResult = base.Json(new { State = 1, Msg = "添加成功！", Result = "" });
				}
			}
			return jsonResult;
		}

		public JsonResult Addmenu(string menuname)
		{
			Menu r = new Menu()
			{
				Id = Guid.NewGuid(),
				Title = menuname,
				IsDeleted = false,
				CreateTime = DateTime.Now
			};
			MenuProvider.Instance.Create(r);
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "添加成功！", Result = "" });
			return jsonResult;
		}

		public JsonResult Addrole(string rolename)
		{
			Role r = new Role()
			{
				Id = Guid.NewGuid(),
				Name = rolename,
				Type = new int?(0),
				SchoolId = new Guid?(base.CurrentAdmin.SchoolId),
				IsDeleted = false,
				CreateTime = DateTime.Now
			};
			RoleProvider.Instance.Create(r);
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "添加成功！", Result = "" });
			return jsonResult;
		}

		public JsonResult AddRoot(int type, string title, Guid? module)
		{
			JsonResult jsonResult;
			if (!string.IsNullOrEmpty(title))
			{
				Menu root = new Menu()
				{
					Title = title,
					Type = new int?(type),
					ModuleId = module,
					Tier = new int?(0),
					IsDeleted = false,
					CreateTime = DateTime.Now
				};
				MenuProvider.Instance.Create(root);
				jsonResult = base.Json(new { State = 1, Msg = "添加成功！", Result = "" });
			}
			else
			{
				jsonResult = base.Json(new { State = 0, Msg = "节点标题不能为空！", Result = "" });
			}
			return jsonResult;
		}

		[HttpPost]
		public JsonResult AddSchool(string schoolname, List<School_Module> ids, string ips, string end, string remind, int tier, Guid sell, int trial, Guid province)
		{
			JsonResult jsonResult;
			char[] chrArray;
			if (string.IsNullOrEmpty(schoolname.Trim()))
			{
				jsonResult = base.Json(new { State = 0, Msg = "学校名称不能为空！", Result = "" });
			}
			else if (!string.IsNullOrEmpty(end.Trim()))
			{
				string str = "";
				string ipStr = "";
				foreach (string item in IpHelper.IpToList(ips.Trim()))
				{
					if (IpHelper.CheckIp(item))
					{
						ipStr = string.Concat(ipStr, item, ",");
					}
					else
					{
						str = string.Concat(str, item, "<br />");
					}
				}
				if (!string.IsNullOrEmpty(str))
				{
					string str1 = string.Concat(str, "不是合法IP");
					chrArray = new char[] { ',' };
					jsonResult = base.Json(new { State = 0, Msg = str1, Result = ipStr.TrimEnd(chrArray) });
				}
				else if (SchoolProvider.Instance.GetEntity(schoolname.Trim()) == null)
				{
					School school = new School()
					{
						Id = Guid.NewGuid(),
						Name = schoolname.Trim(),
						StopRemind = remind.Trim(),
						StartTime = new DateTime?(DateTime.Now),
						EndTime = new DateTime?(Convert.ToDateTime(end)),
						SellId = (sell == Guid.Empty ? base.CurrentAdmin.Id : sell),
						Tier = tier,
						TrialType = new int?(trial),
						CreateTime = DateTime.Now,
						IsDeleted = false,
						ProvinceId = province
					};
					SchoolProvider.Instance.Create(school);
					Role r1 = new Role();
					Role r2 = new Role();
					Role r3 = new Role();
					r1.Name = "学校管理员";
					r1.SchoolId = new Guid?(school.Id);
					r1.CreateTime = DateTime.Now;
					r1.IsDeleted = false;
					r1.Type = new int?(1);
					r2.Name = "院系管理员";
					r2.SchoolId = new Guid?(school.Id);
					r2.CreateTime = DateTime.Now;
					r2.IsDeleted = false;
					r2.Type = new int?(2);
					r3.Name = "教师";
					r3.SchoolId = new Guid?(school.Id);
					r3.CreateTime = DateTime.Now;
					r3.IsDeleted = false;
					r3.Type = new int?(4);
					RoleProvider.Instance.Create(r1);
					RoleProvider.Instance.Create(r2);
					RoleProvider.Instance.Create(r3);
					if (ids != null)
					{
						for (int i = 0; i < ids.Count; i++)
						{
							ids[i].Id = Guid.NewGuid();
							ids[i].CreateTime = DateTime.Now;
							ids[i].IsDeleted = false;
							ids[i].SchoolId = school.Id;
							School_ModuleProvider.Instance.Create(ids[i]);
						}
					}
					chrArray = new char[] { ',' };
					string[] strArrays = ips.Split(chrArray, StringSplitOptions.RemoveEmptyEntries);
					for (int j = 0; j < (int)strArrays.Length; j++)
					{
						string ip = strArrays[j];
						Ip p = new Ip()
						{
							SchoolId = school.Id,
							CreateTime = DateTime.Now,
							IsDeleted = false
						};
						if (!ip.Contains<char>('-'))
						{
							p.IpStart = IpHelper.ParseByte(ip.Trim());
							p.IpEnd = IpHelper.ParseByte(ip.Trim());
						}
						else
						{
							chrArray = new char[] { '-' };
							string[] temp = ip.Split(chrArray, StringSplitOptions.RemoveEmptyEntries);
							p.IpStart = IpHelper.ParseByte(temp[0].Trim());
							p.IpEnd = IpHelper.ParseByte(temp[1].Trim());
						}
						p.Tier = tier;
						Ip ps = IpProvider.Instance.GetEntity(p.IpStart);
						Ip pe = IpProvider.Instance.GetEntity(p.IpEnd);
						if ((ps != null ? true : pe != null))
						{
							str = string.Concat(str, ip, "已存在！<br />");
						}
						else
						{
							IpProvider.Instance.Create(p);
						}
					}
					jsonResult = (string.IsNullOrEmpty(str) ? base.Json(new { State = 1, Msg = "添加成功！", Result = "" }) : base.Json(new { State = 0, Msg = str, Result = "" }));
				}
				else
				{
					jsonResult = base.Json(new { State = 0, Msg = "添加失败，学校已存在！", Result = "" });
				}
			}
			else
			{
				jsonResult = base.Json(new { State = 0, Msg = "到期时间不能为空！", Result = "" });
			}
			return jsonResult;
		}

		public JsonResult Addsell(string sellname, string sellloginname, string sellloginpwd)
		{
			UserInfo r = new UserInfo()
			{
				Id = Guid.NewGuid(),
				Name = sellname,
				LoginName = sellloginname,
				LoginPwd = sellloginpwd,
				IsDeleted = false,
				CreateTime = DateTime.Now,
				Type = new int?(3),
				SchoolId = base.CurrentAdmin.SchoolId
			};
			UserInfoProvider.Instance.Create(r);
			User_Role u = new User_Role()
			{
				Id = Guid.NewGuid(),
				UserId = r.Id,
				RoleId = RoleProvider.Instance.GetRoleid(base.CurrentAdmin.SchoolId),
				IsCustom = false,
				CreateTime = DateTime.Now,
				IsDeleted = false
			};
			User_RoleProvider.Instance.Create(u);
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "添加成功！", Result = "" });
			return jsonResult;
		}

		public ActionResult Admin()
		{
			return base.View();
		}

		public JsonResult Affirm(List<Guid> ids, int aff)
		{
			int i;
			JsonResult jsonResult;
			if ((ids == null ? true : ids.Count <= 0))
			{
				jsonResult = base.Json(new { State = 1, Msg = "操作失败!", Result = "" });
			}
			else
			{
				if (aff == -1)
				{
					for (i = 0; i < ids.Count; i++)
					{
						if (ids[i] != base.CurrentAdmin.Id)
						{
							UserInfoProvider.Instance.Delete(ids[i]);
							User_RoleProvider.Instance.DeleteByUserId(ids[i]);
						}
					}
				}
				else if (aff == 0)
				{
					for (i = 0; i < ids.Count; i++)
					{
						UserInfo user = UserInfoProvider.Instance.GetUser(ids[i]);
						user.IsDeleted = false;
						UserInfoProvider.Instance.Update(user);
					}
				}
				else if (aff == 1)
				{
					for (i = 0; i < ids.Count; i++)
					{
						if (ids[i] != base.CurrentAdmin.Id)
						{
							UserInfoProvider.Instance.SoftDelete(ids[i]);
						}
					}
				}
				jsonResult = base.Json(new { State = 1, Msg = "操作成功!", Result = "" });
			}
			return jsonResult;
		}

		public JsonResult AffirmSchool(List<Guid> ids, int aff)
		{
			int i;
			JsonResult jsonResult;
			if ((ids == null ? true : ids.Count <= 0))
			{
				jsonResult = base.Json(new { State = 1, Msg = "操作失败!", Result = "" });
			}
			else
			{
				if (aff == 0)
				{
					for (i = 0; i < ids.Count; i++)
					{
						School school = SchoolProvider.Instance.GetEntityById(ids[i]);
						school.IsDeleted = false;
						SchoolProvider.Instance.Update(school);
					}
				}
				else if (aff == 1)
				{
					for (i = 0; i < ids.Count; i++)
					{
						if (ids[i] != base.CurrentAdmin.SchoolId)
						{
							SchoolProvider.Instance.SoftDelete(ids[i]);
						}
					}
				}
				jsonResult = base.Json(new { State = 1, Msg = "操作成功!", Result = "" });
			}
			return jsonResult;
		}

		public int CompareDate(DateTime dt)
		{
			int i = 1;
			if (DateTime.Compare(dt, DateTime.Now) > 0)
			{
				i = ((dt - DateTime.Now).TotalDays < 30 ? 0 : 1);
			}
			return i;
		}

		private static DocX CreateFiratTemp(DocX template, Guid schoolid)
		{
			int i;
			Paragraph r_c1;
			List<DownloadDetails> item;
			Paragraph r_c2;
			DateTime value;
			object[] tag2;
			School sh = SchoolProvider.Instance.GetEntity(schoolid);
			int num = 0;
			int BrowseCount = LogDetailProvider.Instance.GetCountResource(schoolid, 0, num.ToString());
			num = 0;
			int DownloadCount = LogDetailProvider.Instance.GetCountResource(schoolid, 2, num.ToString());
			List<DownloadDetails> ips = LogDetailProvider.Instance.GetIpDetailsResource(schoolid, 2, 0);
			string dayMuch = "";
			int downCount = 0;
			num = 0;
			DownloadDetails logday = LogDetailProvider.Instance.GetMostDayResource(schoolid, 2, num.ToString());
			if (logday != null)
			{
				downCount = logday.DownloadCount;
				dayMuch = logday.Tag2;
			}
			else
			{
				dayMuch = "暂无";
			}
			string sourcename = "";
			int sourceDownCount = 0;
			string resourcepack = "";
			int packcount = 0;
			num = 0;
			DownloadDetails logRM = LogDetailProvider.Instance.GetMostResourceMaterial(schoolid, 2, num.ToString());
			if (logRM != null)
			{
				sourceDownCount = logRM.DownloadCount;
				sourcename = logRM.Tag2;
			}
			else
			{
				sourcename = "暂无";
			}
			num = 0;
			DownloadDetails logRR = LogDetailProvider.Instance.GetMostResourcePack(schoolid, 2, num.ToString());
			if (logRR != null)
			{
				packcount = logRR.DownloadCount;
				resourcepack = logRR.Tag2;
			}
			else
			{
				packcount = 0;
				resourcepack = "暂无";
			}
			num = 0;
			List<DownloadDetails> cateDetailsSRnew = LogDetailProvider.Instance.GetCateDetailsSRnew(schoolid, 2, num.ToString());
			num = 0;
			List<DownloadDetails> subs = LogDetailProvider.Instance.GetSubDetailsSRnew(schoolid, 2, num.ToString());
			List<DownloadDetails> mons = LogDetailProvider.Instance.GetMonthDetailsResource(schoolid, 2, 0);
			num = 0;
			List<DownloadDetails> iplist = LogDetailProvider.Instance.GetListTrialreport(schoolid, 2, num.ToString());
			if (sh != null)
			{
				if (sh.Name != null)
				{
					template.AddCustomProperty(new CustomProperty("shname", sh.Name));
				}
				if (sh.StartTime.HasValue)
				{
					value = sh.StartTime.Value;
					template.AddCustomProperty(new CustomProperty("shStartTime", value.ToLongDateString()));
					value = sh.StartTime.Value;
					template.AddCustomProperty(new CustomProperty("starttime  ", value.ToLongDateString()));
				}
			}
			value = DateTime.Now;
			template.AddCustomProperty(new CustomProperty("endtime", value.ToLongDateString()));
			template.AddCustomProperty(new CustomProperty("BrowseCount", BrowseCount));
			template.AddCustomProperty(new CustomProperty("DownloadCount", DownloadCount));
			if (!string.IsNullOrEmpty(dayMuch))
			{
				if (dayMuch != "暂无")
				{
					value = DateTime.Parse(dayMuch);
					template.AddCustomProperty(new CustomProperty("dayMuch", value.ToLongDateString()));
				}
			}
			template.AddCustomProperty(new CustomProperty("downCount", downCount));
			template.AddCustomProperty(new CustomProperty("sourcename", sourcename));
			template.AddCustomProperty(new CustomProperty("sourceDownCount", sourceDownCount));
			template.AddCustomProperty(new CustomProperty("resourcepack", resourcepack));
			template.AddCustomProperty(new CustomProperty("packcount", packcount));
			int rowcount = 0;
			int r_num = 0;
			int c_num = 0;
			if (ips != null)
			{
				rowcount = (ips.Count % 3 != 0 ? ips.Count / 3 + 1 : ips.Count / 3);
			}
			Table t = template.Tables[0];
			Table invoice_table = t.InsertTableAfterSelf(rowcount, 3);
			t.Remove();
			invoice_table.Design = TableDesign.TableGrid;
			invoice_table.Alignment = Alignment.center;
			if (ips != null)
			{
				for (i = 0; i < ips.Count; i++)
				{
					r_c1 = invoice_table.Rows[r_num].Cells[c_num].Paragraphs[0];
					tag2 = new object[] { ips[i].Tag2, "：", ips[i].DownloadCount, "次" };
					r_c1.InsertText(string.Concat(tag2), false, null);
					invoice_table.Rows[r_num].Cells[c_num].Width = 200;
					c_num++;
					if ((i + 1) % 3 == 0)
					{
						r_num++;
						c_num = 0;
					}
				}
			}
			Formatting formmatting = new Formatting()
			{
				FontColor = new Color?(Color.Blue)
			};
			int rowcount_one = 0;
			int r_numo = 0;
			int itemcount = 0;
			if ((subs == null ? false : cateDetailsSRnew != null))
			{
				for (int j = 0; j < cateDetailsSRnew.Count; j++)
				{
					item = (
						from p in subs
						where p.Id == cateDetailsSRnew[j].Id
						select p).ToList<DownloadDetails>();
					itemcount = (item.Count % 4 != 0 ? itemcount + item.Count / 4 + 1 : itemcount + item.Count / 4);
				}
			}
			rowcount_one = itemcount + cateDetailsSRnew.Count;
			Table t1 = template.Tables[1];
			Table invoice_table1 = t1.InsertTableAfterSelf(rowcount_one, 4);
			t1.Remove();
			invoice_table1.Design = TableDesign.None;
			invoice_table1.Alignment = Alignment.center;
			if ((subs == null ? false : cateDetailsSRnew != null))
			{
				for (int k = 0; k < cateDetailsSRnew.Count; k++)
				{
					item = (
						from p in subs
						where p.Id == cateDetailsSRnew[k].Id
						select p).ToList<DownloadDetails>();
					if (item != null)
					{
						if (item.Count > 0)
						{
							r_c1 = invoice_table1.Rows[r_numo].Cells[0].Paragraphs[0];
							tag2 = new object[] { cateDetailsSRnew[k].Tag2, "：", cateDetailsSRnew[k].DownloadCount, "次" };
							r_c1.InsertText(string.Concat(tag2), false, formmatting);
							invoice_table1.Rows[r_numo].Height = 20;
							invoice_table1.Rows[r_numo].MergeCells(0, 3);
							r_numo++;
							int c_numo = 0;
							for (int s = 0; s < item.Count; s++)
							{
								r_c2 = invoice_table1.Rows[r_numo].Cells[c_numo].Paragraphs[0];
								tag2 = new object[] { item[s].Tag2, "：", item[s].DownloadCount, "次" };
								r_c2.InsertText(string.Concat(tag2), false, null);
								c_numo++;
								if ((s + 1) % 4 == 0)
								{
									if (s + 1 != item.Count)
									{
										r_numo++;
										c_numo = 0;
									}
								}
							}
							r_numo++;
						}
					}
				}
			}
			int rowcount_two = 0;
			int r_numt = 0;
			int c_numt = 0;
			if (mons != null)
			{
				rowcount_two = (mons.Count % 3 != 0 ? mons.Count / 3 + 1 : mons.Count / 3);
			}
			Table t2 = template.Tables[2];
			Table invoice_table2 = t2.InsertTableAfterSelf(rowcount_two, 3);
			t2.Remove();
			invoice_table2.Design = TableDesign.None;
			if (mons != null)
			{
				for (i = 0; i < mons.Count; i++)
				{
					r_c1 = invoice_table2.Rows[r_numt].Cells[c_numt].Paragraphs[0];
					tag2 = new object[] { mons[i].Tag2, "：", mons[i].DownloadCount, "次" };
					r_c1.InsertText(string.Concat(tag2), false, null);
					c_numt++;
					if ((i + 1) % 3 == 0)
					{
						r_numt++;
						c_numt = 0;
					}
				}
			}
			Table t3 = template.Tables[3];
			Table invoice_ip = t3.InsertTableAfterSelf(iplist.Count + 1, 3);
			t3.Remove();
			invoice_ip.Design = TableDesign.TableGrid;
			invoice_ip.Alignment = Alignment.center;
			Paragraph cell_0 = invoice_ip.Rows[0].Cells[0].Paragraphs[0];
			cell_0.InsertText("资源名称", false, null);
			invoice_ip.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
			invoice_ip.Rows[0].Cells[0].Width = 200;
			Paragraph cell_1 = invoice_ip.Rows[0].Cells[1].Paragraphs[0];
			cell_1.InsertText("下载IP", false, null);
			invoice_ip.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
			invoice_ip.Rows[0].Cells[1].Width = 200;
			Paragraph cell_2 = invoice_ip.Rows[0].Cells[2].Paragraphs[0];
			cell_2.InsertText("下载日期", false, null);
			invoice_ip.Rows[0].Cells[2].Paragraphs[0].Alignment = Alignment.center;
			invoice_ip.Rows[0].Cells[2].Width = 200;
			int row_numip = 1;
			if (iplist != null)
			{
				for (i = 0; i < iplist.Count; i++)
				{
					r_c1 = invoice_ip.Rows[row_numip].Cells[0].Paragraphs[0];
					r_c1.InsertText(iplist[i].ResourceName, false, null);
					r_c2 = invoice_ip.Rows[row_numip].Cells[1].Paragraphs[0];
					r_c2.InsertText(iplist[i].Ip, false, null);
					invoice_ip.Rows[row_numip].Cells[1].Paragraphs[0].Alignment = Alignment.center;
					DateTime createTime = iplist[i].CreateTime;
					Paragraph r_c3 = invoice_ip.Rows[row_numip].Cells[2].Paragraphs[0];
					value = iplist[i].CreateTime;
					r_c3.InsertText(value.ToShortDateString().ToString(), false, null);
					invoice_ip.Rows[row_numip].Cells[2].Paragraphs[0].Alignment = Alignment.center;
					row_numip++;
				}
			}
			BarChart bc = new BarChart()
			{
				BarDirection = BarDirection.Column,
				BarGrouping = BarGrouping.Standard,
				GapWidth = 400
			};
			bc.AddLegend(ChartLegendPosition.Bottom, false);
			List<ChartData> company1 = ChartData.CreateCompanyList(LogDetailProvider.Instance.GetSumPack());
			List<ChartData> company2 = ChartData.CreateCompanyList(LogDetailProvider.Instance.GetSumCourse());
			List<ChartData> company3 = ChartData.CreateCompanyList(LogDetailProvider.Instance.GetSumMaterial());
			Series s1 = new Series("课件包");
			s1.Bind(company1, "Name", "Count");
			bc.AddSeries(s1);
			Series s2 = new Series("课件章节");
			s2.Bind(company2, "Name", "Count");
			bc.AddSeries(s2);
			Series s3 = new Series("素材");
			s3.Bind(company3, "Name", "Count");
			bc.AddSeries(s3);
			template.InsertParagraph("资源库图表").FontSize(20);
			template.InsertChart(bc);
			return template;
		}

		private static DocX CreateFiratTempExam(DocX template, Guid schoolid)
		{
			int i;
			Paragraph r_c1;
			List<DownloadDetails> item;
			Paragraph r_c2;
			DateTime value;
			object[] tag2;
			School sh = SchoolProvider.Instance.GetEntity(schoolid);
			int numExam = 6;
			int CountED = 0;
			string DayED = "";
			int EDCount = 0;
			string ResourceED = "";
			int CountE = 0;
			string DayE = "";
			int ECount = 0;
			string ResourceE = "";
			int num = 4;
			DownloadDetails logDED = LogDetailProvider.Instance.GetMostDayExam(schoolid, 2, num.ToString(), numExam);
			if (logDED != null)
			{
				CountED = logDED.DownloadCount;
				DayED = logDED.Tag2;
			}
			else
			{
				CountED = 0;
				DayED = "暂无";
			}
			num = 4;
			DownloadDetails logED = LogDetailProvider.Instance.GetMostResourceE(schoolid, 2, num.ToString(), numExam);
			if (logED != null)
			{
				EDCount = logED.DownloadCount;
				ResourceED = logED.Tag2;
			}
			else
			{
				EDCount = 0;
				ResourceED = "暂无";
			}
			num = 4;
			DownloadDetails logDE = LogDetailProvider.Instance.GetMostDayExam(schoolid, 0, num.ToString(), numExam);
			if (logDE != null)
			{
				CountE = logDE.DownloadCount;
				DayE = logDE.Tag2;
			}
			else
			{
				CountE = 0;
				DayE = "暂无";
			}
			num = 4;
			DownloadDetails logE = LogDetailProvider.Instance.GetMostResourceE(schoolid, 0, num.ToString(), numExam);
			if (logE != null)
			{
				ECount = logE.DownloadCount;
				ResourceE = logE.Tag2;
			}
			else
			{
				ECount = 0;
				ResourceE = "暂无";
			}
			num = 4;
			int BrowseCountE = LogDetailProvider.Instance.GetCountExam(schoolid, 0, num.ToString(), numExam);
			num = 4;
			int DownloadCountE = LogDetailProvider.Instance.GetCountExam(schoolid, 2, num.ToString(), numExam);
			List<DownloadDetails> ipDetailsExamBrowse = LogDetailProvider.Instance.GetIpDetailsExamBrowse(schoolid, 2, 4, numExam);
			List<DownloadDetails> ips = ipDetailsExamBrowse;
			ips = ipDetailsExamBrowse;
			num = 4;
			List<DownloadDetails> cateDetailsSE = LogDetailProvider.Instance.GetCateDetailsSE(schoolid, 2, num.ToString());
			num = 4;
			List<DownloadDetails> subs = LogDetailProvider.Instance.GetSubDetailsSE(schoolid, 2, num.ToString());
			List<DownloadDetails> mons = LogDetailProvider.Instance.GetMonthDetailsExam(schoolid, 2, 4, numExam);
			num = 4;
			List<DownloadDetails> iplist = LogDetailProvider.Instance.GetListTrialreportExam(schoolid, 0, num.ToString());
			if (sh != null)
			{
				if (sh.Name != null)
				{
					template.AddCustomProperty(new CustomProperty("shname", sh.Name));
				}
				if (sh.StartTime.HasValue)
				{
					value = sh.StartTime.Value;
					template.AddCustomProperty(new CustomProperty("shStartTime", value.ToLongDateString()));
					value = sh.StartTime.Value;
					template.AddCustomProperty(new CustomProperty("starttime  ", value.ToLongDateString()));
				}
			}
			value = DateTime.Now;
			template.AddCustomProperty(new CustomProperty("endtime", value.ToLongDateString()));
			template.AddCustomProperty(new CustomProperty("CountED", CountED));
			template.AddCustomProperty(new CustomProperty("DayED", DayED));
			template.AddCustomProperty(new CustomProperty("EDCount", EDCount));
			template.AddCustomProperty(new CustomProperty("ResourceED", ResourceED));
			template.AddCustomProperty(new CustomProperty("CountE", CountE));
			template.AddCustomProperty(new CustomProperty("DayE", DayE));
			template.AddCustomProperty(new CustomProperty("ECount", ECount));
			template.AddCustomProperty(new CustomProperty("ResourceE", ResourceE));
			template.AddCustomProperty(new CustomProperty("BrowseCountE", BrowseCountE));
			template.AddCustomProperty(new CustomProperty("DownloadCountE", DownloadCountE));
			int rowcount = 0;
			int r_num = 0;
			int c_num = 0;
			if (ips != null)
			{
				rowcount = (ips.Count % 3 != 0 ? ips.Count / 3 + 1 : ips.Count / 3);
			}
			Table t = template.Tables[0];
			Table invoice_table = t.InsertTableAfterSelf(rowcount, 3);
			t.Remove();
			invoice_table.Design = TableDesign.TableGrid;
			invoice_table.Alignment = Alignment.center;
			if (ips != null)
			{
				for (i = 0; i < ips.Count; i++)
				{
					r_c1 = invoice_table.Rows[r_num].Cells[c_num].Paragraphs[0];
					tag2 = new object[] { ips[i].Tag2, "：", ips[i].DownloadCount, "次" };
					r_c1.InsertText(string.Concat(tag2), false, null);
					invoice_table.Rows[r_num].Cells[c_num].Width = 200;
					c_num++;
					if ((i + 1) % 3 == 0)
					{
						r_num++;
						c_num = 0;
					}
				}
			}
			Formatting formmatting = new Formatting()
			{
				FontColor = new Color?(Color.Blue)
			};
			int rowcount_one = 0;
			int r_numo = 0;
			int itemcount = 0;
			if ((subs == null ? false : cateDetailsSE != null))
			{
				for (int j = 0; j < cateDetailsSE.Count; j++)
				{
					item = (
						from p in subs
						where p.Id == cateDetailsSE[j].Id
						select p).ToList<DownloadDetails>();
					itemcount = (item.Count % 4 != 0 ? itemcount + item.Count / 4 + 1 : itemcount + item.Count / 4);
				}
			}
			rowcount_one = itemcount + cateDetailsSE.Count;
			Table t1 = template.Tables[1];
			Table invoice_table1 = t1.InsertTableAfterSelf(rowcount_one, 4);
			t1.Remove();
			invoice_table1.Design = TableDesign.None;
			invoice_table1.Alignment = Alignment.center;
			if ((subs == null ? false : cateDetailsSE != null))
			{
				for (int k = 0; k < cateDetailsSE.Count; k++)
				{
					item = (
						from p in subs
						where p.Id == cateDetailsSE[k].Id
						select p).ToList<DownloadDetails>();
					if (item != null)
					{
						if (item.Count > 0)
						{
							r_c1 = invoice_table1.Rows[r_numo].Cells[0].Paragraphs[0];
							tag2 = new object[] { cateDetailsSE[k].Tag2, "：", cateDetailsSE[k].DownloadCount, "次" };
							r_c1.InsertText(string.Concat(tag2), false, formmatting);
							invoice_table1.Rows[r_numo].Height = 20;
							invoice_table1.Rows[r_numo].MergeCells(0, 3);
							r_numo++;
							int c_numo = 0;
							for (int s = 0; s < item.Count; s++)
							{
								r_c2 = invoice_table1.Rows[r_numo].Cells[c_numo].Paragraphs[0];
								tag2 = new object[] { item[s].Tag2, "：", item[s].DownloadCount, "次" };
								r_c2.InsertText(string.Concat(tag2), false, null);
								c_numo++;
								if ((s + 1) % 4 == 0)
								{
									if (s + 1 != item.Count)
									{
										r_numo++;
										c_numo = 0;
									}
								}
							}
							r_numo++;
						}
					}
				}
			}
			int rowcount_two = 0;
			int r_numt = 0;
			int c_numt = 0;
			if (mons != null)
			{
				rowcount_two = (mons.Count % 3 != 0 ? mons.Count / 3 + 1 : mons.Count / 3);
			}
			Table t2 = template.Tables[2];
			Table invoice_table2 = t2.InsertTableAfterSelf(rowcount_two, 3);
			t2.Remove();
			invoice_table2.Design = TableDesign.None;
			if (mons != null)
			{
				for (i = 0; i < mons.Count; i++)
				{
					r_c1 = invoice_table2.Rows[r_numt].Cells[c_numt].Paragraphs[0];
					tag2 = new object[] { mons[i].Tag2, "：", mons[i].DownloadCount, "次" };
					r_c1.InsertText(string.Concat(tag2), false, null);
					c_numt++;
					if ((i + 1) % 3 == 0)
					{
						r_numt++;
						c_numt = 0;
					}
				}
			}
			Table t3 = template.Tables[3];
			Table invoice_ip = t3.InsertTableAfterSelf(iplist.Count + 1, 3);
			t3.Remove();
			invoice_ip.Design = TableDesign.TableGrid;
			invoice_ip.Alignment = Alignment.center;
			Paragraph cell_0 = invoice_ip.Rows[0].Cells[0].Paragraphs[0];
			cell_0.InsertText("资源名称", false, null);
			invoice_ip.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
			invoice_ip.Rows[0].Cells[0].Width = 200;
			Paragraph cell_1 = invoice_ip.Rows[0].Cells[1].Paragraphs[0];
			cell_1.InsertText("下载IP", false, null);
			invoice_ip.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
			invoice_ip.Rows[0].Cells[1].Width = 200;
			Paragraph cell_2 = invoice_ip.Rows[0].Cells[2].Paragraphs[0];
			cell_2.InsertText("下载日期", false, null);
			invoice_ip.Rows[0].Cells[2].Paragraphs[0].Alignment = Alignment.center;
			invoice_ip.Rows[0].Cells[2].Width = 200;
			int row_numip = 1;
			if (iplist != null)
			{
				for (i = 0; i < iplist.Count; i++)
				{
					r_c1 = invoice_ip.Rows[row_numip].Cells[0].Paragraphs[0];
					r_c1.InsertText(iplist[i].ResourceName, false, null);
					r_c2 = invoice_ip.Rows[row_numip].Cells[1].Paragraphs[0];
					r_c2.InsertText(iplist[i].Ip, false, null);
					invoice_ip.Rows[row_numip].Cells[1].Paragraphs[0].Alignment = Alignment.center;
					DateTime createTime = iplist[i].CreateTime;
					Paragraph r_c3 = invoice_ip.Rows[row_numip].Cells[2].Paragraphs[0];
					value = iplist[i].CreateTime;
					r_c3.InsertText(value.ToShortDateString().ToString(), false, null);
					invoice_ip.Rows[row_numip].Cells[2].Paragraphs[0].Alignment = Alignment.center;
					row_numip++;
				}
			}
			BarChart bc = new BarChart()
			{
				BarDirection = BarDirection.Column,
				BarGrouping = BarGrouping.Standard,
				GapWidth = 400
			};
			bc.AddLegend(ChartLegendPosition.Bottom, false);
			List<ChartData> company1 = ChartData.CreateCompanyList(LogDetailProvider.Instance.GetSumCountExam());
			Series s1 = new Series("题量")
			{
				Color = Color.Blue
			};
			s1.Bind(company1, "Name", "Count");
			bc.AddSeries(s1);
			template.InsertParagraph("医学库图表").FontSize(20);
			template.InsertChart(bc);
			return template;
		}

		private static DocX CreateFiratTempMedical(DocX template, Guid schoolid)
		{
			int i;
			Paragraph r_c1;
			List<DownloadDetails> item;
			Paragraph r_c2;
			DateTime value;
			object[] tag2;
			School sh = SchoolProvider.Instance.GetEntity(schoolid);
			int num = 1;
			int BrowseCount = LogDetailProvider.Instance.GetCount(schoolid, 0, num.ToString());
			num = 1;
			int CollectCount = LogDetailProvider.Instance.GetCount(schoolid, 1, num.ToString());
			num = 1;
			int DownloadCount = LogDetailProvider.Instance.GetCount(schoolid, 2, num.ToString());
			List<DownloadDetails> ips = LogDetailProvider.Instance.GetIpDetails(schoolid, 2, 1);
			int DCountM = 0;
			string DayM = "";
			num = 1;
			DownloadDetails logDM = LogDetailProvider.Instance.GetMostDay(schoolid, 2, num.ToString());
			if (logDM != null)
			{
				DCountM = logDM.DownloadCount;
				DayM = logDM.Tag2;
			}
			else
			{
				DCountM = 0;
				DayM = "暂无";
			}
			int MCount = 0;
			string ResourceM = "";
			num = 1;
			DownloadDetails logM = LogDetailProvider.Instance.GetMostResourceM(schoolid, 2, num.ToString());
			if (logM != null)
			{
				MCount = logM.DownloadCount;
				ResourceM = logM.Tag2;
			}
			else
			{
				MCount = 0;
				ResourceM = "暂无";
			}
			List<DownloadDetails> subDetailsSM = LogDetailProvider.Instance.GetSubDetailsSM(schoolid, 2, "1");
			num = 1;
			List<DownloadDetails> subs = LogDetailProvider.Instance.GetSubDetailsSMTwo(schoolid, 2, num.ToString());
			List<DownloadDetails> mons = LogDetailProvider.Instance.GetMonthDetails(schoolid, 2, 1);
			num = 1;
			List<DownloadDetails> iplist = LogDetailProvider.Instance.GetListTrialreport(schoolid, 2, num.ToString());
			if (sh != null)
			{
				if (sh.Name != null)
				{
					template.AddCustomProperty(new CustomProperty("shname", sh.Name));
				}
				if (sh.StartTime.HasValue)
				{
					value = sh.StartTime.Value;
					template.AddCustomProperty(new CustomProperty("shStartTime", value.ToLongDateString()));
					value = sh.StartTime.Value;
					template.AddCustomProperty(new CustomProperty("starttime  ", value.ToLongDateString()));
				}
			}
			value = DateTime.Now;
			template.AddCustomProperty(new CustomProperty("endtime", value.ToLongDateString()));
			template.AddCustomProperty(new CustomProperty("BrowseCount", BrowseCount));
			template.AddCustomProperty(new CustomProperty("CollectCount", CollectCount));
			template.AddCustomProperty(new CustomProperty("DownloadCount", DownloadCount));
			if (!string.IsNullOrEmpty(DayM))
			{
				if (DayM != "暂无")
				{
					value = DateTime.Parse(DayM);
					template.AddCustomProperty(new CustomProperty("DayM", value.ToLongDateString()));
				}
			}
			template.AddCustomProperty(new CustomProperty("DCountM", DCountM));
			template.AddCustomProperty(new CustomProperty("MCount", MCount));
			template.AddCustomProperty(new CustomProperty("ResourceM", ResourceM));
			int rowcount = 0;
			int r_num = 0;
			int c_num = 0;
			if (ips != null)
			{
				rowcount = (ips.Count % 3 != 0 ? ips.Count / 3 + 1 : ips.Count / 3);
			}
			Table t = template.Tables[0];
			Table invoice_table = t.InsertTableAfterSelf(rowcount, 3);
			t.Remove();
			invoice_table.Design = TableDesign.TableGrid;
			invoice_table.Alignment = Alignment.center;
			if (ips != null)
			{
				for (i = 0; i < ips.Count; i++)
				{
					r_c1 = invoice_table.Rows[r_num].Cells[c_num].Paragraphs[0];
					tag2 = new object[] { ips[i].Tag2, "：", ips[i].DownloadCount, "次" };
					r_c1.InsertText(string.Concat(tag2), false, null);
					invoice_table.Rows[r_num].Cells[c_num].Width = 200;
					c_num++;
					if ((i + 1) % 3 == 0)
					{
						r_num++;
						c_num = 0;
					}
				}
			}
			Formatting formmatting = new Formatting()
			{
				FontColor = new Color?(Color.Blue)
			};
			int rowcount_one = 0;
			int r_numo = 0;
			int itemcount = 0;
			if ((subs == null ? false : subDetailsSM != null))
			{
				for (int j = 0; j < subDetailsSM.Count; j++)
				{
					item = (
						from p in subs
						where p.Id == subDetailsSM[j].Id
						select p).ToList<DownloadDetails>();
					itemcount = (item.Count % 4 != 0 ? itemcount + item.Count / 4 + 1 : itemcount + item.Count / 4);
				}
			}
			rowcount_one = itemcount + subDetailsSM.Count;
			Table t1 = template.Tables[1];
			Table invoice_table1 = t1.InsertTableAfterSelf(rowcount_one, 4);
			t1.Remove();
			invoice_table1.Design = TableDesign.None;
			invoice_table1.Alignment = Alignment.center;
			if ((subs == null ? false : subDetailsSM != null))
			{
				for (int k = 0; k < subDetailsSM.Count; k++)
				{
					item = (
						from p in subs
						where p.Id == subDetailsSM[k].Id
						select p).ToList<DownloadDetails>();
					if (item != null)
					{
						if (item.Count > 0)
						{
							r_c1 = invoice_table1.Rows[r_numo].Cells[0].Paragraphs[0];
							tag2 = new object[] { subDetailsSM[k].Tag2, "：", subDetailsSM[k].DownloadCount, "次" };
							r_c1.InsertText(string.Concat(tag2), false, formmatting);
							invoice_table1.Rows[r_numo].Height = 20;
							invoice_table1.Rows[r_numo].MergeCells(0, 3);
							r_numo++;
							int c_numo = 0;
							for (int s = 0; s < item.Count; s++)
							{
								r_c2 = invoice_table1.Rows[r_numo].Cells[c_numo].Paragraphs[0];
								tag2 = new object[] { item[s].Tag2, "：", item[s].DownloadCount, "次" };
								r_c2.InsertText(string.Concat(tag2), false, null);
								c_numo++;
								if ((s + 1) % 4 == 0)
								{
									if (s + 1 != item.Count)
									{
										r_numo++;
										c_numo = 0;
									}
								}
							}
							r_numo++;
						}
					}
				}
			}
			int rowcount_two = 0;
			int r_numt = 0;
			int c_numt = 0;
			if (mons != null)
			{
				rowcount_two = (mons.Count % 3 != 0 ? mons.Count / 3 + 1 : mons.Count / 3);
			}
			Table t2 = template.Tables[2];
			Table invoice_table2 = t2.InsertTableAfterSelf(rowcount_two, 3);
			t2.Remove();
			invoice_table2.Design = TableDesign.None;
			if (mons != null)
			{
				for (i = 0; i < mons.Count; i++)
				{
					r_c1 = invoice_table2.Rows[r_numt].Cells[c_numt].Paragraphs[0];
					tag2 = new object[] { mons[i].Tag2, "：", mons[i].DownloadCount, "次" };
					r_c1.InsertText(string.Concat(tag2), false, null);
					c_numt++;
					if ((i + 1) % 3 == 0)
					{
						r_numt++;
						c_numt = 0;
					}
				}
			}
			Table t3 = template.Tables[3];
			Table invoice_ip = t3.InsertTableAfterSelf(iplist.Count + 1, 3);
			t3.Remove();
			invoice_ip.Design = TableDesign.TableGrid;
			invoice_ip.Alignment = Alignment.center;
			Paragraph cell_0 = invoice_ip.Rows[0].Cells[0].Paragraphs[0];
			cell_0.InsertText("资源名称", false, null);
			invoice_ip.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
			invoice_ip.Rows[0].Cells[0].Width = 200;
			Paragraph cell_1 = invoice_ip.Rows[0].Cells[1].Paragraphs[0];
			cell_1.InsertText("下载IP", false, null);
			invoice_ip.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
			invoice_ip.Rows[0].Cells[1].Width = 200;
			Paragraph cell_2 = invoice_ip.Rows[0].Cells[2].Paragraphs[0];
			cell_2.InsertText("下载日期", false, null);
			invoice_ip.Rows[0].Cells[2].Paragraphs[0].Alignment = Alignment.center;
			invoice_ip.Rows[0].Cells[2].Width = 200;
			int row_numip = 1;
			if (iplist != null)
			{
				for (i = 0; i < iplist.Count; i++)
				{
					r_c1 = invoice_ip.Rows[row_numip].Cells[0].Paragraphs[0];
					r_c1.InsertText(iplist[i].ResourceName, false, null);
					r_c2 = invoice_ip.Rows[row_numip].Cells[1].Paragraphs[0];
					r_c2.InsertText(iplist[i].Ip, false, null);
					invoice_ip.Rows[row_numip].Cells[1].Paragraphs[0].Alignment = Alignment.center;
					DateTime createTime = iplist[i].CreateTime;
					Paragraph r_c3 = invoice_ip.Rows[row_numip].Cells[2].Paragraphs[0];
					value = iplist[i].CreateTime;
					r_c3.InsertText(value.ToShortDateString().ToString(), false, null);
					invoice_ip.Rows[row_numip].Cells[2].Paragraphs[0].Alignment = Alignment.center;
					row_numip++;
				}
			}
			BarChart bc = new BarChart()
			{
				BarDirection = BarDirection.Column,
				BarGrouping = BarGrouping.Standard,
				GapWidth = 400
			};
			bc.AddLegend(ChartLegendPosition.Bottom, false);
			List<ChartData> company1 = ChartData.CreateCompanyList(LogDetailProvider.Instance.GetSumCount());
			Series s1 = new Series("素材")
			{
				Color = Color.Blue
			};
			s1.Bind(company1, "Name", "Count");
			bc.AddSeries(s1);
			template.InsertParagraph("医学库图表").FontSize(20);
			template.InsertChart(bc);
			return template;
		}

		private static DocX CreateFiratTempQualityCourse(DocX template, Guid schoolid)
		{
			int i;
			Paragraph r_c1;
			List<DownloadDetails> item;
			Paragraph r_c2;
			DateTime value;
			object[] tag2;
			School sh = SchoolProvider.Instance.GetEntity(schoolid);
			int CountQ = 0;
			string DayQ = "";
			int QCount = 0;
			string ResourceQ = "";
			int num = 2;
			DownloadDetails logQ = LogDetailProvider.Instance.GetMostDay(schoolid, 2, num.ToString());
			if (logQ != null)
			{
				CountQ = logQ.DownloadCount;
				DayQ = logQ.Tag2;
			}
			else
			{
				CountQ = 0;
				DayQ = "暂无";
			}
			num = 2;
			DownloadDetails logQB = LogDetailProvider.Instance.GetMostResourceQ(schoolid, 2, num.ToString());
			if (logQB != null)
			{
				QCount = logQB.DownloadCount;
				ResourceQ = logQB.Tag2;
			}
			else
			{
				QCount = 0;
				ResourceQ = "暂无";
			}
			num = 2;
			int BrowseCountQ = LogDetailProvider.Instance.GetCount(schoolid, 2, num.ToString());
			List<DownloadDetails> ips = LogDetailProvider.Instance.GetIpDetails(schoolid, 2, 2);
			num = 2;
			List<DownloadDetails> subZeroSQ = LogDetailProvider.Instance.GetSubZeroSQ(schoolid, 2, num.ToString());
			num = 2;
			List<DownloadDetails> subs = LogDetailProvider.Instance.GetSubOneSQ(schoolid, 2, num.ToString());
			List<DownloadDetails> mons = LogDetailProvider.Instance.GetMonthDetails(schoolid, 2, 2);
			num = 2;
			List<DownloadDetails> iplist = LogDetailProvider.Instance.GetListTrialreportSQ(schoolid, 2, num.ToString());
			if (sh != null)
			{
				if (sh.Name != null)
				{
					template.AddCustomProperty(new CustomProperty("shname", sh.Name));
				}
				if (sh.StartTime.HasValue)
				{
					value = sh.StartTime.Value;
					template.AddCustomProperty(new CustomProperty("shStartTime", value.ToLongDateString()));
					value = sh.StartTime.Value;
					template.AddCustomProperty(new CustomProperty("starttime  ", value.ToLongDateString()));
				}
			}
			value = DateTime.Now;
			template.AddCustomProperty(new CustomProperty("endtime", value.ToLongDateString()));
			template.AddCustomProperty(new CustomProperty("CountQ", CountQ));
			template.AddCustomProperty(new CustomProperty("DayQ", DayQ));
			template.AddCustomProperty(new CustomProperty("QCount", QCount));
			template.AddCustomProperty(new CustomProperty("ResourceQ", ResourceQ));
			template.AddCustomProperty(new CustomProperty("BrowseCountQ", BrowseCountQ));
			int rowcount = 0;
			int r_num = 0;
			int c_num = 0;
			if (ips != null)
			{
				rowcount = (ips.Count % 3 != 0 ? ips.Count / 3 + 1 : ips.Count / 3);
			}
			Table t = template.Tables[0];
			Table invoice_table = t.InsertTableAfterSelf(rowcount, 3);
			t.Remove();
			invoice_table.Design = TableDesign.TableGrid;
			invoice_table.Alignment = Alignment.center;
			if (ips != null)
			{
				for (i = 0; i < ips.Count; i++)
				{
					r_c1 = invoice_table.Rows[r_num].Cells[c_num].Paragraphs[0];
					tag2 = new object[] { ips[i].Tag2, "：", ips[i].DownloadCount, "次" };
					r_c1.InsertText(string.Concat(tag2), false, null);
					invoice_table.Rows[r_num].Cells[c_num].Width = 200;
					c_num++;
					if ((i + 1) % 3 == 0)
					{
						r_num++;
						c_num = 0;
					}
				}
			}
			Formatting formmatting = new Formatting()
			{
				FontColor = new Color?(Color.Blue)
			};
			int rowcount_one = 0;
			int r_numo = 0;
			int itemcount = 0;
			if ((subs == null ? false : subZeroSQ != null))
			{
				for (int j = 0; j < subZeroSQ.Count; j++)
				{
					item = (
						from p in subs
						where p.Id == subZeroSQ[j].Id
						select p).ToList<DownloadDetails>();
					itemcount = (item.Count % 4 != 0 ? itemcount + item.Count / 4 + 1 : itemcount + item.Count / 4);
				}
			}
			rowcount_one = itemcount + subZeroSQ.Count;
			Table t1 = template.Tables[1];
			Table invoice_table1 = t1.InsertTableAfterSelf(rowcount_one, 4);
			t1.Remove();
			invoice_table1.Design = TableDesign.None;
			invoice_table1.Alignment = Alignment.center;
			if ((subs == null ? false : subZeroSQ != null))
			{
				for (int k = 0; k < subZeroSQ.Count; k++)
				{
					item = (
						from p in subs
						where p.Id == subZeroSQ[k].Id
						select p).ToList<DownloadDetails>();
					if (item != null)
					{
						if (item.Count > 0)
						{
							r_c1 = invoice_table1.Rows[r_numo].Cells[0].Paragraphs[0];
							tag2 = new object[] { subZeroSQ[k].Tag2, "：", subZeroSQ[k].DownloadCount, "次" };
							r_c1.InsertText(string.Concat(tag2), false, formmatting);
							invoice_table1.Rows[r_numo].Height = 20;
							invoice_table1.Rows[r_numo].MergeCells(0, 3);
							r_numo++;
							int c_numo = 0;
							for (int s = 0; s < item.Count; s++)
							{
								r_c2 = invoice_table1.Rows[r_numo].Cells[c_numo].Paragraphs[0];
								tag2 = new object[] { item[s].Tag2, "：", item[s].DownloadCount, "次" };
								r_c2.InsertText(string.Concat(tag2), false, null);
								c_numo++;
								if ((s + 1) % 4 == 0)
								{
									if (s + 1 != item.Count)
									{
										r_numo++;
										c_numo = 0;
									}
								}
							}
							r_numo++;
						}
					}
				}
			}
			int rowcount_two = 0;
			int r_numt = 0;
			int c_numt = 0;
			if (mons != null)
			{
				rowcount_two = (mons.Count % 3 != 0 ? mons.Count / 3 + 1 : mons.Count / 3);
			}
			Table t2 = template.Tables[2];
			Table invoice_table2 = t2.InsertTableAfterSelf(rowcount_two, 3);
			t2.Remove();
			invoice_table2.Design = TableDesign.None;
			if (mons != null)
			{
				for (i = 0; i < mons.Count; i++)
				{
					r_c1 = invoice_table2.Rows[r_numt].Cells[c_numt].Paragraphs[0];
					tag2 = new object[] { mons[i].Tag2, "：", mons[i].DownloadCount, "次" };
					r_c1.InsertText(string.Concat(tag2), false, null);
					c_numt++;
					if ((i + 1) % 3 == 0)
					{
						r_numt++;
						c_numt = 0;
					}
				}
			}
			Table t3 = template.Tables[3];
			Table invoice_ip = t3.InsertTableAfterSelf(iplist.Count + 1, 3);
			t3.Remove();
			invoice_ip.Design = TableDesign.TableGrid;
			invoice_ip.Alignment = Alignment.center;
			Paragraph cell_0 = invoice_ip.Rows[0].Cells[0].Paragraphs[0];
			cell_0.InsertText("资源名称", false, null);
			invoice_ip.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
			invoice_ip.Rows[0].Cells[0].Width = 200;
			Paragraph cell_1 = invoice_ip.Rows[0].Cells[1].Paragraphs[0];
			cell_1.InsertText("下载IP", false, null);
			invoice_ip.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
			invoice_ip.Rows[0].Cells[1].Width = 200;
			Paragraph cell_2 = invoice_ip.Rows[0].Cells[2].Paragraphs[0];
			cell_2.InsertText("下载日期", false, null);
			invoice_ip.Rows[0].Cells[2].Paragraphs[0].Alignment = Alignment.center;
			invoice_ip.Rows[0].Cells[2].Width = 200;
			int row_numip = 1;
			if (iplist != null)
			{
				for (i = 0; i < iplist.Count; i++)
				{
					r_c1 = invoice_ip.Rows[row_numip].Cells[0].Paragraphs[0];
					r_c1.InsertText(iplist[i].ResourceName, false, null);
					r_c2 = invoice_ip.Rows[row_numip].Cells[1].Paragraphs[0];
					r_c2.InsertText(iplist[i].Ip, false, null);
					invoice_ip.Rows[row_numip].Cells[1].Paragraphs[0].Alignment = Alignment.center;
					DateTime createTime = iplist[i].CreateTime;
					Paragraph r_c3 = invoice_ip.Rows[row_numip].Cells[2].Paragraphs[0];
					value = iplist[i].CreateTime;
					r_c3.InsertText(value.ToShortDateString().ToString(), false, null);
					invoice_ip.Rows[row_numip].Cells[2].Paragraphs[0].Alignment = Alignment.center;
					row_numip++;
				}
			}
			BarChart bc = new BarChart()
			{
				BarDirection = BarDirection.Column,
				BarGrouping = BarGrouping.Standard,
				GapWidth = 400
			};
			bc.AddLegend(ChartLegendPosition.Bottom, false);
			List<ChartData> company1 = ChartData.CreateCompanyList(LogDetailProvider.Instance.GetSumCountQC());
			Series s1 = new Series("题量")
			{
				Color = Color.Blue
			};
			s1.Bind(company1, "Name", "Count");
			bc.AddSeries(s1);
			template.InsertParagraph("精品课程图表").FontSize(20);
			template.InsertChart(bc);
			return template;
		}

		private static DocX CreateFiratTempTest(DocX template, Guid schoolid)
		{
			int TCount;
			string ResourceT;
			int i;
			Paragraph r_c1;
			DateTime value;
			object[] tag2;
			School sh = SchoolProvider.Instance.GetEntity(schoolid);
			int num = 3;
			int num1 = 3;
			DownloadDetails logT = LogDetailProvider.Instance.GetMostResourceT(schoolid, 0, num1.ToString(), num);
			if (logT != null)
			{
				TCount = logT.DownloadCount;
				ResourceT = logT.Tag2;
			}
			else
			{
				TCount = 0;
				ResourceT = "暂无";
			}
			num1 = 3;
			int BrowseCountT = LogDetailProvider.Instance.GetCountTest(schoolid, 0, num1.ToString(), num);
			List<DownloadDetails> ips = LogDetailProvider.Instance.GetIpDetailsTestBrowse(schoolid, 0, 3, num);
			num1 = 3;
			List<DownloadDetails> cates = LogDetailProvider.Instance.GetTestSubjectDetails(schoolid, 0, num1.ToString(), num);
			List<DownloadDetails> mons = LogDetailProvider.Instance.GetMonthDetailsTest(schoolid, 0, 3, num);
			num1 = 3;
			List<DownloadDetails> iplist = LogDetailProvider.Instance.GetListTrialreportTest(schoolid, 0, num1.ToString());
			if (sh != null)
			{
				if (sh.Name != null)
				{
					template.AddCustomProperty(new CustomProperty("shname", sh.Name));
				}
				if (sh.StartTime.HasValue)
				{
					value = sh.StartTime.Value;
					template.AddCustomProperty(new CustomProperty("shStartTime", value.ToLongDateString()));
					value = sh.StartTime.Value;
					template.AddCustomProperty(new CustomProperty("starttime  ", value.ToLongDateString()));
				}
			}
			value = DateTime.Now;
			template.AddCustomProperty(new CustomProperty("endtime", value.ToLongDateString()));
			template.AddCustomProperty(new CustomProperty("TCount", TCount));
			template.AddCustomProperty(new CustomProperty("ResourceT", ResourceT));
			template.AddCustomProperty(new CustomProperty("BrowseCountT", BrowseCountT));
			int rowcount = 0;
			int r_num = 0;
			int c_num = 0;
			if (ips != null)
			{
				rowcount = (ips.Count % 3 != 0 ? ips.Count / 3 + 1 : ips.Count / 3);
			}
			Table t = template.Tables[0];
			Table invoice_table = t.InsertTableAfterSelf(rowcount, 3);
			t.Remove();
			invoice_table.Design = TableDesign.TableGrid;
			invoice_table.Alignment = Alignment.center;
			if (ips != null)
			{
				for (i = 0; i < ips.Count; i++)
				{
					r_c1 = invoice_table.Rows[r_num].Cells[c_num].Paragraphs[0];
					tag2 = new object[] { ips[i].Tag2, "：", ips[i].DownloadCount, "次" };
					r_c1.InsertText(string.Concat(tag2), false, null);
					invoice_table.Rows[r_num].Cells[c_num].Width = 200;
					c_num++;
					if ((i + 1) % 3 == 0)
					{
						r_num++;
						c_num = 0;
					}
				}
			}
			Formatting formmatting = new Formatting()
			{
				FontColor = new Color?(Color.Blue)
			};
			int r_numo = 0;
			Table t1 = template.Tables[1];
			if (cates != null)
			{
				rowcount = (cates.Count % 4 != 0 ? cates.Count / 4 + 1 : cates.Count / 4);
			}
			Table invoice_table1 = t1.InsertTableAfterSelf(rowcount, 4);
			t1.Remove();
			invoice_table1.Design = TableDesign.None;
			invoice_table1.Alignment = Alignment.center;
			int c_numo = 0;
			for (int c = 0; c < cates.Count; c++)
			{
				r_c1 = invoice_table1.Rows[r_numo].Cells[c_numo].Paragraphs[0];
				tag2 = new object[] { cates[c].Tag2, "：", cates[c].DownloadCount, "次" };
				r_c1.InsertText(string.Concat(tag2), false, formmatting);
				invoice_table1.Rows[r_numo].Height = 20;
				c_numo++;
				if ((c + 1) % 4 == 0)
				{
					r_numo++;
					c_numo = 0;
				}
			}
			int rowcount_two = 0;
			int r_numt = 0;
			int c_numt = 0;
			if (mons != null)
			{
				rowcount_two = (mons.Count % 3 != 0 ? mons.Count / 3 + 1 : mons.Count / 3);
			}
			Table t2 = template.Tables[2];
			Table invoice_table2 = t2.InsertTableAfterSelf(rowcount_two, 3);
			t2.Remove();
			invoice_table2.Design = TableDesign.None;
			if (mons != null)
			{
				for (i = 0; i < mons.Count; i++)
				{
					r_c1 = invoice_table2.Rows[r_numt].Cells[c_numt].Paragraphs[0];
					tag2 = new object[] { mons[i].Tag2, "：", mons[i].DownloadCount, "次" };
					r_c1.InsertText(string.Concat(tag2), false, null);
					c_numt++;
					if ((i + 1) % 3 == 0)
					{
						r_numt++;
						c_numt = 0;
					}
				}
			}
			Table t3 = template.Tables[3];
			Table invoice_ip = t3.InsertTableAfterSelf(iplist.Count + 1, 3);
			t3.Remove();
			invoice_ip.Design = TableDesign.TableGrid;
			invoice_ip.Alignment = Alignment.center;
			Paragraph cell_0 = invoice_ip.Rows[0].Cells[0].Paragraphs[0];
			cell_0.InsertText("资源名称", false, null);
			invoice_ip.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
			invoice_ip.Rows[0].Cells[0].Width = 200;
			Paragraph cell_1 = invoice_ip.Rows[0].Cells[1].Paragraphs[0];
			cell_1.InsertText("下载IP", false, null);
			invoice_ip.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
			invoice_ip.Rows[0].Cells[1].Width = 200;
			Paragraph cell_2 = invoice_ip.Rows[0].Cells[2].Paragraphs[0];
			cell_2.InsertText("下载日期", false, null);
			invoice_ip.Rows[0].Cells[2].Paragraphs[0].Alignment = Alignment.center;
			invoice_ip.Rows[0].Cells[2].Width = 200;
			int row_numip = 1;
			if (iplist != null)
			{
				for (i = 0; i < iplist.Count; i++)
				{
					r_c1 = invoice_ip.Rows[row_numip].Cells[0].Paragraphs[0];
					r_c1.InsertText(iplist[i].ResourceName, false, null);
					Paragraph r_c2 = invoice_ip.Rows[row_numip].Cells[1].Paragraphs[0];
					r_c2.InsertText(iplist[i].Ip, false, null);
					invoice_ip.Rows[row_numip].Cells[1].Paragraphs[0].Alignment = Alignment.center;
					DateTime createTime = iplist[i].CreateTime;
					Paragraph r_c3 = invoice_ip.Rows[row_numip].Cells[2].Paragraphs[0];
					value = iplist[i].CreateTime;
					r_c3.InsertText(value.ToShortDateString().ToString(), false, null);
					invoice_ip.Rows[row_numip].Cells[2].Paragraphs[0].Alignment = Alignment.center;
					row_numip++;
				}
			}
			BarChart bc = new BarChart()
			{
				BarDirection = BarDirection.Column,
				BarGrouping = BarGrouping.Standard,
				GapWidth = 400
			};
			bc.AddLegend(ChartLegendPosition.Bottom, false);
			List<ChartData> company1 = ChartData.CreateCompanyList(LogDetailProvider.Instance.GetSumCountTest());
			Series s1 = new Series("题量")
			{
				Color = Color.Blue
			};
			s1.Bind(company1, "Name", "Count");
			bc.AddSeries(s1);
			template.InsertParagraph("医学库图表").FontSize(20);
			template.InsertChart(bc);
			return template;
		}

		public JsonResult DeleteFacDep(Guid id)
		{
			JsonResult jsonResult;
			int count = UserInfoProvider.Instance.GetCountByFacDepId(id);
			if (count <= 0)
			{
				SchoolProvider.Instance.DeleteFacDepByFacId(id);
				jsonResult = base.Json(new { State = 1, Msg = "删除成功！", Result = "" });
			}
			else
			{
				jsonResult = base.Json(new { State = 0, Msg = string.Concat("该院系下有“", count, "”个教师，无法删除！"), Result = "" });
			}
			return jsonResult;
		}

		public JsonResult DeleteSchool(Guid id)
		{
			JsonResult jsonResult;
			int i = UserInfoProvider.Instance.GetListBySchoolId(id).Count;
			if (i <= 0)
			{
				SchoolProvider.Instance.DeleteSchool(id);
				School_ModuleProvider.Instance.DeleteBySchoolId(id);
				IpProvider.Instance.DeleteBySchoolId(id);
				UserInfoProvider.Instance.DeleteBySchoolId(id);
				jsonResult = base.Json(new { State = 1, Msg = "删除成功！", Result = "" });
			}
			else
			{
				jsonResult = base.Json(new { State = 0, Msg = string.Concat("该学校有", i, "个帐号，无法删除！"), Result = "" });
			}
			return jsonResult;
		}

		public JsonResult DeleteTeacher(Guid id)
		{
			JsonResult jsonResult;
			if (!(id == base.CurrentAdmin.Id))
			{
				UserInfoProvider.Instance.Delete(id);
				User_RoleProvider.Instance.DeleteByUserId(id);
				jsonResult = base.Json(new { State = 1, Msg = "删除成功!", Result = "" });
			}
			else
			{
				jsonResult = base.Json(new { State = 0, Msg = "无法删除自己!", Result = "" });
			}
			return jsonResult;
		}

		public JsonResult delmenu(Guid menuid)
		{
			Menu r = MenuProvider.Instance.GetEntity(menuid);
			r.IsDeleted = true;
			int state = 0;
			if (MenuProvider.Instance.Update(r) > 0)
			{
				state = 1;
			}
			JsonResult jsonResult = base.Json(new { State = state, Msg = "删除成功！", Result = "" });
			return jsonResult;
		}

		public JsonResult delrole(Guid roleid)
		{
			Role r = RoleProvider.Instance.GetEntity(roleid);
			r.IsDeleted = true;
			int state = 0;
			if (RoleProvider.Instance.Update(r) > 0)
			{
				state = 1;
			}
			JsonResult jsonResult = base.Json(new { State = state, Msg = "删除成功！", Result = "" });
			return jsonResult;
		}

		public JsonResult delsell(Guid sellid)
		{
			UserInfo r = UserInfoProvider.Instance.GetEntity(sellid);
			r.IsDeleted = true;
			int state = 0;
			if (UserInfoProvider.Instance.Update(r) > 0)
			{
				state = 1;
			}
			JsonResult jsonResult = base.Json(new { State = state, Msg = "删除成功！", Result = "" });
			return jsonResult;
		}

		public JsonResult DepAddXls(Guid? schoolid)
		{
			List<object> list = new List<object>()
			{
				new { UserName = base.CurrentAdmin.Name, SchoolId = (!schoolid.HasValue ? new Guid?(UserInfoProvider.Instance.GetEntity(base.CurrentAdmin.Id).SchoolId) : schoolid) }
			};
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "ok", Result = list });
			return jsonResult;
		}

		public JsonResult Department()
		{
			bool flag;
			List<object> list = new List<object>();
			UserInfo admin = UserInfoProvider.Instance.GetEntity(base.CurrentAdmin.Id);
			string dis = "";
			List<SelectListItem> School = null;
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
				dis = "style='display:none'";
				Guid schoolId = admin.SchoolId;
				School = SystemManageController.HtmlSchool(admin.SchoolId);
			}
			else
			{
				dis = "";
				Guid guid = admin.SchoolId;
				School = SystemManageController.HtmlSchools(admin.SchoolId);
			}
			list.Add(new { Id = admin.Id, UserName = admin.Name, SchoolId = admin.SchoolId, dis = dis, School = School, count = SchoolProvider.Instance.GetFacDepCount(admin.SchoolId, "", 1) });
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "ok", Result = list });
			return jsonResult;
		}

		public JsonResult DownException()
		{
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = "" });
			return jsonResult;
		}

		public JsonResult EditChild(Guid id, string title, string url)
		{
			JsonResult jsonResult;
			if (string.IsNullOrEmpty(title))
			{
				jsonResult = base.Json(new { State = 0, Msg = "节点标题不能为空！", Result = "" });
			}
			else if (!string.IsNullOrEmpty(url))
			{
				Menu child = MenuProvider.Instance.GetInfo(id);
				child.Title = title;
				child.Url = url;
				MenuProvider.Instance.Update(child);
				jsonResult = base.Json(new { State = 1, Msg = "修改成功！", Result = "" });
			}
			else
			{
				jsonResult = base.Json(new { State = 0, Msg = "页面地址不能为空！", Result = "" });
			}
			return jsonResult;
		}

		public JsonResult EditFacDep(Guid id, string name)
		{
			JsonResult jsonResult;
			if (name.Trim().Length <= 0)
			{
				jsonResult = base.Json(new { State = 0, Msg = "院系名称为空！", Result = "" });
			}
			else
			{
				School dep = SchoolProvider.Instance.GetEntity(id);
				dep.Name = name;
				SchoolProvider.Instance.Update(dep);
				jsonResult = base.Json(new { State = 1, Msg = "修改成功！", Result = "" });
			}
			return jsonResult;
		}

		public JsonResult EditInfo(Guid id, string name, string sex, Guid facultyid, Guid departmentid, Guid job, Guid post, Guid profession, string birthday, string email, string phone)
		{
			UserInfo admin = UserInfoProvider.Instance.GetUser(id);
			admin.Name = name;
			admin.Sex = sex;
			if (facultyid != Guid.Empty)
			{
				admin.FacultyId = new Guid?(facultyid);
			}
			if (departmentid != Guid.Empty)
			{
				admin.DepartmentId = new Guid?(departmentid);
			}
			if (!string.IsNullOrEmpty(birthday))
			{
				admin.Birthday = new DateTime?(Convert.ToDateTime(birthday));
			}
			admin.Email = email;
			admin.Phone = phone;
			admin.JobId = new Guid?(job);
			admin.ProfessionId = new Guid?(profession);
			admin.PostId = new Guid?(post);
			UserInfoProvider.Instance.Update(admin);
			base.CurrentAdmin.Name = name;
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "修改成功！", Result = "" });
			return jsonResult;
		}

		public JsonResult EditIp(Guid id, string ips)
		{
			JsonResult jsonResult;
			string str = "";
			foreach (string item in IpHelper.IpToList(ips.Trim()))
			{
				if (!IpHelper.CheckIp(item))
				{
					str = string.Concat(str, item, "<br />");
				}
			}
			if (string.IsNullOrEmpty(str))
			{
				School school = SchoolProvider.Instance.GetEntity(id);
				IpProvider.Instance.DeleteBySchoolId(id);
				char[] chrArray = new char[] { ',' };
				string[] strArrays = ips.Split(chrArray, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < (int)strArrays.Length; i++)
				{
					string ip = strArrays[i];
					Ip p = new Ip()
					{
						SchoolId = school.Id,
						CreateTime = DateTime.Now,
						IsDeleted = false
					};
					if (!ip.Contains<char>('-'))
					{
						p.IpStart = IpHelper.ParseByte(ip.Trim());
						p.IpEnd = IpHelper.ParseByte(ip.Trim());
					}
					else
					{
						chrArray = new char[] { '-' };
						string[] temp = ip.Split(chrArray, StringSplitOptions.RemoveEmptyEntries);
						p.IpStart = IpHelper.ParseByte(temp[0].Trim());
						p.IpEnd = IpHelper.ParseByte(temp[1].Trim());
					}
					Ip ps = IpProvider.Instance.GetEntity(p.IpStart);
					Ip pe = IpProvider.Instance.GetEntity(p.IpEnd);
					if ((ps != null ? true : pe != null))
					{
						str = string.Concat(str, ip, "已存在！<br />");
					}
					else
					{
						IpProvider.Instance.Create(p);
					}
				}
				jsonResult = (string.IsNullOrEmpty(str) ? base.Json(new { State = 1, Msg = "修改成功！", Result = "" }) : base.Json(new { State = 0, Msg = str, Result = "" }));
			}
			else
			{
				jsonResult = base.Json(new { State = 0, Msg = string.Concat(str, "不是合法IP"), Result = "" });
			}
			return jsonResult;
		}

		public JsonResult EditManager(Guid id)
		{
			bool flag;
			string str;
			List<object> list = new List<object>();
			Guid userRole = Guid.Empty;
			UserInfo user = UserInfoProvider.Instance.GetUser(id);
			User_Role uur = User_RoleProvider.Instance.GetEntityByUserId(user.Id);
			if (uur != null)
			{
				userRole = uur.RoleId;
			}
			int? role = user.Type;
			Enums.UserType type = (Enums.UserType)role.Value;
			List<SelectListItem> School = null;
			role = base.CurrentAdmin.Role;
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
				Guid schoolId = user.SchoolId;
				School = SystemManageController.HtmlSchool(user.SchoolId);
			}
			else
			{
				Guid guid = user.SchoolId;
				School = SystemManageController.HtmlSchools(user.SchoolId);
			}
			List<object> objs = list;
			UserInfo userInfo = user;
			str = (!user.Birthday.HasValue ? "" : user.Birthday.Value.ToLongDateString());
			string longDateString = user.CreateTime.ToLongDateString();
			string name = base.CurrentAdmin.Name;
			string name1 = type.GetName();
			Guid schoolId1 = user.SchoolId;
			List<SelectListItem> selectListItems = SystemManageController.HtmlFaculty(user.SchoolId, (user.FacultyId.HasValue ? user.FacultyId.Value : Guid.Empty));
			List<SelectListItem> selectListItems1 = School;
			Guid guid1 = user.SchoolId;
			objs.Add(new { user = userInfo, Birthday = str, CreateTime = longDateString, UserName = name, UserType = name1, Faculty = selectListItems, School = selectListItems1, Role = SystemManageController.HtmlRole(user.SchoolId, userRole), Profession = SystemManageController.HtmlProfession((user.ProfessionId.HasValue ? user.ProfessionId.Value : Guid.Empty)), Post = SystemManageController.HtmlPost((user.PostId.HasValue ? user.PostId.Value : Guid.Empty)), Job = SystemManageController.HtmlJob((user.JobId.HasValue ? user.JobId.Value : Guid.Empty)), Department = SystemManageController.HtmlDepartment((user.FacultyId.HasValue ? user.FacultyId.Value : Guid.Empty), (user.DepartmentId.HasValue ? user.DepartmentId.Value : Guid.Empty)) });
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "ok", Result = list });
			return jsonResult;
		}

		public JsonResult EditManagerInfo(Guid id, string name, string sex, Guid schoolid, Guid facultyid, Guid departmentid, Guid role, Guid? job, Guid post, Guid? profession, string birthday, string email, string phone)
		{
			UserInfo admin = UserInfoProvider.Instance.GetUser(id);
			admin.Name = name;
			admin.Sex = sex;
			admin.SchoolId = schoolid;
			if (facultyid != Guid.Empty)
			{
				admin.FacultyId = new Guid?(facultyid);
			}
			if (departmentid != Guid.Empty)
			{
				admin.DepartmentId = new Guid?(departmentid);
			}
			if (!string.IsNullOrEmpty(birthday))
			{
				admin.Birthday = new DateTime?(Convert.ToDateTime(birthday));
			}
			User_Role uur = User_RoleProvider.Instance.GetEntityByUserId(id);
			if (uur != null)
			{
				uur.RoleId = role;
				User_RoleProvider.Instance.Update(uur);
			}
			else
			{
				User_Role ur = new User_Role()
				{
					UserId = id,
					RoleId = role,
					IsCustom = false,
					IsDeleted = false,
					CreateTime = DateTime.Now
				};
				User_RoleProvider.Instance.Create(ur);
			}
			admin.Email = email;
			admin.Phone = phone;
			admin.JobId = job;
			admin.ProfessionId = profession;
			admin.PostId = new Guid?(post);
			UserInfoProvider.Instance.Update(admin);
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "修改成功！", Result = "" });
			return jsonResult;
		}

		public JsonResult EditModules(Guid[] ids)
		{
			School_ModuleProvider.Instance.EditBySchoolId(base.CurrentAdmin.SchoolId);
			if (ids != null)
			{
				Guid[] guidArray = ids;
				for (int i = 0; i < (int)guidArray.Length; i++)
				{
					Guid id = guidArray[i];
					School_ModuleProvider.Instance.EditBySchoolIdModuleId(base.CurrentAdmin.SchoolId, id);
				}
			}
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "修改成功", Result = "" });
			return jsonResult;
		}

		public JsonResult EditPwd()
		{
			ViewBag.Id = base.CurrentAdmin.Id;
			ViewBag.UserName = base.CurrentAdmin.Name;
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "ok", Result = "" });
			return jsonResult;
		}

		public JsonResult EditReg(int reg)
		{
			SystemInfo sys = SystemInfoProvider.Instance.GetList()[0];
			sys.IsReg = new bool?((reg == 0 ? true : false));
			SystemInfoProvider.Instance.Update(sys);
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "修改成功", Result = sys.IsReg });
			return jsonResult;
		}

		public JsonResult EditRoot(Guid id, int type, string title, Guid? module)
		{
			JsonResult jsonResult;
			if (!string.IsNullOrEmpty(title))
			{
				Menu root = MenuProvider.Instance.GetInfo(id);
				root.Title = title;
				int? nullable = root.Type;
				if (nullable != type )
				{
					root.Type = new int?(type);
					MenuProvider.Instance.Update(id, type);
				}
				root.ModuleId = module;
				MenuProvider.Instance.Update(root);
				jsonResult = base.Json(new { State = 1, Msg = "修改成功！", Result = "" });
			}
			else
			{
				jsonResult = base.Json(new { State = 0, Msg = "节点标题不能为空！", Result = "" });
			}
			return jsonResult;
		}

		public JsonResult EditSchool(Guid id)
		{
			List<object> list = new List<object>();
			School school = SchoolProvider.Instance.GetEntity(id);
			UserInfoProvider.Instance.GetUser(school.SellId);
			List<object> objs = list;
			Guid guid = school.Id;
			string name = school.Name;
			string str = base.CurrentAdmin.Name;
			List<SelectListItem> selectListItems = SystemManageController.HtmlSeller(school.SellId);
			int? trialType = school.TrialType;
			objs.Add(new { Id = guid, Name = name, UserName = str, Sell = selectListItems, Trial = SystemManageController.HtmlTrialType(trialType.Value), StartTime = school.CreateTime.ToLongDateString(), EndTime = (!school.EndTime.HasValue ? "" : school.EndTime.Value.ToLongDateString()), StopRemind = school.StopRemind, Privince = SystemManageController.HtmlProvince(school.ProvinceId) });
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "修改成功", Result = list });
			return jsonResult;
		}

		public JsonResult EditSchoolName(Guid id, string name)
		{
			JsonResult jsonResult;
			School school = SchoolProvider.Instance.GetEntity(id);
			if (name.Trim().Length <= 0)
			{
				jsonResult = base.Json(new { State = 1, Msg = "名称不能为空！", Result = "" });
			}
			else
			{
				school.Name = name;
				SchoolProvider.Instance.Update(school);
				jsonResult = base.Json(new { State = 1, Msg = "修改成功！", Result = "" });
			}
			return jsonResult;
		}

		[HttpPost]
		public JsonResult EditSchools(Guid id, string schoolname, List<School_Module> ids, string ips, string end, string remind, Guid sellid, int trial, Guid province)
		{
			JsonResult jsonResult;
			string str = "";
			foreach (string item in IpHelper.IpToList(ips.Trim()))
			{
				if (!IpHelper.CheckIp(item))
				{
					str = string.Concat(str, item, "<br />");
				}
			}
			if (string.IsNullOrEmpty(str))
			{
				School school = SchoolProvider.Instance.GetEntity(id);
				school.Name = schoolname.Trim();
				school.StopRemind = remind.Trim();
				school.ModifyTime = new DateTime?(DateTime.Now);
				school.EndTime = new DateTime?(Convert.ToDateTime(end));
				school.SellId = sellid;
				school.TrialType = new int?(trial);
				school.ProvinceId = province;
				SchoolProvider.Instance.Update(school);
				School_ModuleProvider.Instance.DeleteBySchoolId(id);
				if (ids != null)
				{
					for (int i = 0; i < ids.Count; i++)
					{
						ids[i].Id = Guid.NewGuid();
						ids[i].CreateTime = DateTime.Now;
						ids[i].IsDeleted = false;
						ids[i].SchoolId = school.Id;
						School_ModuleProvider.Instance.Create(ids[i]);
					}
				}
				IpProvider.Instance.DeleteBySchoolId(id);
				string str1 = ips.Trim();
				char[] chrArray = new char[] { ',' };
				string[] strArrays = str1.Split(chrArray, StringSplitOptions.RemoveEmptyEntries);
				for (int j = 0; j < (int)strArrays.Length; j++)
				{
					string ip = strArrays[j];
					Ip p = new Ip()
					{
						SchoolId = school.Id,
						CreateTime = DateTime.Now,
						IsDeleted = false
					};
					if (!ip.Contains<char>('-'))
					{
						p.IpStart = IpHelper.ParseByte(ip.Trim());
						p.IpEnd = IpHelper.ParseByte(ip.Trim());
					}
					else
					{
						chrArray = new char[] { '-' };
						string[] temp = ip.Split(chrArray, StringSplitOptions.RemoveEmptyEntries);
						p.IpStart = IpHelper.ParseByte(temp[0].Trim());
						p.IpEnd = IpHelper.ParseByte(temp[1].Trim());
					}
					p.Tier = school.Tier;
					Ip ps = IpProvider.Instance.GetEntity(p.IpStart);
					Ip pe = IpProvider.Instance.GetEntity(p.IpEnd);
					if ((ps != null ? true : pe != null))
					{
						str = string.Concat(str, ip, "已存在！<br />");
					}
					else
					{
						IpProvider.Instance.Create(p);
					}
				}
				jsonResult = (string.IsNullOrEmpty(str) ? base.Json(new { State = 1, Msg = "修改成功！", Result = "" }) : base.Json(new { State = 0, Msg = str, Result = "" }));
			}
			else
			{
				jsonResult = base.Json(new { State = 0, Msg = string.Concat(str, "不是合法IP"), Result = "" });
			}
			return jsonResult;
		}

		public JsonResult EditUserPwd(string oldpwd, string newpwd)
		{
			JsonResult jsonResult;
			UserInfo admin = UserInfoProvider.Instance.GetEntity(base.CurrentAdmin.Id);
			if (!(Encrypt.GetMD5(oldpwd).ToLower() != admin.LoginPwd.ToLower()))
			{
				admin.LoginPwd = Encrypt.GetMD5(newpwd);
				UserInfoProvider.Instance.Update(admin);
				jsonResult = base.Json(new { State = 1, Msg = "修改成功！", Result = "" });
			}
			else
			{
				jsonResult = base.Json(new { State = 0, Msg = "原始密码不正确！", Result = "" });
			}
			return jsonResult;
		}

		public JsonResult EditValid(int val)
		{
			SystemInfo sys = SystemInfoProvider.Instance.GetList()[0];
			sys.IsValid = new bool?((val == 0 ? true : false));
			SystemInfoProvider.Instance.Update(sys);
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "修改成功", Result = val });
			return jsonResult;
		}

		public ViewResult Exam()
		{
			return base.View();
		}

		public JsonResult Exception(int Tier)
		{
			JsonResult jsonResult;
			var list = 
				from p in SchoolProvider.Instance.GetSchoolDownExceptionAll()
				select new { Id = p.Id, Name = p.Name, Count = LogDetailProvider.Instance.GetCounts(p.Id, 2, "0"), Daycountnum = LogDetailProvider.Instance.GetDaycountnum(p.Id, 2, "0"), Datatime = LogDetailProvider.Instance.GetDaycountData(p.Id, 2, "0"), StopRemind = p.StopRemind, SellName = (p.SellName == null ? "" : p.SellName), TrialType = (p.TrialType == 0 ? "试用用户" : "镜像用户") };
			if (Tier == 0)
			{
				var list1 = 
					from q in list
					where q.Daycountnum > 500
					select new { Id = q.Id, Name = q.Name, Count = q.Count, Daycountnum = q.Daycountnum, Datatime = q.Datatime, StopRemind = q.StopRemind, SellName = q.SellName, TrialType = q.TrialType };
				jsonResult = base.Json(new { State = 1, Msg = "", Result = list1 });
			}
			else if (Tier != 1)
			{
				jsonResult = base.Json(new { State = 1, Msg = "", Result = list });
			}
			else
			{
				var list2 = 
					from q in list
					where q.Daycountnum > 1000
					select new { Id = q.Id, Name = q.Name, Count = q.Count, Daycountnum = q.Daycountnum, Datatime = q.Datatime, StopRemind = q.StopRemind, SellName = q.SellName, TrialType = q.TrialType };
				jsonResult = base.Json(new { State = 1, Msg = "", Result = list2 });
			}
			return jsonResult;
		}

		public JsonResult GetDetailsList(Guid id, int pageIndex, string tag)
		{
			
			JsonResult jsonResult;
			List<DownloadDetails> downloadDetails;
			List<DownloadDetails> logList = new List<DownloadDetails>();
			if (tag == 0.ToString())
			{
				logList = LogDetailProvider.Instance.GetListSR(id, 2, pageIndex, 30, tag);
			}
			else if (tag == 1.ToString())
			{
				logList = LogDetailProvider.Instance.GetListSM(id, 2, pageIndex, 30, tag);
			}
			else if (tag == 3.ToString())
			{
				logList = LogDetailProvider.Instance.GetListST(id, 2, pageIndex, 30, tag);
			}
			else if (!(tag == 4.ToString()))
			{
				if (tag == 2.ToString())
				{
					logList = LogDetailProvider.Instance.GetListSQ(id, 2, pageIndex, 30, tag);
					downloadDetails = logList;
					var list1 = 
						from p in downloadDetails
						select new { Id = p.ResourceId, ResourceName = p.ResourceName, TeacherLoginName = (string.IsNullOrEmpty(p.TeacherLoginName) ? "" : p.TeacherLoginName), TeacherName = (string.IsNullOrEmpty(p.TeacherName) ? "" : p.TeacherName), Ip = p.Ip, CreateTime = string.Concat(p.CreateTime.ToShortDateString(), " ", p.CreateTime.ToLongTimeString()) };
					jsonResult = base.Json(new { State = 1, Msg = "", Result = list1 });
					return jsonResult;
				}
				jsonResult = base.Json(new { State = 0, Msg = "参数错误！", Result = "" });
				return jsonResult;
			}
			else
			{
				logList = LogDetailProvider.Instance.GetListSE(id, 2, pageIndex, 30, tag);
			}
			downloadDetails = logList;
			var list = 
				from p in downloadDetails
				select new { Id = p.ResourceId, ResourceName = p.ResourceName, TeacherLoginName = (string.IsNullOrEmpty(p.TeacherLoginName) ? "" : p.TeacherLoginName), TeacherName = (string.IsNullOrEmpty(p.TeacherName) ? "" : p.TeacherName), Ip = p.Ip, CreateTime = string.Concat(p.CreateTime.ToShortDateString(), " ", p.CreateTime.ToLongTimeString()) };
			jsonResult = base.Json(new { State = 1, Msg = "", Result = list });
			return jsonResult;
		}

		public ActionResult GetDetailsListTxt(Guid id, string tag, int status, string str, DateTime start, DateTime end)
		{
			ActionResult actionResult;
			List<DownloadDetails> logList = new List<DownloadDetails>();
			if (tag == 0.ToString())
			{
				logList = LogDetailProvider.Instance.GetListSR(id, status, 1, 999999, tag);
			}
			else if (tag == 1.ToString())
			{
				logList = LogDetailProvider.Instance.GetListSM(id, status, 1, 999999, tag);
			}
			else if (tag == 3.ToString())
			{
				logList = LogDetailProvider.Instance.GetListST(id, status, 1, 999999, tag);
			}
			else if (!(tag == 4.ToString()))
			{
				if (tag == 2.ToString())
				{
					goto Label1;
				}
				actionResult = base.Json(new { State = 0, Msg = "参数错误！", Result = "" });
				return actionResult;
			}
			else
			{
				logList = LogDetailProvider.Instance.GetListSE(id, status, 1, 999999, tag);
			}
		Label2:
			List<DownloadDetails> downloadDetails = logList;
			var list = 
				from p in downloadDetails
				select new { Id = p.ResourceId, ResourceName = p.ResourceName, TeacherLoginName = (string.IsNullOrEmpty(p.TeacherLoginName) ? "" : p.TeacherLoginName), TeacherName = (string.IsNullOrEmpty(p.TeacherName) ? "" : p.TeacherName), Ip = p.Ip, CreateTime = string.Concat(p.CreateTime.ToShortDateString(), " ", p.CreateTime.ToLongTimeString()) };
			list = list.Where((p) => {
				bool flag;
				string createTime = p.CreateTime;
				char[] chrArray = new char[] { ' ' };
				if (DateTime.Parse(createTime.Split(chrArray)[0]) > end)
				{
					flag = false;
				}
				else
				{
					string str1 = p.CreateTime;
					chrArray = new char[] { ' ' };
					flag = DateTime.Parse(str1.Split(chrArray)[0]) >= start;
				}
				return flag;
			});
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("名称,教师帐号,教师名,Ip,时间");
			foreach (var item in list)
			{
				string[] resourceName = new string[] { item.ResourceName, ",", item.TeacherLoginName, ",", item.TeacherName, ",", item.Ip, ",", item.CreateTime };
				sb.AppendLine(string.Concat(resourceName));
			}
			string filename = "";
			School school = SchoolProvider.Instance.GetEntityById(id);
			filename = string.Concat(filename, school.Name);

			filename = string.Concat(filename, tag);
            //filename = string.Concat(filename, (Enums.DbStatus)Convert.ToInt32(tag).GetName());

            filename = string.Concat(filename, str);
			filename = string.Concat(filename, "记录");
			DateTime now = DateTime.Now;
			filename = string.Concat(filename, now.ToString("yyyy-MM-dd"));
			byte[] data = Encoding.Default.GetBytes(sb.ToString());
			actionResult = this.File(data, "Application/octet-stream", string.Concat(filename, ".csv"));
			return actionResult;
		Label1:
			logList = LogDetailProvider.Instance.GetListSQ(id, status, 1, 999999, tag);
			goto Label2;
		}

		public JsonResult GetIpDetails(Guid id, int tag)
		{
			JsonResult jsonResult;
			List<DownloadDetails> ips = null;
			switch (tag)
			{
				case 0:
				{
					ips = LogDetailProvider.Instance.GetIpDetailsResource(id, 2, tag);
					break;
				}
				case 1:
				case 2:
				{
					ips = LogDetailProvider.Instance.GetIpDetails(id, 2, tag);
					break;
				}
				case 3:
				{
					ips = LogDetailProvider.Instance.GetIpDetailsTestBrowse(id, 0, tag, this.num);
					break;
				}
				case 4:
				{
					ips = LogDetailProvider.Instance.GetIpDetailsExamBrowse(id, 2, tag, this.numExam);
					break;
				}
				default:
				{
					goto case 2;
				}
			}
			jsonResult = (ips.Count > 0 ? base.Json(new { State = 1, Msg = "", Result = ips }) : base.Json(new { State = 0, Msg = "", Result = "" }));
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
				macAddress = (!string.Equals(MainID, "none", StringComparison.CurrentCultureIgnoreCase) ? MainID : SystemManageController.GetMacAddress());
			}
			catch
			{
				macAddress = SystemManageController.GetMacAddress();
			}
			return macAddress;
		}

		public JsonResult GetMonthDetails(Guid id, int tag)
		{
			JsonResult jsonResult;
			List<DownloadDetails> mons = null;
			switch (tag)
			{
				case 0:
				{
					mons = LogDetailProvider.Instance.GetMonthDetailsResource(id, 2, tag);
					break;
				}
				case 1:
				case 2:
				{
					mons = LogDetailProvider.Instance.GetMonthDetails(id, 2, tag);
					break;
				}
				case 3:
				{
					mons = LogDetailProvider.Instance.GetMonthDetailsTest(id, 2, tag, this.num);
					break;
				}
				case 4:
				{
					mons = LogDetailProvider.Instance.GetMonthDetailsExam(id, 2, tag, this.numExam);
					break;
				}
				default:
				{
					goto case 2;
				}
			}
			jsonResult = (mons.Count > 0 ? base.Json(new { State = 1, Msg = "", Result = mons }) : base.Json(new { State = 0, Msg = "", Result = "" }));
			return jsonResult;
		}

		public JsonResult GetReminderCount(string Tier, string TrialTag, string Startime, string Endtime)
		{
			int count = SchoolProvider.Instance.GetReminderCountDateAll(Tier, TrialTag, Startime, Endtime, base.CurrentAdmin.Type, new Guid?(base.CurrentAdmin.Id));
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = count });
			return jsonResult;
		}

		public JsonResult GetSubDetailsE(Guid id, string tag)
		{
			JsonResult jsonResult;
			Func<DownloadDetails, int> func = null;
			
			List<DownloadDetails> cates = LogDetailProvider.Instance.GetCateDetailsSE(id, 2, tag);
			List<DownloadDetails> subDetailsSE = LogDetailProvider.Instance.GetSubDetailsSE(id, 2, tag);
			var list = cates.Select((DownloadDetails p) => {
				string tag2 = p.Tag2;
				int downloadCount = p.DownloadCount;
				IEnumerable<DownloadDetails> downloadDetails1 = 
					from s in subDetailsSE
					where s.Id == p.Id
					select s;
				if (func == null)
				{
					func = (DownloadDetails s) => s.DownloadCount;
				}
				IOrderedEnumerable<DownloadDetails> downloadDetails = downloadDetails1.OrderByDescending<DownloadDetails, int>(func);
				
                var listfuncl = downloadDetails.Select((DownloadDetails s) => new { Tag2 = s.Tag2, DownloadCount = s.DownloadCount });

                return new { Tag2 = tag2, DownloadCount = downloadCount, List = listfuncl };
			});
			jsonResult = (cates.Count != 0 ? base.Json(new { State = 1, Msg = "", Result = list }) : base.Json(new { State = 0, Msg = "", Result = "" }));
			return jsonResult;
		}

		public JsonResult GetSubDetailsM(Guid id, string tag)
		{
			JsonResult jsonResult;
			List<DownloadDetails> subs = LogDetailProvider.Instance.GetSubDetailsSM(id, 2, tag);
			jsonResult = (subs.Count > 0 ? base.Json(new { State = 1, Msg = "", Result = subs }) : base.Json(new { State = 0, Msg = "", Result = "" }));
			return jsonResult;
		}

		public JsonResult GetSubDetailsQ(Guid id, string tag)
		{
			JsonResult jsonResult;
			List<DownloadDetails> subs = LogDetailProvider.Instance.GetSubDetailsSQ(id, 2, tag);
			jsonResult = (subs.Count > 0 ? base.Json(new { State = 1, Msg = "", Result = subs }) : base.Json(new { State = 0, Msg = "", Result = "" }));
			return jsonResult;
		}

		public JsonResult GetSubDetailsR(Guid id, string tag)
		{
			JsonResult jsonResult;
			Func<DownloadDetails, int> func = null;
			
			List<DownloadDetails> cates = LogDetailProvider.Instance.GetCateDetailsSRnew(id, 2, tag);
			List<DownloadDetails> subDetailsSRnew = LogDetailProvider.Instance.GetSubDetailsSRnew(id, 2, tag);
			var list = cates.Select((DownloadDetails p) => {
				string tag2 = p.Tag2;
				int downloadCount = p.DownloadCount;
				IEnumerable<DownloadDetails> downloadDetails1 = 
					from s in subDetailsSRnew
					where s.Id == p.Id
					select s;
				if (func == null)
				{
					func = (DownloadDetails s) => s.DownloadCount;
				}
				IOrderedEnumerable<DownloadDetails> downloadDetails = downloadDetails1.OrderByDescending<DownloadDetails, int>(func);
				
                var listfuncl = downloadDetails.Select((DownloadDetails s) => new { Tag2 = s.Tag2, DownloadCount = s.DownloadCount });

                return new { Tag2 = tag2, DownloadCount = downloadCount, List = listfuncl };
			});
			jsonResult = (cates.Count != 0 ? base.Json(new { State = 1, Msg = "", Result = list }) : base.Json(new { State = 0, Msg = "", Result = "" }));
			return jsonResult;
		}

		public JsonResult GetTestSubjectDetails(Guid id, string tag)
		{
			JsonResult jsonResult;
			List<DownloadDetails> Subjects = LogDetailProvider.Instance.GetTestSubjectDetails(id, 0, tag, this.num);
			jsonResult = (Subjects.Count > 0 ? base.Json(new { State = 1, Msg = "", Result = Subjects }) : base.Json(new { State = 0, Msg = "", Result = "" }));
			return jsonResult;
		}

		private static List<SelectListItem> HtmlDepartment(Guid faculty, Guid department)
		{
			List<SelectListItem> selectListItems1 = new List<SelectListItem>();
			List<School> list = SchoolProvider.Instance.GetListSub(faculty);
			if (list.Count > 0)
			{
				list.ForEach((School p) => {
					List<SelectListItem> selectListItems = selectListItems1;
					SelectListItem selectListItem = new SelectListItem();
					selectListItem.Text=p.Name;
					selectListItem.Value=p.Id.ToString();
					selectListItem.Selected=(department == p.Id);
					selectListItems.Add(selectListItem);
				});
			}
			else
			{
				List<SelectListItem> selectListItems2 = selectListItems1;
				SelectListItem selectListItem1 = new SelectListItem();
				selectListItem1.Text="请选择";
				selectListItem1.Value=Guid.Empty.ToString();
				selectListItems2.Add(selectListItem1);
			}
			List<SelectListItem> selectListItems3 = (
				from p in selectListItems1
				orderby p.Selected
				select p).ToList<SelectListItem>();
			return selectListItems3;
		}

		private static List<SelectListItem> HtmlFaculty(Guid school, Guid faculty)
		{
			List<SelectListItem> selectListItems1 = new List<SelectListItem>();
			List<School> list = SchoolProvider.Instance.GetListSub(school);
			if (list.Count > 0)
			{
				list.ForEach((School p) => {
					List<SelectListItem> selectListItems = selectListItems1;
					SelectListItem selectListItem = new SelectListItem();
					selectListItem.Text=p.Name;
					selectListItem.Value=p.Id.ToString();
					selectListItem.Selected=(faculty == p.Id);
					selectListItems.Add(selectListItem);
				});
			}
			else
			{
				List<SelectListItem> selectListItems2 = selectListItems1;
				SelectListItem selectListItem1 = new SelectListItem();
				selectListItem1.Text="请选择";
				selectListItem1.Value=Guid.Empty.ToString();
				selectListItems2.Add(selectListItem1);
			}
			List<SelectListItem> selectListItems3 = (
				from p in selectListItems1
				orderby p.Selected
				select p).ToList<SelectListItem>();
			return selectListItems3;
		}

		private static List<SelectListItem> HtmlJob(Guid job)
		{
			List<SelectListItem> selectListItems1 = new List<SelectListItem>();
			List<Job> list = JobProvider.Instance.GetList();
			list.ForEach((Job p) => {
				List<SelectListItem> selectListItems = selectListItems1;
				SelectListItem selectListItem = new SelectListItem();
				selectListItem.Text=p.Name;
				selectListItem.Value=p.Id.ToString();
				selectListItem.Selected=(job == p.Id);
				selectListItems.Add(selectListItem);
			});
			List<SelectListItem> selectListItems2 = (
				from p in selectListItems1
				orderby p.Selected
				select p).ToList<SelectListItem>();
			return selectListItems2;
		}

		private static List<SelectListItem> HtmlModules(Guid module)
		{
			List<SelectListItem> selectListItems1 = new List<SelectListItem>();
			List<Module> list = ModuleProvider.Instance.GetList();
			list.ForEach((Module p) => {
				List<SelectListItem> selectListItems = selectListItems1;
				SelectListItem selectListItem = new SelectListItem();
				selectListItem.Text=p.Name;
				selectListItem.Value=p.Id.ToString();
				selectListItem.Selected=(module == p.Id);
				selectListItems.Add(selectListItem);
			});
			return selectListItems1;
		}

		private static List<SelectListItem> HtmlPost(Guid post)
		{
			List<SelectListItem> selectListItems1 = new List<SelectListItem>();
			List<Post> list = PostProvider.Instance.GetList();
			list.ForEach((Post p) => {
				List<SelectListItem> selectListItems = selectListItems1;
				SelectListItem selectListItem = new SelectListItem();
				selectListItem.Text=p.Name;
				selectListItem.Value=p.Id.ToString();
				selectListItem.Selected=(post == p.Id);
				selectListItems.Add(selectListItem);
			});
			List<SelectListItem> selectListItems2 = (
				from p in selectListItems1
				orderby p.Selected
				select p).ToList<SelectListItem>();
			return selectListItems2;
		}

		private static List<SelectListItem> HtmlProfession(Guid pro)
		{
			List<SelectListItem> selectListItems1 = new List<SelectListItem>();
			List<Profession> list = ProfessionProvider.Instance.GetList();
			list.ForEach((Profession p) => {
				List<SelectListItem> selectListItems = selectListItems1;
				SelectListItem selectListItem = new SelectListItem();
				selectListItem.Text=p.Name;
				selectListItem.Value=p.Id.ToString();
				selectListItem.Selected=(pro == p.Id);
				selectListItems.Add(selectListItem);
			});
			List<SelectListItem> selectListItems2 = (
				from p in selectListItems1
				orderby p.Selected
				select p).ToList<SelectListItem>();
			return selectListItems2;
		}

		private static List<SelectListItem> HtmlProvince(Guid module)
		{
			List<SelectListItem> selectListItems1 = new List<SelectListItem>();
			List<SelectListItem> selectListItems2 = selectListItems1;
			SelectListItem selectListItem1 = new SelectListItem();
			selectListItem1.Text="请选择";
			selectListItem1.Value=Guid.Empty.ToString();
			selectListItem1.Selected=(true);
			selectListItems2.Add(selectListItem1);
			List<Province> list = ModuleProvider.Instance.GetProvinceList();
			list.ForEach((Province p) => {
				List<SelectListItem> selectListItems = selectListItems1;
				SelectListItem selectListItem = new SelectListItem();
				selectListItem.Text=p.Name;
				selectListItem.Value=p.Id.ToString();
				selectListItem.Selected=(module == p.Id);
				selectListItems.Add(selectListItem);
			});
			List<SelectListItem> selectListItems3 = (
				from p in selectListItems1
				orderby p.Selected
				select p).ToList<SelectListItem>();
			return selectListItems3;
		}

		private static List<SelectListItem> HtmlRole(Guid school, Guid role)
		{
			List<SelectListItem> selectListItems1 = new List<SelectListItem>();
			List<Role> list = RoleProvider.Instance.GetListBySchoolId(school);
			if (list.Count > 0)
			{
				list.ForEach((Role p) => {
					List<SelectListItem> selectListItems = selectListItems1;
					SelectListItem selectListItem = new SelectListItem();
					selectListItem.Text=p.Name;
					selectListItem.Value=p.Id.ToString();
					selectListItem.Selected=(role == p.Id);
					selectListItems.Add(selectListItem);
				});
			}
			else
			{
				List<SelectListItem> selectListItems2 = selectListItems1;
				SelectListItem selectListItem1 = new SelectListItem();
				selectListItem1.Text="请选择";
				selectListItem1.Value=Guid.Empty.ToString();
				selectListItems2.Add(selectListItem1);
			}
			List<SelectListItem> selectListItems3 = (
				from p in selectListItems1
				orderby p.Selected
				select p).ToList<SelectListItem>();
			return selectListItems3;
		}

		private static List<SelectListItem> HtmlSchool(Guid school)
		{
			List<SelectListItem> ddlSchool = new List<SelectListItem>();
			School s = SchoolProvider.Instance.GetEntity(school);
			SelectListItem selectListItem = new SelectListItem();
			selectListItem.Text=s.Name;
			selectListItem.Value=s.Id.ToString();
			selectListItem.Selected=(school == s.Id);
			ddlSchool.Add(selectListItem);
			return ddlSchool;
		}

		private static List<SelectListItem> HtmlSchools(Guid school)
		{
			List<SelectListItem> selectListItems1 = new List<SelectListItem>();
			List<School> list = SchoolProvider.Instance.GetSchoolList();
			list.ForEach((School p) => {
				List<SelectListItem> selectListItems = selectListItems1;
				SelectListItem selectListItem = new SelectListItem();
				selectListItem.Text=p.Name;
				selectListItem.Value=p.Id.ToString();
				selectListItem.Selected=(school == p.Id);
				selectListItems.Add(selectListItem);
			});
			List<SelectListItem> selectListItems2 = (
				from p in selectListItems1
				orderby p.Selected
				select p).ToList<SelectListItem>();
			return selectListItems2;
		}

		public static List<SelectListItem> HtmlSeller(Guid seller)
		{
			List<SelectListItem> selectListItems1 = new List<SelectListItem>();
			List<SelectListItem> selectListItems2 = selectListItems1;
			SelectListItem selectListItem1 = new SelectListItem();
			selectListItem1.Text="请选择";
			selectListItem1.Value=Guid.Empty.ToString();
			selectListItem1.Selected=(true);
			selectListItems2.Add(selectListItem1);
			List<UserInfo> list = UserInfoProvider.Instance.GetSeller();
			list.ForEach((UserInfo p) => {
				List<SelectListItem> selectListItems = selectListItems1;
				SelectListItem selectListItem = new SelectListItem();
				selectListItem.Text=p.Name;
				selectListItem.Value=p.Id.ToString();
				selectListItem.Selected=(seller == p.Id);
				selectListItems.Add(selectListItem);
			});
			List<SelectListItem> selectListItems3 = (
				from p in selectListItems1
				orderby p.Selected
				select p).ToList<SelectListItem>();
			return selectListItems3;
		}

		private static List<SelectListItem> HtmlTrialType(int trial)
		{
			List<SelectListItem> ddlTrial = new List<SelectListItem>();
			SelectListItem selectListItem = new SelectListItem();
			selectListItem.Text="试用用户";
			selectListItem.Value="0";
			selectListItem.Selected=(trial == 0);
			ddlTrial.Add(selectListItem);
			SelectListItem selectListItem1 = new SelectListItem();
			selectListItem1.Text="远程镜像";
			selectListItem1.Value="1";
			selectListItem1.Selected=(trial == 1);
			ddlTrial.Add(selectListItem1);
			List<SelectListItem> list = (
				from p in ddlTrial
				orderby p.Selected
				select p).ToList<SelectListItem>();
			return list;
		}

		public JsonResult Initialize(string guidlist)
		{
			foreach (Guid id in JsonHelper.Deserialize<List<Guid>>(guidlist))
			{
				UserInfo user = UserInfoProvider.Instance.GetUser(id);
				user.LoginPwd = Encrypt.GetMD5("123456");
				UserInfoProvider.Instance.Update(user);
			}
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "初始化密码成功!所有选中用户密码统一改为“123456”。", Result = "" });
			return jsonResult;
		}

		public JsonResult Ip()
		{
			List<object> list = new List<object>();
			UserInfo user = UserInfoProvider.Instance.GetEntity(base.CurrentAdmin.Id);
			List<Ip> ips = IpProvider.Instance.GetList(user.SchoolId);
			StringBuilder sb = new StringBuilder();
			if (ips.Count > 0)
			{
				foreach (Ip ip in ips)
				{
					string start = IpHelper.ParseIp(ip.IpStart);
					string end = IpHelper.ParseIp(ip.IpEnd);
					if (!(start == end))
					{
						sb.AppendLine(string.Concat(start, "-", end, ","));
					}
					else
					{
						sb.AppendLine(string.Concat(start, ","));
					}
				}
			}
			list.Add(new { Name = user.Name, schoolid = user.SchoolId, SchoolName = SchoolProvider.Instance.GetEntity(user.SchoolId).Name, Ip = sb.ToString() });
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = list });
			return jsonResult;
		}

		public ViewResult LogDetails(Guid id, string tag)
		{
			ViewBag.UserName = base.CurrentAdmin.Name;
			ViewBag.Tag = tag;
			School school = SchoolProvider.Instance.GetEntity(id);
			ViewBag.DownloadCount = LogDetailProvider.Instance.GetCount(id, 2, tag);
			return base.View(school);
		}

		public JsonResult ManagerList()
		{
			bool flag;
			bool flag1;
			List<object> list = new List<object>();
			UserInfo user = null;
			Guid schoolId = Guid.Empty;
			Guid facultyId = Guid.Empty;
			string type = "-1";
			user = UserInfoProvider.Instance.GetEntity(base.CurrentAdmin.Id);
			User_Role ur = User_RoleProvider.Instance.GetEntityByUserId(user.Id);
			RoleProvider.Instance.GetEntity(ur.RoleId);
			int? role = base.CurrentAdmin.Role;
			if ((role.GetValueOrDefault() != 0 ? false : role.HasValue))
			{
				flag = false;
			}
			else
			{
				role = base.CurrentAdmin.Role;
				flag = (role != 3 );
			}
			if (!flag)
			{
				schoolId = Guid.Empty;
			}
			role = base.CurrentAdmin.Role;
			if (role== 1 )
			{
				schoolId = user.SchoolId;
			}
			role = base.CurrentAdmin.Role;
			if (role== 2 )
			{
				schoolId = user.SchoolId;
				facultyId = (!user.FacultyId.HasValue ? Guid.Empty : user.FacultyId.Value);
			}
			List<SelectListItem> School = null;
			role = base.CurrentAdmin.Role;
			if ((role.GetValueOrDefault() != 0 ? false : role.HasValue))
			{
				flag1 = false;
			}
			else
			{
				role = base.CurrentAdmin.Role;
				flag1 = role.GetValueOrDefault() != 3;
			}
			if (flag1)
			{
				Guid guid = user.SchoolId;
				School = SystemManageController.HtmlSchool(user.SchoolId);
			}
			else
			{
				Guid guid1 = user.SchoolId;
				School = SystemManageController.HtmlSchools(user.SchoolId);
			}
			List<object> objs = list;
			Guid guid2 = user.SchoolId;
			Guid id = base.CurrentAdmin.Id;
			string name = base.CurrentAdmin.Name;
			int userListCount = UserInfoProvider.Instance.GetUserListCount(schoolId, facultyId, -1, type, "");
			Guid guid3 = user.SchoolId;
			objs.Add(new { SchoolId = guid2, Id = id, UserName = name, count = userListCount, Faculty = SystemManageController.HtmlFaculty(user.SchoolId, (user.FacultyId.HasValue ? user.FacultyId.Value : Guid.Empty)), Department = SystemManageController.HtmlDepartment((user.FacultyId.HasValue ? user.FacultyId.Value : Guid.Empty), (user.DepartmentId.HasValue ? user.DepartmentId.Value : Guid.Empty)), Role = SystemManageController.HtmlRole(user.SchoolId, ur.RoleId), School = School });
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "ok", Result = list });
			return jsonResult;
		}

		public ViewResult MassEdit()
		{
			Guid guid;
			bool flag;
			Guid guid1;
			Guid guid2;
			UserInfo user = UserInfoProvider.Instance.GetEntity(base.CurrentAdmin.Id);
			ViewBag.UserName = user.Name;
			ViewBag.SchoolId = user.SchoolId;
			ViewBag.guidlist =Request["hdData"];
			dynamic viewBag = ViewBag;
			Guid schoolId = user.SchoolId;
			Guid schoolId1 = user.SchoolId;
			guid = (user.FacultyId.HasValue ? user.FacultyId.Value : Guid.Empty);
			viewBag.Faculty = SystemManageController.HtmlFaculty(schoolId1, guid);
			int? role = base.CurrentAdmin.Role;
			if ((role.GetValueOrDefault() != 0 ? false : role.HasValue))
			{
				flag = false;
			}
			else
			{
				role = base.CurrentAdmin.Role;
				//flag = (role.GetValueOrDefault() != 3 ? 0 : (int)role.HasValue) == 0;
                flag = role != 3;
			}
			if (flag)
			{
				dynamic obj = ViewBag;
				Guid schoolId2 = user.SchoolId;
				obj.School = SystemManageController.HtmlSchool(user.SchoolId);
			}
			else
			{
				dynamic viewBag1 = ViewBag;
				Guid schoolId3 = user.SchoolId;
				viewBag1.School = SystemManageController.HtmlSchools(user.SchoolId);
			}
			dynamic obj1 = ViewBag;
			Guid guid3 = user.SchoolId;
			obj1.Role = SystemManageController.HtmlRole(user.SchoolId, Guid.Empty);
			dynamic viewBag2 = ViewBag;
			guid1 = (user.FacultyId.HasValue ? user.FacultyId.Value : Guid.Empty);
			guid2 = (user.DepartmentId.HasValue ? user.DepartmentId.Value : Guid.Empty);
			viewBag2.Department = SystemManageController.HtmlDepartment(guid1, guid2);
			ViewBag.Profession = SystemManageController.HtmlProfession(Guid.Empty);
			ViewBag.Post = SystemManageController.HtmlPost(Guid.Empty);
			ViewBag.Job = SystemManageController.HtmlJob(Guid.Empty);
			return base.View();
		}

		public JsonResult MassEditTeacher(string guidlist, Guid school, Guid department, Guid faculty, Guid? job, Guid? pro, Guid post, Guid role)
		{
			foreach (Guid id in JsonHelper.Deserialize<List<Guid>>(guidlist))
			{
				UserInfo user = UserInfoProvider.Instance.GetUser(id);
				user.SchoolId = school;
				if (faculty != Guid.Empty)
				{
					user.FacultyId = new Guid?(faculty);
				}
				if (department != Guid.Empty)
				{
					user.DepartmentId = new Guid?(department);
				}
				User_Role uur = User_RoleProvider.Instance.GetEntityByUserId(id);
				if (uur != null)
				{
					uur.RoleId = role;
					User_RoleProvider.Instance.Update(uur);
				}
				else
				{
					User_Role ur = new User_Role()
					{
						UserId = id,
						RoleId = role,
						IsCustom = false,
						IsDeleted = false,
						CreateTime = DateTime.Now
					};
					User_RoleProvider.Instance.Create(ur);
				}
				user.JobId = job;
				user.ProfessionId = pro;
				user.PostId = new Guid?(post);
				UserInfoProvider.Instance.Update(user);
			}
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "修改成功！", Result = "" });
			return jsonResult;
		}

		public ViewResult Medical()
		{
			return base.View();
		}

		public ViewResult Menu()
		{
			ViewBag.UserName = base.CurrentAdmin.Name;
			ViewBag.Modules = SystemManageController.HtmlModules(Guid.Empty);
			return base.View();
		}

		public JsonResult menuList()
		{
			List<Menu> list1 = MenuProvider.Instance.GetList();
			var list = list1.Where<Menu>((Menu p) => {
				int? tier = p.Tier;
				return (tier.GetValueOrDefault() != 0 ? false : tier.HasValue);
			}).Select((Menu p) => {
				
				return new { Id = p.Id, Title = p.Title, ParentId = p.ParentId, Url = p.Url, Tier = p.Tier, Isdeleted = p.IsDeleted, list = list1.Where<Menu>((Menu q) => {
					bool flag;
					int? tier = q.Tier;
					if ((tier.GetValueOrDefault() != 1 ? true : !tier.HasValue))
					{
						flag = false;
					}
					else
					{
						Guid? parentId = q.ParentId;
						Guid id = p.Id;
						flag = (!parentId.HasValue ? false : parentId.GetValueOrDefault() == id);
					}
					return flag;
				}).Select((Menu q) => {
					string str;
					Guid guid = q.Id;
					string title = q.Title;
					Guid? nullable = q.ParentId;
					str = (string.IsNullOrEmpty(q.Url) ? "" : q.Url);
					int? nullable1 = q.Tier;
					bool isDeleted = q.IsDeleted;
					IEnumerable<Menu> menus = list1.Where<Menu>((Menu c) => {
						bool flag;
						int? tier = c.Tier;
						if ((tier.GetValueOrDefault() != 2 ? true : !tier.HasValue))
						{
							flag = false;
						}
						else
						{
							Guid? parentId = c.ParentId;
							Guid id = q.Id;
							flag = (!parentId.HasValue ? false : parentId.GetValueOrDefault() == id);
						}
						return flag;
					});
				
                    var listfunc = menus.Select((Menu c) => new { Id = c.Id, Title = c.Title, ParentId = c.ParentId, Url = (string.IsNullOrEmpty(c.Url) ? "" : c.Url), Tier = c.Tier, Isdeleted = c.IsDeleted });

                    return new { Id = guid, Title = title, ParentId = nullable, Url = str, Tier = nullable1, Isdeleted = isDeleted, list = listfunc };
				}) };
			});
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = list });
			return jsonResult;
		}

		public JsonResult PerZL()
		{
			DateTime value;
			string longDateString;
			string str;
			string str1;
			UserInfo admin = UserInfoProvider.Instance.GetEntity(base.CurrentAdmin.Id);
			User_RoleProvider.Instance.GetEntityByUserId(admin.Id);
			Enums.UserType type = (Enums.UserType)admin.Type.Value;
			Guid schoolId = admin.SchoolId;
			List<SelectListItem> Faculty = SystemManageController.HtmlFaculty(admin.SchoolId, (admin.FacultyId.HasValue ? admin.FacultyId.Value : Guid.Empty));
			List<SelectListItem> Department = SystemManageController.HtmlDepartment((admin.FacultyId.HasValue ? admin.FacultyId.Value : Guid.Empty), (admin.DepartmentId.HasValue ? admin.DepartmentId.Value : Guid.Empty));
			List<SelectListItem> Profession = SystemManageController.HtmlProfession((admin.ProfessionId.HasValue ? admin.ProfessionId.Value : Guid.Empty));
			List<SelectListItem> Post = SystemManageController.HtmlPost((admin.PostId.HasValue ? admin.PostId.Value : Guid.Empty));
			List<SelectListItem> Job = SystemManageController.HtmlJob((admin.JobId.HasValue ? admin.JobId.Value : Guid.Empty));
			string MainID = this.GetMainID();
			List<object> list = new List<object>();
			List<object> objs = list;
			Guid id = admin.Id;
			string loginName = admin.LoginName;
			if (!admin.Birthday.HasValue)
			{
				longDateString = "";
			}
			else
			{
				value = admin.Birthday.Value;
				longDateString = value.ToLongDateString();
			}
			string sex = admin.Sex;
			str = (admin.Email == null ? "" : admin.Email);
			str1 = (admin.Phone == null ? "" : admin.Phone);
			string name = admin.Name;
			string name1 = type.GetName();
			value = admin.CreateTime;
			objs.Add(new { Id = id, LoginName = loginName, Birthday = longDateString, Sex = sex, Email = str, Phone = str1, UserName = name, UserType = name1, Faculty = Faculty, Department = Department, Profession = Profession, Post = Post, Job = Job, CreateTime = value.ToLongDateString(), MainID = MainID });
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "ok", Result = list });
			return jsonResult;
		}

		public JsonResult Reminder(string Tier, string TrialTag, string Startime, string Endtime, int pageIndex)
		{
			List<Reminder> REminderlist = SchoolProvider.Instance.GetReminderDateAll(Tier, TrialTag, Startime, Endtime, base.CurrentAdmin.Type, new Guid?(base.CurrentAdmin.Id), pageIndex, 20);
			var list = REminderlist.Select((Reminder p) => {
				Guid id = p.Id;
				string name = p.Name;
				DateTime startTime = p.StartTime;
				string str = p.StartTime.ToString("yyyy-MM-dd");
				DateTime endTime = p.EndTime;
				return new { Id = id, Name = name, StartTime = str, EndTime = p.EndTime.ToString("yyyy-MM-dd"), StopRemind = p.StopRemind, SellName = (p.SellName == null ? "" : p.SellName), TrialType = (p.TrialType == 0 ? "试用用户" : "镜像用户"), TrRed = (this.CompareDate(p.EndTime) == 0 ? "red" : ""), State = (!p.IsDeleted ? "已启用" : "已停用") };
			});
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = list });
			return jsonResult;
		}

		public ViewResult Resource()
		{
			return base.View();
		}

		public JsonResult RoleList()
		{
			List<Role> rolelist = RoleProvider.Instance.GetListBySchoolId(base.CurrentAdmin.SchoolId);
			var list = 
				from p in rolelist
				select new { Id = p.Id, Name = p.Name, Type = p.Type, Schoolid = p.SchoolId, CreateTime = p.CreateTime, Isdeleted = p.IsDeleted };
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = list });
			return jsonResult;
		}

		public ViewResult School()
		{
			UserInfo user = UserInfoProvider.Instance.GetEntity(base.CurrentAdmin.Id);
			ViewBag.UserName = user.Name;
			ViewBag.Id = user.SchoolId;
			ViewBag.SchoolName = SchoolProvider.Instance.GetEntity(user.SchoolId).Name;
			return base.View();
		}

		public JsonResult SchoolList()
		{
			List<object> list = new List<object>();
			UserIdentity user = base.CurrentAdmin;
			Guid schoolId = UserInfoProvider.Instance.GetEntity(user.Id).SchoolId;
			School school = SchoolProvider.Instance.GetEntity(schoolId);
			list.Add(new { SchoolId = school.Id, UserName = user.Name, Sell = SystemManageController.HtmlSeller(Guid.Empty), Trial = SystemManageController.HtmlTrialType(0), count = SchoolProvider.Instance.GetCount(Guid.Empty, Guid.Empty, "", 0, "", "", "", schoolId), Privince = SystemManageController.HtmlProvince(Guid.Empty) });
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "修改成功", Result = list, Identity = user.Role });
			return jsonResult;
		}

		public JsonResult SchoolLog(Guid id)
		{
			int num;
			DateTime now;
			List<object> list = new List<object>();
			List<object> list_resource = new List<object>();
			List<object> list_Medical = new List<object>();
			List<object> list_Test = new List<object>();
			List<object> list_Exam = new List<object>();
			List<object> list_Quality = new List<object>();
			School school = SchoolProvider.Instance.GetEntityById(id);
			List<School_Module> sms = School_ModuleProvider.Instance.GetTag1List(school.Id);
			int DCountR = 0;
			string DayR = "";
			int RMCount = 0;
			string ResourceRM = "";
			int RRCount = 0;
			string ResourceRR = "";
			string Resource = "";
			string RFlag = "";
			if (sms.Count<School_Module>((School_Module p) => p.Tag1 == 0) <= 0)
			{
				RFlag = "1";
				Resource = "display:none";
			}
			else
			{
				RFlag = "0";
				0.ToString();
				num = 0;
				DownloadDetails logDR = LogDetailProvider.Instance.GetMostDayResource(id, 2, num.ToString());
				if (logDR != null)
				{
					DCountR = logDR.DownloadCount;
					DayR = logDR.Tag2;
				}
				else
				{
					DCountR = 0;
					DayR = "暂无";
				}
				num = 0;
				DownloadDetails logRM = LogDetailProvider.Instance.GetMostResourceMaterial(id, 2, num.ToString());
				if (logRM != null)
				{
					RMCount = logRM.DownloadCount;
					ResourceRM = logRM.Tag2;
				}
				else
				{
					RMCount = 0;
					ResourceRM = "暂无";
				}
				num = 0;
				DownloadDetails logRR = LogDetailProvider.Instance.GetMostResourcePack(id, 2, num.ToString());
				if (logRR != null)
				{
					RRCount = logRR.DownloadCount;
					ResourceRR = logRR.Tag2;
				}
				else
				{
					RRCount = 0;
					ResourceRR = "暂无";
				}
			}
			num = 0;
			int countResource = LogDetailProvider.Instance.GetCountResource(id, 0, num.ToString());
			num = 0;
			int countResource1 = LogDetailProvider.Instance.GetCountResource(id, 1, num.ToString());
			num = 0;
			list_resource.Add(new { RFlag = RFlag, Resource = Resource, ResourceRM = ResourceRM, RMCount = RMCount, ResourceRR = ResourceRR, RRCount = RRCount, DCountR = DCountR, DayR = DayR, BrowseCountR = countResource, CollectCountR = countResource1, DownloadCountR = LogDetailProvider.Instance.GetCountResource(id, 2, num.ToString()) });
			string MFlag = "";
			string Medicine = "";
			int DCountM = 0;
			string ResourceM = "";
			int MCount = 0;
			string DayM = "";
			if (sms.Count<School_Module>((School_Module p) => p.Tag1 == 1) <= 0)
			{
				MFlag = "1";
				Medicine = "display:none";
			}
			else
			{
				Medicine = "";
				MFlag = "0";
				num = 1;
				DownloadDetails logDM = LogDetailProvider.Instance.GetMostDay(id, 2, num.ToString());
				if (logDM != null)
				{
					DCountM = logDM.DownloadCount;
					DayM = logDM.Tag2;
				}
				else
				{
					DCountM = 0;
					DayM = "暂无";
				}
				num = 1;
				DownloadDetails logM = LogDetailProvider.Instance.GetMostResourceM(id, 2, num.ToString());
				if (logM != null)
				{
					MCount = logM.DownloadCount;
					ResourceM = logM.Tag2;
				}
				else
				{
					MCount = 0;
					ResourceM = "暂无";
				}
			}
			num = 1;
			int count = LogDetailProvider.Instance.GetCount(id, 0, num.ToString());
			num = 1;
			int count1 = LogDetailProvider.Instance.GetCount(id, 1, num.ToString());
			num = 1;
			list_Medical.Add(new { MFlag = MFlag, Medicine = Medicine, ResourceM = ResourceM, MCount = MCount, DCountM = DCountM, DayM = DayM, BrowseCountM = count, CollectCountM = count1, DownloadCountM = LogDetailProvider.Instance.GetCount(id, 2, num.ToString()) });
			string Test = "";
			string TFlag = "";
			int TCount = 0;
			string ResourceT = "";
			if (sms.Count<School_Module>((School_Module p) => p.Tag1 == 3) <= 0)
			{
				TFlag = "1";
				Test = "display:none";
			}
			else
			{
				Test = "";
				TFlag = "0";
				num = 3;
				DownloadDetails logT = LogDetailProvider.Instance.GetMostResourceT(id, 0, num.ToString(), this.num);
				if (logT != null)
				{
					TCount = logT.DownloadCount;
					ResourceT = logT.Tag2;
				}
				else
				{
					TCount = 0;
					ResourceT = "暂无";
				}
			}
			num = 3;
			list_Test.Add(new { Test = Test, TFlag = TFlag, TCount = TCount, ResourceT = ResourceT, BrowseCountT = LogDetailProvider.Instance.GetCountTest(id, 0, num.ToString(), this.num) });
			string Exam = "";
			string EFlag = "0";
			int CountED = 0;
			string DayED = "";
			int EDCount = 0;
			string ResourceED = "";
			int CountE = 0;
			string DayE = "";
			int ECount = 0;
			string ResourceE = "";
			if (sms.Count<School_Module>((School_Module p) => p.Tag1 == 4) <= 0)
			{
				EFlag = "1";
				Exam = "display:none";
			}
			else
			{
				num = 4;
				DownloadDetails logDED = LogDetailProvider.Instance.GetMostDayExam(id, 2, num.ToString(), this.numExam);
				if (logDED != null)
				{
					CountED = logDED.DownloadCount;
					DayED = logDED.Tag2;
				}
				else
				{
					CountED = 0;
					DayED = "暂无";
				}
				num = 4;
				DownloadDetails logED = LogDetailProvider.Instance.GetMostResourceE(id, 2, num.ToString(), this.numExam);
				if (logED != null)
				{
					EDCount = logED.DownloadCount;
					ResourceED = logED.Tag2;
				}
				else
				{
					EDCount = 0;
					ResourceED = "暂无";
				}
				num = 4;
				DownloadDetails logDE = LogDetailProvider.Instance.GetMostDayExam(id, 0, num.ToString(), this.numExam);
				if (logDE != null)
				{
					CountE = logDE.DownloadCount;
					DayE = logDE.Tag2;
				}
				else
				{
					CountE = 0;
					DayE = "暂无";
				}
				num = 4;
				DownloadDetails logE = LogDetailProvider.Instance.GetMostResourceE(id, 0, num.ToString(), this.numExam);
				if (logE != null)
				{
					ECount = logE.DownloadCount;
					ResourceE = logE.Tag2;
				}
				else
				{
					ECount = 0;
					ResourceE = "暂无";
				}
			}
			num = 4;
			int countExam = LogDetailProvider.Instance.GetCountExam(id, 0, num.ToString(), this.numExam);
			num = 4;
			list_Exam.Add(new { Exam = Exam, EFlag = EFlag, CountED = CountED, DayED = DayED, EDCount = EDCount, ResourceED = ResourceED, CountE = CountE, DayE = DayE, ECount = ECount, ResourceE = ResourceE, BrowseCountE = countExam, DownloadCountE = LogDetailProvider.Instance.GetCountExam(id, 2, num.ToString(), this.numExam) });
			string QualityCourse = "";
			string QFlag = "0";
			int CountQ = 0;
			string DayQ = "";
			int QCount = 0;
			string ResourceQ = "";
			if (sms.Count<School_Module>((School_Module p) => p.Tag1 == 2) <= 0)
			{
				QFlag = "1";
				QualityCourse = "display:none";
			}
			else
			{
				num = 2;
				DownloadDetails logQ = LogDetailProvider.Instance.GetMostDay(id, 2, num.ToString());
				if (logQ != null)
				{
					CountQ = logQ.DownloadCount;
					DayQ = logQ.Tag2;
				}
				else
				{
					CountQ = 0;
					DayQ = "暂无";
				}
				num = 2;
				DownloadDetails logQB = LogDetailProvider.Instance.GetMostResourceQ(id, 2, num.ToString());
				if (logQB != null)
				{
					QCount = logQB.DownloadCount;
					ResourceQ = logQB.Tag2;
				}
				else
				{
					QCount = 0;
					ResourceQ = "暂无";
				}
			}
			num = 2;
			list_Quality.Add(new { QualityCourse = QualityCourse, QFlag = QFlag, CountQ = CountQ, DayQ = DayQ, QCount = QCount, ResourceQ = ResourceQ, BrowseCountQ = LogDetailProvider.Instance.GetCount(id, 2, num.ToString()) });
			string Now = "";
			DateTime? endTime = school.EndTime;
			if (!endTime.HasValue)
			{
				now = DateTime.Now;
				Now = now.ToString("yyyy-MM-dd");
			}
			else
			{
				endTime = school.EndTime;
				now = DateTime.Now;
				if (endTime >= now)
				{
					endTime = school.EndTime;
					now = endTime.Value;
					Now = now.ToString("yyyy-MM-dd");
				}
				else
				{
					now = DateTime.Now;
					Now = now.ToString("yyyy-MM-dd");
				}
			}
			string strname = HttpUtility.UrlEncode(school.Name, Encoding.GetEncoding("GB2312"));
			Guid guid = school.Id;
			string name = school.Name;
			string tag1 = school.Tag1;
			now = school.CreateTime;
			list.Add(new { Id = guid, Name = name, Tag1 = tag1, Tag2 = strname, CreateTime = now.ToLongDateString(), UserName = base.CurrentAdmin.Name, List_Resource = list_resource, list_Medical = list_Medical, list_Quality = list_Quality, list_Test = list_Test, list_Exam = list_Exam, Now = Now });
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "ok", Result = list }, 0);
			return jsonResult;
		}

		public JsonResult SchoolReminder()
		{
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = "" });
			return jsonResult;
		}

		public JsonResult sellList()
		{
			var list = 
				from p in UserInfoProvider.Instance.GetSeller()
				select new { Id = p.Id, Name = p.Name };
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = list });
			return jsonResult;
		}

		public JsonResult SoftDelMenu(Guid id, int isdel)
		{
			MenuProvider.Instance.SoftDelMenu(id, isdel);
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "", Result = "" });
			return jsonResult;
		}

		public JsonResult StartSchool(Guid id)
		{
			School school = SchoolProvider.Instance.GetEntityById(id);
			school.IsDeleted = false;
			SchoolProvider.Instance.Update(school);
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "启用成功!", Result = "" });
			return jsonResult;
		}

		public JsonResult StartTeacher(Guid id)
		{
			UserInfo user = UserInfoProvider.Instance.GetUser(id);
			user.IsDeleted = false;
			UserInfoProvider.Instance.Update(user);
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "启用成功!", Result = "" });
			return jsonResult;
		}

		public JsonResult StopSchool(Guid id)
		{
			SchoolProvider.Instance.SoftDelete(id);
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "停用成功!", Result = "" });
			return jsonResult;
		}

		public JsonResult StopTeacher(Guid id)
		{
			JsonResult jsonResult;
			if (!(id == base.CurrentAdmin.Id))
			{
				UserInfoProvider.Instance.SoftDelete(id);
				jsonResult = base.Json(new { State = 1, Msg = "停用成功!", Result = "" });
			}
			else
			{
				jsonResult = base.Json(new { State = 0, Msg = "无法停用自己!", Result = "" });
			}
			return jsonResult;
		}

		public ViewResult System()
		{
			UserInfo user = UserInfoProvider.Instance.GetEntity(base.CurrentAdmin.Id);
			ViewBag.UserName = user.Name;
			ViewBag.SchoolId = user.SchoolId;
			ViewBag.Id = user.Id;
			ViewBag.SchoolName = SchoolProvider.Instance.GetEntity(user.SchoolId).Name;
			List<SystemInfo> sysList = SystemInfoProvider.Instance.GetList();
			SystemInfo sys = new SystemInfo();
			if (sysList.Count != 0)
			{
				sys = sysList[0];
			}
			else
			{
				sys.Id = Guid.NewGuid();
				sys.IsReg = new bool?(true);
				sys.IsValid = new bool?(false);
				sys.CreatTime = DateTime.Now;
				sys.IsDeleted = false;
				SystemInfoProvider.Instance.Create(sys);
			}
			if (!sys.IsReg.Value)
			{
				ViewBag.IsRegCK = "";
			}
			else
			{
				ViewBag.IsRegCK = "checked=checked";
			}
			if (!sys.IsValid.Value)
			{
				ViewBag.IsValidCK = "";
			}
			else
			{
				ViewBag.IsValidCK = "checked=checked";
			}
			ViewBag.Reg = (sys.IsReg.Value ? 1 : 0);
			ViewBag.Valid = (sys.IsValid.Value ? 1 : 0);
			return base.View();
		}

		public ViewResult Test()
		{
			return base.View();
		}

		public FileResult Trialreport(Guid schoolid)
		{
			School sh = SchoolProvider.Instance.GetEntity(schoolid);
			string ss = Server.MapPath("/Content/word/synctemp.docx");
			DocX g_document = SystemManageController.CreateFiratTemp(DocX.Load(ss), schoolid);
			MemoryStream sw = new MemoryStream();
			g_document.SaveAs(sw);
			sw.Position = (long)0;
			byte[] b = sw.ToArray();
			sw.Close();
			sw.Dispose();
			string filename = HttpUtility.UrlEncode(string.Concat(sh.Name, "教学素材资源库试用报告.doc"), Encoding.UTF8);
			return this.File(b, "application/ms-word", filename);
		}

		public FileResult TrialreportExam(Guid schoolid)
		{
			School sh = SchoolProvider.Instance.GetEntity(schoolid);
			string ss = Server.MapPath("/Content/word/synctempExam.docx");
			DocX g_document = SystemManageController.CreateFiratTempExam(DocX.Load(ss), schoolid);
			MemoryStream sw = new MemoryStream();
			g_document.SaveAs(sw);
			sw.Position = (long)0;
			byte[] b = sw.ToArray();
			sw.Close();
			sw.Dispose();
			string filename = HttpUtility.UrlEncode(string.Concat(sh.Name, "教学模考库试用报告.doc"), Encoding.UTF8);
			return this.File(b, "application/ms-word", filename);
		}

		public FileResult TrialreportMedical(Guid schoolid)
		{
			School sh = SchoolProvider.Instance.GetEntity(schoolid);
			string ss = Server.MapPath("/Content/word/synctempMedical.docx");
			DocX g_document = SystemManageController.CreateFiratTempMedical(DocX.Load(ss), schoolid);
			MemoryStream sw = new MemoryStream();
			g_document.SaveAs(sw);
			sw.Position = (long)0;
			byte[] b = sw.ToArray();
			sw.Close();
			sw.Dispose();
			string filename = HttpUtility.UrlEncode(string.Concat(sh.Name, "教学医学库试用报告.doc"), Encoding.UTF8);
			return this.File(b, "application/ms-word", filename);
		}

		public FileResult TrialreportQualityCourse(Guid schoolid)
		{
			School sh = SchoolProvider.Instance.GetEntity(schoolid);
			string ss = Server.MapPath("/Content/word/synctempQualityCourse.docx");
			DocX g_document = SystemManageController.CreateFiratTempQualityCourse(DocX.Load(ss), schoolid);
			MemoryStream sw = new MemoryStream();
			g_document.SaveAs(sw);
			sw.Position = (long)0;
			byte[] b = sw.ToArray();
			sw.Close();
			sw.Dispose();
			string filename = HttpUtility.UrlEncode(string.Concat(sh.Name, "教学精品课程试用报告.doc"), Encoding.UTF8);
			return this.File(b, "application/ms-word", filename);
		}

		public FileResult TrialreportTest(Guid schoolid)
		{
			School sh = SchoolProvider.Instance.GetEntity(schoolid);
			string ss = Server.MapPath("/Content/word/synctempTest.docx");
			DocX g_document = SystemManageController.CreateFiratTempTest(DocX.Load(ss), schoolid);
			MemoryStream sw = new MemoryStream();
			g_document.SaveAs(sw);
			sw.Position = (long)0;
			byte[] b = sw.ToArray();
			sw.Close();
			sw.Dispose();
			string filename = HttpUtility.UrlEncode(string.Concat(sh.Name, "教学试题库试用报告.doc"), Encoding.UTF8);
			return this.File(b, "application/ms-word", filename);
		}

		public JsonResult Upmenu(string menuname, Guid menuid)
		{
			Menu r = MenuProvider.Instance.GetEntity(menuid);
			r.Title = menuname;
			int state = 0;
			if (MenuProvider.Instance.Update(r) > 0)
			{
				state = 1;
			}
			JsonResult jsonResult = base.Json(new { State = state, Msg = "修改成功！", Result = "" });
			return jsonResult;
		}

		public JsonResult Uprole(string rolename, Guid roleid)
		{
			Role r = RoleProvider.Instance.GetEntity(roleid);
			r.Name = rolename;
			int state = 0;
			if (RoleProvider.Instance.Update(r) > 0)
			{
				state = 1;
			}
			JsonResult jsonResult = base.Json(new { State = state, Msg = "修改成功！", Result = "" });
			return jsonResult;
		}

		public JsonResult Upsell(string sellname, string zhUpdate, string mmUpdate, Guid sellid)
		{
			UserInfo r = UserInfoProvider.Instance.GetEntity(sellid);
			r.Type = new int?(3);
			r.Name = sellname;
			r.LoginName = zhUpdate;
			r.LoginPwd = Encrypt.GetMD5(mmUpdate);
			int state = 0;
			if (UserInfoProvider.Instance.Update(r) > 0)
			{
				state = 1;
			}
			JsonResult jsonResult = base.Json(new { State = state, Msg = "修改成功！", Result = "" });
			return jsonResult;
		}

		public JsonResult xinxi()
		{
			UserInfo user = UserInfoProvider.Instance.GetEntity(base.CurrentAdmin.Id);
			List<SystemInfo> sysList = SystemInfoProvider.Instance.GetList();
			SystemInfo sys = new SystemInfo();
			if (sysList.Count != 0)
			{
				sys = sysList[0];
			}
			else
			{
				sys.Id = Guid.NewGuid();
				sys.IsReg = new bool?(true);
				sys.IsValid = new bool?(false);
				sys.CreatTime = DateTime.Now;
				sys.IsDeleted = false;
				SystemInfoProvider.Instance.Create(sys);
			}
			string IsRegCK = "";
			string IsValidCK = "";
			if (sys.IsReg.Value)
			{
				IsRegCK = "checked=checked";
			}
			if (sys.IsValid.Value)
			{
				IsValidCK = "checked=checked";
			}
			List<object> list = new List<object>()
			{
				new { Id = user.Id, school = user.SchoolId, UserName = user.Name, SchoolId = user.SchoolId, SchoolName = SchoolProvider.Instance.GetEntity(user.SchoolId).Name, IsRegCK = IsRegCK, IsValidCK = IsValidCK, Reg = (sys.IsReg.Value ? 1 : 0), Valid = (sys.IsValid.Value ? 1 : 0) }
			};
			JsonResult jsonResult = base.Json(new { State = 1, Msg = "ok", Result = list });
			return jsonResult;
		}
	}
}