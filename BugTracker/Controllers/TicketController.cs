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
        private readonly IUserProjectRepository userProjectRepo;
        private readonly ITicketHistoryRecordRepository ticketHistoryRepo;
        private readonly ITicketCommentRepository ticketCommentRepo;
        private readonly ITicketAttachmentRepository ticketAttachmentRepo;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ProjectHelper projectHelper;
        private readonly TicketHelper ticketHelper;
        private readonly TicketAttachmentHelper attachmentHelper;

        public TicketController(ITicketRepository repo, IProjectRepository projectRepo, IUserProjectRepository userProjectRepo, ITicketHistoryRecordRepository ticketHistoryRepo,
            ITicketCommentRepository ticketCommentRepo, ITicketAttachmentRepository ticketAttachmentRepo, UserManager<ApplicationUser> userManager, ProjectHelper projectHelper, 
            TicketHelper ticketHelper, TicketAttachmentHelper attachmentHelper)
        {
            this.repo = repo;
            this.projectRepo = projectRepo;
            this.userProjectRepo = userProjectRepo;
            this.ticketHistoryRepo = ticketHistoryRepo;
            this.ticketCommentRepo = ticketCommentRepo;
            this.ticketAttachmentRepo = ticketAttachmentRepo;
            this.userManager = userManager;
            this.projectHelper = projectHelper;
            this.ticketHelper = ticketHelper;
            this.attachmentHelper = attachmentHelper;
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

            /*if (tickets.ToList().Count == 0)
            {
                throw new Exception("No tickets to filter based on predicate");
            }*/

            var filteredTickets = tickets.Where(t => 
                t.Title.ToLowerInvariant().Contains(searchTerm)
                || t.Status.ToLowerInvariant().Contains(searchTerm)
                || t.Priority.ToLowerInvariant().Contains(searchTerm)
            );

            return PartialView("_TicketList", filteredTickets.ToPagedList(1, 5));
        }

        [Authorize(Roles = "Admin, Demo Admin, Project Manager, Demo Project Manager, Submitter, Demo Submitter")]
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

        [Authorize(Roles = "Admin, Demo Admin, Project Manager, Demo Project Manager, Submitter, Demo Submitter")]
        [HttpPost]
        public async Task<IActionResult> Edit(EditTicketViewModel model)
        {            
            Ticket ticket = repo.GetTicketById(model.Id);            

            // Update property and property history if new value differs from original
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

            // Ensure that the assigned developer is assigned to the corresponding project
            IEnumerable<ApplicationUser> assignedUsers = userProjectRepo.GetUsersByProjectId(ticket.ProjectId);
            bool isDeveloperAssigned = assignedUsers.Select(u => u.Id).Contains(ticket.AssignedDeveloperId);

            if (!isDeveloperAssigned)
            {
                UserProject userProject = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = ticket.AssignedDeveloperId,
                    ProjectId = ticket.ProjectId,
                };
                userProjectRepo.Create(userProject);
            }
            repo.Update(ticket);
            return RedirectToAction("Details", new { id = ticket.Id });
        }

        [Authorize(Roles = "Developer, Demo Developer")]
        [HttpPost]
        public async Task <IActionResult> EditStatus(TicketViewModel model)
        {
            Ticket ticket = repo.GetTicketById(model.Id);
            ApplicationUser modifier = await GetCurrentUserAsync();

            if (ticket.Status != model.Status)
            {
                TicketHistoryRecord record = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    TicketId = ticket.Id,
                    Property = "Status",
                    OldValue = ticket.Status,
                    NewValue = model.Status,
                    Modifier = modifier.UserName,
                    ModifiedAt = DateTime.Now,
                };

                ticketHistoryRepo.Create(record);
                ticket.Status = model.Status;
                repo.Update(ticket);
            }                        
            return RedirectToAction("Details", new { id = ticket.Id });
        }

        [Authorize(Roles = "Admin, Demo Admin, Project Manager, Demo Project Manager, Submitter, Demo Submitter")]
        public IActionResult Delete(string id)
        {
            Ticket ticket = repo.GetTicketById(id);                    
          
            ticketHistoryRepo.DeleteRecordsByTicketId(id);
            foreach (var attachment in ticketAttachmentRepo.GetAttachmentsByTicketId(id))
            {
                attachmentHelper.RemoveUploadedFileAttachment(attachment);
            }
            ticketAttachmentRepo.DeleteAttachmentsByTicketId(id);            
            ticketCommentRepo.DeleteCommentsByTicketId(id);
            repo.Delete(id);
            return RedirectToAction("ListTickets");
        }
    }
}
