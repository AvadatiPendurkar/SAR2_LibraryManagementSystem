using System.Data.SqlClient;
using System.Data;
using System.Net;

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
            var cmd = new SqlCommand("sp_viewAllBooks", con);
            cmd.CommandType = CommandType.StoredProcedure;
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

    //View book by ID
    public Books ViewBookById(int bookId)
    {
        Books book = null;
        using (var con = new SqlConnection(_connectionString))
        {
            con.Open();

            using (var cmd = new SqlCommand("sp_viewBookById", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@bookId", bookId);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())  
                    {
                        book = new Books
                        {
                            bookId = Convert.ToInt32(reader["bookId"]),
                            bookName = reader["bookName"].ToString(),
                            authorName = reader["authorName"].ToString(),
                            isbn = reader["isbn"].ToString(),
                            genre = reader["genre"].ToString(),
                            quantity = Convert.ToInt32(reader["quantity"])
                        };
                    }
                }

            }
        }
        return book;
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
    //update books
    public void UpdateBooks(Books book)
    {
        using (var con= new SqlConnection(_connectionString))
        {
            con.Open();

            using (var cmd = new SqlCommand("sp_updateBook", con))
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
    // delete books
    public void DeleteBooks(int bookId)
    {
        using (var con = new SqlConnection(_connectionString))
        {
            con.Open();

            using (var cmd = new SqlCommand("sp_deleteBook", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@bookId", bookId);                 

                int affectedrow = cmd.ExecuteNonQuery();
            }
        }
    }

}
