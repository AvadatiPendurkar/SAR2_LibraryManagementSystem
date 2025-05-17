using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAR2_LibraryManagementSystem.Model;

namespace SAR2_LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssueBookController : ControllerBase
    {
        private readonly IssueBookDAL _issueBookDAL;

        public IssueBookController(IssueBookDAL issueBookDAL)
        {
            _issueBookDAL = issueBookDAL;
        }

        [HttpPost]
        public IActionResult AddIssueBooks(IssueBook issueBook)
        {
            _issueBookDAL.AddIssueBooks(issueBook);
            return Ok("Book Issued Successfully");
        }
    }
}
