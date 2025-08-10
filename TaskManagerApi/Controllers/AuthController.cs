using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagerApi.Models;

namespace TaskManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
        {
        private readonly IConfiguration _config;
        /* Simple in-memory user store for demo purposes
        for real scenarios - create migration file for users
        instead of using hardcoded username password pairs - save and retrieve from database*/
        private static List<User> _users = new()
        {
            new User { Id = 1, Username = "admin", Password = "password" }
        };

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Logs in a user.
        /// </summary>
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost("login")]
        public IActionResult Login([FromBody] User loginUser)
        {
            var user = _users.FirstOrDefault(u => 
                u.Username == loginUser.Username && u.Password == loginUser.Password);

            if (user == null)
                return Unauthorized("Invalid credentials");

            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        private string GenerateJwtToken(User user)
        {
            var authSettings = _config.GetSection("AuthSettings").Get<AuthSettings>();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim("userId", user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: authSettings.Issuer,
                audience: authSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(authSettings.DurationInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
