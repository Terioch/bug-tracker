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
            IEnumerable<Ticket> tickets = repository.GetAllTickets();
            ApplicationUser user = await GetCurrentUserAsync();
            IEnumerable<Ticket> filteredTickets = await ticketHelper.FilterTicketsByRole(tickets, user);
            return View(filteredTickets.ToPagedList(page ?? 1, 5));
        }
    
        [Authorize(Roles = "Admin")]        
        [HttpGet]        
        public IActionResult Create()
        {                     
            return View();
        }

        [Authorize(Roles = "Admin, Project Manager, Submitter")]
        [HttpGet]
        public async Task<IActionResult> CreateForProject(string projectName, string id)
        {
            ApplicationUser user = await GetCurrentUserAsync();
            var isAdmin = await userManager.IsInRoleAsync(user, "Admin");

            if  (!isAdmin && !projectHelper.IsUserInProject(user.UserName, id))
            {
                return View("~/Areas/Identity/Pages/Account/AccessDenied.cshtml");
            }
            return View("Create", new CreateTicketViewModel { ProjectName = projectName });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]        
        public async Task<IActionResult> Create(CreateTicketViewModel model)
        {
            ApplicationUser submitter = await GetCurrentUserAsync();            

            Ticket ticket = new()
            {
                Id = Guid.NewGuid().ToString(),
                ProjectId = projectRepository.GetProjectByName(model.ProjectName).Id,
                Submitter = submitter.UserName,
                AssignedDeveloper = model.AssignedDeveloper,
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
        public async Task<IActionResult> CreateForProject(CreateTicketViewModel model)
        {
            ApplicationUser submitter = await GetCurrentUserAsync();
            var isAdmin = await userManager.IsInRoleAsync(submitter, "Admin");
            string? projectId = projectRepository.GetProjectByName(model.ProjectName).Id;

            if (!isAdmin && !projectHelper.IsUserInProject(submitter.UserName, projectId))
            {
                return View("~/Account/Denied");
            }

            Ticket ticket = new()
            {
                Id = Guid.NewGuid().ToString(),
                ProjectId = projectRepository.GetProjectByName(model.ProjectName).Id,
                Submitter = submitter.UserName,
                AssignedDeveloper = model.AssignedDeveloper,
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
            List<TicketHistoryRecord> historyRecords = ticketHistoryRecordRepository.GetRecordsByTicket(id);
            TicketViewModel model = new()
            {
                Id = ticket.Id,
                ProjectName = projectRepository.GetProjectById(ticket.ProjectId).Name,
                Title = ticket.Title,
                Description = ticket.Description,
                SubmittedDate = ticket.SubmittedDate,
                Submitter = ticket.Submitter,
                AssignedDeveloper = ticket.AssignedDeveloper,
                Type = ticket.Type,
                Status = ticket.Status,
                Priority = ticket.Priority,
                HistoryRecords = historyRecords.ToPagedList(page ?? 1, 5)
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult FilterTicketsReturnPartial(string? searchTerm)
        {
            IEnumerable<Ticket> tickets = repository.GetAllTickets();

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
            ApplicationUser user = await GetCurrentUserAsync();
            var isAdmin = await userManager.IsInRoleAsync(user, "Admin");
            var isSubmitter = await userManager.IsInRoleAsync(user, "Submitter");

            if (!isAdmin && !projectHelper.IsUserInProject(user.UserName, ticket.ProjectId) 
                || isSubmitter && user.UserName != ticket.Submitter)
            {
                return View("~/Account/Denied");
            } 

            EditTicketViewModel model = new()
            {        
                Id = id,
                Title = ticket.Title,
                Description = ticket.Description,
                ProjectName = projectRepository.GetProjectById(ticket.ProjectId).Name,               
                AssignedDeveloper = ticket.AssignedDeveloper,
                Type = ticket.Type,
                Status = ticket.Status,
                Priority = ticket.Priority,
            };            
            return View(model);
        }

        [Authorize(Roles = "Admin, Project Manager, Submitter")]
        [HttpPost]
        public async Task<IActionResult> Update(EditTicketViewModel model)
        {            
            // Authorization: Verify user is the ticket submitter, project manager or admin

            Ticket ticket = repository.GetTicketById(model.Id);

            if (ticket == null)
            {
                ViewBag.ErrorMessage = $"Ticket with Id {model.Id} cannot be found";
                return View("NotFound");
            }

            PropertyInfo[] modelProperties = model.GetType().GetProperties();
            ApplicationUser modifier = await GetCurrentUserAsync();
            var isAdmin = await userManager.IsInRoleAsync(modifier, "Admin");
            var isSubmitter = await userManager.IsInRoleAsync(modifier, "Submitter");            

            if (!isAdmin && !projectHelper.IsUserInProject(modifier.UserName, ticket.ProjectId)
                || isSubmitter && modifier.UserName != ticket.Submitter)
            {
                return View("~/Account/Denied");
            }            

            // Add new record to ticket history if projectId differs
            Project project = projectRepository.GetProjectByName(model.ProjectName);

            if (ticket.ProjectId != project.Id)
            {
                TicketHistoryRecord record = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    TicketId = ticket.Id,
                    Property = "ProjectId",
                    OldValue = ticket.ProjectId,
                    NewValue = project.Id,
                    Modifier = modifier.UserName,
                    DateChanged = DateTime.Now
                };

                ticketHistoryRecordRepository.Create(record);
            }

            ticket.ProjectId = project.Id;
          
            foreach (var property in modelProperties) 
            {          
                PropertyInfo? ticketProperty = ticket.GetType().GetProperty(property.Name);               

                if (ticketProperty != null)
                {
                    string? ticketPropertyValue = ticketProperty.GetValue(ticket).ToString();
                    string? propertyValue = property.GetValue(model).ToString();

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
                    }
                    ticketProperty.SetValue(ticket, property.GetValue(model));
                }                                
            }
            
            repository.Update(ticket);
            return RedirectToAction("Details", new { id = ticket.Id });
        }

        [Authorize(Roles = "Admin, Project Manager, Submitter")]
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            Ticket ticket = repository.GetTicketById(id);

            if (ticket == null)
            {
                ViewBag.ErrorMessage = $"Ticket with Id {id} cannot be found";
                return View("NotFound");
            }

            ApplicationUser user = await GetCurrentUserAsync();
            var isAdmin = await userManager.IsInRoleAsync(user, "Admin");
            var isSubmitter = await userManager.IsInRoleAsync(user, "Submitter");

            if (!isAdmin && !projectHelper.IsUserInProject(user.UserName, ticket.ProjectId)
                || isSubmitter && user.UserName != ticket.Submitter)
            {
                return View("~/Account/Denied");
            }

            ticketHistoryRecordRepository.DeleteRecordsByTicketId(id);
            repository.Delete(id);
            return Json(ticket);
        }
    }
}
