using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IslamiTexts.Data;
using IslamiTexts.Models;

namespace IslamiTexts.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(new DocumentRepository().GetSurahs());
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