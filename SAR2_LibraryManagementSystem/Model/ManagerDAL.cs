using System.Data.SqlClient;
using System.Data;
namespace SAR2_LibraryManagementSystem.Model;

public class ManagerDAL
{

    public readonly string _connectionString;

    public ManagerDAL(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public void AddManagers(Managers manager)
    {
        using (var conn = new SqlConnection(_connectionString))
        {
            
            string insertquery = "INSERT INTO Managers (mfirstName, mlastName, email, pass, mobileNo) VALUES (@mfirstName, @mlastName, @email,@mobileNo, @pass)";
            conn.Open();
            using (var cmd = new SqlCommand(insertquery, conn))
            {                
                cmd.Parameters.AddWithValue("@mfirstName", manager.mfirstName);
                cmd.Parameters.AddWithValue("@mlastName", manager.mlastName);
                cmd.Parameters.AddWithValue("@email", manager.email);
                cmd.Parameters.AddWithValue("@mobileNo", manager.mobileNo);
                cmd.Parameters.AddWithValue("@pass", manager.pass);

                int affectedrow = cmd.ExecuteNonQuery();

            }









        }
    }
}

