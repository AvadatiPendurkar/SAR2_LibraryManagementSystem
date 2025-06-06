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

        public IssueBookController(EmailService emailService, IIssueBookRepository issueBookRepository)
        {
            //_issueBookDAL = issueBookDAL;
            EmailService = emailService;
            _issueBookRepository = issueBookRepository;
        }
        private readonly IIssueBookRepository _issueBookRepository;
        public EmailService EmailService { get; }

        [HttpPost("AddIssuBook")]
        public IActionResult AddIssueBooks(IssueBook issueBook)
        {
            _issueBookRepository.AddIssueBooks(issueBook);
            return Ok(new { results = "Book Issued Successfully" });
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
