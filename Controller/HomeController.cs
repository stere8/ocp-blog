using Microsoft.AspNetCore.Mvc;

namespace ocp_blog.Controller
{
    public class HomeController : ControllerBase
    {
        [HttpGet("/")]
        public IActionResult Index()
        {
            return Content("Welcome to OCP Blog!");
        }
    }
}