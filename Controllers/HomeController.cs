using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ST1109348.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {    
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Farmer, Admin")]
        public ActionResult About()
        {
            ViewBag.Message = "Only farmers here to add products " + User.Identity.Name;


            return View();
        }

        [Authorize(Roles = "Employee, Admin")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Only employees here to add farmers and view products";

            return View();
        }
    }
}