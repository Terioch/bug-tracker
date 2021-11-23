using BugTracker.Models;
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

        public ProjectController(IProjectRepository repository)
        {
            this.repository = repository;
        }

        public IActionResult Index()
        {
            IEnumerable<Project> projects = repository.GetAllProjects();
            return View(projects);
        } 
        
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Project project)
        {
            project.Id = Guid.NewGuid().ToString();
            Project createdProject = repository.Create(project);
            return View("Index", createdProject);
        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            Project project = repository.GetProject(id);
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

        [Authorize(Roles = "Admin, ProjectManager")]
        [HttpDelete]
        public IActionResult Delete(string id)
        {
            Project deletedProject = repository.Delete(id);
            return Json(deletedProject);
        }
    }
}
