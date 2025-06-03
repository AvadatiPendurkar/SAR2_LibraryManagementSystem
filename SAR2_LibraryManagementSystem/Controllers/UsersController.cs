using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAR2_LibraryManagementSystem.Model;

namespace SAR2_LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataAccessLayer _dataAccessLayer;

        public UsersController(DataAccessLayer dataAccessLayer,EmailService emailService)
        {
            _dataAccessLayer = dataAccessLayer;
            _emailService = emailService;
        }
        public EmailService _emailService { get; }


        [HttpPost("register")]
        public async Task<IActionResult> AddUser(Users user)
        {
            _dataAccessLayer.AddUser(user);
            const string subject = "Account Created";
            var body = $"""
                <html>
                    <body>
                        <h1>Hello, {user.firstName} {user.lastName}</h1>
                        <h2>
                            Your account has been created and we have sent approval request to admin. 
                            Once the request is approved by admin you will receive email, and you will be
                            able to login in to your account.
                        </h2>
                        <h3>Thanks</h3>
                    </body>
                </html>
            """;
            await _emailService.SendEmailAsync(user.email, subject, body);
            
            //return Ok("User added successfully");
            return Ok(new { success = true, message = "User added successfully" });
        }

        [HttpPost("login")]
        public IActionResult Login(Login login)
        {
            if (login == null || string.IsNullOrWhiteSpace(login.email) || string.IsNullOrWhiteSpace(login.password))
            {
                return BadRequest("Email and password are required.");
            }
            if (_dataAccessLayer.LoginUser(login, out string message) )
            {
                return Ok(new { success = true, message });
            }
            
            else
            {
                return Unauthorized(new { success = false, message });
            }
        }

        [HttpPut("update")]
        public IActionResult UpdateUser(Users user)
        {
            if (user.userId <= 0)
                return BadRequest("Invalid user ID.");

            _dataAccessLayer.UpdateUser(user);
            return Ok(new { success = true, message = "User updated successfully." });
        }

        [HttpGet("allusers")]
        public IActionResult GetAllUsers()
        {
            var users = _dataAccessLayer.GetAllUsers();
            return Ok(users);
        }

        [HttpDelete("{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            if (userId <= 0)
                return BadRequest("Invalid user ID.");

            _dataAccessLayer.DeleteUser(userId);
            return Ok(new { success = true, message = "User deleted successfully." });
        }
        // view by id
        [HttpGet("ViewbyId")]
        public IActionResult GetUsersById(int id)
        {
            var user=_dataAccessLayer.GetUsersById(id);
            if(user == null)
            {
                return NotFound(new { Message = $"User with ID{id} not found" });
            }
            return Ok(user);
        }

        // Block User
        [HttpPut("block/{id}")]
        public IActionResult BlockUser(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid user ID.");

            _dataAccessLayer.BlockUser(id);
            return Ok(new { success = true, message = $"User with ID {id} has been blocked." });
        }

        // Unblock User
        [HttpPut("unblock/{id}")]
        public IActionResult UnblockUser(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid user ID.");

            _dataAccessLayer.UnblockUser(id);
            return Ok(new { success = true, message = $"User with ID {id} has been unblocked." });
        }





    }
}
