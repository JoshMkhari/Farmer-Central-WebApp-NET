using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ST1109348.Models
{
    public class UserModel
    {
        [Required]
        public string UserId { get; set; }

        public string UserEmail { get; set; }

        public int UserRole { get; set; }

        public string UserType { get; set; }

        public static List<UserModel> UserList { get; private set; }

        public static string LoggedInUserRole { get; set; }

        public string FullName { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        
        public ImageModel ProfilePicture { get; set; }

        
        public static List<ImageModel> SystemImages { get; set; }
        public static void PopulateUserList()
        {
            ProductModel.PopulateProductsList();
            var users = ProgramDal.GetAllUsers().ToList();
            var defaultImage = new ImageModel();
            var systemImages = ProgramDal.GetAllSystemImages();
            foreach (var img in systemImages)
            {
                if (img.Name.Equals("219986.png"))
                {
                   
                    defaultImage = img;
                }
            }

            SystemImages = new List<ImageModel> { defaultImage };
            foreach (var user in users)
            {
                if (string.IsNullOrEmpty(user.ProfilePicture.Name))
                {
                    user.ProfilePicture = defaultImage;
                }
            }
            UserList = users;
            FarmerModel.PopulateFarmerList();
            ProductModel.PopulateProductsList();
        }
        
    }
}