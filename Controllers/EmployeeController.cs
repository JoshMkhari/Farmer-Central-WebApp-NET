using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows;
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
            return View(InitilizeFarmers());
        }

        private FarmerModel InitilizeFarmers()
        {
            FarmerModel fm = new FarmerModel();
            fm.farmerView = FarmerModel.farmerList;
            //MessageBox.Show(User.Identity.Name);

            EmployeeModel em = new EmployeeModel();
            em.EmployeeEmail = User.Identity.Name;
            String userName = "";
            foreach (var item in UserModel.userList)
            {
                if (item.UserEmail.Equals(em.EmployeeEmail))
                {
                    for (int i = 0; i < item.UserName.Length; i++)
                    {
                        if (!item.UserName.Substring(i,1).Equals("@"))
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
            fm.currentEmployee = em;

            return fm;;
        }
    }
}