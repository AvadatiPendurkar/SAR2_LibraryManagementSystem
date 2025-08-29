using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SAR2_LibraryManagementSystem.Model;
using SAR2_LibraryManagementSystem.Repositories.Interfaces;

namespace SAR2_LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssueBookController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IssueBookDAL _issueBookDAL;
        private readonly BooksDAL _booksDAL;
        private readonly DataAccessLayer _dataAccessLayer;

        public IssueBookController(IConfiguration configuration,EmailService emailService, IIssueBookRepository issueBookRepository, BooksDAL booksDAL,DataAccessLayer dataAccessLayer)
        {
            //_issueBookDAL = issueBookDAL;
            EmailService = emailService;
            _issueBookRepository = issueBookRepository;
            _booksDAL = booksDAL;
            _dataAccessLayer = dataAccessLayer;
            _configuration = configuration;
        }
        private readonly IIssueBookRepository _issueBookRepository;
        public EmailService EmailService { get; }

        [HttpPost("AddIssuBook")]
        public async Task<IActionResult> AddIssueBooks([FromBody] IssueBook issueBook)
        {
            try
            {
                // 1. Save the issue record
                _issueBookRepository.AddIssueBooks(issueBook); // should use a new SqlConnection per call

                // 2. Get book and user safely
                var book = _booksDAL.ViewBookById(issueBook.bookId);
                var user = _dataAccessLayer.GetUsersById(issueBook.userId);

                if (book == null || user == null)
                    return BadRequest("Book or User not found.");

                // 3. Compose the email
                const string subject = "Book Issued Successfully";
                var body = $"""
<html>
    <body style="font-family: Arial, sans-serif; line-height: 1.6; background-color: #f9f9f9; padding: 20px;">
        <div style="max-width: 600px; margin: auto; background-color: #ffffff; border-radius: 8px; padding: 30px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);">
            <h1 style="font-size: 24px; color: #333;">Hello, {user.firstName} {user.lastName}</h1>
            <p style="font-size: 16px; color: #555;">
                The book titled <strong>{book.bookName}</strong> has been successfully issued to your account on <strong>{DateTime.Now:MMMM dd, yyyy}</strong>.
            </p>
            <p style="font-size: 16px; color: #555;">
                Please return it by <strong>{issueBook.dueDate:MMMM dd, yyyy}</strong> to avoid late fees.
            </p>
            <p style="font-size: 16px; color: #333; font-weight: bold;">Thanks,<br/>Digital Library Team</p>
        </div>
    </body>
</html>
""";

                // 4. Try sending email
                try
                {
                    Console.WriteLine($"Sending email to: {user.email}");
                    await EmailService.SendEmailAsync(user.email, subject, body);
                    Console.WriteLine("Email sent.");
                }
                catch (Exception emailEx)
                {
                    // Don't block issue success if email fails
                    Console.WriteLine("❌ Email send failed: " + emailEx.ToString());
                }

                return Ok(new { message = "Book Issued Successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Backend error in AddIssueBooks: " + ex.ToString());
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }



        [HttpPut("updateIssuBook")]
        public IActionResult UpdateIssueBooks(IssueBook issueBook)
        {
            if (issueBook.issueId <= 0)
                return BadRequest("Invalid Issued Book id.");

            _issueBookRepository.UpdateIssueBooks(issueBook);
            return Ok(new { success = true, message = "Issued book updated successfully." });
        }

        [HttpGet("ViewAllIssueBook")]
        public IActionResult ViewAllIssueBook()
        {
            var issueBooks = _issueBookRepository.ViewIssuesBooks();
            return Ok(issueBooks);
        }

        [HttpGet("{id}")]
        public IActionResult GetIssueBookById(int id)
        {
            var issueBook = _issueBookRepository.ViewIssueBooksById(id);
            if (issueBook == null)
                return NotFound(new { Message = $"Issue book with ID {id} not found." });

            return Ok(issueBook);
        }

        [HttpPut("returnBook/{issueId}")]
        public IActionResult ReturnBook(int issueId)
        {
            try
            {
                _issueBookRepository.ReturnIssuedBook(issueId);
                return Ok(new { message = "Book returned successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to return book.", error = ex.Message });
            }
        }

        [HttpGet("issue-book-report")]
        public async Task<ActionResult<IEnumerable<IssueBookReportDto>>> GetIssueBookReport()
        {
            var reportList = new List<IssueBookReportDto>();

            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Sp_IssueBookReport", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            reportList.Add(new IssueBookReportDto
                            {
                                UserId = reader.GetInt32(reader.GetOrdinal("userId")),
                                FirstName = reader.GetString(reader.GetOrdinal("firstName")),
                                LastName = reader.GetString(reader.GetOrdinal("lastName")),
                                TotalBooksIssued = reader.GetInt32(reader.GetOrdinal("TotalBooksIssued"))
                            });
                        }
                    }
                }
            }

            return Ok(reportList);
        }


        [HttpGet("top-genres")]
        public async Task<IActionResult> GetTop3GenresByBirthdate([FromQuery] DateTime birthdate)
        {
            var results = new List<GenreRecommendation>();

            using (var con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (var cmd = new SqlCommand("sp_RecommendTop3GenresByAge", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@birthdate", birthdate);

                    await con.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            results.Add(new GenreRecommendation
                            {
                                GenreId = reader.GetInt32(reader.GetOrdinal("genreId")),
                                Genre = reader.GetString(reader.GetOrdinal("genre")),
                                TotalIssuedBooks = reader.GetInt32(reader.GetOrdinal("totalIssuedBooks"))
                            });
                        }
                    }
                }
            }

            return Ok(results);
        }


        [HttpGet("recommend-books")]
        public async Task<IActionResult> GetRecommendedBooksByUserId([FromQuery] int userId, [FromQuery] string searchTerm = "")
        {
            var books = new List<Books>();
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            // 1. Get user birthdate
            DateTime? birthdate = null;
            await using (var con = new SqlConnection(connectionString))
            await using (var cmd = new SqlCommand("Sp_getbirthDate", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userId", userId);
                await con.OpenAsync();

                var result = await cmd.ExecuteScalarAsync();
                if (result != null && result != DBNull.Value)
                    birthdate = Convert.ToDateTime(result);
            }

            if (birthdate == null)
                return NotFound("User or birthdate not found.");

            // 2. Get top 3 genreIds based on age
            List<int> topGenreIds = new();
            await using (var con = new SqlConnection(connectionString))
            await using (var cmd = new SqlCommand("sp_RecommendTop3GenresByAge", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@birthdate", birthdate);
                await con.OpenAsync();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    topGenreIds.Add(reader.GetInt32(reader.GetOrdinal("genreId")));
                }
            }

            if (!topGenreIds.Any())
                return Ok(new List<Books>());

            // 3. Get recommended books from top genres with isInWishlist
            await using (var con = new SqlConnection(connectionString))
            await using (var cmd = new SqlCommand("sp_GetBooksByGenreList", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@genreIds", string.Join(",", topGenreIds));
                cmd.Parameters.AddWithValue("@SearchTerm", string.IsNullOrWhiteSpace(searchTerm) ? DBNull.Value : searchTerm);
                cmd.Parameters.AddWithValue("@UserId", userId); // ✅ Pass userId for wishlist check

                await con.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    books.Add(new Books
                    {
                        bookId = reader.GetInt32(reader.GetOrdinal("bookId")),
                        bookName = reader.GetString(reader.GetOrdinal("bookName")),
                        authorName = reader.GetString(reader.GetOrdinal("authorName")),
                        genreId = reader.GetInt32(reader.GetOrdinal("genreId")),
                        isbn = reader.GetString(reader.GetOrdinal("isbn")),
                        Base64Image = reader["bookImage"] != DBNull.Value
                            ? Convert.ToBase64String((byte[])reader["bookImage"])
                            : null,
                        genre = reader.GetString(reader.GetOrdinal("genre")),
                        average_rating = reader["average_rating"] != DBNull.Value
                            ? Convert.ToDouble(reader["average_rating"]) : 0,
                        total_ratings = reader["total_ratings"] != DBNull.Value
                            ? Convert.ToInt32(reader["total_ratings"]) : 0,

                        // ✅ Read isInWishlist value
                        isInWishlist = reader["isInWishlist"] != DBNull.Value
                            ? Convert.ToBoolean(reader["isInWishlist"]) : false
                    });
                }
            }

            return Ok(books);
        }

    }

}

