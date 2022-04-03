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
        public static List<UserModel> userList = new List<UserModel>();

        public static void populateUserList()
        {
            ProgramDAL progDal = new ProgramDAL();
            userList = progDal.GetAllUsers().ToList();
        }
    }
}