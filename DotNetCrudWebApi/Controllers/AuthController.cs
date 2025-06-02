using Asp.Versioning;
using DotNetCrudWebApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DotNetCrudWebApi.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost, Route("login")]
        [MapToApiVersion("1.0")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            if (loginModel == null || string.IsNullOrEmpty(loginModel.UserName) || string.IsNullOrEmpty(loginModel.Password))
            {
                return BadRequest("Invalid login credentials.");
            }

            if (loginModel.UserName == "Test" || loginModel.Password == "Test@123")
            {
                var claims = new List<Claim>
            {
                new(ClaimTypes.Name, "Admin"),
                new(ClaimTypes.Role, "Admin")
            };

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("AdcbSecretKey@12345678901234567890123456789012")); // Must be at least 32 bytes
                var siginingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var tokenOption = new JwtSecurityToken(
                    issuer: "https://localhost:5001",
                    audience: "https://localhost:5001",
                    expires: DateTime.Now.AddMinutes(5),
                       claims: claims,
                       signingCredentials: siginingCredentials

                    );


                var jwtToken = new JwtSecurityTokenHandler().WriteToken(tokenOption);
                Log.Information(jwtToken);
                return Ok(new { Token = jwtToken });
            }
            return Unauthorized();
        }

    }
}
