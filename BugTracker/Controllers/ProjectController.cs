using BugTracker.Helpers;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly ProjectHelper _projectHelper;
        private readonly TicketHelper _ticketHelper;

        public ProjectController(IUnitOfWork unitOfWork, ProjectHelper projectHelper, TicketHelper ticketHelper)
        {
            _unitOfWork = unitOfWork;
            _projectHelper = projectHelper;
            _ticketHelper = ticketHelper;            
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {   
            return _unitOfWork.UserManager.GetUserAsync(HttpContext.User);
        }

        public async Task<IActionResult> ListProjects(int? page)
        {   
            IEnumerable<Project> projects = await _projectHelper.GetUserRoleProjects();            
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
            var admins = await _unitOfWork.UserManager.GetUsersInRoleAsync("Admin");

            foreach (var admin in admins)
            {                
                _unitOfWork.Projects.AddUser(admin, project.Id);
            }

            _unitOfWork.Projects.Add(project);

            return RedirectToAction("ListProjects", "Project");
        }

        [HttpGet] 
        public async Task<IActionResult> Details(string id, int? usersPage, int? ticketsPage)
        {                        
            var project = await _unitOfWork.Projects.Get(id);         
            var userRoleTickets = await _ticketHelper.GetUserRoleTickets();                         
            var unassignedUsers = _unitOfWork.UserManager.Users.Where(u => !project.Users.Contains(u));

            var model = new ProjectViewModel()
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
            IEnumerable<Project> projects = await _projectHelper.GetUserRoleProjects();
            
            if (searchTerm == null)
            {
                return PartialView("_ProjectList", projects.ToPagedList(1, 8));
            }          

            var filteredProjects = projects.Where(p => p.Name.ToLowerInvariant().Contains(searchTerm));
            return PartialView("_ProjectList", filteredProjects.ToPagedList(1, 8));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult FilterUserProjectsByNameReturnPartial(string id, string? searchTerm)
        {
            var user = _unitOfWork.UserManager.Users.First(u => u.Id == id);      

            if (searchTerm == null)
            {
                return PartialView("~/Views/User/_UserProjectList.cshtml", user.Projects.ToPagedList(1, 5));
            }

            var filteredProjects = user.Projects.Where(p => p.Name.ToLowerInvariant().Contains(searchTerm));

            return PartialView("~/Views/User/_UserProjectList.cshtml", filteredProjects.ToPagedList(1, 5));
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
        public async Task<IActionResult> Delete(string id)
        {
            var tickets = _unitOfWork.Tickets.Find(t => t.ProjectId == id);

            foreach (var ticket in tickets)
            {
                _unitOfWork.Tickets.Delete(ticket);
                _unitOfWork.TicketHistoryRecords.DeleteRange(_unitOfWork.TicketHistoryRecords.Find(r => r.TicketId == ticket.Id));
                _unitOfWork.TicketAttachments.DeleteRange(_unitOfWork.TicketAttachments.Find(a => a.TicketId == ticket.Id));
                _unitOfWork.TicketComments.DeleteRange(_unitOfWork.TicketComments.Find(c => c.TicketId == ticket.Id));
            }
            
            var project = await _unitOfWork.Projects.Get(id);

            foreach (var user in project.Users)
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
            ApplicationUser? user = await _unitOfWork.UserManager.FindByIdAsync(model.ToBeAssignedUserId);

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

            userProjectRepo.Add(userProject);
            return RedirectToAction("Details", new { id });     
        }

        [Authorize(Roles = "Admin, Project Manager")]
        public async Task<IActionResult> RemoveUser(string id, string userId)
        {
            Project project = repo.GetProjectById(id);
            ApplicationUser? user = await _unitOfWork.UserManager.FindByIdAsync(userId);
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
