using BugTracker.Models;
using BugTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly IProjectRepository repository;
        private readonly ITicketRepository ticketRepository;

        public ProjectController(IProjectRepository repository, ITicketRepository ticketRepository)
        {
            this.repository = repository;
            this.ticketRepository = ticketRepository;
        }

        public IActionResult ListProjects()
        {
            IEnumerable<Project> projects = repository.GetAllProjects();
            return View(projects);
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
        public IActionResult Details(string id)
        {
            Project project = repository.GetProject(id);
            project.Tickets = ticketRepository.GetTicketsByProject(id);
            return View(project);
        }

        [HttpGet("/Project/FilterProjectsByNameReturnPartial/{searchTerm}")]
        public IActionResult FilterProjectsByNameReturnPartial(string searchTerm = "")
        {                       
            List<Project> projects = repository.GetAllProjects().ToList();

            if (searchTerm == "null")
            {
                return PartialView("_ProjectList", projects);
            }

            if (projects.Count == 0)
            {
                throw new Exception("No projects to filter based on predicate");
            }

            var filteredProjects = projects.Where(p => p.Name.ToLowerInvariant().Contains(searchTerm));
            return PartialView("_ProjectList", filteredProjects);
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
    }
}
