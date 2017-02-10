using Microsoft.CSharp.RuntimeBinder;
using PetaPoco;
using Common;
using Models;
using Providers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;

namespace Controllers
{
    [ModuleFilter(Order = 1)]
    [WebFilter(Order = 0)]
    public class ExamController : BaseController
    {
        public ExamController()
        {
        }

        public JsonResult AddCollectPaper(Guid paperId)
        {
            JsonResult jsonResult;
            ExamCollect model = new ExamCollect()
            {
                Id = Guid.NewGuid(),
                ExamPaperId = paperId,
                CreateTime = DateTime.Now,
                IsDeleted = false
            };
            int count = this.GetPaperByPaper(paperId);
            if (base.IsLoginWeb())
            {
                model.UserId = base.CurrentUser.Id;
                ExamCollectProvider.Instance.Create(model);
                jsonResult = base.Json(new { State = 1, Msg = "收藏成功", Result = count });
            }
            else
            {
                jsonResult = base.Json(new { State = 0, Msg = "收藏功能需要账号登录", Result = count });
            }
            return jsonResult;
        }

        [HttpPost]
        public JsonResult CategoryList()
        {
            //var func = null;
            List<ExamPaperType> list1 = ExamPaperTypeProvider.Instance.GetList(Enums.PaperTier.Level1);
            List<ExamPaperType> examPaperTypes = ExamPaperTypeProvider.Instance.GetList(Enums.PaperTier.Level2);
            var list = list1.Select((ExamPaperType p) =>
            {
                string str;
                Guid id = p.Id;
                string name = p.Name;
                str = (p.Name.Length >= 13 ? p.Name.Substring(0, 13) : p.Name);
                int? paperCount = p.PaperCount;
                string logo = p.Logo;
                IEnumerable<ExamPaperType> parentId =
                    from s in examPaperTypes
                    where s.ParentId == p.Id
                    select s;
                //if (func == null)
                //{
                //    func = (ExamPaperType s) => new { Id = s.Id, Title = s.Name, Name = (s.Name.Length >= 13 ? s.Name.Substring(0, 13) : s.Name), Count = s.PaperCount };
                //}
                var Listparentid = parentId.Select((ExamPaperType s) => new { Id = s.Id, Title = s.Name, Name = (s.Name.Length >= 13 ? s.Name.Substring(0, 13) : s.Name), Count = s.PaperCount }).ToList();
                //return new { Id = id, Title = name, Name = str, Count = paperCount, Logo = logo, List = parentId.Select(func).ToList() };
                return new { Id = id, Title = name, Name = str, Count = paperCount, Logo = logo, List = Listparentid };
            });
            JsonResult jsonResult = base.Json(new { State = 1, Msg = "模考库列表", Result = list });
            return jsonResult;
        }

        public JsonResult DelCollectPaper(Guid paperId)
        {
            JsonResult jsonResult;
            if (base.CurrentUser != null)
            {
                ExamCollectProvider.Instance.Delete(base.CurrentUser.Id, paperId);
                jsonResult = base.Json(new { State = 1, Msg = "取消收藏", Result = "" });
            }
            else
            {
                List<ExamCollect> list = base.GetPaperCollect();
                list.RemoveAll((ExamCollect p) => p.ExamPaperId == paperId);
                base.SetPaperCollect(list);
                jsonResult = base.Json(new { State = 0, Msg = "取消收藏", Result = "" });
            }
            return jsonResult;
        }

        public JsonResult DownPaper(Guid paperId)
        {
            ExamPaper ep = ExamPaperProvider.Instance.GetEntity(paperId);
            if (ep != null)
            {
                ep.SortId = new int?(1);
                ExamPaperProvider.Instance.Update(ep);
            }
            JsonResult jsonResult = base.Json(new { State = 1, Msg = "可用！", Result = "" });
            return jsonResult;
        }

        public JsonResult GetLastTests(Guid paperId)
        {
            JsonResult jsonResult;
            if (!base.IsLoginWeb())
            {
                jsonResult = base.Json(new { State = 0, Msg = "未登录", Result = new List<ExamUserLog>() });
            }
            else
            {
                List<ExamUserLog> list = ExamUserLogProvider.Instance.GetList(base.CurrentUser.Id, paperId, 5);
                jsonResult = base.Json(new { State = 1, Msg = "最近记录", Result = list });
            }
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetPaper(Guid id)
        {
            string item = ConfigurationManager.AppSettings["K5FileUrl"];
            if (!item.EndsWith("/"))
            {
                item = string.Concat(item, "/");
            }
            List<ExamItemPart> Parts = ExamItemPartProvider.Instance.GetListByPaper(id);
            List<ExamItem> listByPaper = ExamItemProvider.Instance.GetListByPaper(id);
            var list =
                from p in Parts
                select new
                {
                    pName = (p.Name.Length >= 5 ? p.Name.Substring(0, 5) : p.Name),
                    Part = p,
                    List = listByPaper.Where<ExamItem>((ExamItem i) =>
                    {
                        bool flag;
                        Guid? partId = i.PartId;
                        Guid guid = p.Id;
                        if ((!partId.HasValue ? true : partId.GetValueOrDefault() != guid))
                        {
                            flag = false;
                        }
                        else
                        {
                            int? itemCount = p.ItemCount;
                            flag = (itemCount.GetValueOrDefault() <= 0 ? false : itemCount.HasValue);
                        }
                        return flag;
                    }).Select((ExamItem s) => new { Id = s.Id, SortId = s.SortId, Question = ExamHelper.ToHtmlFile(s.Question, item), Answer = ExamHelper.ToHtmlFile(s.Answer, item), Explain = ExamHelper.ToHtmlFile(s.Explain, item) }).ToList()
                };
            JsonResult jsonResult = base.Json(new { State = 1, Msg = "试卷内容", Result = list });
            return jsonResult;
        }

        public int GetPaperByPaper(Guid paperId)
        {
            int num = Math.Max(ExamCollectProvider.Instance.GetCount(paperId), 1);
            return num;
        }

        [HttpPost]
        public JsonResult GetTestPaper(Guid id)
        {
            string item1 = ConfigurationManager.AppSettings["K5FileUrl"];
            if (!item1.EndsWith("/"))
            {
                item1 = string.Concat(item1, "/");
            }
            List<ExamItemPart> Parts = ExamItemPartProvider.Instance.GetListByPaper(id);
            List<ExamItem> listByPaper = ExamItemProvider.Instance.GetListByPaper(id);
            var IEnum =
                from p in Parts
                select new
                {
                    Part = p,
                    List = listByPaper.Where<ExamItem>((ExamItem i) =>
                    {
                        Guid? partId = i.PartId;
                        Guid guid = p.Id;
                        return (!partId.HasValue ? false : partId.GetValueOrDefault() == guid);
                    }).ToList<ExamItem>()
                };
            List<object> list = new List<object>();
            foreach (var item in IEnum)
            {
                var temp = item.List.Select((ExamItem p) =>
                {
                    Enums.ExamOption examOption;
                    int num;
                    Guid guid1 = p.Id;
                    string htmlFile = ExamHelper.ToHtmlFile(p.Question, item1);
                    string str = ExamHelper.ToHtmlFile(p.Answer, item1);
                    string htmlFile1 = ExamHelper.ToHtmlFile(p.Explain, item1);
                    int? sortId = p.SortId;
                    int? typeId = p.TypeId;
                    num = ((typeId.GetValueOrDefault() != 0 ? false : typeId.HasValue) ? 0 : 1);
                    typeId = p.TypeId;
                    string str1 = ExamHelper.ShowAnswer((Enums.ExamOption)typeId, p.Answer);
                    //string str1 = ExamHelper.ShowAnswer((Enums.ExamOption)((typeId.HasValue ? typeId.GetValueOrDefault() : Enums.ExamOption.Text)), p.Answer);
                    typeId = p.TypeId;
                    examOption = (Enums.ExamOption)typeId;
                    //examOption = (Enums.ExamOption)((typeId.HasValue ? typeId.GetValueOrDefault() : Enums.ExamOption.Text));
                    typeId = p.OptionCount;
                    return new { Id = guid1, Q = htmlFile, A = str, E = htmlFile1, S = sortId, B = num, V = str1, C = ExamHelper.ToHtmlOption(examOption, (typeId.HasValue ? typeId.GetValueOrDefault() : 0), p.Id) };
                }).ToList();
                list.Add(new { pName = (item.Part.Name.Length >= 5 ? item.Part.Name.Substring(0, 5) : item.Part.Name), Part = item.Part, List = temp });
            }
            JsonResult jsonResult = base.Json(new { State = 1, Msg = "试卷内容", Result = list });
            return jsonResult;
        }

        private static List<SelectListItem> HtmlMeans(int val = -1)
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            SelectListItem selectListItem = new SelectListItem();
            selectListItem.Text = "请选择";
            selectListItem.Value = "-1";
            selectListItems.Add(selectListItem);
            SelectListItem selectListItem1 = new SelectListItem();
            selectListItem1.Text = Enums.PaperMeans.Simulate.GetName();
            selectListItem1.Value = 0.ToString();
            selectListItems.Add(selectListItem1);
            SelectListItem selectListItem2 = new SelectListItem();
            selectListItem2.Text = Enums.PaperMeans.Past.GetName();
            selectListItem2.Value = 1.ToString();
            selectListItems.Add(selectListItem2);
            SelectListItem selectListItem3 = new SelectListItem();
            selectListItem3.Text = Enums.PaperMeans.Custom.GetName();
            selectListItem3.Value = 2.ToString();
            selectListItems.Add(selectListItem3);
            List<SelectListItem> ddlMeans = selectListItems;
            ddlMeans.ForEach((SelectListItem p) => p.Selected = (p.Value == val.ToString()));
            return ddlMeans;
        }

        private static List<SelectListItem> HtmlYears(int year = -1)
        {
            List<SelectListItem> selectListItems1 = new List<SelectListItem>();
            SelectListItem selectListItem1 = new SelectListItem();
            selectListItem1.Text = "请选择";
            selectListItem1.Value = "-1";
            selectListItems1.Add(selectListItem1);
            List<SelectListItem> selectListItems2 = selectListItems1;
            List<int> list = ExamPaperProvider.Instance.GetAllYear();
            list.ForEach((int p) =>
            {
                List<SelectListItem> selectListItems = selectListItems2;
                SelectListItem selectListItem = new SelectListItem();
                selectListItem.Text = string.Concat(p, "年");
                selectListItem.Value = p.ToString();
                selectListItem.Selected = p == year;
                selectListItems.Add(selectListItem);
            });
            return selectListItems2;
        }

        public JsonResult Import(string filepath)
        {
            Exception ex;
            JsonResult jsonResult;
            int? nullable;
            try
            {
                filepath = Server.MapPath(filepath);
                ExamImport package = JsonHelper.Deserialize<ExamImport>(System.IO.File.ReadAllText(filepath));
                DB db = DB.GetInstance();
                db.BeginTransaction();
                try
                {
                    string paperTypeName = package.Paper.TypeString.Trim();
                    Guid paparTypeParentId = ExamImport.PaparTypeParentId;
                    object[] objArray = new object[] { 2, paparTypeParentId, paperTypeName };
                    Guid? paperTypeId = db.ExecuteScalar<Guid?>(new Sql("select id from ExamPaperType where Tier=@0 and ParentId=@1 and Name=@2", objArray));
                    if (!paperTypeId.HasValue)
                    {
                        paperTypeId = new Guid?(Guid.NewGuid());
                        ExamPaperType paperType = new ExamPaperType()
                        {
                            Id = paperTypeId.Value,
                            Name = paperTypeName,
                            SortId = new int?(0),
                            ParentId = paparTypeParentId,
                            PaperCount = new int?(0),
                            Tier = 2,
                            CreateTime = DateTime.Now,
                            IsDeleted = false
                        };
                        db.Insert(paperType);
                    }
                    ExamPaper paper = new ExamPaper()
                    {
                        Id = package.Paper.Id,
                        Name = package.Paper.Name,
                        Means = new int?(2),
                        TimeOut = new int?(package.Paper.TimeOut),
                        Year = new int?(package.Paper.Year),
                        Month = new int?(package.Paper.Month),
                        TypeId = paperTypeId.Value,
                        CreateTime = package.Paper.CreateTime,
                        IsDeleted = package.Paper.IsDeleted,
                        SortId = new int?(package.Paper.SortId)
                    };
                    db.Insert(paper);
                    objArray = new object[] { paperTypeId };
                    db.Execute(new Sql("with CTE as\r\n(\r\nselect Id,ParentId from ExamPaperType where id = @0\r\nunion all\r\nselect ExamPaperType.Id,ExamPaperType.ParentId from ExamPaperType join cte on ExamPaperType.Id=CTE.ParentId\r\n)\r\nupdate ExamPaperType set PaperCount=PaperCount+1 where Id in (select Id from CTE)", objArray));
                    List<ExamItemPart> parts = new List<ExamItemPart>();
                    foreach (ExamImportItem examImportItem in package.Items)
                    {
                        int partIndex = parts.FindIndex((ExamItemPart p) =>
                        {
                            double? itemScore = p.ItemScore;
                            double score = (double)examImportItem.Score;
                            return ((double)itemScore.GetValueOrDefault() != score ? false : itemScore.HasValue);
                        });
                        if (partIndex == -1)
                        {
                            ExamItemPart part = new ExamItemPart()
                            {
                                Id = Guid.NewGuid(),
                                Name = string.Concat(examImportItem.Score, "分/题"),
                                Explain = "",
                                PaperId = new Guid?(paper.Id),
                                ItemScore = new double?((double)examImportItem.Score),
                                ItemCount = new int?(0),
                                SortId = new int?(parts.Count + 1),
                                CreateTime = DateTime.Now,
                                IsDeleted = false
                            };
                            partIndex = parts.Count;
                            parts.Add(part);
                        }
                        ExamItem model = new ExamItem()
                        {
                            Id = examImportItem.Id,
                            Question = examImportItem.Question,
                            Answer = examImportItem.Answer,
                            Explain = examImportItem.Explain,
                            OptionCount = new int?(examImportItem.OptionCount),
                            PaperId = new Guid?(paper.Id),
                            TypeId = new int?(examImportItem.TypeId),
                            SortId = examImportItem.SortId,
                            CreateTime = examImportItem.CreateTime,
                            IsDeleted = examImportItem.IsDeleted,
                            PartId = new Guid?(parts[partIndex].Id)
                        };
                        db.Insert(model);
                        ExamItemPart examItemPart = parts[partIndex];
                        int? itemCount = examItemPart.ItemCount;
                        if (itemCount.HasValue)
                        {
                            nullable = new int?(itemCount.GetValueOrDefault() + 1);
                        }
                        else
                        {
                            nullable = null;
                        }
                        examItemPart.ItemCount = nullable;
                    }
                    foreach (ExamItemPart item in parts)
                    {
                        db.Insert(item);
                    }
                    db.CompleteTransaction();
                }
                catch (Exception exception)
                {
                    ex = exception;
                    db.AbortTransaction();
                    jsonResult = base.Json(new { State = 0, Msg = ex.Message });
                    return jsonResult;
                }
            }
            catch (Exception exception1)
            {
                ex = exception1;
                jsonResult = base.Json(new { State = 0, Msg = ex.Message });
                return jsonResult;
            }
            jsonResult = base.Json(new { State = 1 });
            return jsonResult;
        }

        [HttpGet]
        public ViewResult Index()
        {
            return base.View();
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

        public JsonResult IsCollect(Guid paperId)
        {
            JsonResult jsonResult;
            if (!base.IsLoginWeb())
            {
                jsonResult = base.Json(new { State = 0, Msg = "未收藏", Result = "" });
            }
            else
            {
                jsonResult = (!ExamCollectProvider.Instance.IsCollect(base.CurrentUser.Id, paperId) ? base.Json(new { State = 0, Msg = "未收藏", Result = "" }) : base.Json(new { State = 1, Msg = "已收藏", Result = "" }));
            }
            return jsonResult;
        }

        public JsonResult OtherPaperList(Guid id, int pageIndex)
        {
            List<ExamPaper> eplist = ExamPaperProvider.Instance.GetList(id, pageIndex, 12, Enums.PaperMeans.Past);
            bool flag = false;
            string str1 = ConfigurationManager.AppSettings["hostAddress"].ToString();
            char[] chrArray = new char[] { '|' };
            if (str1.Split(chrArray).Contains<string>(Request.UserHostAddress))
            {
                flag = true;
            }
            var list = eplist.Select((ExamPaper p) =>
            {
                string str;
                ExamPaper examPaper = p;
                if (flag)
                {
                    int? sortId = p.SortId;
                    str = ((sortId.GetValueOrDefault() != 0 ? false : sortId.HasValue) ? "" : "已审");
                }
                else
                {
                    str = "";
                }
                return new { E = examPaper, Sort = str };
            });
            JsonResult jsonResult = base.Json(new { State = 1, Msg = "分页", Result = list });
            return jsonResult;
        }

        public JsonResult PaperList(Guid id, int pageIndex)
        {
            List<ExamPaper> eplist = ExamPaperProvider.Instance.GetList(id, pageIndex, 12);
            bool trialTag = false;
            Guid sid = base.CurrentUser.SchoolId;
            Guid ModuleId = new Guid("19221C35-6066-41CF-8D8E-6197FEBB9F1F");
            School_Module school = School_ModuleProvider.Instance.GetSchoolModuleType(sid, ModuleId);
            trialTag = school.TrialTag == 0;
            var list =
                from p in eplist
                select new { E = p, SortId = (trialTag ? p.SortId : new int?(20000)), Sort = p.SortId };
            JsonResult jsonResult = base.Json(new { State = 1, Msg = "分页", Result = list });
            return jsonResult;
        }

        public JsonResult PassPaper(Guid paperId)
        {
            ExamPaper ep = ExamPaperProvider.Instance.GetEntity(paperId);
            if (ep != null)
            {
                ep.SortId = new int?(2);
                ExamPaperProvider.Instance.Update(ep);
            }
            JsonResult jsonResult = base.Json(new { State = 1, Msg = "完全通过！", Result = "" });
            return jsonResult;
        }

        public JsonResult PostScore(Guid paperId, string score)
        {
            JsonResult jsonResult;
            if (!base.IsLoginWeb())
            {
                jsonResult = base.Json(new { State = 0, Msg = "未登录", Result = "" });
            }
            else
            {
                ExamUserLogProvider instance = ExamUserLogProvider.Instance;
                ExamUserLog examUserLog = new ExamUserLog()
                {
                    Id = Guid.NewGuid(),
                    CreateTime = DateTime.Now,
                    IsDeleted = false,
                    PaperId = paperId,
                    UserId = base.CurrentUser.Id,
                    Score = score
                };
                instance.Create(examUserLog);
                jsonResult = base.Json(new { State = 1, Msg = "OK", Result = "" });
            }
            return jsonResult;
        }

        public ViewResult Search(Guid? id, string key, int year = -1, int mean = -1)
        {
            ViewResult viewResult;
            bool flag;
            
            if (id != null && id != Guid.Empty)
                flag = true;
            else
                flag = false;

            if (flag)
            {
                ExamPaperType model = ExamPaperTypeProvider.Instance.GetEntity(id.Value);
                model.PaperCount = new int?(ExamPaperProvider.Instance.GetCount(id.Value, key, year, mean));
                model.Name = key;
                viewResult = base.View(model);
            }
            else
            {
                ExamPaperType examPaperType = new ExamPaperType()
                {
                    Id = Guid.Empty,
                    Name = key,
                    Tier = -1,
                    PaperCount = new int?(ExamPaperProvider.Instance.GetCount(key, year, mean))
                };
                viewResult = base.View(examPaperType);
            }
            return viewResult;
        }

        public PartialViewResult SearchControl(int year = -1, int mean = -1)
        {
            ViewBag.ddlMeans = ExamController.HtmlMeans(mean);
            ViewBag.ddlYears = ExamController.HtmlYears(year);
            return base.PartialView();
        }

        public JsonResult SearchPage(Guid id, string key, int pageIndex, int year = -1, int mean = -1)
        {
            List<ExamPaper> eplist;
            bool flag;
            eplist = (!(id == Guid.Empty) ? ExamPaperProvider.Instance.SearchPage(id, key, pageIndex, 12, year, mean) : ExamPaperProvider.Instance.SearchPage(key, pageIndex, 12, year, mean));
            bool trialTag = false;
            Guid sid = base.CurrentUser.SchoolId;
            Guid ModuleId = new Guid("19221C35-6066-41CF-8D8E-6197FEBB9F1F");
            School_Module school = School_ModuleProvider.Instance.GetSchoolModuleType(sid, ModuleId);
            trialTag = school.TrialTag == 0;
            bool flag1 = false;
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["hostAddress"].ToString()))
            {
                flag = true;
            }
            else
            {
                string str1 = ConfigurationManager.AppSettings["hostAddress"].ToString();
                char[] chrArray = new char[] { '|' };
                flag = !str1.Split(chrArray).Contains<string>(Request.UserHostAddress);
            }
            if (!flag)
            {
                flag1 = true;
            }
            var list = eplist.Select((ExamPaper p) =>
            {
                int? nullable;
                string str;
                ExamPaper examPaper = p;
                nullable = (trialTag ? p.SortId : new int?(20000));
                if (flag1)
                {
                    int? sortId = p.SortId;
                    str = ((sortId.GetValueOrDefault() != 0 ? false : sortId.HasValue) ? "" : "已审");
                }
                else
                {
                    str = "";
                }
                return new { E = examPaper, SortId = nullable, Sort = str };
            });
            JsonResult jsonResult = base.Json(new { State = 1, Msg = "ok", Result = list });
            return jsonResult;
        }

        public JsonResult SearchSub(Guid id, int tier, string key, int year = -1, int mean = -1)
        {
            List<ExamPaperType> list1;
            if (tier < 2)
            {
                list1 = (!(id == Guid.Empty) ? (
                    from p in ExamPaperTypeProvider.Instance.GetList(id)
                    orderby p.Tier descending
                    select p).ToList<ExamPaperType>() : (
                    from p in ExamPaperTypeProvider.Instance.GetList()
                    orderby p.Tier descending
                    select p).ToList<ExamPaperType>());
            }
            else
            {
                list1 = new List<ExamPaperType>();
            }
            List<ExamPaperType> list2 = ExamPaperTypeProvider.Instance.GetList(key, year, mean);
            foreach (ExamPaperType examPaperType in list1)
            {
                examPaperType.PaperCount = new int?(0);
                if (examPaperType.Tier != 2)
                {
                    examPaperType.PaperCount = (
                        from p in list1
                        where p.ParentId == examPaperType.Id
                        select p).Sum<ExamPaperType>((ExamPaperType s) => s.PaperCount);
                }
                else
                {
                    ExamPaperType item2 = list2.SingleOrDefault<ExamPaperType>((ExamPaperType p) => p.Id == examPaperType.Id);
                    examPaperType.PaperCount = (item2 == null ? new int?(0) : item2.PaperCount);
                }
            }
            IOrderedEnumerable<ExamPaperType> list = list1.Where<ExamPaperType>((ExamPaperType p) =>
            {
                bool flag;
                if (p.Tier != tier)
                {
                    flag = false;
                }
                else
                {
                    int? paperCount = p.PaperCount;
                    flag = (paperCount.GetValueOrDefault() <= 0 ? false : paperCount.HasValue);
                }
                return flag;
            }).OrderBy<ExamPaperType, int?>((ExamPaperType p) => p.SortId);
            var result =
                from p in list
                select new { Title = p.Name, Id = p.Id, Name = (p.Name.Length >= 13 ? p.Name.Substring(0, 13) : p.Name), PaperCount = p.PaperCount };
            JsonResult jsonResult = base.Json(new { State = 1, Msg = "下级数据", Result = result });
            return jsonResult;
        }

        public JsonResult SearchSup(Guid id, string key)
        {
            List<ExamPaperType> list = ExamPaperTypeProvider.Instance.GetSuperList(id);
            if (!string.IsNullOrWhiteSpace(key))
            {
                ExamPaperType examPaperType = new ExamPaperType()
                {
                    Id = Guid.Empty,
                    Name = string.Concat("\"", key, "\"")
                };
                list.Insert(0, examPaperType);
            }
            JsonResult jsonResult = base.Json(new { State = 1, Msg = "上级类型", Result = list });
            return jsonResult;
        }

        public JsonResult show_type()
        {
            string prompt = "";
            if (!base.IsLoginWeb())
            {
                Ip ips = IpProvider.Instance.GetEntity(IpHelper.ParseByte(HttpContext.Request.UserHostAddress));
                prompt = TestProvider.Instance.GetSchool_StopRemind(ips.SchoolId);
            }
            else
            {
                prompt = TestProvider.Instance.GetSchool_StopRemind(base.CurrentUser.SchoolId);
            }
            JsonResult jsonResult = base.Json(new { State = 1, Msg = "ok", Result = prompt });
            return jsonResult;
        }

        public JsonResult SoftDelPaper(Guid paperId)
        {
            ExamPaper ep = ExamPaperProvider.Instance.GetEntity(paperId);
            if (ep != null)
            {
                ep.SortId = new int?(-1);
                ep.IsDeleted = true;
                ExamPaperProvider.Instance.Update(ep);
            }
            JsonResult jsonResult = base.Json(new { State = 1, Msg = "删除！", Result = "" });
            return jsonResult;
        }

        [HttpGet]
        public ActionResult Study(Guid id)
        {
            ExamPaper model = ExamPaperProvider.Instance.GetEntity(id);
            ViewBag.Count = this.GetPaperByPaper(id);
            ViewBag.UserInfo = base.CurrentUser;
            this.InsertLog(id, base.CurrentUser.SchoolId, (base.IsLoginWeb() ? base.CurrentUser.Id : Guid.Empty), HttpContext.Request.UserHostAddress, 0, 4);
            return base.View(model);
        }

        public JsonResult SupList(Guid id)
        {
            var result =
                from p in ExamPaperTypeProvider.Instance.GetSuperList(id)
                select new { Id = p.Id, Name = (p.Name.Length >= 13 ? p.Name.Substring(0, 13) : p.Name), Title = p.Name, PaperCount = p.PaperCount };
            JsonResult jsonResult = base.Json(new { State = 1, Msg = "上级类型", Result = result });
            return jsonResult;
        }

        [HttpGet]
        public ViewResult Test(Guid id)
        {
            ExamPaper model = ExamPaperProvider.Instance.GetEntity(id);
            ViewBag.Count = this.GetPaperByPaper(id);
            ViewBag.UserInfo = base.CurrentUser;
            this.InsertLog(id, base.CurrentUser.SchoolId, (base.IsLoginWeb() ? base.CurrentUser.Id : Guid.Empty), HttpContext.Request.UserHostAddress, 2, 4);
            return base.View(model);
        }

        [HttpGet]
        public ActionResult Type(Guid id)
        {
            return base.View(ExamPaperTypeProvider.Instance.GetEntity(id) ?? new ExamPaperType());
        }
    }
}