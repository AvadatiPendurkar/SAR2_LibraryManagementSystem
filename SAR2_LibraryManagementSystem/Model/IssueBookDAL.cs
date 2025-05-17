using System.Data;
using System.Data.SqlClient;

namespace SAR2_LibraryManagementSystem.Model
{
    public class IssueBookDAL
    {
        public readonly string _connectionString;

        public IssueBookDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void AddIssueBooks(IssueBook issueBook)
        {
            using (var conn = new SqlConnection(_connectionString))
            {

                //string insertquery = "INSERT INTO IssueBook (userId, bookId, issueDate, dueDate, bookQty, status) VALUES (@userId, @bookId, @issueDate, @dueDate, @bookQty, @status)";
                conn.Open();
                using (var cmd = new SqlCommand("sp_issueBook", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userId", issueBook.userId);
                    cmd.Parameters.AddWithValue("@bookId", issueBook.bookId);
                    //cmd.Parameters.AddWithValue("@issueDate", issueBook.issueDate);
                    //cmd.Parameters.AddWithValue("@dueDate", issueBook.dueDate);
                    cmd.Parameters.AddWithValue("@bookQty", issueBook.bookQty);
                    cmd.Parameters.AddWithValue("@status", issueBook.status);

                    int affectedrow = cmd.ExecuteNonQuery();

                }

            }
        }
    }
}
