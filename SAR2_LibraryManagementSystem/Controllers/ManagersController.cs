using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SAR2_LibraryManagementSystem.Model;
using System.Data;

namespace SAR2_LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagersController : ControllerBase
    {
        private readonly ManagerDAL _managerDAL;
        private readonly string _connectionString;

        private readonly IRepo<Managers> managerRepo;

        public ManagersController(ManagerDAL managerDAL, IRepo<Managers> _managerRepo,EmailService emailService,IConfiguration configuration)
        {
            _managerDAL = managerDAL;
            managerRepo = _managerRepo;
            _emailService = emailService;
                _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public EmailService _emailService { get; }


        [HttpPost("registerByAdmin")]
        public async Task<IActionResult> AddManagerByAdmin(Managers manager)
        {

            //_managerDAL.AddManagers(manager);
            //return Ok(new { message = "Manager registered successfully" });
            try
            {
                // your ADO.NET insert logic here...
                managerRepo.Save(manager);
                _managerDAL.AddManagers(manager);



                const string subject1 = "🎉 Welcome Aboard! Your Manager Account Has Been Created";
                var body = $"""
<html>
  <body style="font-family: Arial, sans-serif; line-height: 1.6; background-color: #f9f9f9; padding: 20px;">
    <div style="max-width: 600px; margin: auto; background-color: #ffffff; border-radius: 8px; padding: 30px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);">
      <h1 style="font-size: 24px; color: #333;">Hello, {manager.mfirstName} {manager.mlastName},</h1>
      <p style="font-size: 16px; color: #555;">
        You have been successfully added as a <strong>Manager</strong> in our Digital Library system.
      </p>
      <p style="font-size: 16px; color: #555;">
        Here are your login credentials:
      </p>
      <ul style="font-size: 16px; color: #555;">
        <li><strong>EmailId (ID):</strong> {manager.email}</li>
        <li><strong>Temporary Password:</strong> {manager.pass}</li>
      </ul>
      <p style="font-size: 16px; color: #555;">
        For security reasons, please log in and <strong>change your password</strong> as soon as possible.
      </p>
      <p style="font-size: 16px; color: #555;">
        If you have any questions or need assistance, feel free to contact us.
      </p>
      <p style="font-size: 16px; color: #333; font-weight: bold;">
        Thanks,<br/>
        Digital Library Team
      </p>
    </div>
  </body>
</html>
""";

                await _emailService.SendEmailAsync(manager.email, subject1, body);
                return Ok(new { message = "Manager registered successfully." });

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }


        [HttpPost("register")]
        public async Task<IActionResult> AddManagers(Managers manager)
        {

            //_managerDAL.AddManagers(manager);
            //return Ok(new { message = "Manager registered successfully" });
            try
            {
                // your ADO.NET insert logic here...
                managerRepo.Save(manager);
                _managerDAL.AddManagers(manager);

               

                const string subject = "Account Created ";
                var body = $"""
            <html>
                <body style="font-family: Arial, sans-serif; line-height: 1.6; background-color: #f9f9f9; padding: 20px;">
                    <div style="max-width: 600px; margin: auto; background-color: #ffffff; border-radius: 8px; padding: 30px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);">
                        <h1 style="font-size: 24px; color: #333;">Hello, {manager.mfirstName} {manager.mlastName}</h1>
                             <p style="font-size: 16px; color: #555;">
                               Your account has been created and an approval request has been sent to the admin.
                                Once your request is approved, you will receive a confirmation email and will be able to log in to your account.
                             </p>
                             <p style="font-size: 16px; color: #555;">
                               If you have any questions, feel free to contact us.
                             </p>
                               <p style="font-size: 16px; color: #333; font-weight: bold;">Thanks,<br/>Digital Library Team</p>
                    </div>
                </body>
            </html>
            """;

                await _emailService.SendEmailAsync(manager.email, subject, body);

                const string subject2 = "New Manager Registration Request";

                var body2 = $"""
<html>
    <body style="font-family: Arial, sans-serif; line-height: 1.6; background-color: #f9f9f9; padding: 20px;">
        <div style="max-width: 600px; margin: auto; background-color: #ffffff; border-radius: 8px; padding: 30px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);">
            <h1 style="font-size: 24px; color: #333;">New Manager Account Request</h1>
            <p style="font-size: 16px; color: #555;">
                A new manager has registered and is awaiting your approval.
            </p>
            <p style="font-size: 16px; color: #555;">
                <strong>Name:</strong> {manager.mfirstName} {manager.mlastName}<br/>
                <strong>Email:</strong> {manager.email}
            </p>
            <p style="font-size: 16px; color: #555;">
                Please review the request and authorize the account if appropriate.
            </p>
            <p style="font-size: 16px; color: #333; font-weight: bold;">
                Thanks,<br/>Digital Library System
            </p>
        </div>
    </body>
</html>
""";

                await _emailService.SendEmailAsync("riteshalim237@gmail.com", subject2, body2);
                return Ok(new { message = "Manager registered successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }
        [HttpGet("get")]
        public IActionResult GetAllManagers()
        {
            var manager = _managerDAL.GetAllManagers();
            return Ok(manager);

        }


        [HttpPut("update/{mId}")]
        public IActionResult UpdateManager(int mId, [FromBody] Managers managers)
        {
            //Console.WriteLine($"Route ID: {id}, Body ID: {manager?.mId}");

            if (mId == null)
                return BadRequest(new { error = "Manager data is missing" });

            _managerDAL.UpdateManager(managers);
            return Ok(new { success = "Manager updated (even if ID mismatch)" });
        }


        [HttpPut("updatePassword/{mId}")]
        public IActionResult UpdateManagerPassword(int mId, [FromBody] ManagerPasswordUpdateDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.newPass))
                return BadRequest(new { error = "New password is required." });

            _managerDAL.UpdateManagerPassword(mId, dto.newPass);
            return Ok(new { success = "Password updated successfully." });
        }


        //[HttpPut("updatePassword/{id}")]
        //public IActionResult UpdateManagerPassword(int id, [FromBody] Managers manager)
        //{
        //    Console.WriteLine($"Route ID: {id}, Body ID: {manager?.mId}");

        //    if (manager == null)
        //    {
        //        return BadRequest(new { error = "Manager data is missing" });
        //    }
        //    _managerDAL.UpdateManagerPassword(manager);
        //    return Ok(new { success = "Manager updated (even if ID mismatch)" });
        //}



        [HttpDelete("delete/{mId}")]
        public IActionResult DeleteManager(int mId)
        {
            if (mId <= 0)
            { return BadRequest("Invalid Id"); }

            _managerDAL.DeleteManager(mId);
            return Ok(new { succes = true, message = "Manager Deleted Succesfully" });
        }

        [HttpGet("getById/{id}")]
        public IActionResult GetManagerById(int id)
        {
            var manager = _managerDAL.GetManagerById(id);
            if(manager == null)
            {
                return NotFound(new {message =$"Manager with ID {id} not found." });

            }
            return Ok(manager);
        }

        [HttpGet("getUnAuthorizedManager")]
        public async Task<ActionResult<IEnumerable<Managers>>> unAuthorizedManager()
        {
            var managers = new List<Managers>();

            using (var connection = new SqlConnection(_connectionString))
            {
                //connection.Open();
                await connection.OpenAsync();
                var command = new SqlCommand("Sp_unAuthorizedManager", connection);
                command.CommandType = CommandType.StoredProcedure;
                var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    managers.Add(new Managers
                    {
                        mId = reader.GetInt32(0),
                        mfirstName = reader.GetString(1),
                        mlastName = reader.GetString(2),
                        email = reader.GetString(3),
                        mobileNo = reader.GetString(4)
                    });
                }
            }

            return Ok(managers);
        }

        [HttpPut("authorized/{mId}")]
        public async Task<IActionResult> UpdateAuthorizationStatus(int mId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Step 1: Get manager details
                var selectCommand = new SqlCommand("Sp_UpdateAuthorizationStatus", connection);
                selectCommand.CommandType = CommandType.StoredProcedure;
                selectCommand.Parameters.AddWithValue("@mId", mId);

                string firstName = null;
                string lastName = null;
                string email = null;

                using (var reader = await selectCommand.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        firstName = reader["mfirstName"].ToString();
                        lastName = reader["mlastName"].ToString();
                        email = reader["email"].ToString();
                    }
                    else
                    {
                        return NotFound($"Manager with ID {mId} not found.");
                    }
                }

                // Step 2: Update IsAuthorized status
                var updateCommand = new SqlCommand("Sp_UpdateAuthorizationStatus1", connection);
                updateCommand.CommandType = CommandType.StoredProcedure;
                updateCommand.Parameters.AddWithValue("@mId", mId);

                await updateCommand.ExecuteNonQueryAsync();

                // Step 3: Send Email
                const string subject = "Your Account Request Has Been Approved";

                var body = $"""
<html>
    <body style="font-family: Arial, sans-serif; line-height: 1.6; background-color: #f9f9f9; padding: 20px;">
        <div style="max-width: 600px; margin: auto; background-color: #ffffff; border-radius: 8px; padding: 30px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);">
            <h1 style="font-size: 24px; color: #333;">Hello, {firstName} {lastName}</h1>
            <p style="font-size: 16px; color: #555;">
                Your account request has been accepted by the admin.
            </p>
            <p style="font-size: 16px; color: #555;">
                You can now log in to your account using your registered email and password.
            </p>

            <!-- Login Button -->
            <p style="text-align: center; margin: 30px 0;">
                <a href="http://localhost:4200/login" 
                   style="background-color: #007bff; color: #ffffff; padding: 12px 24px; text-decoration: none; border-radius: 5px; display: inline-block; font-size: 16px;">
                    Log In Now
                </a>
            </p>

            <p style="font-size: 16px; color: #555;">
                If you have any questions or need assistance, feel free to contact our support team.
            </p>
            <p style="font-size: 16px; color: #333; font-weight: bold;">
                Thanks,<br/>Digital Library Team
            </p>
        </div>
    </body>
</html>
""";

                await _emailService.SendEmailAsync(email, subject, body);



                //return NoContent();
                return Ok(new { message = "Manager registered successfully." });
            }
        }


        [HttpDelete("deleteRequestedManager/{mId}")]
        public IActionResult DeleteRequestedUser(int mId)
        {

            _managerDAL.DeleteRequestedUser(mId);
            return Ok(new { success = true, message = "Manager deleted successfully." });
        }



        [HttpGet("email-exists")]
        public async Task<IActionResult> CheckEmailExists([FromQuery] string email)
        {
            var exists = await _managerDAL.DoesEmailExistAsync(email);
            return Ok(exists);
        }
    }
}