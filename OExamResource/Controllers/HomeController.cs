using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OExamResource.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var list = new List<Models.Module>();
            list.Add(new Models.Module() { Tag1 = 1,Name="aa",Code="1" });
            list.Add(new Models.Module() { Tag1 = 2, Name = "bb",Code="2" });
            ViewBag.LayModule = list;

            ViewBag.Title = "testtitle";
            return View();
        }
    }
}