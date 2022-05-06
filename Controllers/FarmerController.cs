using ST1109348.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows;

namespace ST1109348.Controllers
{
    [Authorize(Roles = "Farmer, Admin")]
    public class FarmerController : Controller
    {
        // GET: Farmer
        private static RegisterViewModel rvm;
        private static UserModel currentUser;
        private List<ProductModel> myProducts;
        public ActionResult Index()
        {
            UserModel.populateUserList();
            FarmerModel.populateFarmerList();
            ProductModel pm = new ProductModel();
            ProductModel.PopulateProductsList();

            rvm = new RegisterViewModel();
            rvm.farmer = InitilizeFarmers();
            rvm.Product = new ProductModel();
            myProducts = pm.PopulateMyProducts(currentUser.UserID);
            rvm.ProductList= myProducts;
            rvm.MyStockList = pm.PopulateMyStock(myProducts);
            return View(rvm);
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

            return fm; ;
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
        public ActionResult Products()
        {
            ProgramDAL pal = new ProgramDAL();
            rvm.CategoryList = pal.GetAllCategories().ToList();
            rvm.MovementList = pal.GetAllMovments().ToList();
            return View(rvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Products(RegisterViewModel model, FormCollection formData)
        {

            try
            {
                ProgramDAL pal = new ProgramDAL();
                model.Product.ProductionDate = Convert.ToDateTime(checkDate(formData["productionValue"] == "" ? null : formData["productionValue"]));
                model.Product.ExpirationDate = Convert.ToDateTime(checkDate(formData["expirationValue"] == "" ? null : formData["expirationValue"]));
                model.Product.FreezeByDate = checkDate(formData["freezeByValue"] == "" ? null : formData["freezeByValue"]);
                model.Product.SellByDate = checkDate(formData["sellByValue"] == "" ? null : formData["sellByValue"]);
                //MessageBox.Show("Movement ID " + model.Product.MovementID);
                if (model.Product.MovementID.Equals("3"))
                {
                    if (model.Product.Quantity>0)
                    {
                        model.Product.Quantity *= -1;
                    }
                }
                pal.AddProduct(model.Product, currentUser.UserID);
                return RedirectToAction("Index", "Farmer");
            }
            catch (Exception)
            {
                return RedirectToAction("Products", "Farmer");
            }

        }

        private String checkDate(string date)
        {
            DateTime convert = new DateTime();
            try
            {
                convert = Convert.ToDateTime(date);
                return date;
            }
            catch (Exception)
            {
                return " ";
            }
        }
        public ActionResult MyProfile()
        {
           
            return View(rvm);
        }
        private string ValidateUpdate(String check, String old)
        {
            if (String.IsNullOrEmpty(check))
            {
                return old;
            }
            else if (check.Equals(old))
            {
                return old;
            }
            else
                return check;
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

            use.FullName = use.FirstName + " " + use.LastName;
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
    }
}