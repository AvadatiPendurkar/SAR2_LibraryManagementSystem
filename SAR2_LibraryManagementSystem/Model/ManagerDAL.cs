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

           // string insertquery = "INSERT INTO Managers (mfirstName, mlastName, email, pass, mobileNo) VALUES (@mfirstName, @mlastName, @email,@mobileNo, @pass)";

            conn.Open();
            using (var cmd = new SqlCommand("ps_AddManager", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mfirstName", manager.mfirstName);
                cmd.Parameters.AddWithValue("@mlastName", manager.mlastName);
                cmd.Parameters.AddWithValue("@email", manager.email);
                cmd.Parameters.AddWithValue("@mobileNo", manager.mobileNo);
                cmd.Parameters.AddWithValue("@pass", manager.pass);

                int affectedrow = cmd.ExecuteNonQuery();

            }
        }

    }

    //update manager
    public void UpdateManager(Managers manager)
    {
        using (var conn = new SqlConnection(_connectionString))
        {

            conn.Open();
            string updateQuery = "UPDATE Managers  SET mfirstName = @mfirstName, mlastName = @mlastName, email = @email, mobileNo =@mobileNo = @pass WHERE mId = @mId";

            using (var cmd = new SqlCommand(updateQuery, conn))
            {
                cmd.Parameters.AddWithValue("@mId", manager.mId);
                cmd.Parameters.AddWithValue("@mfirstName", manager.mfirstName);
                cmd.Parameters.AddWithValue("@mlastName", manager.mlastName);
                cmd.Parameters.AddWithValue("@email", manager.email);
                cmd.Parameters.AddWithValue("@mobileNo", manager.mobileNo);
                cmd.Parameters.AddWithValue("@pass", manager.pass);


            }

        }
    }
    //show All Manager
    public List<Managers> GetAllManagers() {
        var manager = new List<Managers>();
        
        using (var conn = new SqlConnection(_connectionString))
        {
            //  string query = "SELECT * FROM Managers";
            var command = new SqlCommand("sp_getAllManager", conn);
            conn.Open ();
            command.CommandType = CommandType.StoredProcedure;
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    manager.Add(new Managers
                    {
                        mId = Convert.ToInt32(reader["mId"]),
                        mfirstName = reader["mfirstName"].ToString(),
                        mlastName = reader["mlastName"].ToString(),
                        email = reader["email"].ToString(),
                        pass = reader["pass"].ToString(),
                        mobileNo = reader["mobileNo"].ToString()

                    });
                }
            }
        }
        return manager;
    }
    //get by Id
   






}


