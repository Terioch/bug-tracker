using Microsoft.AspNetCore.Mvc;
using BugTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using X.PagedList;
using BugTracker.Helpers;
using BugTracker.Repositories.Interfaces;

namespace BugTracker.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IUserProjectRepository userProjectRepo;
        private readonly IProjectRepository projectRepo;
        private readonly ITicketRepository ticketRepo;
        private readonly TicketHelper ticketHelper;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUserProjectRepository userProjectRepo, IProjectRepository projectRepo, ITicketRepository ticketRepository, TicketHelper ticketHelper) 
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.userProjectRepo = userProjectRepo;
            this.projectRepo = projectRepo;
            this.ticketRepo = ticketRepository;
            this.ticketHelper = ticketHelper;
        }
        
        [HttpGet]
        public IActionResult ListUsers(int? page)
        {
            IPagedList<ApplicationUser> users = userManager.Users.ToPagedList(page ?? 1, 8);
            return View(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Details(string id, int? projectsPage, int? ticketsPage)
        {
            ApplicationUser? user = await userManager.FindByIdAsync(id);
            IEnumerable<Project> projects = userProjectRepo.GetProjectsByUserId(id);
            IEnumerable<Ticket> tickets = ticketRepo.GetAllTickets().Where(t => t.SubmitterId == id || t.AssignedDeveloperId == id);

            List<Project> unassignedProjects = projectRepo.GetAllProjects().ToList();
            projects.ToList().ForEach(p => unassignedProjects.Remove(p));

            UserViewModel model = new()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                UnassignedProjects = unassignedProjects,
                Projects = projects.ToPagedList(projectsPage ?? 1, 2),
                Tickets = tickets.ToPagedList(ticketsPage ?? 1, 2),
            };
            return View(model);
        }
        
        [HttpGet]
        public IActionResult FilterUsersByNameReturnPartial(string? searchTerm)
        {
            IEnumerable<ApplicationUser> users = userManager.Users;

            if (searchTerm == null)
            {          
                return PartialView("_UserList", users.ToPagedList(1, 8));
            }

            var filteredUsers = users.Where(u => u.UserName.ToLowerInvariant().Contains(searchTerm));
            return PartialView("_UserList", filteredUsers.ToPagedList(1, 8));
        }        

        [HttpGet]
        public IActionResult FilterProjectUsersByNameReturnPartial(string id, string? searchTerm)
        {
            Project project = projectRepo.GetProjectById(id);
            IEnumerable<ApplicationUser> users = userProjectRepo.GetUsersByProjectId(id);
            TempData["ProjectId"] = id;
            TempData["ProjectName"] = project.Name;

            if (searchTerm == null)
            {
                return PartialView("~/Views/Project/_ProjectUserList.cshtml", users.ToPagedList(1, 5));
            }

            var filteredUsers = users.Where(u => u.UserName.ToLowerInvariant().Contains(searchTerm));
            return PartialView("~/Views/Project/_ProjectUserList.cshtml", filteredUsers.ToPagedList(1, 5));
        }        

        [Authorize(Roles = "Owner")]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            ApplicationUser user = await userManager.FindByIdAsync(id);
            return View(user);
        }

        [Authorize(Roles = "Owner")]
        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUser model)
        {
            ApplicationUser user = await userManager.FindByIdAsync(model.Id);
     
            if (ModelState.IsValid)
            {                
                if (model.UserName != user.UserName)
                {
                    var result = await userManager.SetUserNameAsync(user, model.UserName);

                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }

                        return View(model);
                    }                                        
                }

                if (model.Email != user.Email)
                {
                    var result = await userManager.SetEmailAsync(user, model.Email);

                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }

                        return View(model);
                    }
                }                
                return RedirectToAction("Details", new { id = user.Id });
            }           
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult AddProject(string id, UserViewModel model)
        {
            var projects = userProjectRepo.GetProjectsByUserId(id);
            bool isInProject = projects.Select(p => p.Id).Contains(model.ToBeAssignedProjectId);

            if (isInProject)
            {
                TempData["Error"] = "The project you're attempting to add is already assigned to this user";
            }

            UserProject userProject = new()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = id,
                ProjectId = model.ToBeAssignedProjectId,
            };
            userProjectRepo.Create(userProject);
            return RedirectToAction("Details", new { id });
        }

        [Authorize(Roles = "Admin")]
        public IActionResult RemoveProject(string id, string projectId)
        {
            IEnumerable<Project> projects = userProjectRepo.GetProjectsByUserId(id);
            Project project = projectRepo.GetProjectById(projectId);
            bool isInProject = projects.Select(p => p.Id).Contains(projectId);

            if (!isInProject)
            {
                TempData["Error"] = "The project you're attempting to remove is not assigned to this user";
            }
            
            bool IsUserOnTicketsInProject = project.Tickets.Where(t => t.AssignedDeveloperId == id || t.SubmitterId == id).Any();

            if (IsUserOnTicketsInProject)
            {
                TempData["Error"] = "This user is associated with at least one ticket within the assigned project and must be disassociated with all tickets before they can be removed.";
                return RedirectToAction("Details", new { id });
            }

            userProjectRepo.Delete(id, projectId);
            return RedirectToAction("Details", new { id });
        }
    }
}
