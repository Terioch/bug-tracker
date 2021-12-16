using BugTracker.Areas.Identity.Data;
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

        public TicketController(ITicketRepository repository, IProjectRepository projectRepository, ITicketHistoryRecordRepository ticketHistoryRecordRepository, UserManager<ApplicationUser> userManager, ProjectHelper projectHelper)
        {
            this.repository = repository;
            this.projectRepository = projectRepository;
            this.ticketHistoryRecordRepository = ticketHistoryRecordRepository;
            this.userManager = userManager;
            this.projectHelper = projectHelper;
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return userManager.GetUserAsync(HttpContext.User);
        }

        private Task<ApplicationUser> FindUserByNameAsync(string name)
        {
            return userManager.FindByNameAsync(name);
        }

        public IActionResult ListTickets(int? page)
        {
            IEnumerable<Ticket> tickets = repository.GetAllTickets();
            IPagedList<Ticket> pagedTickets = tickets.ToPagedList(page ?? 1, 5);
            return View(pagedTickets);
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
            if (!projectHelper.IsUserInProject(await GetCurrentUserAsync(), id))
            {
                return View("~/Account/Denied");
            }
            return View("Create", new CreateTicketViewModel { ProjectName = projectName });
        }

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
        public IActionResult Edit(string id)
        {
            
            Ticket ticket = repository.GetTicketById(id); 

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

        [HttpPost]
        public async Task<IActionResult> Update(EditTicketViewModel model)
        {            
            Ticket ticket = repository.GetTicketById(model.Id);
            PropertyInfo[] modelProperties = model.GetType().GetProperties();
            ApplicationUser Modifier = await GetCurrentUserAsync();

            if (ticket == null)
            {
                ViewBag.ErrorMessage = $"Ticket with Id {model.Id} cannot be found";
                return View("NotFound");
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
                    Modifier = Modifier.UserName,
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
                    string ticketPropertyValue = ticketProperty.GetValue(ticket).ToString();
                    string propertyValue = property.GetValue(model).ToString();

                    if (ticketPropertyValue != propertyValue)
                    {
                        TicketHistoryRecord record = new()
                        {
                            Id = Guid.NewGuid().ToString(),
                            TicketId = ticket.Id,
                            Property = property.Name,
                            OldValue = ticketPropertyValue,
                            NewValue = propertyValue,
                            Modifier = Modifier.UserName,
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
        public IActionResult Delete(string id)
        {
            ticketHistoryRecordRepository.DeleteRecordsByTicketId(id);
            Ticket ticket = repository.Delete(id);
            return Json(ticket);
        }
    }
}
