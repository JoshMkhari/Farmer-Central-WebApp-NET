using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ST1109348.Controllers
{
    [Authorize(Roles = "Farmer, Admin")]
    public class FarmerController : Controller
    {
        // GET: Farmer
        public ActionResult Index()
        {
            return View();
        }
    }
}