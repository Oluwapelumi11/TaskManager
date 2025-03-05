using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Interfaces;
using TaskManager.API.Model;

namespace TaskManager.API.Controllers
{
    [Authorize]
    [Route("task")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private ITaskRepository _taskRepository;
        private UserManager<User> _userManager;
        private IAuthService _authService;
        public TaskController(ITaskRepository _taskRepo, UserManager<User> userManager,IAuthService authService)
        {
            _taskRepository = _taskRepo;
            _userManager = userManager;
            _authService = authService;
        }


        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            var loggedUser = await _authService.GetLoggedInUser(User, _userManager);
            var tasks = await _taskRepository.GetAllUserTasksAsync( loggedUser!);
            return Ok(ApiResponse<IEnumerable<TaskItem>>.SuccessResponse(tasks));
        }

    }
}
