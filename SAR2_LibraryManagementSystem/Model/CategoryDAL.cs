using System.Data;
using System.Data.SqlClient;

namespace SAR2_LibraryManagementSystem.Model
{
    public class CategoryDAL
    {
        private readonly string _connectionString;

        public CategoryDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        //add genre 
        public void AddGenre(Category category)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();

                using (var cmd = new SqlCommand("sp_addGenre", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@genre", category.genre);
                    int affectedrow = cmd.ExecuteNonQuery();
                }
            }
        }


        public List<Category> GetGenres()
        {
            var category = new List<Category>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("sp_viewGenre", connection);
                connection.Open();
                command.CommandType = CommandType.StoredProcedure;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        category.Add(new Category
                        {
                            genreId = Convert.ToInt32(reader["genreId"]),
                            genre = reader["genre"].ToString(),
                            
                        });
                    }
                }
            }
            return category;
        }

        public void DeleteGenre(int genreId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();

                using (var cmd = new SqlCommand("sp_deleteGenre", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@genreId", genreId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
