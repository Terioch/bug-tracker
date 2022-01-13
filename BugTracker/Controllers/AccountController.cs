using Microsoft.AspNetCore.Mvc;

namespace BugTracker.Controllers
{
    public class AccountController : Controller
    {                  
        public IActionResult DisplayDemoLoginForm()
        {
            return View("DemoLoginForm");
        }
    }
}
