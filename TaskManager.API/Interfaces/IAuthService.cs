using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TaskManager.API.Model;

namespace TaskManager.API.Interfaces
{
    public interface IAuthService
    {
        string GenerateJWTToken(User user);

        Task<User?> GetLoggedInUser(ClaimsPrincipal user, UserManager<User> userManager);
    }
}
