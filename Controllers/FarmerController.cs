using ST1109348.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static System.String;

namespace ST1109348.Controllers
{
    [Authorize(Roles = "Farmer, Admin")]
    public class FarmerController : Controller
    {
        // GET: Farmer
        private static RegisterViewModel _rvm;
        private static UserModel _currentUser;
        private List<ProductModel> _myProducts;
        public ActionResult Index()
        {
            UserModel.PopulateUserList();
            _rvm = new RegisterViewModel
            {
                Farmer = InitializeFarmers(),
                Product = new ProductModel()
            };
            _myProducts = ProductModel.PopulateMyProducts(_currentUser.UserId);
            _rvm.ProductList= _myProducts;
            _rvm.MyStockList = ProductModel.PopulateMyStock(_myProducts);
            return View(_rvm);
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

            var index = 0;
            for (; index < UserModel.UserList.Count; index++)
            {
                var item = UserModel.UserList[index];
                if (item.UserEmail.Equals(_currentUser.UserEmail))
                {
                    _currentUser = item;
                }
            }

            SetCurrentUserNames();
            _currentUser.UserType = UserModel.LoggedInUserRole;
        }
        public ActionResult Products()
        {
            _rvm.CategoryList = ProgramDal.GetAllCategories().ToList();
            _rvm.MovementList = ProgramDal.GetAllMovements().ToList();
            return View(_rvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Products(RegisterViewModel model, FormCollection formData)
        {

            try
            {
                model.Product.ProductionDate = Convert.ToDateTime(CheckDate(formData["productionValue"] == "" ? null : formData["productionValue"]));
                model.Product.ExpirationDate = Convert.ToDateTime(CheckDate(formData["expirationValue"] == "" ? null : formData["expirationValue"]));
                model.Product.FreezeByDate = CheckDate(formData["freezeByValue"] == "" ? null : formData["freezeByValue"]);
                model.Product.SellByDate = CheckDate(formData["sellByValue"] == "" ? null : formData["sellByValue"]);
                //MessageBox.Show("Movement ID " + model.Product.MovementID);
                if (model.Product.MovementId.Equals("3"))
                {
                    if (model.Product.Quantity>0)
                    {
                        model.Product.Quantity *= -1;
                    }
                }
                ProgramDal.AddProduct(model.Product, _currentUser.UserId);
                return RedirectToAction("Index", "Farmer");
            }
            catch (Exception)
            {
                return RedirectToAction("Products", "Farmer");
            }

        }

        private static string CheckDate(string date)
        {
            try
            {
                return date;
            }
            catch (Exception)
            {
                return " ";
            }
        }
        public ActionResult MyProfile()
        {
           
            return View(_rvm);
        }
        private static string ValidateUpdate(string check, string old)
        {
            if (IsNullOrEmpty(check))
            {
                return old;
            }

            return check.Equals(old) ? old : check;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MyProfile(FormCollection formData)
        {
            var use = new UserModel();
            InitializeCurrentUser();

            //check if details have changed or are empty
            use.FirstName = ValidateUpdate(formData["FirstName"] == "" ? null : formData["FirstName"], _currentUser.FirstName);
            use.LastName = ValidateUpdate(formData["LastName"] == "" ? null : formData["LastName"], _currentUser.LastName);
            use.Address = ValidateUpdate(formData["Address"] == "" ? null : formData["Address"], _currentUser.Address);
            use.Phone = ValidateUpdate(formData["Phone"] == "" ? null : formData["Phone"], _currentUser.Phone);
            use.UserEmail = ValidateUpdate(formData["Email"] == "" ? null : formData["Email"], _currentUser.UserEmail);
            use.DisplayName = ValidateUpdate(formData["DisplayName"] == "" ? null : formData["DisplayName"], _currentUser.DisplayName);
            
            use.FullName = use.FirstName + " " + use.LastName;
            ProgramDal.UpdateUser(use, _currentUser.UserEmail);

            HttpPostedFileBase file = Request.Files["ImageData"];
            if (file != null)
            {
                Console.WriteLine("THIS IS FILE NAME " + file.FileName);
                Console.WriteLine("THIS IS FILE Type " + file.ContentType);
            }
            //HttpPostedFileBase file = formData["ImageData"];
            
            

            return !User.Identity.Name.Equals(use.UserEmail) ? RedirectToAction("SignOut", "Account") : RedirectToAction("Index");
            //MessageBox.Show("New display name " + use.DisplayName);          
        }
    }
}