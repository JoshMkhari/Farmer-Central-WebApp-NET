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
        private UserModel currentUser;

        public ActionResult Index()
        {
            UserModel.populateUserList();
            FarmerModel.populateFarmerList();

            RegisterViewModel rvm = new RegisterViewModel();
            rvm.EmployeeName = getEmployeeName(User.Identity.Name);
            rvm.fm = InitilizeFarmers();
            return View(rvm);

        }

        public ActionResult MyProfile()
        {
            RegisterViewModel rvm = new RegisterViewModel();
            rvm.EmployeeName = getEmployeeName(User.Identity.Name);
            rvm.fm = InitilizeFarmers();
            return View(rvm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MyProfile(FormCollection formData)
        {
            UserModel use = new UserModel();

            //check if details have changed or are empty
            use.FullName = ValidateUpdate(formData["fullName"] == "" ? null : formData["fullName"], currentUser.FullName);
            use.Address = ValidateUpdate(formData["Address"] == "" ? null : formData["Address"], currentUser.Address);
            use.Phone = ValidateUpdate(formData["Phone"] == "" ? null : formData["Phone"], currentUser.Phone);
            use.UserEmail = ValidateUpdate(formData["Email"] == "" ? null : formData["Email"], currentUser.UserEmail);
            use.UserName = ValidateUpdate(formData["UserName"] == "" ? null : formData["UserName"], currentUser.UserName);

            MessageBox.Show("New username " + use.UserName);
            ProgramDAL pal = new ProgramDAL();
            pal.UpdateUser(use, currentUser.UserEmail);

            RegisterViewModel rvm = new RegisterViewModel();
            rvm.EmployeeName = getEmployeeName(User.Identity.Name);
            rvm.fm = InitilizeFarmers();
            return View(rvm);
        }

        private string ValidateUpdate(String check, String old)
        {
            if (String.IsNullOrEmpty(check))
            {
                return old;
            }
            else if(check.Equals(old))
            {
                return old;
            }else 
                return check;
        }

        public ActionResult Farmers()
        {
            RegisterViewModel rvm = new RegisterViewModel();
            rvm.EmployeeName = getEmployeeName(User.Identity.Name);
            rvm.fm = InitilizeFarmers();
            return View(rvm);
        }

        // POST: /Account/Register
        [HttpPost]
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

            InitializeCurrentUser();
            fm.CurrentUser = currentUser;

            return fm;;
        }

        private void InitializeCurrentUser()
        {
            currentUser = new UserModel();
            currentUser.UserEmail = User.Identity.Name;

            foreach (var item in UserModel.UserList)
            {
                if (item.UserEmail.Equals(currentUser.UserEmail))
                {
                    currentUser = item;
                }
            }
            currentUser.UserType = UserModel.LoggedInUserRole;
        }

        private String getEmployeeName(String userEmail)
        {
            String userName = "";

            foreach (var item in UserModel.UserList)
            {
                if (item.UserEmail.Equals(userEmail))
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