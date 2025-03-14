using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManager.API.Interfaces;
using TaskManager.API.Model;

namespace TaskManager.API.Service.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        public AuthService(IConfiguration config)
        {
            _config = config;
        }
        public string GenerateJWTToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims ,
                expires: DateTime.Now.AddHours(5),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public async Task<User?> GetLoggedInUser(ClaimsPrincipal user, UserManager<User> userManager)
        {
            var userID = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userID == null) return null!;

            return await userManager.FindByIdAsync(userID);
        }
    }
}
