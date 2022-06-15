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
                project.Users.Add(admin); // May not add user correctly
                //_unitOfWork.Projects.AddUser(admin, project);
            }

            _unitOfWork.Projects.Add(project);

            await _unitOfWork.CompleteAsync();

            return RedirectToAction("ListProjects", "Project");
        }

        [HttpGet] 
        public async Task<IActionResult> Details(string id, int? usersPage, int? ticketsPage)
        {                        
            var project = await _unitOfWork.Projects.GetAsync(id);         
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
            var user = _unitOfWork.Users.Find(u => u.Id == id).FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            if (searchTerm == null)
            {
                return PartialView("~/Views/User/_UserProjectList.cshtml", user.Projects.ToPagedList(1, 5));
            }
  
            var filteredProjects = user.Projects.Where(p => p.Name.ToLowerInvariant().Contains(searchTerm));

            return PartialView("~/Views/User/_UserProjectList.cshtml", filteredProjects.ToPagedList(1, 5));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var project = await _unitOfWork.Projects.GetAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(Project model)
        {
            if (ModelState.IsValid)
            {
                var project = await _unitOfWork.Projects.GetAsync(model.Id);

                if (project == null)
                {
                    return NotFound();
                }

                project.Name = model.Name;
                project.Description = model.Description;

                await _unitOfWork.CompleteAsync();

                return RedirectToAction("Details", new { id = project.Id });
            }

            return View(model);
        }

        [Authorize(Roles = "Admin")]    
        public async Task<IActionResult> Delete(string id)
        {
            var tickets = _unitOfWork.Tickets.Find(t => t.ProjectId == id);

            /*foreach (var ticket in tickets)
            {
                _unitOfWork.Tickets.Delete(ticket);
                _unitOfWork.TicketHistoryRecords.DeleteRange(_unitOfWork.TicketHistoryRecords.Find(r => r.TicketId == ticket.Id));
                _unitOfWork.TicketAttachments.DeleteRange(_unitOfWork.TicketAttachments.Find(a => a.TicketId == ticket.Id));
                _unitOfWork.TicketComments.DeleteRange(_unitOfWork.TicketComments.Find(c => c.TicketId == ticket.Id));
            }*/
            
            var project = await _unitOfWork.Projects.GetAsync(id);

            /*foreach (var user in project.Users)
            {
                project.Users.Remove(user);
                userProjectRepo.Delete(user.Id, id);
            }*/

            _unitOfWork.Projects.Delete(project);

            return RedirectToAction("ListProjects");
        }

        [Authorize(Roles = "Admin, Project Manager")]
        [HttpPost]
        public async Task<IActionResult> AddUser(string id, ProjectViewModel model)
        {
            var user = await _unitOfWork.UserManager.FindByIdAsync(model.ToBeAssignedUserId);          
            var project = await _unitOfWork.Projects.GetAsync(id);

            if (project.Users.Any(u => u.Id == user.Id))
            {
                TempData["Error"] = "The user you're attempting to add is already assigned to this project";
                return RedirectToAction("Details", new { id });
            }

            project.Users.Add(user);            

            return RedirectToAction("Details", new { id });     
        }

        [Authorize(Roles = "Admin, Project Manager")]
        public async Task<IActionResult> RemoveUser(string id, string userId)
        {
            var project = await _unitOfWork.Projects.GetAsync(id);
            var user = await _unitOfWork.UserManager.FindByIdAsync(userId);        

            if (!project.Users.Any(u => u.Id == user.Id))
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

            project.Users.Remove(user);

            await _unitOfWork.CompleteAsync();

            return RedirectToAction("Details", new { id });
        }
    }
}
