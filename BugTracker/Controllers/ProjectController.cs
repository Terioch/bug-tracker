using BugTracker.Areas.Identity.Data;
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
        private readonly IProjectRepository repository;
        private readonly ITicketRepository ticketRepository;
        private readonly IUserProjectRepository userProjectRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public ProjectController(IProjectRepository repository, ITicketRepository ticketRepository, IUserProjectRepository userProjectRepository, UserManager<ApplicationUser> userManager)
        {
            this.repository = repository;
            this.ticketRepository = ticketRepository;
            this.userProjectRepository = userProjectRepository;
            this.userManager = userManager;
        }

        public IActionResult ListProjects(int? page)
        {
            IEnumerable<Project> projects = repository.GetAllProjects();
            IPagedList<Project> pagedProjects = projects.ToPagedList(page ?? 1, 8);
            return View(pagedProjects);
        } 
        
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Project project)
        {
            project.Id = Guid.NewGuid().ToString();            
            repository.Create(project);
            return RedirectToAction("ListProjects", "Project");
        }

        [HttpGet]
        public IActionResult Details(string id, int? page)
        {
            Project project = repository.GetProjectById(id);
            IEnumerable<Ticket> tickets = ticketRepository.GetTicketsByProject(id);
            List<string>? userIds = userProjectRepository.GetProjectUsers(id);         
            List<ApplicationUser> users = new();

            foreach (var userId in userIds)
            {
                ApplicationUser user = userManager.Users.First(u => u.Id == userId);
                users.Add(user);
            }

            ProjectViewModel model = new()
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Users = users.ToPagedList(page ?? 1, 5),
                Tickets = tickets.ToPagedList(page ?? 1, 5),
            };
            return View(model);
        }
      
        [HttpGet]
        public IActionResult FilterProjectsByNameReturnPartial(string? searchTerm)
        {                       
            IEnumerable<Project> projects = repository.GetAllProjects();           
            
            if (searchTerm == null)
            {
                return PartialView("_ProjectList", projects.ToPagedList(1, 5));
            }

            if (projects.ToList().Count == 0)
            {
                throw new Exception("No projects to filter based on predicate");
            }

            var filteredProjects = projects.Where(p => p.Name.ToLowerInvariant().Contains(searchTerm));
            return PartialView("_ProjectList", filteredProjects.ToPagedList(1, 5));
        }        

        [HttpPut]
        public IActionResult Update(Project project)
        {
            return View(project);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public IActionResult Delete(string id)
        {
            Project deletedProject = repository.Delete(id);
            return Json(deletedProject);
        }

        // [Authorize(Roles = "Admin, Project Manager")]        
        [HttpPost]
        public IActionResult AddUser(string id, string? userName)
        {
            if (userName == null)
            {
                return BadRequest(new { message = "UserName cannot be empty" });
            }

            ApplicationUser? user = userManager.Users.FirstOrDefault(u => u.UserName == userName);

            if (user == null)
            {
                return BadRequest(new { message = "UserName could not be found" });
            }

            List<string>? userIds = userProjectRepository.GetProjectUsers(id);

            if (userIds.Contains(user.Id))
            {
                return BadRequest(new { message = "User is already assigned to the current project" });
            }

            UserProject userProject = new()
            {   
                Id = Guid.NewGuid().ToString(),
                UserId = user.Id,
                ProjectId = id,
            };

            userProjectRepository.Create(userProject);
            return Json(userProject);            
        }

        // [Authorize(Roles = "Admin, Project Manager")]
        [HttpDelete]
        public IActionResult RemoveUser(string id, string? userName)
        {            
            if (userName == null)
            {
                return BadRequest(new { message = "UserName can not be empty" });
            }

            ApplicationUser? user = userManager.Users.FirstOrDefault(u => u.UserName == userName);

            if (user == null)
            {
                return BadRequest(new { message = "UserName could not be found" });
            }

            List<string>? userIds = userProjectRepository.GetProjectUsers(id);

            if (!userIds.Contains(user.Id))
            {
                return BadRequest(new { message = "User is not assigned to the current project" });
            }

            UserProject userProject = userProjectRepository.Delete(user.Id, id);
            return Json(userProject);
        }
    }
}
