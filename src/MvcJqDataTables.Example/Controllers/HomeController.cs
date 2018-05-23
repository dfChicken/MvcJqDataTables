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

            var jsonData = GetJsonObject(result.Count(), model, result, p => new IComparable[]
            {
                p.Id,
                p.Name,
                p.Description,
                p.Name + p.Description
            });

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public object GetJsonObject(int? totalRow, DataTableSettings dt, IEnumerable<Post> query, Func<Post, IComparable[]> funcSelectedProperties)
        {
            var selectQuery = query.ToList();

            var total = totalRow.GetValueOrDefault();

            var rows = selectQuery.Select(it => funcSelectedProperties.Invoke(it).ToList());
            
            return new
            {
                dt.draw,
                recordsTotal = selectQuery.Count,
                recordsFiltered = selectQuery.Count,
                data = rows.ToArray(),
            };
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