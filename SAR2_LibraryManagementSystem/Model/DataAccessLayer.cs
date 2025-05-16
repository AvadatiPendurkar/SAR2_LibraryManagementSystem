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
                cmd.Parameters.AddWithValue("@firstName", user.firstName);
                cmd.Parameters.AddWithValue("@lastName", user.lastName);
                cmd.Parameters.AddWithValue("@email", user.email);
                cmd.Parameters.AddWithValue("@pass", user.pass);
                cmd.Parameters.AddWithValue("@mobileNo", user.mobileNo);

                int affectedrow = cmd.ExecuteNonQuery();
            }
        }
    }


}
