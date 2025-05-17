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

        public void UpdateIssueBooks(IssueBook issueBook)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();

                using (var cmd = new SqlCommand("", con))
                {
                    cmd.Parameters.AddWithValue("@issueId", issueBook.issueId);
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

        public void DeleteIssueBook(int issueBookId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();

                using (var cmd = new SqlCommand("",con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@issueId", issueBookId);

                    int affectedrow = cmd.ExecuteNonQuery();

                }
            }
        }

        public List<IssueBook> ViewIssuesBooks()
        {
            var IssueBooks = new List<IssueBook>();
            using (var con = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IssueBooks.Add(new IssueBook
                        {
                            issueId = Convert.ToInt32(reader["issueId"]),
                            userId = Convert.ToInt32(reader["userId"]),
                            bookId = Convert.ToInt32(reader["bookId"]),
                            issueDate = Convert.ToDateTime(reader["issueDate"]),
                            dueDate = Convert.ToDateTime(reader["dueDate"]),
                            bookQty = Convert.ToInt32(reader["bookQty"]),
                            status = reader["status"].ToString()
                        });
                    }
                }
            }
            return IssueBooks;
        }

        public IssueBook ViewIssueBooksById(int issueId)
        {
            IssueBook issueBook = null;
            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();

                using (var cmd = new SqlCommand("", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@issueId", issueId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            issueBook = new IssueBook
                            {
                                issueId = Convert.ToInt32(reader["issueId"]),
                                userId = Convert.ToInt32(reader["userId"]),
                                bookId = Convert.ToInt32(reader["bookId"]),
                                issueDate = Convert.ToDateTime(reader["issueDate"]),
                                dueDate = Convert.ToDateTime(reader["dueDate"]),
                                bookQty = Convert.ToInt32(reader["bookQty"]),
                                status = reader["status"].ToString()
                            };
                        }
                    }
                }
                return issueBook;
            }
        }
    }
}
