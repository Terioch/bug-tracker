using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BugTracker.Services;
using BugTracker.Models;

namespace BugTracker.Controllers
{
    public class ProjectUsersController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IUserProjectRepository repository;

        public ProjectUsersController(UserManager<IdentityUser> userManager, IUserProjectRepository repository)
        {
            this.userManager = userManager;
            this.repository = repository;
        }

        [HttpPost]
        public IActionResult Add(string id, string? userName)
        {
            if (userName == null)
            {
                return BadRequest(new { message = "UserName cannot be empty" });
            }

            IdentityUser? user = userManager.Users.FirstOrDefault(u => u.UserName == userName);

            if (user == null)
            {
                return BadRequest(new { message = "UserName was not found" });
            }

            UserProject userProject = new() 
            { 
                ProjectId = id,
                UserId = user.Id 
            };
            repository.Create(userProject);
            return Json(userProject);
        }

        [HttpDelete]
        public IActionResult Remove(string id, string? userName)
        {
            if (userName == null)
            {
                return BadRequest(new { message = "UserName cannot be empty" });
            }

            IdentityUser? user = userManager.Users.FirstOrDefault(u => u.UserName == userName);

            if (user == null)
            {
                return BadRequest(new { message = "UserName could not be found" });
            }

            UserProject userProject = repository.Delete(user.Id, id);            
            return Json(userProject);
        }
    }
}
