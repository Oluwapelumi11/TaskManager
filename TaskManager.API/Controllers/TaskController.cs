using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Model;

namespace TaskManager.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {

        [HttpGet]
        public IActionResult GetTasks()
        {
            
        }

    }
}
