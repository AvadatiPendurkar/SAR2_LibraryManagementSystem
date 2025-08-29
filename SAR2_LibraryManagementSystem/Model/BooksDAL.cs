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
            var cmd = new SqlCommand("sp_newViewAllBooks", con);
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
                        genreId = Convert.ToInt32(reader["genreId"]),
                        quantity = Convert.ToInt32(reader["quantity"]),

                        Base64Image = Convert.ToBase64String((byte[])reader["bookImage"]),

                        genre = reader["genre"].ToString()
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
                    while (reader.Read())  
                    {
                        book = new Books
                        {
                            bookId = Convert.ToInt32(reader["bookId"]),
                            bookName = reader["bookName"].ToString(),
                            authorName = reader["authorName"].ToString(),
                            isbn = reader["isbn"].ToString(),
                            genreId = Convert.ToInt32(reader["genreId"]),
                            quantity = Convert.ToInt32(reader["quantity"]),
                            Base64Image = Convert.ToBase64String((byte[])reader["bookImage"])
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
       
            byte[] imageBytes = Convert.FromBase64String(book.Base64Image);
            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();
                using (var cmd = new SqlCommand("sp_addBooks", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@bookName", book.bookName);
                    cmd.Parameters.AddWithValue("@authorName", book.authorName);
                    cmd.Parameters.AddWithValue("@isbn", book.isbn);
                    cmd.Parameters.AddWithValue("@genreId", book.genreId);
                    cmd.Parameters.AddWithValue("@quantity", book.quantity);
                    cmd.Parameters.AddWithValue("@bookImage", imageBytes);

                int affectedrow = cmd.ExecuteNonQuery();
                }
            }
        }
   
    //update books
    public void UpdateBooks(UpdateBookDto book)
    {
        byte[] imageBytes = Convert.FromBase64String(book.Base64Image);
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
                cmd.Parameters.AddWithValue("@genreId", book.genreId);
                cmd.Parameters.AddWithValue("@quantity", book.quantity);
                cmd.Parameters.AddWithValue("@bookImage", imageBytes);

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
     // view by gener 
    public  List <Books> ViewByGener(int genreId)
    {

        var book = new List<Books>();
        using (var con = new SqlConnection(_connectionString))
        {
            con.Open();
            using (var cmd = new SqlCommand("sp_viewBooksByGenre", con)) 
            {
                cmd.CommandType= CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@genreId", genreId);
           
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        book.Add(new Books
                        {
                            bookId = Convert.ToInt32(reader["bookId"]),
                            bookName = reader["bookName"].ToString(),
                            authorName = reader["authorName"].ToString(),
                            isbn = reader["isbn"].ToString(),
                            genreId = Convert.ToInt32(reader["genreId"]),
                            quantity = Convert.ToInt32(reader["quantity"]),
                            Base64Image = Convert.ToBase64String((byte[])reader["bookImage"])
                        });
                    }
                }  
            }
        }
        return book;
    }

    public List<Books> GetMostIssuedBooksThisMonth()
    {
        List<Books> popularBooks = new List<Books>();

        using (SqlConnection con = new SqlConnection(_connectionString))
        {
            con.Open();

            using (SqlCommand cmd = new SqlCommand("sp_popularBookM", con))
            {

                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        popularBooks.Add(new Books
                        {
                            bookId = Convert.ToInt32(reader["bookId"]),
                            bookName = reader["bookName"].ToString(),
                            authorName = reader["authorName"].ToString(),
                            genreId = Convert.ToInt32(reader["genreId"]),
                            Base64Image = Convert.ToBase64String((byte[])reader["bookImage"])
                        });
                    }
                }
            }
        }
        return popularBooks;
    }

    public List<Books> GetBooksByGener()
    {
        List<Books> PopularGener = new List<Books>();

        using(SqlConnection con = new SqlConnection(_connectionString))
        {
            con.Open();

            using(SqlCommand cmd = new SqlCommand("sp_popularBooksByGenre", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PopularGener.Add(new Books
                        {
                            bookId = Convert.ToInt32(reader["bookId"]),
                            bookName = reader["bookName"].ToString(),
                            authorName = reader["authorName"].ToString(),
                            genreId = Convert.ToInt32(reader["genreId"]),
                            genre = reader["genre"].ToString(),
                            Base64Image = Convert.ToBase64String((byte[])reader["bookImage"])
                        });
                    }
                }
            }
        }
        return PopularGener;
    }


    //most Likes Books

    public List<Books> likedBooks()
    {
        List<Books> books = new List<Books>();

        using (SqlConnection con = new SqlConnection(_connectionString))
        {
            con.Open();
            using (SqlCommand cmd = new SqlCommand("sp_likedBooks", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        books.Add(new Books
                        {
                            bookId = Convert.ToInt32(reader["bookId"]),
                            bookName = reader["bookName"].ToString(),
                            authorName = reader["authorName"].ToString(),
                            genre = reader["genre"].ToString(),
                            Base64Image = Convert.ToBase64String((byte[])reader["bookImage"])

                        });
                    }
                }
            }
        }
       return books;
    }


    public List<Books> RecentBooks()
    {
        var books = new List<Books>();

        using (var con = new SqlConnection(_connectionString))
        {
            var cmd = new SqlCommand("sp_recentBooks", con);
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
                        Base64Image = Convert.ToBase64String((byte[])reader["bookImage"])
                    });
                }
            }
        }
        return books;
    }

    public async Task RateBook(int userId, int bookId, int ratings)
    {
        await using var con = new SqlConnection(_connectionString);
        await con.OpenAsync().ConfigureAwait(false);
        await using var cmd = new SqlCommand("sp_rate_book", con)
        {

            CommandType = CommandType.StoredProcedure
        };
        cmd.Parameters.AddWithValue("@userId", userId);
        cmd.Parameters.AddWithValue("@bookId", bookId);
        cmd.Parameters.AddWithValue("@ratings", ratings);


        await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);

    }


}
