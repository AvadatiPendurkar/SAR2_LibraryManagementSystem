using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SAR2_LibraryManagementSystem.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
        }

        [HttpPost]
        [Route("Login")]
        public string GenToken(Login admin)
        {
            string token = "";
            if (admin.email == "admin@gmail.com" && admin.password == "123")
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                    new Claim("Email",admin.email),
                    new Claim("role","Admin"),
                    new Claim("role","User"),
                    new Claim("role","Manager")
                };

                var tokenFinal = new JwtSecurityToken(config["Jwt:Issuer"],
                    config["Jwt:Issuer"],
                    claims,
                    expires: DateTime.Now.AddMinutes(120),
                    signingCredentials: credentials
                );

                token = new JwtSecurityTokenHandler().WriteToken(tokenFinal);

            }
            return token;
        }
    }
}
