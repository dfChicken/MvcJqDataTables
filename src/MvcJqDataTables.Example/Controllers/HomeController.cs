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
        public ActionResult GetTestData(DataTableSettings model)
        {
            var dt = new PostsData();
            var result = dt.Post.ToList();
            return Json(new
            {
                draw = model.draw,
                recordsTotal = result.Count,
                recordsFiltered = result.Count,
                data = result.ToArray()
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