﻿using System;
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
        private static RegisterViewModel rvm;

        public ActionResult Index()
        {
            UserModel.populateUserList();
            FarmerModel.populateFarmerList();

            rvm = new RegisterViewModel();
            rvm.fm = InitilizeFarmers();
            rvm.rpm = new ResetPasswordViewModel();
            return View(rvm);

        }

        public ActionResult MyProfile()
        {
           // MessageBox.Show("Last name " + rvm.fm.CurrentUser.LastName);
            return View(rvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MyProfile(FormCollection formData)
        {
            UserModel use = new UserModel();
            InitializeCurrentUser();

            //check if details have changed or are empty
            use.FirstName = ValidateUpdate(formData["FirstName"] == "" ? null : formData["FirstName"], currentUser.FirstName);
            use.LastName = ValidateUpdate(formData["LastName"] == "" ? null : formData["LastName"], currentUser.LastName);
            use.Address = ValidateUpdate(formData["Address"] == "" ? null : formData["Address"], currentUser.Address);
            use.Phone = ValidateUpdate(formData["Phone"] == "" ? null : formData["Phone"], currentUser.Phone);
            use.UserEmail = ValidateUpdate(formData["Email"] == "" ? null : formData["Email"], currentUser.UserEmail);
            use.DisplayName = ValidateUpdate(formData["DisplayName"] == "" ? null : formData["DisplayName"], currentUser.DisplayName);

            use.FullName = use.FirstName +" " + use.LastName;
            ProgramDAL pal = new ProgramDAL();
            pal.UpdateUser(use, currentUser.UserEmail);

            if (!User.Identity.Name.Equals(use.UserEmail))//Sign out if user changes email
            {
                return RedirectToAction("SignOut", "Account");
            }
            else
            {
                return RedirectToAction("Index");
            }
            //MessageBox.Show("New display name " + use.DisplayName);          
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
            if (String.IsNullOrEmpty(currentUser.DisplayName))
            {
                currentUser.DisplayName = currentUser.UserEmail;
            }
            setCurrentUserNames();
            fm.CurrentUser = currentUser;

            return fm;;
        }

        private void setCurrentUserNames()
        {
            Boolean foundSpace = false;
            for (int i = 0; i < currentUser.FullName.Length; i++)
            {
                if (currentUser.FullName.Substring(i, 1).Equals(" "))
                {
                    currentUser.FirstName = currentUser.FullName.Substring(0, i);
                    currentUser.LastName = currentUser.FullName.Substring(i);
                    foundSpace = true;
                    break;
                }
            }

            if (!foundSpace)
            {
                currentUser.FirstName = currentUser.FullName;
            }
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
            setCurrentUserNames();
            currentUser.UserType = UserModel.LoggedInUserRole;
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