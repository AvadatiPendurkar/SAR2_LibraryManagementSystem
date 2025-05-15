namespace SAR2_LibraryManagementSystem.Model;
using System.Data.SqlClient;
using System.Data;

public class DataAccessLayer
{
    private readonly string _connectionString;

    public DataAccessLayer(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public void AddUser(Users user)
    {
        using (var con = new SqlConnection(_connectionString))
        {
            con.Open();

            string sqlqury = "sp_addUser";


            using (var cmd = new SqlCommand(sqlqury, con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@firstName", user.firstName);
                cmd.Parameters.AddWithValue("@lastName", user.lastName);
                cmd.Parameters.AddWithValue("@email", user.email);
                cmd.Parameters.AddWithValue("@password", user.password);
                cmd.Parameters.AddWithValue("@mobileNo", user.mobileNo);

                int affectedrow = cmd.ExecuteNonQuery();

            }
        }
    }

}
