using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcJqDataTables.Example.Models;

namespace MvcJqDataTables.Example.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetTestData(DataTableSettings model, ExtraParam modelParam)
        {
            var dt = new PostsData();
            var result = dt.Post.ToList();

            var _result = from c in result select new{c.Id, c.Name, c.Description, Action = c.Description + c.Name};

            return Json(new
            {
                draw = 0,
                recordsTotal = _result.Count(),
                recordsFiltered = _result.Count(),
                data = _result.ToList()
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}