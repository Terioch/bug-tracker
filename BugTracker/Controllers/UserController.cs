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
        private readonly IUserProjectRepository userProjectRepository;
        private readonly IProjectRepository projectRepository;
        private readonly ITicketRepository ticketRepository;
        private readonly TicketHelper ticketHelper;

        public UserController(UserManager<ApplicationUser> userManager, IUserProjectRepository userProjectRepository, IProjectRepository projectRepository, ITicketRepository ticketRepository, TicketHelper ticketHelper) 
        {
            this.userManager = userManager;
            this.userProjectRepository = userProjectRepository;
            this.projectRepository = projectRepository;
            this.ticketRepository = ticketRepository;
            this.ticketHelper = ticketHelper;
        }

        [Authorize]
        [HttpGet]
        public IActionResult ListUsers(int? page)
        {
            /*List<UserProjectViewModel> users = new();

            foreach (var user in userManager.Users)
            {
                List<Project> projects = repository.GetProjectsByUserId(user.Id);               

                users.Add(new()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Projects = projects,
                });
            }*/
            IPagedList<ApplicationUser> users = userManager.Users.ToPagedList(page ?? 1, 8);
            return View(users);
        }
               
        [HttpGet]
        public IActionResult Details(string id, int? page)
        {
            ApplicationUser? user = userManager.Users.FirstOrDefault(u => u.Id == id);      
            IEnumerable<Project> projects = userProjectRepository.GetProjectsByUserId(id);
            IEnumerable<Ticket> tickets = ticketRepository.GetAllTickets().Where(t => t.SubmitterId == id || t.AssignedDeveloperId == id);

            List<Project> unassignedProjects = projectRepository.GetAllProjects().ToList();
            projects.ToList().ForEach(p => unassignedProjects.Remove(p));

            UserViewModel model = new()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                UnassignedProjects = unassignedProjects,
                Projects = projects.ToPagedList(page ?? 1, 5),
                Tickets = tickets.ToPagedList(page ?? 1, 5),
            };
            return View(model);
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
            userProjectRepository.Create(userProject);
            return RedirectToAction("Details", new { id });
        }
        
        public IActionResult RemoveProject(string id, string projectId)
        {
            userProjectRepository.Delete(id, projectId);
            return RedirectToAction("Details", new { id });
        }
    }
}
