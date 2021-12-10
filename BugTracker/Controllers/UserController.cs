using Microsoft.AspNetCore.Mvc;
using BugTracker.Areas.Identity.Data;
using BugTracker.Models;
using BugTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BugTracker.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UserController(UserManager<ApplicationUser> userManager) 
        {
            this.userManager = userManager;
        }

        public IActionResult ListUsers()
        {
            return View();
        }
    }
}
