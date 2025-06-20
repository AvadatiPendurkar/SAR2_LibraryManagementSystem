using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography.X509Certificates;
namespace SAR2_LibraryManagementSystem.Model;

public class ManagerDAL
{

    public readonly string _connectionString;

    public ManagerDAL(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }
    //add manager
    public void AddManagers(Managers manager)
    {
        using (var conn = new SqlConnection(_connectionString))
        {

            conn.Open();
            using (var cmd = new SqlCommand("sp_AddManager", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mfirstName", manager.mfirstName);
                cmd.Parameters.AddWithValue("@mlastName", manager.mlastName);
                cmd.Parameters.AddWithValue("@email", manager.email);
                cmd.Parameters.AddWithValue("@mobileNo", manager.mobileNo);
                cmd.Parameters.AddWithValue("@pass", manager.pass);
                cmd.Parameters.AddWithValue("@isAuthorized", manager.isAuthorized);

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
        
            using (var cmd = new SqlCommand("sp_updateManager", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mId", manager.mId);
                cmd.Parameters.AddWithValue("@mfirstName", manager.mfirstName);
                cmd.Parameters.AddWithValue("@mlastName", manager.mlastName);
                cmd.Parameters.AddWithValue("@email", manager.email);
                cmd.Parameters.AddWithValue("@mobileNo", manager.mobileNo);
                //cmd.Parameters.AddWithValue("@pass", manager.pass);

                int affectedrow = cmd.ExecuteNonQuery();

            }

        }
    }
    //show All Manager
    public List<Managers> GetAllManagers()
    {
        var manager = new List<Managers>();

        using (var conn = new SqlConnection(_connectionString))
        {
        
            var command = new SqlCommand("sp_viewAllManager", conn);
            conn.Open();
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
                        mobileNo = reader["mobileNo"].ToString(),
                        isAuthorized = Convert.ToBoolean(reader["isAuthorized"])
                    });
                }
            }
        }
        return manager;
    }
    //view by Id

    public Managers GetManagerById(int mId)
    {
        Managers manager = null;
        using (var conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            using (var cmd = new SqlCommand("sp_viewManagerById", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mId", mId);
                using (var reader = cmd.ExecuteReader())
                {

                    if (reader.Read())
                    {
                        manager = new Managers
                        {
                            mId = Convert.ToInt16(reader["mId"]),
                            mfirstName = reader["mfirstName"].ToString(),
                            mlastName = reader["mlastName"].ToString(),
                            email = reader["email"].ToString(),
                            pass = reader["pass"].ToString(),
                            mobileNo = reader["mobileNo"].ToString(),
                            isAuthorized = Convert.ToBoolean(reader["isAuthorized"])
                        };
                    }                   
                }
            }
        }
        return manager;
    }
    // delete manager by Id
    public void DeleteManager(int mId)
    {

        using (var conn = new SqlConnection(_connectionString))
        {
            conn.Open();

            using (var cmd = new SqlCommand("sp_deleteManager", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mId", mId);

                int effectedrow = cmd.ExecuteNonQuery();
            }
        }
    }

    public void DeleteRequestedUser(int mId)
    {
        using (var con = new SqlConnection(_connectionString))
        {
            con.Open();            
            using (var cmd = new SqlCommand("sp_deleteRequestedManager", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mId", mId);

                cmd.ExecuteNonQuery();
            }
        }
    }

    public bool IsEmailExists(string email)
    {
        using (SqlConnection con = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand("sp_isManagerEmailExist", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Email", email);

            con.Open();
            int count = (int)cmd.ExecuteScalar();
            return count > 0;
        }
    }

    //emil exist
    public async Task<bool> DoesEmailExistAsync(string email)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            using (SqlCommand cmd = new SqlCommand("sp_doesEmailExist", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", email);

                await conn.OpenAsync();
                int count = (int)await cmd.ExecuteScalarAsync();

                return count > 0;
            }
        }
    }

}



