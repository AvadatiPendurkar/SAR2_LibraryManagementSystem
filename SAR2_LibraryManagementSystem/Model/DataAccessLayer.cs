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

            //string sqlquery = "insert into Users(firstName, lastName, email, pass, mobileNo) values (@firstName, @lastName, @email, @pass, @mobileNo)";
            using (var cmd = new SqlCommand("sp_addUser1", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@firstName", user.firstName);
                cmd.Parameters.AddWithValue("@lastName", user.lastName);
                cmd.Parameters.AddWithValue("@email", user.email);
                cmd.Parameters.AddWithValue("@pass", user.pass);
                cmd.Parameters.AddWithValue("@mobileNo", user.mobileNo);

                

                int affectedrow = cmd.ExecuteNonQuery();
            }
        }
    }


    public bool IsEmailExists(string email)
    {
        using (SqlConnection con = new SqlConnection(_connectionString))
        {
            string query = "SELECT COUNT(*) FROM Users WHERE email = @email";
            SqlCommand cmd = new SqlCommand(query, con);
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
            string query = "SELECT COUNT(1) FROM Users WHERE email = @Email";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
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
                        IsAuthorized = Convert.ToBoolean(reader["isAuthorized"])

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
            var command = "DELETE FROM Users WHERE userId = @userId";
            using (var cmd = new SqlCommand(command, con))
            {
                //cmd.CommandType = CommandType.StoredProcedure;
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
                            //IsBlocked = Convert.ToBoolean(reader["IsBlocked"]),



                        };
                    }
                }


            }
        }
        return user;

    }
    // Block User
    public void BlockUser(int userId)
    {
        using (var con = new SqlConnection(_connectionString))
        {
            con.Open();
            using (var cmd = new SqlCommand("sp_blockUser", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.ExecuteNonQuery();
            }
        }
    }




    // Unblock User
    public void UnblockUser(int userId)
    {
        using (var con = new SqlConnection(_connectionString))
        {
            con.Open();
            using (var cmd = new SqlCommand("sp_unblockUser", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.ExecuteNonQuery();
            }
        }
    }

    public void AddFeedback(Feedback feedback)
    {
        using (var con = new SqlConnection(_connectionString))
        {
            con.Open();

            string sqlquery = "insert into Feedback(CName,CEmail,CMessage) values (@CName,@CEmail,@CMessage)";
            using (var cmd = new SqlCommand(sqlquery, con))
            {
                //cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CName", feedback.CName);
                cmd.Parameters.AddWithValue("@CEmail", feedback.CEmail);
                cmd.Parameters.AddWithValue("@CMessage", feedback.CMessage);

                int affectedrow = cmd.ExecuteNonQuery();
            }
        }
    }


}
