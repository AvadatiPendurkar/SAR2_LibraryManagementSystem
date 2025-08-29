using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SAR2_LibraryManagementSystem.Model;
using static System.Reflection.Metadata.BlobBuilder;

namespace SAR2_LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BooksDAL _booksDAL;
        public readonly string _connectionString;

        public BooksController(BooksDAL booksDAL, IConfiguration configuration)
        {
            _booksDAL = booksDAL;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet("ViewAllBooks")]        
        public IActionResult ViewAllBooks()
        {
            var books = _booksDAL.ViewAllBooks();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public IActionResult GetBookById(int id)
        {
            var book = _booksDAL.ViewBookById(id);  

            if (book == null)
            {
                return NotFound(new { Message = $"Book with ID {id} not found." });
            }

            return Ok(book);  
        }

        [HttpPost("AddBook")]
       // [Authorize(Roles = "Admin, Manager")]
        public IActionResult AddBooks(Books books)
        {
            _booksDAL.AddBooks(books);
            return Ok("Book added successfully");
        }

        [HttpPut("{id}")]
      //  [Authorize(Roles = "Admin, Manager")]
        public IActionResult UpdateBooks(int id, [FromBody] UpdateBookDto book)
        {
            if (id <= 0 || id != book.bookId)
                return BadRequest("Invalid Book ID.");

            _booksDAL.UpdateBooks(book);
            return Ok(new { success = true, message = "Book updated successfully." });
        }

        [HttpDelete("{bookId}")]
        [Authorize(Roles = "Admin, Manager")]
        public IActionResult DeleteBooks(int bookId)
        {
            if (bookId <= 0)
                return BadRequest("Invalid Book ID.");

            _booksDAL.DeleteBooks(bookId);
            return Ok(new { success = true, message = "Book deleted successfully." });
        }

        [HttpGet("books-by-category/{genreId}")]
        public  IActionResult getByGener(int genreId)
        {
            //return Ok(new { success = true,
            var books = _booksDAL.ViewByGener(genreId);
            return Ok(books);
        }

        [HttpGet("popularBook")]
        public IActionResult getPopularBook()
        {
            var book = _booksDAL.GetMostIssuedBooksThisMonth();
            return Ok(book);
        }

        [HttpGet("getBookByGenre")]
        public IActionResult getBookByGenre()
        {
            var book = _booksDAL.GetBooksByGener();
            return Ok(book);
        }


        [HttpGet("books-paged")]
        public IActionResult GetBooksPaged(int pageNumber = 1, int pageSize = 10, 
            string excludeBookIds = "", string searchTerm = "",  int? userId = null)
        {
            var excludedIds = excludeBookIds?.Split(',')
                .Select(id => int.TryParse(id, out var parsed) ? parsed : (int?)null)
                .Where(id => id.HasValue)
                .Select(id => id.Value)
                .ToHashSet();

            var books = new List<Books>();
            int totalCount = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_GetPaginatedBooks", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);
                cmd.Parameters.AddWithValue("@ExcludedIds", excludeBookIds ?? "");
                cmd.Parameters.AddWithValue("@SearchTerm", searchTerm ?? "");
                cmd.Parameters.AddWithValue("@UserId", (object?)userId ?? DBNull.Value);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        books.Add(new Books
                        {
                            bookId = Convert.ToInt32(reader["bookId"]),
                            bookName = reader["bookName"].ToString(),
                            authorName = reader["authorName"].ToString(),
                            isbn = reader["isbn"].ToString(),
                            quantity = Convert.ToInt32(reader["quantity"]),
                            Base64Image = Convert.ToBase64String((byte[])reader["bookImage"]),
                            genreId = Convert.ToInt32(reader["genreId"]),
                            genre = reader["genre"].ToString(),
                            average_rating = reader["average_rating"] != DBNull.Value
                ? Convert.ToDouble(reader["average_rating"]) : 0,

                            total_ratings = reader["total_ratings"] != DBNull.Value
                ? Convert.ToInt32(reader["total_ratings"]) : 0,
                            isInWishlist = reader["isInWishlist"] != DBNull.Value
                        ? Convert.ToBoolean(reader["isInWishlist"]) : false
                        });
                    }

                    if (reader.NextResult() && reader.Read())
                    {
                        totalCount = Convert.ToInt32(reader["TotalCount"]);
                    }
                }
            }

            return Ok(new
            {
                data = books,
                totalRecords = totalCount
            });
        }


        [HttpGet("likedBooks")]
        public IActionResult getMostLikedBookd()
        {
            var book = _booksDAL.likedBooks();
            return Ok(book);
        }

        [HttpGet("RecentBooks")]
        public IActionResult RecentBooks()
        {
            var books = _booksDAL.RecentBooks();
            return Ok(books);
        }


        [HttpPost("rateBook")]
        public async Task<IActionResult> RateBook([FromBody] BookRating rating)
        {
            if (rating == null
                || rating.userId <= 0
                || rating.bookId <= 0
                || rating.ratings < 1
                || rating.ratings > 5)
            {
                return BadRequest(new { error = "Invalid rating data" });
            }

            try
            {
                await _booksDAL.RateBook(rating.userId, rating.bookId, rating.ratings);
                return Ok(new { message = "Rating saved successfully" });
            }
            catch (Exception ex)
            {
                // Ideally log the exception here
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
