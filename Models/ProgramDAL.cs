using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Windows;

namespace ST1109348.Models
{
    public class ProgramDAL
    {
        //Desktop Connection Strings
        string connectionStringLocalDEV = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=aspnet-ST1109348-20220402012207;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

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

        public void AddFarmer(String UserID)
        {
            using (SqlConnection con = new SqlConnection(connectionStringLocalDEV))
            {
                SqlCommand cmd = new SqlCommand("SP_AddFarmer", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@RoleID", "2");

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        //User Related
        public IEnumerable<UserModel> GetAllUsers()
        {
            List<UserModel> userList = new List<UserModel>();
            using (     SqlConnection con = new SqlConnection(connectionStringLocalDEV))
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
                    use.Address = Convert.ToString(dr["Address"].ToString());
                    use.FullName = Convert.ToString(dr["FullName"].ToString());
                    use.Phone = Convert.ToString(dr["PhoneNumber"].ToString());
                    use.DisplayName = Convert.ToString(dr["DisplayName"].ToString());
                    userList.Add(use);
                }

                con.Close();
            }

            return userList;

        }

        //Update User
        public void UpdateUser(UserModel use, String OldEmail)
        {
            MessageBox.Show("We running with update " + use.DisplayName);
            using (SqlConnection con = new SqlConnection(connectionStringLocalDEV))
            {
                SqlCommand cmd = new SqlCommand("SP_UpdateUser", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FullName", use.FullName);
                cmd.Parameters.AddWithValue("@Address", use.Address);
                cmd.Parameters.AddWithValue("@Phone", use.Phone);
                cmd.Parameters.AddWithValue("@UserEmail", use.UserEmail);
                cmd.Parameters.AddWithValue("@DisplayName", use.DisplayName);
                cmd.Parameters.AddWithValue("@OldEmail", OldEmail);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
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