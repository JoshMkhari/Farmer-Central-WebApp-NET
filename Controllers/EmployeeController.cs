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
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ActionResult Index()
        {
            UserModel.populateUserList();
            FarmerModel.populateFarmerList();
            return View(InitilizeFarmers());
        }

        public ActionResult MyProfile()
        {
            return View(InitilizeFarmers());
        }

        public ActionResult Farmers()
        {
            RegisterViewModel rvm = new RegisterViewModel();
            rvm.EmployeeName = getEmployeeName();
            return View(rvm);
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Farmers(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // edit asp net user roles
                    //AspNetUserRoles

                    ProgramDAL pal = new ProgramDAL();
                    pal.AddFarmer(user.Id);

                    return RedirectToAction("Index", "Employee");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
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

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}