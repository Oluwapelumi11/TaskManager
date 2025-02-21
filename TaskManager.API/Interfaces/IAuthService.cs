using TaskManager.API.Model;

namespace TaskManager.API.Interfaces
{
    public interface IAuthService
    {
        string GenerateJWTToken(User user);
    }
}
