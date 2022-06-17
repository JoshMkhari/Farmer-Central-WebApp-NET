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
        public ImageModel ProductPicture { get; set; }


        //Used to track movements
        public static List<ProductModel> ProductList { get; private set;}
        public CardModel CardList { get; set;}
        
        public static void PopulateProductsList()
        {

            var progDal = new ProgramDal();
            MovementModel.MovementList = new List<MovementModel>();
            CategoryModel.CategoryList = new List<CategoryModel>();
            ProductList = new List<ProductModel>();
            ProductList = progDal.GetAllProducts().ToList();
            
            var defaultImage = new ImageModel();
            var systemImages = ProgramDal.GetAllSystemImages();
            foreach (var img in systemImages)
            {
                if (img.Name.Equals("219986.png"))
                {
                   
                    defaultImage = img;
                }
            }
            foreach (var product in ProductList)
            {
                if (string.IsNullOrEmpty(product.ProductPicture.Name))
                {
                    product.ProductPicture = defaultImage;
                }
            }

            //Get images
            
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
            var outgoingCount = 0;
            var incomingCount = 0;
            var categoryTrack = new[] { 0, 0, 0, 0, 0, 0,0};
            var categoryName = new[] { "Fruit", "Vegetable", "Milk", "Dairy", "Eggs", "Meat and Poultry","Grains"};
            for (var s = 0; s < myProducts.Count; s++)
            {
                Console.WriteLine("Inside For");
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
                switch (currentProduct.MovementId)
                {
                    case "Incoming":
                        incomingCount++;
                        break;
                    case "Outgoing":
                        outgoingCount++;
                        break;
                }

                switch (currentProduct.CategoryId)
                {
                    case "Fruit":
                        categoryTrack[0] += 1;
                        break;
                    case "Vegetable":
                        categoryTrack[1] += 1;
                        break;
                    case "Milk":
                        categoryTrack[2] += 1;
                        break;
                    case "Dairy":
                        categoryTrack[3] += 1;
                        break;
                    case "Eggs":
                        categoryTrack[4] += 1;
                        break;
                    case "Meat and Poultry":
                        categoryTrack[5] += 1;
                        break;
                    default:
                        categoryTrack[6] += 1;
                        break;
                }
            }
            if (stockTrack.Count > 0)
            {
                myProducts.ElementAt(0).CardList = new CardModel
                {
                    incoming = incomingCount,
                    outgoing = outgoingCount,
                    pieChart = categoryTrack,
                    allCats = categoryName
                };
            }
            else
            {
                stockTrack.Add(new StockModel()
                {
                    Name = " ",
                    Category = " ",
                    Stock = 0
                });
                myProducts.Add(new ProductModel());
                myProducts.ElementAt(0).CardList = new CardModel
                {
                    incoming = 0,
                    outgoing = 0,
                    pieChart = new int[7]
                };
                for (var i = 0; i < 7; i++)
                {
                    myProducts.ElementAt(0).CardList.pieChart[0] = 0;
                }

                myProducts.ElementAt(0).CardList.allCats = categoryName;
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