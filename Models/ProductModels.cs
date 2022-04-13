using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ST1109348.Models
{
    public class ProductModel
    {
        public int ProductID { get; set; }

        [Required]
        [Display(Name = "Select Movement Type")]
        public string MovementID { get; set; }
        [Required]
        [Display(Name = "Select Category Name")]
        public string CategoryID { get; set; }
        public string UserID { get; set; }

        [Required]
        [Display(Name = "Enter Product Name")]
        public string Name { get; set; }
        public int Quantity { get; set; }

        [Required]
        [Display(Name = "Enter Producttion Date")]
        public DateTime ProductionDate { get; set; }
        [Required]
        [Display(Name = "Enter Expiry Date")]
        public DateTime ExpirationDate { get; set; }
        public DateTime FreezeByDate { get; set; }
        public DateTime SellByDate { get; set; }
        public static List<ProductModel> ProductList { get; set; }
        public static void PopulateProductsList()
        {
            ProgramDAL progDal = new ProgramDAL();
            MovementModel.MovementList = new List<MovementModel>();
            CategoryModel.CategoryList = new List<CategoryModel>();
            ProductList = new List<ProductModel>(); ;
            ProductList = progDal.GetAllProducts().ToList();
            MovementModel.MovementList = progDal.GetAllMovments().ToList();
            CategoryModel.CategoryList = progDal.GetAllCategories().ToList();

            foreach (var product in ProductList)
            {
                foreach (var category in CategoryModel.CategoryList)
                {
                    if (Convert.ToInt32(product.CategoryID) == category.Id)
                    {
                        product.CategoryID = category.Name;
                        break;
                    }
                }
                foreach (var movement in MovementModel.MovementList)
                {
                    if (Convert.ToInt32(product.MovementID) == movement.Id)
                    {
                        product.MovementID = movement.Name;
                        break;
                    }
                }

            }
        }

        public List<ProductModel> PopulateMyProducts(UserModel currentUser)
        {
            List<ProductModel> myProducts = new List<ProductModel>();
            foreach (var prod in ProductList)
            {
                if (prod.UserID.Equals(currentUser.UserID))
                {
                    myProducts.Add(prod);
                    break;
                }
            }

            return myProducts;
        }
    }

    public class MovementModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static List<MovementModel> MovementList;
    }

    public class CategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static List<CategoryModel> CategoryList;
    }
}