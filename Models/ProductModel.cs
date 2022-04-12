using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ST1109348.Models
{
    public class ProductModel
    {
        public string ProductID { get; set; }
        public string MovementID { get; set; }
        public string CategoryID { get; set; }
        public string Name { get; set; }
        public string Quantity { get; set; }
        public DateTime ProductionDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime FreezeByDate { get; set; }
        public DateTime SellByDate { get; set; }

        public static List<ProductModel> productList = new List<ProductModel>();
        public static void populateProductsList()
        {
            ProgramDAL progDal = new ProgramDAL();
            productList = progDal.GetAllFarmers().ToList();


            foreach (var user in UserModel.UserList)
            {
                for (int i = 0; i < farmerList.Count; i++)
                {
                    if (farmerList.ElementAt(i).FarmerID.Equals(user.UserID))
                    {
                        farmerList.ElementAt(i).FarmerEmail = user.UserEmail;
                    }
                }
            }


        }

        public static void populateMyProducts()
        {

        }
    }
}