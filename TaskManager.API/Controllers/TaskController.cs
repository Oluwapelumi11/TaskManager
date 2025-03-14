using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskManager.API.Interfaces;
using TaskManager.API.Model;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        private async Task<User> GetLoggedUserAsync() => (await _authService.GetLoggedInUser(User, _userManager))!;

        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            var tasks = await _taskRepository.GetAllUserTasksAsync( await GetLoggedUserAsync());
            return Ok(ApiResponse<IEnumerable<TaskItem>>.SuccessResponse(tasks));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById([FromRoute]int id)
        {
            var task = await _taskRepository.GetTaskByIdAsync(id);
            if (task != null) return Ok(ApiResponse<TaskItem>.SuccessResponse(task));
            return NotFound(ApiResponse<TaskItem>.ErrorResponse($"Task of ID {id} not found on the server"));

        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskItem newtask)
        {
            var created = await _taskRepository.CreateTaskAsync(newtask,await GetLoggedUserAsync());
            if (created != null) return Created();
            return BadRequest(ApiResponse<TaskItem>.ErrorResponse());
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTask([FromBody] TaskItem task)
        {
            var updated = await _taskRepository.UpdateTaskAsync(task);
            if (updated > 0)  return Ok(ApiResponse<TaskItem>.SuccessResponse(task, $"{updated} row{(updated == 1 ? "" : "s")} updated"));
            return NotFound(ApiResponse<TaskItem>.ErrorResponse($"Task with ID {task.TaskID} not found on the server"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask([FromRoute] int id)
        {
            var deleted = await _taskRepository.DeleteTaskAsync(id);
            if (deleted > 0) return Ok(ApiResponse<TaskItem>.SuccessResponse(null!, $"{deleted} row{(deleted == 1 ? "" : "s")} deleted"));
            return NotFound(ApiResponse<TaskItem>.ErrorResponse($"Task with ID {id} not found on the server"));
        } 

    }
}
