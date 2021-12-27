﻿using BugTracker.Helpers;
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
        private readonly ITicketHistoryRecordRepository ticketHistoryRecordRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ProjectHelper projectHelper;

        public ProjectController(IProjectRepository repository, ITicketRepository ticketRepository, IUserProjectRepository userProjectRepository, ITicketHistoryRecordRepository ticketHistoryRecordRepository, UserManager<ApplicationUser> userManager, ProjectHelper projectHelper)
        {
            this.repository = repository;
            this.ticketRepository = ticketRepository;
            this.userProjectRepository = userProjectRepository;
            this.ticketHistoryRecordRepository = ticketHistoryRecordRepository;
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
        
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
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
            project.Tickets = ticketRepository.GetTicketsByProjectId(id);
            project.Users = userProjectRepository.GetUsersByProjectId(id);

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

            if (projects.ToList().Count == 0)
            {
                throw new Exception("No projects to filter based on predicate");
            }

            var filteredProjects = projects.Where(p => p.Name.ToLowerInvariant().Contains(searchTerm));
            return PartialView("_ProjectList", filteredProjects.ToPagedList(1, 8));
        }   
        
        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }

        [HttpPut]
        public IActionResult Update(Project project)
        {
            repository.Update(project);
            return RedirectToAction("Details", "Project", project.Id);
        }       

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public IActionResult Delete(string id)
        {            
            List<Ticket> tickets = ticketRepository.GetTicketsByProjectId(id);

            foreach (var ticket in tickets)
            {
                ticketRepository.Delete(ticket.Id);
                ticketHistoryRecordRepository.DeleteRecordsByTicketId(ticket.Id);
            }

            List<ApplicationUser> users = userProjectRepository.GetUsersByProjectId(id); 

            foreach (var user in users)
            {
                userProjectRepository.Delete(user.Id, id);
            }

            Project deletedProject = repository.Delete(id);
            return Json(deletedProject);
        }        

        [Authorize(Roles = "Admin, Project Manager")]
        [HttpPost]
        public IActionResult AddUser(string id, string userId)
        {            
            ApplicationUser? user = userManager.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id {userId} cannot be found";
                return View("NotFound");
            }

            List<ApplicationUser> users = userProjectRepository.GetUsersByProjectId(id);

            if (users.Contains(user))
            {
                return RedirectToAction("Details", id);
            }

            UserProject userProject = new()
            {   
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                ProjectId = id,
            };

            userProjectRepository.Create(userProject);
            return RedirectToAction("Details", id);     
        }

        [Authorize(Roles = "Admin, Project Manager")]
        public IActionResult RemoveUser(string id, string userId)
        {
            ApplicationUser? user = userManager.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id {userId} cannot be found";
                return View("NotFound");
            }

            List<ApplicationUser> users = userProjectRepository.GetUsersByProjectId(id);

            if (!users.Contains(user))
            {
                return RedirectToAction("Details", id);
            }

            UserProject userProject = userProjectRepository.Delete(userId, id);
            return RedirectToAction("Details", id);
        }
    }
}
