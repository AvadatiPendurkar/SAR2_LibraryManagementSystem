using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAR2_LibraryManagementSystem.Model;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace SAR2_LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataAccessLayer _dataAccessLayer;
        private readonly string _connectionString;

        //public UsersController(IConfiguration configuration)
        //{
        //    _connectionString = configuration.GetConnectionString("DefaultConnection");
        //}

        public UsersController(DataAccessLayer dataAccessLayer,EmailService emailService, IConfiguration configuration)
        {
            _dataAccessLayer = dataAccessLayer;
            _emailService = emailService;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public EmailService _emailService { get; }


        [HttpPost("register")]
        public async Task<IActionResult> AddUser(Users user)
        {
            if (_dataAccessLayer.IsEmailExists(user.email))
            {
                return Conflict(new { message = "Email already exists." }); // 409 Conflict
            }

            _dataAccessLayer.AddUser(user);

            //return Ok("User added successfully");
            //return Ok(new { success = true, message = "User added successfully" });
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
            return Ok(new { message = "User registered successfully." });

        }

        [HttpGet("email-exists")]
        public async Task<IActionResult> CheckEmailExists([FromQuery] string email)
        {
            var exists = await _dataAccessLayer.DoesEmailExistAsync(email);
            return Ok(exists);
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


        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtpEmail([FromBody] OtpEmailRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Otp))
                return BadRequest("Email and OTP are required.");

            var subject = "Your OTP Code";
            var body = $"Your OTP for password reset is: <strong>{request.Otp}</strong>";

            try
            {
                await _emailService.SendEmailAsync(request.Email, subject, body);
                return Ok(new { message = "OTP sent successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to send email: {ex.Message}");
            }
        }

        public class OtpEmailRequest
        {
            public string Email { get; set; }
            public string Otp { get; set; }
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
        [HttpGet("ViewbyId/{id}")]
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

        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateUser(int id, [FromBody] Users user)
        //{
        //    if (id != user.userId)
        //        return BadRequest();

        //    _dataAccessLayer.Entry(user).State = EntityState.Modified;
        //    await _dataAccessLayer.SavechnagesTask(user);
        //    return NoContent();
        //}


        [HttpGet("getUnauthorized")]
        public async Task<ActionResult<IEnumerable<Users>>> GetAuthorizedUsers()
        {
            var users = new List<Users>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT userId, firstName, lastName, email, mobileNo FROM Users WHERE isAuthorized = 0", connection);
                var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    users.Add(new Users
                    {
                        userId = reader.GetInt32(0),
                        firstName = reader.GetString(1),
                        lastName = reader.GetString(2),
                        email = reader.GetString(3),
                        mobileNo = reader.GetString(4)
                    });
                }
            }

            return Ok(users);
        }

        [HttpPut("authorized/{userId}")]
        public async Task<IActionResult> UpdateAuthorizationStatus(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = new SqlCommand("UPDATE Users SET IsAuthorized = 1 WHERE UserId = @UserId", connection);
                command.Parameters.AddWithValue("@UserId", userId);

                int rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected == 0)
                {
                    return NotFound($"User with ID {userId} not found.");
                }

                return NoContent(); // 204 No Content
            }
        }

        [HttpDelete("deleteRequestedUser/{userId}")]
        public IActionResult DeleteRequestedUser(int userId)
        {

            _dataAccessLayer.DeleteRequestedUser(userId);
            return Ok(new { success = true, message = "User deleted successfully." });
        }

        [HttpPost("addFeedback")]
        public async Task<IActionResult> AddFeedback(Feedback feedback)
        {
            _dataAccessLayer.AddFeedback(feedback);

            return Ok();
        }
        }
}
