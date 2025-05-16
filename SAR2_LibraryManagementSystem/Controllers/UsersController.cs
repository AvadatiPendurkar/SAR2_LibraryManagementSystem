using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAR2_LibraryManagementSystem.Model;

namespace SAR2_LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataAccessLayer _dataAccessLayer;

        public UsersController(DataAccessLayer dataAccessLayer)
        {
            _dataAccessLayer = dataAccessLayer;
        }

        [HttpPost("register")]
        public IActionResult AddUser(Users user)
        {
            _dataAccessLayer.AddUser(user);
            return Ok("User added successfully");
        }

        [HttpPost("login")]
        public IActionResult Login(Login login)
        {
            if (login == null || string.IsNullOrWhiteSpace(login.email) || string.IsNullOrWhiteSpace(login.password))
            {
                return BadRequest("Email and password are required.");
            }
            if (_dataAccessLayer.LoginUser(login, out string message))
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
    }
}
