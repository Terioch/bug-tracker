using BugTracker.Helpers;
using BugTracker.Models;
using BugTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace BugTracker.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly IProjectRepository repo;
        private readonly ITicketRepository ticketRepo;
        private readonly IUserProjectRepository userProjectRepo;
        private readonly ITicketHistoryRecordRepository ticketHistoryRepo;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ProjectHelper projectHelper;
        
        public ProjectController(IProjectRepository repo, ITicketRepository ticketRepo, IUserProjectRepository userProjectRepo, ITicketHistoryRecordRepository ticketHistoryRepo, 
            UserManager<ApplicationUser> userManager, ProjectHelper projectHelper)
        {
            this.repo = repo;
            this.ticketRepo = ticketRepo;
            this.userProjectRepo = userProjectRepo;
            this.ticketHistoryRepo = ticketHistoryRepo;
            this.userManager = userManager;
            this.projectHelper = projectHelper;
        }    
        
        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return userManager.GetUserAsync(HttpContext.User);
        }

        public async Task<IActionResult> ListProjects(int? page)
        {   
            IEnumerable<Project> projects = await projectHelper.GetUserRoleProjects();               
            return View(projects.ToPagedList(page ?? 1, 8));
        } 
        
        [Authorize(Roles = "Admin, Demo Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Demo Admin")]
        [HttpPost]
        public IActionResult Create(Project project)
        {
            project.Id = Guid.NewGuid().ToString();            
            repo.Create(project);
            return RedirectToAction("ListProjects", "Project");
        }

        [HttpGet]
        public IActionResult Details(string id, int? page)
        {
            Project project = repo.GetProjectById(id);
            // project.Tickets = ticketRepository.GetTicketsByProjectId(id);
            // project.Users = userProjectRepo.GetUsersByProjectId(id);

            List<ApplicationUser> unassignedUsers = userManager.Users.ToList();
            project.Users.ToList().ForEach(u => unassignedUsers.Remove(u));

            ProjectViewModel model = new()
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,                
                UnassignedUsers = unassignedUsers,
                Users = project.Users.ToPagedList(page ?? 1, 5),
                Tickets = project.Tickets.ToPagedList(page ?? 1, 5),
            };
            return View(model);
        }
      
        [HttpGet]
        public async Task<IActionResult> FilterProjectsByNameReturnPartial(string? searchTerm)
        {
            IEnumerable<Project> projects = await projectHelper.GetUserRoleProjects();
            
            if (searchTerm == null)
            {
                return PartialView("_ProjectList", projects.ToPagedList(1, 8));
            }

           /* if (projects.ToList().Count == 0)
            {
                throw new Exception("No projects to filter based on predicate");
            }*/

            var filteredProjects = projects.Where(p => p.Name.ToLowerInvariant().Contains(searchTerm));
            return PartialView("_ProjectList", filteredProjects.ToPagedList(1, 8));
        }

        [Authorize(Roles = "Admin, Demo Admin")]
        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Demo Admin")]
        [HttpPut]
        public IActionResult Edit(Project project)
        {
            repo.Update(project);
            return RedirectToAction("Details", "Project", project.Id);
        }

        [Authorize(Roles = "Admin, Demo Admin")]    
        public IActionResult Delete(string id)
        {            
            List<Ticket> tickets = ticketRepo.GetTicketsByProjectId(id);

            foreach (var ticket in tickets)
            {
                ticketRepo.Delete(ticket.Id);
                ticketHistoryRepo.DeleteRecordsByTicketId(ticket.Id);
            }

            List<ApplicationUser> users = userProjectRepo.GetUsersByProjectId(id); 

            foreach (var user in users)
            {
                userProjectRepo.Delete(user.Id, id);
            }

            repo.Delete(id);
            return RedirectToAction("ListProjects");
        }

        [Authorize(Roles = "Admin, Demo Admin, Project Manager, Demo Project Manager")]
        [HttpPost]
        public IActionResult AddUser(string id, ProjectViewModel model)
        {            
            ApplicationUser? user = userManager.Users.FirstOrDefault(u => u.Id == model.ToBeAssignedUserId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id {model.ToBeAssignedUserId} cannot be found";
                return View("NotFound");
            }

            List<ApplicationUser> users = userProjectRepo.GetUsersByProjectId(id);

            if (users.Contains(user))
            {
                return RedirectToAction("Details", new { id });
            }

            UserProject userProject = new()
            {   
                Id = Guid.NewGuid().ToString(),
                UserId = user.Id,
                ProjectId = id,
            };

            userProjectRepo.Create(userProject);
            return RedirectToAction("Details", new { id });     
        }

        [Authorize(Roles = "Admin, Demo Admin, Project Manager, Demo Project Manager")]
        public IActionResult RemoveUser(string id, string userId)
        {
            ApplicationUser? user = userManager.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id {userId} cannot be found";
                return View("NotFound");
            }

            List<ApplicationUser> users = userProjectRepo.GetUsersByProjectId(id);

            if (!users.Contains(user))
            {
                return RedirectToAction("Details", new { id });
            }

            UserProject userProject = userProjectRepo.Delete(userId, id);
            return RedirectToAction("Details", new { id });
        }
    }
}
