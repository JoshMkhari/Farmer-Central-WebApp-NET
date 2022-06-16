using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using static System.String;

namespace ST1109348.Models
{
    public class ProgramDal
    {

        //Desktop
        private const string ConnectionStringLocalDev = "Server=localhost;Database=progTaskTwo;UID=sa;PWD=10171906Josh@;";
        //Laptop
        //private const string ConnectionStringLocalDev = "Server=localhost;Database=progTaskTwo;UID=sa;PWD=1017Josh;";
        //Farmer Related 
        public static IEnumerable<FarmerModel> GetAllFarmers()
        {
            var userList = new List<FarmerModel>();
            using (var con = new SqlConnection(ConnectionStringLocalDev))
            {
                var cmd = new SqlCommand("SP_GetAllFarmers", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var use = new FarmerModel
                    {
                        FarmerId = Convert.ToString(dr["UserId"].ToString())
                    };

                    userList.Add(use);
                }
                con.Close();
            }

            return userList;

        }

        public static void AddUser(string userId, string roleId)
        {
            if (IsNullOrEmpty(userId))
                throw new ArgumentException("Value cannot be null or empty.", nameof(userId));
            using (var con = new SqlConnection(ConnectionStringLocalDev))
            {
                var cmd = new SqlCommand("SP_AddUerRole", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@RoleID", roleId);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public static IEnumerable<ImageModel> GetAllSystemImages()
        {
            var imagesList = new List<ImageModel>();
            using (var con = new SqlConnection(ConnectionStringLocalDev))
            {
                var cmd = new SqlCommand("SP_GetAllSystemImages", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var img = new ImageModel()
                    {
                        Name = dr["NAME"].ToString(),
                        ContentType = dr["CONTENT_TYPE"].ToString(),
                        Data = (byte[])dr["Data"],
                        Type = Convert.ToInt32(dr["Type"].ToString()),
                    };
                    imagesList.Add(img);
                }
                con.Close();
            }
            return imagesList;
        }
        public static IEnumerable<ImageModel> GetAllUserImages()
        {
            var imagesList = new List<ImageModel>();
            using (var con = new SqlConnection(ConnectionStringLocalDev))
            {
                var cmd = new SqlCommand("SP_GetAllUserImages", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var img = new ImageModel()
                    {
                        Name = dr["NAME"].ToString(),
                        ContentType = dr["CONTENT_TYPE"].ToString(),
                        Data = (byte[])dr["Data"],
                        UserId = Convert.ToString(dr["User_ID"].ToString()),
                    };
                    imagesList.Add(img);
                }
                con.Close();
            }
            return imagesList;
        }
        //User Related
        public static IEnumerable<UserModel> GetAllUsers()
        {
            var userList = new List<UserModel>();
            using (var con = new SqlConnection(ConnectionStringLocalDev))
            {
                var cmd = new SqlCommand("SP_GetAllUsers", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var use = new UserModel
                    {
                        UserId = Convert.ToString(dr["Id"].ToString()),
                        UserEmail = Convert.ToString(dr["Email"].ToString()),
                        Address = Convert.ToString(dr["Address"].ToString()),
                        FullName = Convert.ToString(dr["FullName"].ToString()),
                        Phone = Convert.ToString(dr["PhoneNumber"].ToString()),
                        DisplayName = Convert.ToString(dr["DisplayName"].ToString()),
                        ProfilePicture = new ImageModel()
                    };
                    userList.Add(use);
                }
                con.Close();
            }
            
            //Check if an image is already set for the current user
            var imagesList = (List<ImageModel>)GetAllUserImages();
            
            foreach (var user in userList)
            {
                foreach (var img in imagesList)
                {
                    if (user.UserId.Equals(img.UserId))
                    {
                        user.ProfilePicture = img;
                        break;
                    }
                }
            }
            
            
            return userList;

        }
        
        private static void UpdateUserImage(UserModel use)
        {
            using (var con = new SqlConnection(ConnectionStringLocalDev))
            {
                var cmd = new SqlCommand("SP_UpdateUserImage", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", use.ProfilePicture.Name);
                cmd.Parameters.AddWithValue("@ContentType", use.ProfilePicture.ContentType);
                cmd.Parameters.AddWithValue("@DATA", use.ProfilePicture.Data);
                cmd.Parameters.AddWithValue("@UserId", use.UserId);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        //Update User
        public static void UpdateUser(UserModel use, string oldEmail)
        {
            using (var con = new SqlConnection(ConnectionStringLocalDev))
            {
                var cmd = new SqlCommand("SP_UpdateUser", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FullName", use.FullName);
                cmd.Parameters.AddWithValue("@Address", use.Address);
                cmd.Parameters.AddWithValue("@Phone", use.Phone);
                cmd.Parameters.AddWithValue("@UserEmail", use.UserEmail);
                cmd.Parameters.AddWithValue("@DisplayName", use.DisplayName);
                cmd.Parameters.AddWithValue("@OldEmail", oldEmail);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            var imagesList = (List<ImageModel>)GetAllUserImages();

            var found = false;
            //foreach (var VARIABLE in COLLECTION)
            foreach (var img in imagesList)
            {
                if (img.UserId.Equals(use.UserId))
                {
                    found = true;
                    break;
                }
            }

            if (found)
            {
                //Update user image
                UpdateUserImage(use);
            }
            else
            {
                //Adds an image
                AddUserImage(use);
            }
            
        }
        public static void AddUserImage(UserModel use)
        {
            using (var con = new SqlConnection(ConnectionStringLocalDev))
            {
                //Check if an image already exists for current user
                var cmd = new SqlCommand("SP_AddUserImage", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", use.ProfilePicture.Name);
                cmd.Parameters.AddWithValue("@ContentType", use.ProfilePicture.ContentType);
                cmd.Parameters.AddWithValue("@DATA", use.ProfilePicture.Data);
                cmd.Parameters.AddWithValue("@UserId", use.UserId);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        //Roles Related
        public static IEnumerable<UserModel> GetAllRoles()
        {
            var userList = new List<UserModel>();
            using (var con = new SqlConnection(ConnectionStringLocalDev))
            {
                var cmd = new SqlCommand("SP_GetAllRoles", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {

                    var use = new UserModel
                    {
                        UserId = Convert.ToString(dr["UserId"].ToString()),
                        UserRole = Convert.ToInt32(dr["RoleId"].ToString())
                    };
                    userList.Add(use);
                }

                con.Close();
            }

            return userList;

        }

        //Product Related
        public IEnumerable<ProductModel> GetAllProducts()
        {
            var productList = new List<ProductModel>();
            using (var con = new SqlConnection(ConnectionStringLocalDev))
            {
                var cmd = new SqlCommand("SP_GetAllProducts", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var prod = new ProductModel
                    {
                        ProductId = Convert.ToInt32(dr["ProductId"].ToString()),
                        MovementId = Convert.ToString(dr["MovementId"].ToString()),
                        CategoryId = Convert.ToString(dr["CategoryId"].ToString()),
                        UserId = Convert.ToString(dr["Id"].ToString()),
                        Name = Convert.ToString(dr["Name"].ToString()),
                        Quantity = Convert.ToInt32(dr["Quantity"].ToString()),
                        Weight = Convert.ToInt32(dr["Weight"].ToString()),
                        ProductionDate = Convert.ToDateTime(dr["ProductionDate"].ToString()),
                        ExpirationDate = Convert.ToDateTime(dr["ExpiryDate"].ToString()),
                        FreezeByDate = CheckNull(dr["FreezeByDate"].ToString()),
                        SellByDate = CheckNull(dr["SellByDate"].ToString()),
                        DateAdded = CheckNull(dr["DateAdded"].ToString())
                    };


                    productList.Add(prod);
                }

                con.Close();
            }
            return productList;
        }

        private static string CheckNull(string date)
        {
            return IsNullOrEmpty(date) ? " " : date;
        }
        public static void AddProduct(ProductModel product, string userId)
        {
            
            using (var con = new SqlConnection(ConnectionStringLocalDev))
            {
                var cmd = new SqlCommand("SP_AddProduct", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@MovementID", product.MovementId);
                cmd.Parameters.AddWithValue("@CategoryID", product.CategoryId);
                cmd.Parameters.AddWithValue("@Name", product.Name);
                cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                cmd.Parameters.AddWithValue("@Weight", product.Weight);
                cmd.Parameters.AddWithValue("@ProductionDate", product.ProductionDate);
                cmd.Parameters.AddWithValue("@ExpiryDate", product.ExpirationDate);
                cmd.Parameters.AddWithValue("@DateAdded", DateTime.Today);

                
                if (IsNullOrEmpty(product.FreezeByDate) || product.FreezeByDate.Equals(" "))
                {

                }
                else
                {
                    cmd.Parameters.AddWithValue("@FreezeByDate", product.FreezeByDate);
                }
                
                if (IsNullOrEmpty(product.SellByDate) || product.SellByDate.Equals(" "))
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

        public static IEnumerable<MovementModel> GetAllMovements()
        {
            var movementList = new List<MovementModel>();
            using (var con = new SqlConnection(ConnectionStringLocalDev))
            {
                var cmd = new SqlCommand("SP_GetAllMovements", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var movement = new MovementModel
                    {
                        Id = Convert.ToInt32(dr["MovementId"].ToString()),
                        Name = Convert.ToString(dr["MovemenetName"].ToString())
                    };
                    movementList.Add(movement);
                }

                con.Close();
            }
            return movementList;
        }

        public static IEnumerable<CategoryModel> GetAllCategories()
        {
            var categoryList = new List<CategoryModel>();
            using (var con = new SqlConnection(ConnectionStringLocalDev))
            {
                var cmd = new SqlCommand("SP_GetAllCategory", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var category = new CategoryModel
                    {
                        Id = Convert.ToInt32(dr["CategoryId"].ToString()),
                        Name = Convert.ToString(dr["CategoryName"].ToString())
                    };
                    categoryList.Add(category);
                }

                con.Close();
            }
            return categoryList;
        }
    }
}