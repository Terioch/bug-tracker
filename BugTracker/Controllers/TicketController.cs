using BugTracker.Helpers;
using BugTracker.Models;
using BugTracker.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using X.PagedList;

namespace BugTracker.Controllers
{
    public class TicketController : Controller
    {
        private readonly ITicketRepository repo;
        private readonly IProjectRepository projectRepo;
        private readonly IUserProjectRepository userProjectRepo;
        private readonly ITicketHistoryRepository ticketHistoryRepo;
        private readonly ITicketCommentRepository ticketCommentRepo;
        private readonly ITicketAttachmentRepository ticketAttachmentRepo;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ProjectHelper projectHelper;
        private readonly TicketHelper ticketHelper;
        private readonly TicketAttachmentHelper attachmentHelper;

        public TicketController(ITicketRepository repo, IProjectRepository projectRepo, IUserProjectRepository userProjectRepo, ITicketHistoryRepository ticketHistoryRepo,
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
            return View(tickets.ToPagedList(page ?? 1, 8));
        }

        [HttpGet]
        public IActionResult Details(string id, int? historyPage, int? attachmentsPage, int? commentsPage)
        {
            Ticket ticket = repo.Get(id);
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
                TicketHistoryRecords = ticket.TicketHistoryRecords.ToPagedList(historyPage ?? 1, 5),
                TicketAttachments = ticket.TicketAttachments.ToPagedList(attachmentsPage ?? 1, 6),
                TicketComments = ticket.TicketComments.ToPagedList(commentsPage ?? 1, 8),
            };
            return View(model);
        }

        [Authorize(Roles = "Admin, Project Manager, Submitter")]
        [HttpGet]        
        public IActionResult Create()
        {                         
            return View();
        }

        [Authorize(Roles = "Admin, Project Manager, Submitter")]
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
                    CreatedAt = DateTimeOffset.Now,
                    Type = model.Type,
                    Status = model.Status,
                    Priority = model.Priority,
                };
                
                // Ensure that the submitter is assigned to the corresponding project
                var assignedUsers = userProjectRepo.GetUsersByProjectId(ticket.ProjectId);
                bool isSubmitterAssigned = assignedUsers.Select(u => u.Id).Contains(ticket.SubmitterId);

                if (!isSubmitterAssigned)
                {
                    UserProject userProject = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = ticket.SubmitterId,
                        ProjectId = ticket.ProjectId,
                    };

                    userProjectRepo.Add(userProject);
                }
                
                ticket = repo.Create(ticket);

                return RedirectToAction("Details", new { id = ticket.Id });
            }

            return View();
        }                

        [HttpGet]
        public async Task<IActionResult> FilterTicketsReturnPartial(string? searchTerm)
        {
            IEnumerable<Ticket> tickets = await ticketHelper.GetUserRoleTickets();

            if (searchTerm == null)
            {
                return PartialView("_TicketList", tickets.ToPagedList(1, 8));
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

           return PartialView("_TicketList", filteredTickets.ToPagedList(1, 8));
        }

        [HttpGet]
        public IActionResult FilterProjectTicketsReturnPartial(string id, string? searchTerm)
        {
            IEnumerable<Ticket> tickets = repo.GetByProjectId(id);
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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult FilterUserTicketsReturnPartial(string id, string? searchTerm)
        {
            var tickets = repo.GetAll().Where(t => t.AssignedDeveloperId == id || t.SubmitterId == id);

            if (searchTerm == null)
            {
                ViewBag.Id = id;
                return PartialView("~/Views/User/_UserTicketList.cshtml", tickets.ToPagedList(1, 5));
            }

            var filteredTickets = tickets.Where(t =>
            {
                if (t.AssignedDeveloperId == null)
                {
                    t.AssignedDeveloper = new ApplicationUser() { UserName = "" };
                }

                return t.Title.ToLowerInvariant().Contains(searchTerm)
                || t.Status.ToLowerInvariant().Contains(searchTerm)
                || t.AssignedDeveloper.UserName.ToLowerInvariant().Contains(searchTerm)
                || t.Submitter.UserName.ToLowerInvariant().Contains(searchTerm);
            });

            ViewBag.Id = id;
            return PartialView("~/Views/User/_UserTicketList.cshtml", filteredTickets.ToPagedList(1, 5));
        }

        [Authorize(Roles = "Admin, Project Manager, Submitter")]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {            
            Ticket ticket = repo.Get(id);

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
                AssignableUsers = await projectHelper.GetUsersInRolesOnProject(ticket.ProjectId, new string[] { "Admin", "Developer" }),
            };
            return View(model);
        }

        [Authorize(Roles = "Admin, Project Manager, Submitter")]
        [HttpPost]
        public async Task<IActionResult> Edit(EditTicketViewModel model)
        {            
            Ticket ticket = repo.Get(model.Id);            

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
                        TicketHistoryRecord ticketHistoryRecord = new()
                        {
                            Id = Guid.NewGuid().ToString(),
                            TicketId = ticket.Id,
                            ModifierId = modifier.Id,
                            Property = property.Name,
                            OldValue = ticketPropertyValue,
                            NewValue = propertyValue,                           
                            ModifiedAt = DateTimeOffset.Now
                        };

                        ticketHistoryRepo.Add(ticketHistoryRecord);
                        ticketProperty.SetValue(ticket, property.GetValue(model));
                    }                 
                }                                
            }            

            // Ensure that the assigned developer is assigned to the corresponding project
            if (ticket.AssignedDeveloperId != null)
            {                
                var assignedUsers = userProjectRepo.GetUsersByProjectId(ticket.ProjectId);
                bool isDeveloperAssigned = assignedUsers.Select(u => u.Id).Contains(ticket.AssignedDeveloperId);

                if (!isDeveloperAssigned)
                {
                    UserProject userProject = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = ticket.AssignedDeveloperId,
                        ProjectId = ticket.ProjectId,
                    };
                    userProjectRepo.Add(userProject);
                }
            }            

            repo.Update(ticket);

            return RedirectToAction("Details", new { id = ticket.Id });
        }

        [Authorize(Roles = "Developer")]
        [HttpPost]
        public async Task <IActionResult> EditStatus(TicketViewModel model)
        {
            Ticket ticket = repo.Get(model.Id);
            ApplicationUser modifier = await GetCurrentUserAsync();

            if (ticket.Status != model.Status)
            {
                TicketHistoryRecord record = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    TicketId = ticket.Id,
                    ModifierId = modifier.Id,
                    Property = "Status",
                    OldValue = ticket.Status,
                    NewValue = model.Status,                    
                    ModifiedAt = DateTimeOffset.Now,
                };

                ticketHistoryRepo.Add(record);
                ticket.Status = model.Status;
                repo.Update(ticket);
            }                        
            return RedirectToAction("Details", new { id = ticket.Id });
        }

        [Authorize(Roles = "Admin, Project Manager, Submitter")]
        public IActionResult Delete(string id)
        {                      
            ticketHistoryRepo.DeleteByTicketId(id);
            /*foreach (var attachment in ticketAttachmentRepo.GetAttachmentsByTicketId(id))
            {
                attachmentHelper.RemoveUploadedFileAttachment(attachment);
            }*/
            ticketAttachmentRepo.DeleteByTicketId(id);            
            ticketCommentRepo.DeleteCommentsByTicketId(id);
            repo.Delete(id);
            return RedirectToAction("ListTickets");
        }
    }
}
