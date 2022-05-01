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
        string connectionStringLocalDEV = "Data Source=DESKTOP-PLRUMT6\\SQLEXPRESS;Initial Catalog=ST10119348PROG7311;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
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

        //Product Related
        public IEnumerable<ProductModel> GetAllProducts()
        {
            List<ProductModel> productList = new List<ProductModel>();
            using (SqlConnection con = new SqlConnection(connectionStringLocalDEV))
            {
                SqlCommand cmd = new SqlCommand("SP_GetAllProducts", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ProductModel prod = new ProductModel();
                    prod.ProductID = Convert.ToInt32(dr["ProductId"].ToString());
                    prod.MovementID = Convert.ToString(dr["MovementId"].ToString());
                    prod.CategoryID = Convert.ToString(dr["CategoryId"].ToString());
                    prod.UserID = Convert.ToString(dr["Id"].ToString());
                    prod.Name = Convert.ToString(dr["Name"].ToString());
                    prod.Quantity = Convert.ToInt32(dr["Quantity"].ToString());
                    prod.Weight = Convert.ToInt32(dr["Weight"].ToString());
                    prod.ProductionDate = Convert.ToDateTime(dr["ProductionDate"].ToString());
                    prod.ExpirationDate = Convert.ToDateTime(dr["ExpiryDate"].ToString());


                    prod.FreezeByDate = checkNull(dr["FreezeByDate"].ToString());
                    prod.SellByDate = checkNull(dr["SellByDate"].ToString());
                    productList.Add(prod);
                }

                con.Close();
            }
            return productList;
        }

        private String checkNull(string date)
        {
            if (String.IsNullOrEmpty(date))
            {
                return " ";
            }
            else
                return date;
        }
        public void AddProduct(ProductModel product, String UserID)
        {
            
            using (SqlConnection con = new SqlConnection(connectionStringLocalDEV))
            {
                SqlCommand cmd = new SqlCommand("SP_AddProduct", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@MovementID", product.MovementID);
                cmd.Parameters.AddWithValue("@CategoryID", product.CategoryID);
                cmd.Parameters.AddWithValue("@Name", product.Name);
                cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                cmd.Parameters.AddWithValue("@Weight", product.Weight);
                cmd.Parameters.AddWithValue("@ProductionDate", product.ProductionDate);
                cmd.Parameters.AddWithValue("@ExpiryDate", product.ExpirationDate);

                
                if (String.IsNullOrEmpty(product.FreezeByDate) || product.FreezeByDate.Equals(" "))
                {

                }
                else
                {
                cmd.Parameters.AddWithValue("@FreezeByDate", product.FreezeByDate);
                }
                
                if (String.IsNullOrEmpty(product.SellByDate) || product.SellByDate.Equals(" "))
                {

                }
                else
                {
                    cmd.Parameters.AddWithValue("@SellByDate", product.SellByDate);
                }


                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            
        }

        public IEnumerable<MovementModel> GetAllMovments()
        {
            List<MovementModel> movementList = new List<MovementModel>();
            using (SqlConnection con = new SqlConnection(connectionStringLocalDEV))
            {
                SqlCommand cmd = new SqlCommand("SP_GetAllMovements", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    MovementModel movement = new MovementModel();
                    movement.Id = Convert.ToInt32(dr["MovementId"].ToString());
                    movement.Name = Convert.ToString(dr["MovemenetName"].ToString());
                    movementList.Add(movement);
                }

                con.Close();
            }
            return movementList;
        }

        public IEnumerable<CategoryModel> GetAllCategories()
        {
            List<CategoryModel> categoryList = new List<CategoryModel>();
            using (SqlConnection con = new SqlConnection(connectionStringLocalDEV))
            {
                SqlCommand cmd = new SqlCommand("SP_GetAllCategory", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    CategoryModel category = new CategoryModel();
                    category.Id = Convert.ToInt32(dr["CategoryId"].ToString());
                    category.Name = Convert.ToString(dr["CategoryName"].ToString());
                    categoryList.Add(category);
                }

                con.Close();
            }
            return categoryList;
        }
    }
}