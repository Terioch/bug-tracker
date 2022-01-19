using Microsoft.AspNetCore.Mvc;
using BugTracker.Models;
using BugTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using X.PagedList;
using BugTracker.Helpers;

namespace BugTracker.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserProjectRepository userProjectRepo;
        private readonly IProjectRepository projectRepo;
        private readonly ITicketRepository ticketRepo;
        private readonly TicketHelper ticketHelper;

        public UserController(UserManager<ApplicationUser> userManager, IUserProjectRepository userProjectRepository, IProjectRepository projectRepository, ITicketRepository ticketRepository, TicketHelper ticketHelper) 
        {
            this.userManager = userManager;
            this.userProjectRepo = userProjectRepository;
            this.projectRepo = projectRepository;
            this.ticketRepo = ticketRepository;
            this.ticketHelper = ticketHelper;
        }

        [Authorize]
        [HttpGet]
        public IActionResult ListUsers(int? page)
        {
            IPagedList<ApplicationUser> users = userManager.Users.ToPagedList(page ?? 1, 8);
            return View(users);
        }
               
        [HttpGet]
        public IActionResult Details(string id, int? projectsPage, int? ticketsPage)
        {
            ApplicationUser? user = userManager.Users.FirstOrDefault(u => u.Id == id);      
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

            if (searchTerm == null)
            {
                ViewBag.Id = id;
                ViewBag.UserName = user.UserName;
                return PartialView("~/Views/User/_UserProjectList.cshtml", projects.ToPagedList(1, 2));
            }

            var filteredUsers = projects.Where(p => p.Name.ToLowerInvariant().Contains(searchTerm));
            ViewBag.Id = id;
            ViewBag.UserName = user.UserName;
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
                t.Title.ToLowerInvariant().Contains(searchTerm)
                || t.Status.ToLowerInvariant().Contains(searchTerm)                
                || t.AssignedDeveloper.UserName.ToLowerInvariant().Contains(searchTerm)
                || t.Submitter.UserName.ToLowerInvariant().Contains(searchTerm));

            ViewBag.Id = id;
            return PartialView("~/Views/User/_UserTicketList.cshtml", filteredUsers.ToPagedList(1, 2));
        }

        [HttpPost]
        public IActionResult AddProject(string id, UserViewModel model)
        {
            UserProject userProject = new()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = id,
                ProjectId = model.ToBeAssignedProjectId,
            };
            userProjectRepo.Create(userProject);
            return RedirectToAction("Details", new { id });
        }
        
        public IActionResult RemoveProject(string id, string projectId)
        {
            userProjectRepo.Delete(id, projectId);
            return RedirectToAction("Details", new { id });
        }
    }
}
