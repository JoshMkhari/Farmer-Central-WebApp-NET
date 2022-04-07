using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ST1109348.Models
{
    public class ProgramDAL
    {
        //Desktop Connection Strings
        string connectionStringLocalDEV = "Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=D:\\Github\\PROG7311\\ST1109348\\App_Data\\aspnet-ST1109348-20220402012207.mdf;Initial Catalog=aspnet-ST1109348-20220402012207;Integrated Security=True";

        //Laptop Connection Strings
        //string connectionStringLocalDEV = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=aspnet-ST1109348-20220402012207;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        //Farmer Related 
        public IEnumerable<FarmerModel> GetAllFarmers()
        {
            List<FarmerModel> userList = new List<FarmerModel>();
            using (SqlConnection con = new SqlConnection(connectionStringLocalDEV))
            {
                SqlCommand cmd = new SqlCommand("SP_GetAllFarmers", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    FarmerModel use = new FarmerModel();
                    use.FarmerID = Convert.ToString(dr["UserId"].ToString());

                    userList.Add(use);
                }

                con.Close();
            }

            return userList;

        }

        //User Related
        public IEnumerable<UserModel> GetAllUsers()
        {
            List<UserModel> userList = new List<UserModel>();
            using (SqlConnection con = new SqlConnection(connectionStringLocalDEV))
            {
                SqlCommand cmd = new SqlCommand("SP_GetAllUsers", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {

                       UserModel use = new UserModel();
                    use.UserID = Convert.ToString(dr["Id"].ToString());
                    use.UserEmail = Convert.ToString(dr["Email"].ToString());
                    use.UserName = Convert.ToString(dr["UserName"].ToString());

                    userList.Add(use);
                }

                con.Close();
            }

            return userList;

        }

        //Roles Related
        public IEnumerable<UserModel> GetAllRoles()
        {
            List<UserModel> userList = new List<UserModel>();
            using (SqlConnection con = new SqlConnection(connectionStringLocalDEV))
            {
                SqlCommand cmd = new SqlCommand("SP_GetAllRoles", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {

                    UserModel use = new UserModel();
                    use.UserID = Convert.ToString(dr["UserId"].ToString());
                    use.UserRole = Convert.ToInt32(dr["RoleId"].ToString());
                    userList.Add(use);
                }

                con.Close();
            }

            return userList;

        }
    }
}