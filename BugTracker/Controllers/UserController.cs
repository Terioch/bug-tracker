using Microsoft.AspNetCore.Mvc;
using BugTracker.Models;
using BugTracker.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using X.PagedList;
using BugTracker.Helpers;

namespace BugTracker.Controllers
{
    [Authorize]
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
        public IActionResult FilterProjectsByNameReturnPartial(string id, string? searchTerm)
        {
            ApplicationUser user = userManager.Users.First(u => u.Id == id);
            IEnumerable<Project> projects = userProjectRepo.GetProjectsByUserId(id);
            TempData["UserId"] = id;
            TempData["UserName"] = user.UserName;

            if (searchTerm == null)
            {         
                return PartialView("~/Views/User/_UserProjectList.cshtml", projects.ToPagedList(1, 2));
            }

            var filteredUsers = projects.Where(p => p.Name.ToLowerInvariant().Contains(searchTerm));  
            return PartialView("~/Views/User/_UserProjectList.cshtml", filteredUsers.ToPagedList(1, 2));
        }

        [HttpGet]
        public IActionResult FilterTicketsReturnPartial(string id, string? searchTerm)
        {
            IEnumerable<Ticket> tickets = ticketRepo.GetAllTickets()
                .Where(t => t.AssignedDeveloperId == id || t.SubmitterId == id);

            if (searchTerm == null)
            {
                ViewBag.Id = id;
                return PartialView("~/Views/User/_UserTicketList.cshtml", tickets.ToPagedList(1, 2));
            }

            var filteredUsers = tickets.Where(t => 
            {
                if (t.AssignedDeveloperId == null)
                {
                    t.AssignedDeveloper = new ApplicationUser() { UserName = "" };
                }

                return t.Title.ToLowerInvariant().Contains(searchTerm)
                || t.Status.ToLowerInvariant().Contains(searchTerm)
                || t.AssignedDeveloper.UserName.ToLowerInvariant().Contains(searchTerm)
                || t.Submitter.UserName.ToLowerInvariant().Contains(searchTerm);
            });             

            ViewBag.Id = id;
            return PartialView("~/Views/User/_UserTicketList.cshtml", filteredUsers.ToPagedList(1, 2));
        }

        [Authorize(Roles = "Super Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            ApplicationUser user = await userManager.FindByIdAsync(id);
            return View(user);
        }

        [Authorize(Roles = "Super Admin")]
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
                await signInManager.RefreshSignInAsync(user);
                return RedirectToAction("Details", new { id = user.Id });
            }           
            return View(model);
        }

        [Authorize(Roles = "Super Admin")]
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

        [Authorize(Roles = "Super Admin")]
        public IActionResult RemoveProject(string id, string projectId)
        {
            var projects = userProjectRepo.GetProjectsByUserId(id);
            bool isInProject = projects.Select(p => p.Id).Contains(projectId);

            if (!isInProject)
            {
                TempData["Error"] = "The project you're attempting to remove is not assigned to this user";
            }

            userProjectRepo.Delete(id, projectId);
            return RedirectToAction("Details", new { id });
        }
    }
}
