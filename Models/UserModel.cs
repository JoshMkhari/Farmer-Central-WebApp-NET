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

        public static List<UserModel> userList { get; set; }


        public static void populateUserList()
        {
            ProgramDAL progDal = new ProgramDAL();
            List<UserModel> users = new List<UserModel>();

            users = progDal.GetAllUsers().ToList();
            userList = users;
        }
    }
}