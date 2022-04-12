using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ST1109348.Models
{
    public class UserModel
    {
        [Required]
        public string UserID { get; set; }
        [Required]
        public string UserName { get; set; }

        public string UserEmail { get; set; }

        public int UserRole { get; set; }

        public string UserType { get; set; }

        public static List<UserModel> UserList { get; set; }

        public static String LoggedInUserRole { get; set; }

        public string FullName { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }


        public static void populateUserList()
        {
            ProgramDAL progDal = new ProgramDAL();
            List<UserModel> users = new List<UserModel>();

            users = progDal.GetAllUsers().ToList();
            UserList = users;
        }
    }
}