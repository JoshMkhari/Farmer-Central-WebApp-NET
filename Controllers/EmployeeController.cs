using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ST1109348.Models;
using static System.String;


namespace ST1109348.Controllers
{
    [Authorize(Roles = "Employee, Admin")]
    public class EmployeeController : Controller
    {
        private readonly ApplicationUserManager _userManager;
        private static UserModel _currentUser;
        private static RegisterViewModel _rvm;

        public ActionResult Index()
        {
            UserModel.PopulateUserList();
            _rvm = new RegisterViewModel
            {
                Farmer = InitializeFarmers(),
                ProductList = ProductModel.ProductList
            };
            return View(_rvm);

        }

        public ActionResult MyProfile()
        {
            return View(_rvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MyProfile(FormCollection formData)
        {
            InitializeCurrentUser();
            var use = new UserModel
            {
                //check if details have changed or are empty
                FirstName = ValidateUpdate(formData["FirstName"] == "" ? null : formData["FirstName"], _currentUser.FirstName),
                LastName = ValidateUpdate(formData["LastName"] == "" ? null : formData["LastName"], _currentUser.LastName),
                Address = ValidateUpdate(formData["Address"] == "" ? null : formData["Address"], _currentUser.Address),
                Phone = ValidateUpdate(formData["Phone"] == "" ? null : formData["Phone"], _currentUser.Phone),
                UserEmail = ValidateUpdate(formData["Email"] == "" ? null : formData["Email"], _currentUser.UserEmail),
                DisplayName = ValidateUpdate(formData["DisplayName"] == "" ? null : formData["DisplayName"], _currentUser.DisplayName)
            };


            use.FullName = use.FirstName +" " + use.LastName;
            ProgramDal.UpdateUser(use, _currentUser.UserEmail);

            return !User.Identity.Name.Equals(use.UserEmail) ? RedirectToAction("SignOut", "Account") : RedirectToAction("Index");
            //MessageBox.Show("New display name " + use.DisplayName);          
        }


        private static string ValidateUpdate(string check, string old)
        {
            if (IsNullOrEmpty(check))
            {
                return old;
            }

            return check.Equals(old) ? old : check;
        }

        public ActionResult Farmers()
        {
            return View(_rvm);
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Farmers(RegisterViewModel model)
        {
            if (IsNullOrEmpty(model.Email) || IsNullOrEmpty(model.Password) || IsNullOrEmpty(model.ConfirmPassword))
                return RedirectToAction("Farmers", "Employee");
            if (!model.ConfirmPassword.Equals(model.Password)) return RedirectToAction("Farmers", "Employee");
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // edit asp net user roles
                    //AspNetUserRoles
                    //MessageBox.Show("error please");
                    ProgramDal.AddFarmer(user.Id);

                    return RedirectToAction("Index", "Employee");
                }
                AddErrors(result);
            }
            // If we got this far, something failed, redisplay form
            return RedirectToAction("Farmers", "Employee");
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            //MessageBox.Show("person we deleting is " + id); 
            //Sign out user and store 
            return RedirectToAction("Index", "Employee");
        }
        private FarmerModel InitializeFarmers()
        {
            var fm = new FarmerModel
            {
                FarmerView = FarmerModel.FarmerList
            };

            InitializeCurrentUser();
            if (IsNullOrEmpty(_currentUser.DisplayName))
            {
                _currentUser.DisplayName = _currentUser.UserEmail;
            }
            SetCurrentUserNames();
            fm.CurrentUser = _currentUser;

            return fm;
        }

        private static void SetCurrentUserNames()
        {
            var foundSpace = false;
            for (var i = 0; i < _currentUser.FullName.Length; i++)
            {
                if (!_currentUser.FullName.Substring(i, 1).Equals(" ")) continue;
                _currentUser.FirstName = _currentUser.FullName.Substring(0, i);
                _currentUser.LastName = _currentUser.FullName.Substring(i);
                foundSpace = true;
                break;
            }

            if (!foundSpace)
            {
                _currentUser.FirstName = _currentUser.FullName;
            }
        }
        private void InitializeCurrentUser()
        {
            _currentUser = new UserModel
            {
                UserEmail = User.Identity.Name
            };

            foreach (var item in UserModel.UserList.Where(item => item.UserEmail.Equals(_currentUser.UserEmail)))
            {
                _currentUser = item;
            }
            SetCurrentUserNames();
            _currentUser.UserType = UserModel.LoggedInUserRole;
        }


        private ApplicationUserManager UserManager => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}