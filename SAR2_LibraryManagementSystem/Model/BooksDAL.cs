using System.Data.SqlClient;
using System.Data;

namespace SAR2_LibraryManagementSystem.Model;

public class BooksDAL
{
    private readonly string _connectionString;

    //Connection String
    public BooksDAL(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    // View all Books
    public List<Books> ViewAllBooks()
    {
        var books = new List<Books>();
        using (var con = new SqlConnection(_connectionString))
        {
            var cmd = new SqlCommand("sp_GetAllBooks",con);
            con.Open();

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    books.Add(new Books
                    {
                        bookId = Convert.ToInt32(reader["bookId"]),
                        bookName = reader["bookName"].ToString(),
                        authorName = reader["authorName"].ToString(),
                        isbn = reader["isbn"].ToString(),
                        genre = reader["genre"].ToString(),
                        quantity = Convert.ToInt32(reader["quantity"])
                    });
                }             
            }
        }
        return books;
    }

    // Add new book
    public void AddBooks(Books book)
    {
        using (var con = new SqlConnection(_connectionString))
        {
            con.Open();

            //string sqlquery = "insert into Users(firstName, lastName, email, pass, mobileNo) values (@firstName, @lastName, @email, @pass, @mobileNo)";
            using (var cmd = new SqlCommand("sp_addBooks", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@bookName", book.bookName);
                cmd.Parameters.AddWithValue("@authorName", book.authorName);
                cmd.Parameters.AddWithValue("@isbn", book.isbn);
                cmd.Parameters.AddWithValue("@genre", book.genre);
                cmd.Parameters.AddWithValue("@quantity", book.quantity);

                int affectedrow = cmd.ExecuteNonQuery();
            }
        }
    }

    public void UpdateBooks(Books book)
    {
        using (var con= new SqlConnection(_connectionString))
        {
            con.Open();

            using (var cmd = new SqlCommand("sp_updateBooks", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@bookId", book.bookId);
                cmd.Parameters.AddWithValue("@bookName", book.bookName);
                cmd.Parameters.AddWithValue("@authorName", book.authorName);
                cmd.Parameters.AddWithValue("@isbn", book.isbn);
                cmd.Parameters.AddWithValue("@genre", book.genre);
                cmd.Parameters.AddWithValue("@quantity", book.quantity);

                int affectedrow = cmd.ExecuteNonQuery();
            }
        }
    }

    public void DeleteBooks(Books book)
    {
        using (var con = new SqlConnection(_connectionString))
        {
            con.Open();

            using (var cmd = new SqlCommand("sp_updateBooks", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@bookId", book.bookId);
                

                int affectedrow = cmd.ExecuteNonQuery();
            }
        }
    }


}
