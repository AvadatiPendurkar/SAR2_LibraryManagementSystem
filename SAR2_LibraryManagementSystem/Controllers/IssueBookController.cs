using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAR2_LibraryManagementSystem.Model;
using SAR2_LibraryManagementSystem.Repositories.Interfaces;

namespace SAR2_LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssueBookController : ControllerBase
    {
        private readonly IssueBookDAL _issueBookDAL;
        private readonly BooksDAL _booksDAL;
        private readonly DataAccessLayer _dataAccessLayer;

        public IssueBookController(EmailService emailService, IIssueBookRepository issueBookRepository, BooksDAL booksDAL,DataAccessLayer dataAccessLayer)
        {
            //_issueBookDAL = issueBookDAL;
            EmailService = emailService;
            _issueBookRepository = issueBookRepository;
            _booksDAL = booksDAL;
            _dataAccessLayer = dataAccessLayer;
        }
        private readonly IIssueBookRepository _issueBookRepository;
        public EmailService EmailService { get; }

        [HttpPost("AddIssuBook")]
        public async Task<IActionResult> AddIssueBooks(IssueBook issueBook)
        {
            // 1. Save the issue record
            _issueBookRepository.AddIssueBooks(issueBook);

            // 2. Fetch related book and user data
            var book = _booksDAL.ViewBookById(issueBook.bookId);
            var user = _dataAccessLayer.GetUsersById(issueBook.userId);

            if (book == null || user == null)
                return BadRequest("Book or User not found.");
            

            // 3. Compose and send the email
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
                Please make sure to return the book by <strong>{issueBook.dueDate:MMMM dd, yyyy}</strong> to avoid any late fees.
            </p>
            <p style="font-size: 16px; color: #555;">
                If you have any questions or need assistance, feel free to contact the library team.
            </p>
            <p style="font-size: 16px; color: #333; font-weight: bold;">Thanks,<br/>Digital Library Team</p>
        </div>
    </body>
</html>
""";

            await EmailService.SendEmailAsync(user.email, subject, body);
            return Ok(new { message = "Book Issued Successfully" });

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
    }
}
