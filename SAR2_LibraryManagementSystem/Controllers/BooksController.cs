using Microsoft.AspNetCore.Authorization;
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
        public IActionResult UpdateBooks(int id, [FromBody] Books book)
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
    }
}
