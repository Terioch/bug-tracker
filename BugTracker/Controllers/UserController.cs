using Microsoft.AspNetCore.Mvc;
using BugTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using X.PagedList;
using BugTracker.Helpers;
using BugTracker.Repositories.Interfaces;

namespace BugTracker.Controllers
{
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;        
        private readonly SignInManager<ApplicationUser> _signInManager;        
        private readonly TicketHelper _ticketHelper;

        public UserController(IUnitOfWork unitOfWork, SignInManager<ApplicationUser> signInManager, TicketHelper ticketHelper)
        {
            _unitOfWork = unitOfWork;
            _signInManager = signInManager;
            _ticketHelper = ticketHelper;
        }

        [HttpGet]
        public IActionResult ListUsers(int? page)
        {
            var users = _unitOfWork.UserManager.Users.ToPagedList(page ?? 1, 8);
            return View(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Details(string id, int? projectsPage, int? ticketsPage)
        {
            var user = await _unitOfWork.Users.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            var tickets = _unitOfWork.Tickets.Find(t => t.SubmitterId == id || t.AssignedDeveloperId == id);
            var unassignedProjects = _unitOfWork.Projects.GetAll().Where(p => !p.Users.Contains(user));            

            var model = new UserViewModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                UnassignedProjects = unassignedProjects,
                Projects = user.Projects.ToPagedList(projectsPage ?? 1, 2),
                Tickets = tickets.ToPagedList(ticketsPage ?? 1, 2),
            };

            return View(model);
        }
        
        [HttpGet]
        public IActionResult FilterUsersByNameReturnPartial(string? searchTerm)
        {          
            if (searchTerm == null)
            {          
                return PartialView("_UserList", _unitOfWork.UserManager.Users.ToPagedList(1, 8));
            }

            var filteredUsers = _unitOfWork.UserManager.Users.ToList().Where(u => u.UserName.ToLowerInvariant().Contains(searchTerm));

            return PartialView("_UserList", filteredUsers.ToPagedList(1, 8));
        }        

        [HttpGet]
        public async Task<IActionResult> FilterProjectUsersByNameReturnPartial(string id, string? searchTerm)
        {
            var project = await _unitOfWork.Projects.Get(id);
            
            TempData["ProjectId"] = id;
            TempData["ProjectName"] = project.Name;

            if (searchTerm == null)
            {
                return PartialView("~/Views/Project/_ProjectUserList.cshtml", project.Users.ToPagedList(1, 5));
            }

            var filteredUsers = project.Users.Where(u => u.UserName.ToLowerInvariant().Contains(searchTerm));

            return PartialView("~/Views/Project/_ProjectUserList.cshtml", filteredUsers.ToPagedList(1, 5));
        }        

        [Authorize(Roles = "Owner")]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _unitOfWork.UserManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [Authorize(Roles = "Owner")]
        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUser model)
        {
            var user = await _unitOfWork.UserManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {                
                if (model.UserName != user.UserName)
                {
                    var result = await _unitOfWork.UserManager.SetUserNameAsync(user, model.UserName);

                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }

                        return View(model);
                    }                                        
                }

                if (model.Email != user.Email)
                {
                    var result = await _unitOfWork.UserManager.SetEmailAsync(user, model.Email);

                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }

                        return View(model);
                    }
                }                

                return RedirectToAction("Details", new { id = user.Id });
            }           

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddProject(string id, UserViewModel model)
        {       
            var user = await _unitOfWork.Users.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            bool isInProject = user.Projects.Select(p => p.Id).Contains(model.ToBeAssignedProjectId);

            if (isInProject)
            {
                TempData["Error"] = "The project you're attempting to add is already assigned to this user";
            }

            var project = await _unitOfWork.Projects.Get(model.ToBeAssignedProjectId);

            user.Projects.Add(project);

            await _unitOfWork.CompleteAsync();

            return RedirectToAction("Details", new { id });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveProject(string id, string projectId)
        {
            var user = await _unitOfWork.Users.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            var project = await _unitOfWork.Projects.Get(projectId);
            bool isInProject = _unitOfWork.Projects.GetAll().Select(p => p.Id).Contains(projectId);

            if (!isInProject)
            {
                TempData["Error"] = "The project you're attempting to remove is not assigned to this user";
            }
            
            bool IsUserOnTicketsInProject = project.Tickets.Where(t => t.AssignedDeveloperId == id || t.SubmitterId == id).Any();

            if (IsUserOnTicketsInProject)
            {
                TempData["Error"] = "This user is associated with at least one ticket within the assigned project and must be disassociated with all tickets before they can be removed.";
                return RedirectToAction("Details", new { id });
            }

            user.Projects.Remove(project);

            await _unitOfWork.CompleteAsync();

            return RedirectToAction("Details", new { id });
        }
    }
}
