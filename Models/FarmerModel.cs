using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Windows;

namespace ST1109348.Models
{
    public class FarmerModel
    {
        [Required]
        public string FarmerID { get; set; }
        public string FarmerEmail { get; set; }
        public string DisplayName { get; set; }
        public string FarmerPassword { get; set; }
        public static List<FarmerModel> farmerList = new List<FarmerModel>();

        public List<FarmerModel> farmerView;


        //For interface purposes
        public UserModel CurrentUser { get; set; }

        public static void populateFarmerList()
        {
            ProgramDAL progDal = new ProgramDAL();
            farmerList = progDal.GetAllFarmers().ToList();
            foreach (var user in UserModel.UserList)
            {
                for (int i = 0; i < farmerList.Count; i++)
                {
                    if (farmerList.ElementAt(i).FarmerID.Equals(user.UserID))
                    {
                        farmerList.ElementAt(i).FarmerEmail = user.UserEmail;
                        if (String.IsNullOrEmpty(user.DisplayName))
                        {
                            farmerList.ElementAt(i).DisplayName = user.UserEmail;
                        }
                        else
                            farmerList.ElementAt(i).DisplayName = user.DisplayName;
                    }
                }
            }

        
        }


    }
}