﻿using BugTracker.Helpers;
using BugTracker.Models;
using BugTracker.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace BugTracker.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectRepository repo;
        private readonly ITicketRepository ticketRepo;
        private readonly IUserProjectRepository userProjectRepo;
        private readonly ITicketHistoryRepository ticketHistoryRepo;
        private readonly ITicketAttachmentRepository ticketAttachmentRepo;
        private readonly ITicketCommentRepository ticketCommentRepo;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ProjectHelper projectHelper;
        private readonly TicketHelper ticketHelper;        

        public ProjectController(IProjectRepository repo, ITicketRepository ticketRepo, IUserProjectRepository userProjectRepo, ITicketHistoryRepository ticketHistoryRepo, 
            ITicketAttachmentRepository ticketAttachmentRepo, ITicketCommentRepository ticketCommentRepo, UserManager<ApplicationUser> userManager, 
            ProjectHelper projectHelper, TicketHelper ticketHelper)
        {
            this.repo = repo;
            this.ticketRepo = ticketRepo;
            this.userProjectRepo = userProjectRepo;
            this.ticketHistoryRepo = ticketHistoryRepo;
            this.ticketAttachmentRepo = ticketAttachmentRepo;
            this.ticketCommentRepo = ticketCommentRepo;
            this.userManager = userManager;
            this.projectHelper = projectHelper;
            this.ticketHelper = ticketHelper;                                
        }    
        
        private Task<ApplicationUser> GetCurrentUserAsync()
        {   
            return userManager.GetUserAsync(HttpContext.User);
        }

        public async Task<IActionResult> ListProjects(int? page)
        {   
            IEnumerable<Project> projects = await projectHelper.GetUserRoleProjects();
            foreach (var project in projects)
            {
                project.Tickets = ticketRepo.GetTicketsByProjectId(project.Id).ToList();
            }
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
        public async Task<IActionResult> Create(Project project)
        {
            project.Id = Guid.NewGuid().ToString();

            // Assign all administrators 
            var admins = await userManager.GetUsersInRoleAsync("Admin");
            foreach (var admin in admins)
            {
                UserProject userProject = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = admin.Id,
                    ProjectId = project.Id
                };
                userProjectRepo.Create(userProject);
            }               
            repo.Create(project);
            return RedirectToAction("ListProjects", "Project");
        }

        [HttpGet] 
        public async Task<IActionResult> Details(string id, int? usersPage, int? ticketsPage)
        {                        
            Project project = repo.GetProjectById(id);         
            var userRoleTickets = await ticketHelper.GetUserRoleTickets(); 
            project.Tickets = userRoleTickets.Where(t => t.ProjectId == id).ToList();
            project.Users = userProjectRepo.GetUsersByProjectId(id).ToList();            
            var unassignedUsers = userManager.Users.Where(u => !project.Users.Contains(u));

            ProjectViewModel model = new()
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,                
                UnassignedUsers = unassignedUsers,
                Users = project.Users.ToPagedList(usersPage ?? 1, 5),
                Tickets = project.Tickets.ToPagedList(ticketsPage ?? 1, 5),
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

            var filteredProjects = projects.Where(p => p.Name.ToLowerInvariant().Contains(searchTerm));
            return PartialView("_ProjectList", filteredProjects.ToPagedList(1, 8));
        }

        [HttpGet]
        public IActionResult FilterTicketsReturnPartial(string id, string? searchTerm)
        {
            IEnumerable<Ticket> tickets = ticketRepo.GetTicketsByProjectId(id);
            TempData["ProjectId"] = id;

            if (searchTerm == null)
            {                
                return PartialView("~/Views/Project/_ProjectTicketList.cshtml", tickets.ToPagedList(1, 5));
            }

            var filteredTickets = tickets.Where(t =>
            {
                if (t.AssignedDeveloperId == null)
                {
                    t.AssignedDeveloper = new ApplicationUser() { UserName = "" };
                }

                return t.Title.ToLowerInvariant().Contains(searchTerm)
                || t.Status.ToLowerInvariant().Contains(searchTerm)
                || t.Priority.ToLowerInvariant().Contains(searchTerm)
                || t.AssignedDeveloper.UserName.ToLowerInvariant().Contains(searchTerm)
                || t.Submitter.UserName.ToLowerInvariant().Contains(searchTerm);
            });

            return PartialView("~/Views/Project/_ProjectTicketList.cshtml", filteredTickets.ToPagedList(1, 5));
        }

        [HttpGet]
        public IActionResult FilterUsersByNameReturnPartial(string id, string? searchTerm)
        {
            Project project = repo.GetProjectById(id);
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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(string id)
        {
            Project project = repo.GetProjectById(id);
            return View(project);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(Project project)
        {
            if (ModelState.IsValid)
            {
                repo.Update(project);
                return RedirectToAction("Details", new { id = project.Id });
            }
            return View(project);
        }

        [Authorize(Roles = "Admin")]    
        public IActionResult Delete(string id)
        {            
            IEnumerable<Ticket> tickets = ticketRepo.GetTicketsByProjectId(id);

            foreach (var ticket in tickets)
            {
                ticketRepo.Delete(ticket.Id);
                ticketHistoryRepo.DeleteRecordsByTicketId(ticket.Id);
                ticketAttachmentRepo.DeleteAttachmentsByTicketId(ticket.Id);
                ticketCommentRepo.DeleteCommentsByTicketId(ticket.Id);
            }

            IEnumerable<ApplicationUser> users = userProjectRepo.GetUsersByProjectId(id); 

            foreach (var user in users)
            {
                userProjectRepo.Delete(user.Id, id);
            }

            repo.Delete(id);
            return RedirectToAction("ListProjects");
        }

        [Authorize(Roles = "Admin, Project Manager")]
        [HttpPost]
        public async Task<IActionResult> AddUser(string id, ProjectViewModel model)
        {
            ApplicationUser? user = await userManager.FindByIdAsync(model.ToBeAssignedUserId);

            IEnumerable<ApplicationUser> users = userProjectRepo.GetUsersByProjectId(id);

            if (users.Contains(user))
            {
                TempData["Error"] = "The user you're attempting to add is already assigned to this project";
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

        [Authorize(Roles = "Admin, Project Manager")]
        public async Task<IActionResult> RemoveUser(string id, string userId)
        {
            Project project = repo.GetProjectById(id);
            ApplicationUser? user = await userManager.FindByIdAsync(userId);
            IEnumerable<ApplicationUser> users = userProjectRepo.GetUsersByProjectId(id);

            if (!users.Contains(user))
            {
                TempData["Error"] = "The user you're attempting to remove is not assigned to this project";
                return RedirectToAction("Details", new { id });
            }

            bool IsUserOnTicketsInProject = project.Tickets.Where(t => t.AssignedDeveloperId == userId || t.SubmitterId == userId).Any();

            if (IsUserOnTicketsInProject)
            {
                TempData["Error"] = "This user is associated with at least one ticket within the assigned project and must be disassociated with all tickets before they can be removed.";                
                return RedirectToAction("Details", new { id });
            }

            UserProject userProject = userProjectRepo.Delete(userId, id);
            return RedirectToAction("Details", new { id });
        }
    }
}
