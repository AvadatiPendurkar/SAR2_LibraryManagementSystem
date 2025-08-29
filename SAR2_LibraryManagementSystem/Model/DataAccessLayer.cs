namespace SAR2_LibraryManagementSystem.Model;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Http.HttpResults;

public class DataAccessLayer
{
    private readonly string _connectionString;

    public DataAccessLayer(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }
    //add user 
    public void AddUser(Users user)
    {
        using (var con = new SqlConnection(_connectionString))
        {
            con.Open();

            using (var cmd = new SqlCommand("sp_addUser1", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@firstName", user.firstName);
                cmd.Parameters.AddWithValue("@lastName", user.lastName);
                cmd.Parameters.AddWithValue("@email", user.email);
                cmd.Parameters.AddWithValue("@pass", user.pass);
                cmd.Parameters.AddWithValue("@mobileNo", user.mobileNo);
                cmd.Parameters.AddWithValue("@ubirthDate", user.ubirthDate);

                int affectedrow = cmd.ExecuteNonQuery();
            }
        }
    }


    public bool IsEmailExists(string email)
    {
        using (SqlConnection con = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand("Sp_IsEmailExists", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Email", email);

            con.Open();
            int count = (int)cmd.ExecuteScalar();
            return count > 0;
        }
    }




    //public void AddDemo(Demo demo)
    //{
    //    //using (var con = new SqlConnection(_connectionString))
    //    //{
    //    //    con.Open();
    //    //    string cmd = "insert into Demo(fname,status) values(@fname,@status)";
    //    //    using(var sqlcmd = new SqlCommand(cmd, con)
    //    //    {

    //    //    }
    //    //}
    //}
    //user login 
    public bool LoginUser(Login login, out string message)
    {
        message = "";
        using (var con = new SqlConnection(_connectionString))
        {
            con.Open();

            using (var cmd = new SqlCommand("sp_LoginUser", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@email", login.email);
                cmd.Parameters.AddWithValue("@pass", login.password);

                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    message = "Login successful";
                    return true;
                }
                else
                {
                    message = "Invalid email or password";
                    return false;
                }
                   
            }
        }
    }

    //user update 
    public void UpdateUser(Users user)
    {
        using (var con = new SqlConnection(_connectionString))
        {
            con.Open();

             using (var cmd = new SqlCommand("sp_updateUser1", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userId", user.userId);
                cmd.Parameters.AddWithValue("@firstName", user.firstName);
                cmd.Parameters.AddWithValue("@lastName", user.lastName);
                cmd.Parameters.AddWithValue("@email", user.email);
                cmd.Parameters.AddWithValue("@pass", user.pass);
                cmd.Parameters.AddWithValue("@mobileNo", user.mobileNo);
                cmd.Parameters.AddWithValue("@ubirthDate", user.ubirthDate);


                int affectedrow = cmd.ExecuteNonQuery();
            }
        }
    }

    
    public void UpdateUserPassword(int userId, string newPassword)
    {
        using (var con = new SqlConnection(_connectionString))
        {
            con.Open();

            using (var cmd = new SqlCommand("sp_updateUserPassword", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@pass", newPassword);

                int affectedRows = cmd.ExecuteNonQuery();
                // Optionally check affectedRows to confirm update success
            }
        }
    }


    //emil exist
    public async Task<bool> DoesEmailExistAsync(string email)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            
            using (SqlCommand cmd = new SqlCommand("Sp_DoesEmailExistAsync", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", email);

                await conn.OpenAsync();
                int count = (int)await cmd.ExecuteScalarAsync();

                return count > 0;
            }
        }
    }


    //view all users
    public List<Users> GetAllUsers()
    {
        var users = new List<Users>();

        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand("sp_getAllUsers", connection);
            connection.Open();
            command.CommandType = CommandType.StoredProcedure;
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    users.Add(new Users
                    {
                        userId = Convert.ToInt32(reader["userId"]),
                        firstName = reader["firstName"].ToString(),
                        lastName = reader["lastName"].ToString(),
                        email = reader["email"].ToString(),
                        pass = reader["pass"].ToString(),
                        mobileNo = reader["mobileNo"].ToString(),
                        ubirthDate = reader.IsDBNull(reader.GetOrdinal("ubirthDate")) ? (DateTime?)null: reader.GetDateTime(reader.GetOrdinal("ubirthDate"))
                        //IsAuthorized = Convert.ToBoolean(reader["isAuthorized"])

                    });
                }
            }
        }
        return users;
    }


    //public List<Users> GetAuthorizedUsers()
    //{
    //    var users = new List<Users>();

    //    using (var connection = new SqlConnection(_connectionString))
    //    {
    //        var command = new SqlCommand("SELECT userId, firstName, lastName, email, mobileNo FROM users WHERE IsAuthorized = \", connection);
    //        connection.Open();
    //        command.CommandType = CommandType.StoredProcedure;
    //        using (var reader = command.ExecuteReader())
    //        {
    //            while (reader.Read())
    //            {
    //                users.Add(new Users
    //                {
    //                    userId = Convert.ToInt32(reader["userId"]),
    //                    firstName = reader["firstName"].ToString(),
    //                    lastName = reader["lastName"].ToString(),
    //                    email = reader["email"].ToString(),
    //                    pass = reader["pass"].ToString(),
    //                    mobileNo = reader["mobileNo"].ToString(),
    //                    IsAuthorized = Convert.ToBoolean(reader["isAuthorized"])

    //                });
    //            }
    //        }
    //    }
    //    return users;
    //}

    //delete user
    public void DeleteUser(int userId)
    {
        using (var con = new SqlConnection(_connectionString))
        {
            con.Open();

            using (var cmd = new SqlCommand("sp_deleteUser1", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userId", userId);

                cmd.ExecuteNonQuery();
            }
        }
    }

    public void DeleteRequestedUser(int userId)
    {
        using (var con = new SqlConnection(_connectionString))
        {
            con.Open();
            using (var cmd = new SqlCommand("Sp_DeleteRequestedUser", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userId", userId);

                cmd.ExecuteNonQuery();
            }
        }
    }

    //view userbyid
    public Users GetUsersById(int userId)
    {
        Users user = null;
        using (var con = new SqlConnection(_connectionString))
        {
            con.Open();

            using (var cmd = new SqlCommand("sp_getUserById", con))
            {
                cmd.CommandType= CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@userId",userId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user=new Users
                        {
                            userId = Convert.ToInt32(reader["userId"]),
                            firstName = reader["firstName"].ToString(),
                            lastName = reader["lastName"].ToString(),
                            email = reader["email"].ToString(),
                            pass = reader["pass"].ToString(),
                            mobileNo = reader["mobileNo"].ToString(),
                            ubirthDate = reader.IsDBNull(reader.GetOrdinal("ubirthDate"))? (DateTime?)null: reader.GetDateTime(reader.GetOrdinal("ubirthDate"))
                            //IsBlocked = Convert.ToBoolean(reader["IsBlocked"]),

                        };
                    }
                }


            }
        }
        return user;

    }
    // Block User
    //public void BlockUser(int userId)
    //{
    //    using (var con = new SqlConnection(_connectionString))
    //    {
    //        con.Open();
    //        using (var cmd = new SqlCommand("sp_blockUser", con))
    //        {
    //            cmd.CommandType = CommandType.StoredProcedure;
    //            cmd.Parameters.AddWithValue("@userId", userId);
    //            cmd.ExecuteNonQuery();
    //        }
    //    }
    //}




    // Unblock User
    //public void UnblockUser(int userId)
    //{
    //    using (var con = new SqlConnection(_connectionString))
    //    {
    //        con.Open();
    //        using (var cmd = new SqlCommand("sp_unblockUser", con))
    //        {
    //            cmd.CommandType = CommandType.StoredProcedure;
    //            cmd.Parameters.AddWithValue("@userId", userId);
    //            cmd.ExecuteNonQuery();
    //        }
    //    }
    //}

    public void AddFeedback(Feedback feedback)
    {
        using (var con = new SqlConnection(_connectionString))
        {
            con.Open();

            using (var cmd = new SqlCommand("Sp_AddFeedback", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CName", feedback.CName);
                cmd.Parameters.AddWithValue("@CEmail", feedback.CEmail);
                cmd.Parameters.AddWithValue("@CMessage", feedback.CMessage);

                int affectedrow = cmd.ExecuteNonQuery();
            }
        }
    }

    //--------------------------------- WISHLIST -----------------------------------------
    //add to wishlist 
    public void AddToWishlist(int userId, int bookId)
    {
        using (var con = new SqlConnection(_connectionString))
        {
            con.Open();

            using (var cmd = new SqlCommand("addToWishlist", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@bookId", bookId);
                cmd.ExecuteNonQuery();
            }
        }
    }

    public List<WishlistRemove> GetWishlistByUserId(int userId)
    {
        List<WishlistRemove> wishlistBooks = new List<WishlistRemove>();

        using (var con = new SqlConnection(_connectionString))
        {
            con.Open();

            using (var cmd = new SqlCommand("Sp_GetWishlistByUserId", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userId", userId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        wishlistBooks.Add(new WishlistRemove
                        {
                            WishlistId = Convert.ToInt32(reader["wishlistId"]),
                            bookId = Convert.ToInt32(reader["bookId"]),
                            bookName = reader["bookName"].ToString(),
                            authorName = reader["authorName"].ToString(),
                            isbn = reader["isbn"].ToString(),
                            genreId = Convert.ToInt32(reader["genreId"]),
                            quantity = Convert.ToInt32(reader["quantity"]),
                            Base64Image = Convert.ToBase64String((byte[])reader["bookImage"])
                        });
                    }
                }
            }
        }

        return wishlistBooks;
    }

    public void DeleteWishlist(int wishlistId)
    {
        using (var con = new SqlConnection(_connectionString))
        {
            con.Open();

            using (var cmd = new SqlCommand("sp_deleteWishlist", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@wishlistId", wishlistId);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
