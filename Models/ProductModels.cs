using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Windows;

namespace ST1109348.Models
{
    public class ProductModel
    {
        public int ProductID { get; set; }

        public string FarmerName { get; set; }
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
        [Required]
        public int Quantity { get; set; }
        [Required]
        [Display(Name = "Enter Weight in grams")]
        public int Weight { get; set; }

        [Required]
        [Display(Name = "Enter Producttion Date")]
        public DateTime ProductionDate { get; set; }
        [Required]
        [Display(Name = "Enter Expiry Date")]
        public DateTime ExpirationDate { get; set; }
        [Display(Name = "Enter Freeze By Date")]
        public String FreezeByDate { get; set; }
        [Display(Name = "Enter Sell By Date")]
        public String SellByDate { get; set; }

        public Boolean FreezeByEmpty;
        public Boolean SellByEmpty;


        //Used to track movements
        public static List<ProductModel> ProductList { get; set;}

        public static List<ProductModel> StockList { get; set; }
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

                foreach (var farmer in FarmerModel.farmerList)
                {
                    if (product.UserID.Equals(farmer.FarmerID))
                    {
                        product.FarmerName = farmer.DisplayName;
                    }
                }

            }
        }

        public List<ProductModel> PopulateMyProducts(String currentUser)
        {
            List<ProductModel> myProducts = new List<ProductModel>();
            foreach (var prod in ProductList)
            {
                if (prod.UserID.Equals(currentUser))
                {
                    myProducts.Add(prod);
                }
            }
            return myProducts;
        }

        public List<StockModel> PopulateMyStock(List<ProductModel> myProducts)
        {
            List<StockModel> stockTrack = new List<StockModel>();
            List<string> stockNames = new List<string>();
            for (int s = 0; s < myProducts.Count; s++)
            {
                var currentProduct = myProducts.ElementAt(s);

                int stock = currentProduct.Quantity;
                for (int i = s+1; i < myProducts.Count; i++)
                {
                    
                    var compareProduct = myProducts.ElementAt(i);
                    if (currentProduct.Name.Equals(compareProduct.Name))
                    {
                        if (currentProduct.Weight == compareProduct.Weight)
                        {
                            stock += compareProduct.Quantity;
                        }
                    }
                }
                if (!stockNames.Contains(currentProduct.Name+ currentProduct.Weight))
                {
                    stockNames.Add(currentProduct.Name + currentProduct.Weight);
                    stockTrack.Add(new StockModel()
                    {
                        Name = currentProduct.Name +" "+ currentProduct.Weight+"g",
                        Category = currentProduct.CategoryID,
                        Stock = stock
                    });
                }
                
            }
            return stockTrack;
        }
    }

    public class StockModel
    {
        public String Name { get; set; }

        public String Category { get; set; }
        public int Stock { get; set; }
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