using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Capstone.Models;

namespace Capstone.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult Substances()
        {
            ViewBag.Message = "This page allows you to View or Add Substances";

            return View();
        }

        [HttpPost()]
        public ActionResult AddSubstances(string subNameText)
        {
            SubstancesModels.newSubstance(subNameText);

            return Redirect("/Home/SubstanceAdded/1");
        }

        public ActionResult SubstanceAdded(string i)
        {
            if (i == "1")
                ViewBag.Message = "Substance successfully added!";
            else
                ViewBag.Message = "Substance failed to add!";

            return View();
        }

        public ActionResult ViewSubstances()
        {
            ViewBag.SubData = SubstancesModels.SelectAll();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
