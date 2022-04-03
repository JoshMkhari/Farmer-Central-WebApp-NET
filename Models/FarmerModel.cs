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
        public string FarmerUserName { get; set; }

        static List<FarmerModel> farmerList = new List<FarmerModel>();

        public static void populateFarmerList()
        {
            ProgramDAL progDal = new ProgramDAL();
            farmerList = progDal.GetAllFarmers().ToList();

            foreach (var user in UserModel.userList)
            {
                for (int i = 0; i < farmerList.Count; i++)
                {
                    if (farmerList.ElementAt(i).FarmerID.Equals(user.UserID))
                    {
                        farmerList.ElementAt(i).FarmerUserName = user.UserName;
                    }
                }
            }

            MessageBox.Show("farmer Id " + farmerList.ElementAt(0).FarmerID + ": is for user " + farmerList.ElementAt(0).FarmerUserName);

        }



    }
}