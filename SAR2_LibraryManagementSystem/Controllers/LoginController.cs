using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SAR2_LibraryManagementSystem.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Data.SqlClient;
using SAR2_LibraryManagementSystem.Model;

namespace SAR2_LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public readonly IConfiguration config;

        public LoginController(IConfiguration configuration)
        {
            config = configuration;
           // EmailService = emailService;
        }
        public EmailService EmailService { get; }

        //    [HttpPost]
        //    [Route("Login")]
        //    public string GenToken(Login admin)
        //    {
        //        string token = "";
        //        if (admin.email == "admin@gmail.com" && admin.password == "123")
        //        {
        //            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
        //            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        //            var claims = new[]
        //            {
        //                new Claim("Email",admin.email),
        //                new Claim("role","Admin"),
        //                new Claim("role","User"),
        //                new Claim("role","Manager")
        //            };

        //            var tokenFinal = new JwtSecurityToken(config["Jwt:Issuer"],
        //                config["Jwt:Issuer"],
        //                claims,
        //                expires: DateTime.Now.AddMinutes(120),
        //                signingCredentials: credentials
        //            );

        //            token = new JwtSecurityTokenHandler().WriteToken(tokenFinal);

        //        }
        //        return token;
        //    }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Login model)
        {
            using (SqlConnection con = new SqlConnection(config.GetConnectionString("DefaultConnection")))
            {
                con.Open();

                // Check Manager table
                string managerQuery = "SELECT * FROM Managers WHERE email = @Email AND pass = @Password";
                SqlCommand cmd = new SqlCommand(managerQuery, con);
                cmd.Parameters.AddWithValue("@Email", model.email);
                cmd.Parameters.AddWithValue("@Password", model.password); // Ideally use hashed password

                SqlDataReader reader = cmd.ExecuteReader();
                //user block or not 
                if (reader.Read())
                {
                    bool hasIsBloked = false;
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        if (reader.GetName(i).Equals("isBlocked", StringComparison.OrdinalIgnoreCase))
                        {
                            hasIsBloked = true;
                            break;
                        }
                    }
                    if (hasIsBloked)
                    {
                        bool isBloked = Convert.ToBoolean(reader["isBlocked"]);
                        if (isBloked)
                        {;
                            return Unauthorized(new { message = "Your account is blocked. Contact manager." });
                        }
                    }
                    return Ok(new { message = "Login successful" });
                }
                else
                {
                    return Unauthorized(new { message = "Invalid email or password" });
                }
            
            
       


                if (reader.Read())
                {
                    // Manager found
                    return Ok(new { status = "success", userType = "manager" });
                }
                reader.Close();

                // Check User table
                string userQuery = "SELECT * FROM [Users] WHERE email = @Email AND pass = @Password";
                cmd = new SqlCommand(userQuery, con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Email", model.email);
                cmd.Parameters.AddWithValue("@Password", model.password);

                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // User found
                    return Ok(new { status = "success", userType = "user", userId=reader.GetInt32(0) });


                }

                return Unauthorized(new { status = "failed", message = "Invalid email or password" });
            }
        }

    }
}
