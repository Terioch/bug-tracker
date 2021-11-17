using Microsoft.AspNetCore.Mvc;

namespace BugTracker.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
