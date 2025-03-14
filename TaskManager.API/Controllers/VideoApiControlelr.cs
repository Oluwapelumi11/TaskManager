using Microsoft.AspNetCore.Mvc;

namespace TaskManager.API.Controllers
{
    public class VideoApiControlelr : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
