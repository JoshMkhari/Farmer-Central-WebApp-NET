using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ST1109348.Models
{
    public class FarmerModel
    {
        [Required]
        public string FarmerId { get; set; }
        public string FarmerEmail { get; private set; }
        public string DisplayName { get; private set; }
        
        public static List<FarmerModel> FarmerList = new List<FarmerModel>();

        public List<ProductModel> CurrentFarmerProductList;
        public List<FarmerModel> FarmerView { get; set; }
        public ImageModel ProfilePicture { get; set; }

        //For interface purposes
        public UserModel CurrentUser { get; set; }

        public static void PopulateFarmerList()
        {
            FarmerList = ProgramDal.GetAllFarmers().ToList();
            foreach (var user in UserModel.UserList)
            {
                for (var i = 0; i < FarmerList.Count; i++)
                {
                    if (!FarmerList.ElementAt(i).FarmerId.Equals(user.UserId)) continue;
                    FarmerList.ElementAt(i).FarmerEmail = user.UserEmail;
                    FarmerList.ElementAt(i).DisplayName = string.IsNullOrEmpty(user.DisplayName) ? user.UserEmail : user.DisplayName;
                    FarmerList.ElementAt(i).ProfilePicture = user.ProfilePicture;
                }
            }
            
            for (var i = 0; i < FarmerList.Count; i++)
            {
                FarmerList.ElementAt(i).CurrentFarmerProductList = ProductModel.PopulateMyProducts(FarmerList.ElementAt(i).FarmerId);
            }
        }
        
    }
}