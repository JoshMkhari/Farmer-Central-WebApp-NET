using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Windows;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ST1109348.Models;


namespace ST1109348.Controllers
{
    [Authorize(Roles = "Employee, Admin")]
    public class EmployeeController : Controller
    {

        public ActionResult Index()
        {
            UserModel.populateUserList();
            FarmerModel.populateFarmerList();
            return View(InitilizeFarmers());
        }

        public ActionResult Farmers()
        {
            RegisterViewModel rvm = new RegisterViewModel();
            rvm.EmployeeName = getEmployeeName();
            EmployeeModel.addingFarmer = true;
            return View(rvm);
        }

            private FarmerModel InitilizeFarmers()
        {
            FarmerModel fm = new FarmerModel();
            fm.farmerView = FarmerModel.farmerList;

            EmployeeModel em = new EmployeeModel();
            em.EmployeeEmail = User.Identity.Name;

            em.EmployeeName = getEmployeeName();
            fm.currentEmployee = em;

            return fm;;
        }

        private String getEmployeeName()
        {
            String userName = "";
            EmployeeModel em = new EmployeeModel();
            em.EmployeeEmail = User.Identity.Name;

            foreach (var item in UserModel.userList)
            {
                if (item.UserEmail.Equals(em.EmployeeEmail))
                {
                    for (int i = 0; i < item.UserName.Length; i++)
                    {
                        if (!item.UserName.Substring(i, 1).Equals("@"))
                        {
                            userName = userName + item.UserName.Substring(i, 1);
                        }
                        else
                        {
                            break;
                        }
                    }
                    break;
                }
            }
            em.EmployeeName = userName;
            return userName;
        }
    }
}