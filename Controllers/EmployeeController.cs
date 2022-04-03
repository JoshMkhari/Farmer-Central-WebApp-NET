using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ST1109348.Models;

namespace ST1109348.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index()
        {
            //Should probably move this to work earlier in a thread
            UserModel.populateUserList();
            FarmerModel.populateFarmerList();
            return View();
        }
    }
}