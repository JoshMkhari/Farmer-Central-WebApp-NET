using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace ST1109348.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }

        public string FarmerName { get; private set; }
        [Required]
        [Display(Name = "Select Movement Type")]
        public string MovementId { get; set; }
        [Required]
        [Display(Name = "Select Category Name")]
        public string CategoryId { get; set; }
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Enter Product Name")]
        public string Name { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        [Display(Name = "Enter Weight in grams")]
        public int Weight { get; set; }

        [Required]
        [Display(Name = "Enter Production Date")]
        public DateTime ProductionDate { get; set; }
        [Required]
        [Display(Name = "Enter Expiry Date")]
        public DateTime ExpirationDate { get; set; }
        [Display(Name = "Enter Freeze By Date")]
        public string FreezeByDate { get; set; }
        [Display(Name = "Enter Sell By Date")]
        public string SellByDate { get; set; }
        public string DateAdded { get; set; }


        //Used to track movements
        public static List<ProductModel> ProductList { get; private set;}
        
        public static void PopulateProductsList()
        {

            var progDal = new ProgramDal();
            MovementModel.MovementList = new List<MovementModel>();
            CategoryModel.CategoryList = new List<CategoryModel>();
            ProductList = new List<ProductModel>();
            ProductList = progDal.GetAllProducts().ToList();
            MovementModel.MovementList = ProgramDal.GetAllMovements().ToList();
            CategoryModel.CategoryList = ProgramDal.GetAllCategories().ToList();
            foreach (var product in ProductList)
            {
                var index = 0;
                for (; index < CategoryModel.CategoryList.Count; index++)
                {
                    var category = CategoryModel.CategoryList[index];
                    if (Convert.ToInt32(product.CategoryId) != category.Id) continue;
                    product.CategoryId = category.Name;
                    break;
                }

                foreach (var movement in MovementModel.MovementList.Where(movement => Convert.ToInt32(product.MovementId) == movement.Id))
                {
                    product.MovementId = movement.Name;
                    break;
                }

                var i = 0;
                for (; i < FarmerModel.FarmerList.Count; i++)
                {
                    var farmer = FarmerModel.FarmerList[i];
                    if (product.UserId.Equals(farmer.FarmerId))
                    {
                        product.FarmerName = farmer.DisplayName;
                    }
                }
            }
        }

        public static List<ProductModel> PopulateMyProducts(string currentUser)
        {
            var myProducts = new List<ProductModel>();
            var index = 0;
            for (; index < ProductList.Count; index++)
            {
                var prod = ProductList[index];
                if (prod.UserId.Equals(currentUser))
                {
                    myProducts.Add(prod);
                }
            }

            return myProducts;
        }

        public static List<StockModel> PopulateMyStock(List<ProductModel> myProducts)
        {
            var stockTrack = new List<StockModel>();
            var stockNames = new List<string>();
            for (var s = 0; s < myProducts.Count; s++)
            {
                var currentProduct = myProducts.ElementAt(s);

                var stock = currentProduct.Quantity;
                for (var i = s+1; i < myProducts.Count; i++)
                {
                    
                    var compareProduct = myProducts.ElementAt(i);
                    if (!currentProduct.Name.Equals(compareProduct.Name)) continue;
                    if (currentProduct.Weight == compareProduct.Weight)
                    {
                        stock += compareProduct.Quantity;
                    }
                }

                if (stockNames.Contains(currentProduct.Name + currentProduct.Weight)) continue;
                stockNames.Add(currentProduct.Name + currentProduct.Weight);
                stockTrack.Add(new StockModel()
                {
                    Name = currentProduct.Name +" "+ currentProduct.Weight+"g",
                    Category = currentProduct.CategoryId,
                    Stock = stock
                });

            }
            return stockTrack;
        }
    }

    public class StockModel
    {
        public string Name { get; set; }

        public string Category { get; set; }
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