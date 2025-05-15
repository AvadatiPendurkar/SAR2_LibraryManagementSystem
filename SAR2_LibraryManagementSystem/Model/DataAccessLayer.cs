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

}
