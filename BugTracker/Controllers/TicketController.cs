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
        private readonly ITicketRepository repo;
        private readonly IProjectRepository projectRepo;
        private readonly ITicketHistoryRecordRepository ticketHistoryRepo;
        private readonly ITicketCommentRepository ticketCommentRepo;
        private readonly ITicketAttachmentRepository ticketAttachmentRepo;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ProjectHelper projectHelper;
        private readonly TicketHelper ticketHelper;

        public TicketController(ITicketRepository repo, IProjectRepository projectRepo, ITicketHistoryRecordRepository ticketHistoryRepo, ITicketCommentRepository ticketCommentRepo, ITicketAttachmentRepository ticketAttachmentRepo, UserManager<ApplicationUser> userManager, ProjectHelper projectHelper, TicketHelper ticketHelper)
        {
            this.repo = repo;
            this.projectRepo = projectRepo;
            this.ticketHistoryRepo = ticketHistoryRepo;
            this.ticketCommentRepo = ticketCommentRepo;
            this.ticketAttachmentRepo = ticketAttachmentRepo;
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
    
        [Authorize(Roles = "Admin, Demo Admin, Project Manager, Demo Project Manager, Submitter, Demo Submitter")]        
        [HttpGet]        
        public IActionResult Create()
        {              
            // ViewBag.Projects = await projectHelper.GetUserRoleProjects();
            return View();
        }

        [Authorize(Roles = "Admin, Demo Admin, Project Manager, Demo Project Manager, Submitter, Demo Submitter")]
        [HttpPost]        
        public async Task<IActionResult> Create(CreateTicketViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser submitter = await GetCurrentUserAsync();

                Ticket ticket = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    ProjectId = model.ProjectId,
                    SubmitterId = submitter.Id,
                    Title = model.Title,
                    Description = model.Description,
                    CreatedAt = DateTime.Now,
                    Type = model.Type,
                    Status = model.Status,
                    Priority = model.Priority,
                };

                ticket = repo.Create(ticket);
                return RedirectToAction("Details", new { id = ticket.Id });
            }
            return View();
        }        

        [HttpGet]
        public IActionResult Details(string id, int? historyPage, int? attachmentsPage, int? commentsPage)
        {           
            Ticket ticket = repo.GetTicketById(id);
            TicketViewModel model = new()
            {
                Id = ticket.Id,
                ProjectId = ticket.ProjectId,
                SubmitterId = ticket.SubmitterId,
                AssignedDeveloperId = ticket.AssignedDeveloperId,
                Title = ticket.Title,
                Description = ticket.Description,
                SubmittedDate = ticket.CreatedAt,
                Type = ticket.Type,
                Status = ticket.Status,
                Priority = ticket.Priority,
                Project = ticket.Project,
                Submitter = ticket.Submitter,
                AssignedDeveloper = ticket.AssignedDeveloper,
                TicketHistoryRecords = ticket.TicketHistoryRecords.ToPagedList(historyPage ?? 1, 6),
                TicketAttachments = ticket.TicketAttachments.ToPagedList(attachmentsPage ?? 1, 6),
                TicketComments = ticket.TicketComments.ToPagedList(commentsPage ?? 1, 5),
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

        [Authorize(Roles = "Admin, Demo Admin, Project Manager, Demo Project Manager, Submitter, Demo Submitter, Developer, Demo Developer")]
        [HttpGet]
        public IActionResult Edit(string id)
        {            
            Ticket ticket = repo.GetTicketById(id);

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

            // ViewBag.Projects = await projectHelper.GetUserRoleProjects();
            return View(model);
        }

        [Authorize(Roles = "Admin, Demo Admin, Project Manager, Demo Project Manager, Submitter, Demo Submitter, Developer, Demo Developer")]
        [HttpPost]
        public async Task<IActionResult> Edit(EditTicketViewModel model)
        {            
            Ticket ticket = repo.GetTicketById(model.Id);

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
                            ModifiedAt = DateTime.Now
                        };
                        ticketHistoryRepo.Create(record);
                        ticketProperty.SetValue(ticket, property.GetValue(model));
                    }                 
                }                                
            }            
            repo.Update(ticket);
            return RedirectToAction("Details", new { id = ticket.Id });
        }

        [Authorize(Roles = "Admin, Demo Admin, Project Manager, Demo Project Manager, Submitter, Demo Submitter")]
        public IActionResult Delete(string id)
        {
            Ticket ticket = repo.GetTicketById(id);

            if (ticket == null)
            {
                ViewBag.ErrorMessage = $"Ticket with Id {id} cannot be found";
                return View("NotFound");
            }            

            ticketHistoryRepo.DeleteRecordsByTicketId(id);
            ticketAttachmentRepo.DeleteAttachmentsByTicketId(id);
            ticketCommentRepo.DeleteCommentsByTicketId(id);
            repo.Delete(id);
            return RedirectToAction("ListTickets");
        }
    }
}
