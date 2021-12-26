using BugTracker.Helpers;
using BugTracker.Models;
using BugTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using X.PagedList;

namespace BugTracker.Controllers
{
    [Authorize]
    public class TicketController : Controller
    {
        private readonly ITicketRepository repository;
        private readonly IProjectRepository projectRepository;
        private readonly ITicketHistoryRecordRepository ticketHistoryRecordRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ProjectHelper projectHelper;
        private readonly TicketHelper ticketHelper;

        public TicketController(ITicketRepository repository, IProjectRepository projectRepository, ITicketHistoryRecordRepository ticketHistoryRecordRepository, UserManager<ApplicationUser> userManager, ProjectHelper projectHelper, TicketHelper ticketHelper)
        {
            this.repository = repository;
            this.projectRepository = projectRepository;
            this.ticketHistoryRecordRepository = ticketHistoryRecordRepository;
            this.userManager = userManager;
            this.projectHelper = projectHelper;
            this.ticketHelper = ticketHelper;
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return userManager.GetUserAsync(HttpContext.User);
        }

        private Task<ApplicationUser> FindUserByNameAsync(string name)
        {
            return userManager.FindByNameAsync(name);
        }

        public async Task<IActionResult> ListTickets(int? page)
        {
            IEnumerable<Ticket> tickets = await ticketHelper.GetUserRoleTickets();
            return View(tickets.ToPagedList(page ?? 1, 5));
        }
    
        [Authorize(Roles = "Admin, Project Manager, Submitter")]        
        [HttpGet]        
        public async Task<IActionResult> Create()
        {              
            ViewBag.Projects = await projectHelper.GetUserRoleProjects(); ;
            return View();
        }

        [Authorize(Roles = "Admin, Project Manager, Submitter")]
        [HttpGet]
        public async Task<IActionResult> CreateForProject(string projectId)
        {
            ApplicationUser user = await GetCurrentUserAsync();
            var isAdmin = await userManager.IsInRoleAsync(user, "Admin");

            if  (!isAdmin && !projectHelper.IsUserInProject(projectId))
            {
                return View("~/Areas/Identity/Pages/Account/AccessDenied.cshtml");
            }

            return View(new Ticket { Project = projectRepository.GetProjectById(projectId) });
        }

        [Authorize(Roles = "Admin, Project Manager, Submitter")]
        [HttpPost]        
        public async Task<IActionResult> Create(Ticket model)
        {
            ApplicationUser submitter = await GetCurrentUserAsync();
            
            Ticket ticket = new()
            {
                Id = Guid.NewGuid().ToString(),
                ProjectId = model.ProjectId,
                SubmitterId = submitter.Id,                
                Title = model.Title,
                Description = model.Description,
                SubmittedDate = DateTime.Now,                
                Type = model.Type,
                Status = model.Status,
                Priority = model.Priority,                
            };

            ticket = repository.Create(ticket);
            return RedirectToAction("Details", new { id = ticket.Id });
        }

        [Authorize(Roles = "Admin, Project Manager, Submitter")]
        [HttpPost]
        public async Task<IActionResult> CreateForProject(Ticket model)
        {
            ApplicationUser submitter = await GetCurrentUserAsync();            

            Ticket ticket = new()
            {
                Id = Guid.NewGuid().ToString(),
                ProjectId = model.ProjectId,
                SubmitterId = submitter.Id,
                AssignedDeveloperId = model.AssignedDeveloperId,
                Title = model.Title,
                Description = model.Description,
                SubmittedDate = DateTime.Now,
                Type = model.Type,
                Status = model.Status,
                Priority = model.Priority,                
            };

            ticket = repository.Create(ticket);
            return RedirectToAction("Details", new { id = ticket.Id });
        }

        [HttpGet]
        public IActionResult Details(string id, int? page)
        {
            Ticket ticket = repository.GetTicketById(id);

            TicketViewModel model = new()
            {
                Id = ticket.Id,
                ProjectId = ticket.ProjectId,
                SubmitterId = ticket.SubmitterId,
                AssignedDeveloperId = ticket.AssignedDeveloperId,
                Title = ticket.Title,
                Description = ticket.Description,
                SubmittedDate = ticket.SubmittedDate,                
                Type = ticket.Type,
                Status = ticket.Status,
                Priority = ticket.Priority,
                Project = projectRepository.GetProjectById(ticket.ProjectId),
                Submitter = userManager.Users.First(u => u.Id == ticket.SubmitterId),
                AssignedDeveloper = userManager.Users.FirstOrDefault(u => u.Id == ticket.AssignedDeveloperId),
                TicketHistoryRecords = ticketHistoryRecordRepository.GetRecordsByTicket(id).ToPagedList(page ?? 1, 5)
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> FilterTicketsReturnPartial(string? searchTerm)
        {
            IEnumerable<Ticket> tickets = await ticketHelper.GetUserRoleTickets();

            if (searchTerm == null)
            {
                return PartialView("_TicketList", tickets.ToPagedList(1, 5));
            }

            if (tickets.ToList().Count == 0)
            {
                throw new Exception("No tickets to filter based on predicate");
            }

            var filteredTickets = tickets.Where(t => 
                t.Title.ToLowerInvariant().Contains(searchTerm)
                || t.Status.ToLowerInvariant().Contains(searchTerm)
                || t.Priority.ToLowerInvariant().Contains(searchTerm)
            );

            return PartialView("_TicketList", filteredTickets.ToPagedList(1, 5));
        }

        [Authorize(Roles = "Admin, Project Manager, Submitter")]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {            
            Ticket ticket = repository.GetTicketById(id);

            EditTicketViewModel model = new()
            {
                Id = id,
                Title = ticket.Title,
                Description = ticket.Description,
                ProjectId = ticket.ProjectId,
                AssignedDeveloperId = ticket.AssignedDeveloperId,
                Type = ticket.Type,
                Status = ticket.Status,
                Priority = ticket.Priority,
            };

            ViewBag.Projects = await projectHelper.GetUserRoleProjects();
            return View(model);
        }

        [Authorize(Roles = "Admin, Project Manager, Submitter")]
        [HttpPost]
        public async Task<IActionResult> Update(EditTicketViewModel model)
        {            
            Ticket ticket = repository.GetTicketById(model.Id);

            if (ticket == null)
            {
                ViewBag.ErrorMessage = $"Ticket with Id {model.Id} cannot be found";
                return View("NotFound");
            }

            PropertyInfo[] modelProperties = model.GetType().GetProperties();
            ApplicationUser modifier = await GetCurrentUserAsync();            
          
            foreach (var property in modelProperties) 
            {          
                PropertyInfo? ticketProperty = ticket.GetType().GetProperty(property.Name);               

                if (ticketProperty != null)
                {
                    string? ticketPropertyValue = ticketProperty.GetValue(ticket)?.ToString();
                    string? propertyValue = property.GetValue(model)?.ToString();

                    if (ticketPropertyValue != propertyValue)
                    {                        
                        TicketHistoryRecord record = new()
                        {
                            Id = Guid.NewGuid().ToString(),
                            TicketId = ticket.Id,
                            Property = property.Name,
                            OldValue = ticketPropertyValue,
                            NewValue = propertyValue,
                            Modifier = modifier.UserName,
                            DateChanged = DateTime.Now
                        };
                        ticketHistoryRecordRepository.Create(record);
                        ticketProperty.SetValue(ticket, property.GetValue(model));
                    }                 
                }                                
            }            
            repository.Update(ticket);
            return RedirectToAction("Details", new { id = ticket.Id });
        }

        [Authorize(Roles = "Admin, Project Manager, Submitter")]
        [HttpDelete]
        public IActionResult Delete(string id)
        {
            Ticket ticket = repository.GetTicketById(id);

            if (ticket == null)
            {
                ViewBag.ErrorMessage = $"Ticket with Id {id} cannot be found";
                return View("NotFound");
            }            

            ticketHistoryRecordRepository.DeleteRecordsByTicketId(id);
            repository.Delete(id);
            return Json(ticket);
        }
    }
}
