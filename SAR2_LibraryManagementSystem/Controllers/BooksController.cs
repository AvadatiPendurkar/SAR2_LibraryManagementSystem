using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAR2_LibraryManagementSystem.Model;

namespace SAR2_LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BooksDAL _booksDAL;

        public BooksController(BooksDAL booksDAL)
        {
            _booksDAL = booksDAL;
        }

        [HttpGet]
        public IActionResult ViewAllBooks()
        {
            var books = _booksDAL.ViewAllBooks();
            return Ok(books);
        }

        [HttpGet("{bookId}")]
        public IActionResult ViewBookById(Books book)
        {
            _booksDAL.ViewBookById(book);
            return Ok(book);
        }

        [HttpPost("register")]
        public IActionResult AddBooks(Books books)
        {
            _booksDAL.AddBooks(books);
            return Ok("User added successfully");
        }

        [HttpPut("update")]
        public IActionResult UpdateBooks(Books book)
        {
            if (book.bookId <= 0)
                return BadRequest("Invalid user ID.");

            _booksDAL.UpdateBooks(book);
            return Ok(new { success = true, message = "User updated successfully." });
        }

        [HttpDelete("{bookId}")]
        public IActionResult DeleteUser(int bookId)
        {
            if (bookId <= 0)
                return BadRequest("Invalid user ID.");

            _booksDAL.DeleteBooks(bookId);
            return Ok(new { success = true, message = "User deleted successfully." });
        }
        
    }
}
