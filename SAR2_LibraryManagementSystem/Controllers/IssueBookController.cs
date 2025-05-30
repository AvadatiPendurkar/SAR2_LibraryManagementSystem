﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAR2_LibraryManagementSystem.Model;

namespace SAR2_LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssueBookController : ControllerBase
    {
        private readonly IssueBookDAL _issueBookDAL;

        public IssueBookController(IssueBookDAL issueBookDAL, EmailService emailService)
        {
            _issueBookDAL = issueBookDAL;
            EmailService = emailService;
        }
        public EmailService EmailService { get; }

        [HttpPost("AddIssuBook")]
        public IActionResult AddIssueBooks(IssueBook issueBook)
        {
            _issueBookDAL.AddIssueBooks(issueBook);
            return Ok(new { results =  "Book Issued Successfully" });
        }

        [HttpPut("updateIssuBook")]
        public IActionResult UpdateIssueBooks(IssueBook issueBook)
        {
            if (issueBook.issueId <= 0)
                return BadRequest("Invalid Issued Book id.");

            _issueBookDAL.UpdateIssueBooks(issueBook);
            return Ok(new { success = true, message = "Issued book updated successfully." });
        }

        //[HttpDelete("{issueId}")]
        //public IActionResult DeleteIssueBooks(int issueId)
        //{
        //    if (issueId <= 0)
        //        return BadRequest("Invalid issue book id");

        //    _issueBookDAL.DeleteIssueBook(issueId);
        //    return Ok(new { success = true, message = "Issue book deleted successfully" });
        //}

        [HttpGet("ViewAllIssueBook")]
        public IActionResult ViewAllIssueBook()
        {
            var issueBooks = _issueBookDAL.ViewIssuesBooks();
            return Ok(issueBooks);
        }

        [HttpGet("{id}")]
        public IActionResult GetIssueBookById(int id)
        {
            var issuebook = _issueBookDAL.ViewIssueBooksById(id);
            if(issuebook == null)
            {
                return NotFound(new { Message = $"Issue book with ID {id} not found." });
            }
            return Ok(issuebook);
        }

        [HttpPut("returnBook/{issueId}")]
        public IActionResult ReturnBook(int issueId)
        {
            try
            {
                _issueBookDAL.ReturnIssuedBook(issueId);
                return Ok(new { message = "Book returned successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to return book.", error = ex.Message });
            }
        }
    }
}
