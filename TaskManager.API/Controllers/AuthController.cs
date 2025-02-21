using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Interfaces;
using TaskManager.API.Model;

namespace TaskManager.API.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _config;
        private readonly IAuthService _authService;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager,IAuthService authService, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUser register)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = new User { UserName = register.Email, Email = register.Email };
            var result = await _userManager.CreateAsync(user, register.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);
            return Ok(new ApiResponse<string>(true, "user created succesfully"));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUser login)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.FindByNameAsync(login.UserName);
            if (user == null || !await _userManager.CheckPasswordAsync(user, login.Password)) return Unauthorized();

            var token = _authService.GenerateJWTToken(user);
            return Ok(ApiResponse<string>.SuccessResponse(token));
        }

    }
}
