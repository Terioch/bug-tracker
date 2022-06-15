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
        private readonly IUnitOfWork _unitOfWork;
        private readonly ProjectHelper _projectHelper;
        private readonly TicketHelper _ticketHelper;
        private readonly TicketAttachmentHelper _attachmentHelper;

        public TicketController(IUnitOfWork unitOfWork, ProjectHelper projectHelper, TicketHelper ticketHelper, TicketAttachmentHelper attachmentHelper)
        {
            _unitOfWork = unitOfWork;
            _projectHelper = projectHelper;
            _ticketHelper = ticketHelper;
            _attachmentHelper = attachmentHelper;
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _unitOfWork.UserManager.GetUserAsync(HttpContext.User);
        }

        private Task<ApplicationUser> FindUserByNameAsync(string name)
        {
            return _unitOfWork.UserManager.FindByNameAsync(name);
        }

        public async Task<IActionResult> ListTickets(int? page)
        {
            IEnumerable<Ticket> tickets = await _ticketHelper.GetUserRoleTickets();           
            return View(tickets.ToPagedList(page ?? 1, 8));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id, int? historyPage, int? attachmentsPage, int? commentsPage)
        {
            var ticket = await _unitOfWork.Tickets.Get(id);

            if (ticket == null)
            {
                return NotFound();
            }

            var model = new TicketViewModel()
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

                var ticket = new Ticket()
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
                bool isSubmitterAssigned = await _projectHelper.IsUserOnProject(ticket.SubmitterId, ticket.ProjectId);

                if (!isSubmitterAssigned)
                {                   
                    var user = await _unitOfWork.Users.Get(ticket.SubmitterId);
                    var project = await _unitOfWork.Projects.Get(ticket.ProjectId);
                    project.Users.Add(user);
                }
                
                _unitOfWork.Tickets.Add(ticket);

                await _unitOfWork.CompleteAsync();

                return RedirectToAction("Details", new { id = ticket.Id });
            }

            return View();
        }                

        [HttpGet]
        public async Task<IActionResult> FilterTicketsReturnPartial(string? searchTerm)
        {
            var tickets = await _ticketHelper.GetUserRoleTickets();

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
            var tickets = _unitOfWork.Tickets.Find(t => t.ProjectId == id);

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
            var tickets = _unitOfWork.Tickets.Find(t => t.AssignedDeveloperId == id || t.SubmitterId == id);

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
            var ticket = await _unitOfWork.Tickets.Get(id);

            if (ticket == null)
            {
                return NotFound();
            }

            var model = new EditTicketViewModel()
            {
                Id = id,
                Title = ticket.Title,
                Description = ticket.Description,
                ProjectId = ticket.ProjectId,
                AssignedDeveloperId = ticket.AssignedDeveloperId,
                Type = ticket.Type,
                Status = ticket.Status,
                Priority = ticket.Priority,                 
                AssignableUsers = await _projectHelper.GetUsersInRolesOnProject(ticket.ProjectId, new string[] { "Admin", "Developer" }),
            };

            return View(model);
        }

        [Authorize(Roles = "Admin, Project Manager, Submitter")]
        [HttpPost]
        public async Task<IActionResult> Edit(EditTicketViewModel model)
        {
            var ticket = await _unitOfWork.Tickets.Get(model.Id);

            if (ticket == null)
            {
                return NotFound();
            }

            // Update property and property history if new value differs from original
            var modelProperties = model.GetType().GetProperties();
            var modifier = await GetCurrentUserAsync();            
            
            foreach (var property in modelProperties) 
            {          
                PropertyInfo? ticketProperty = ticket.GetType().GetProperty(property.Name);               

                if (ticketProperty != null)
                {
                    string? ticketPropertyValue = ticketProperty.GetValue(ticket)?.ToString();
                    string? propertyValue = property.GetValue(model)?.ToString();

                    if (ticketPropertyValue != propertyValue)
                    {                        
                        var ticketHistoryRecord = new TicketHistoryRecord()
                        {
                            Id = Guid.NewGuid().ToString(),
                            TicketId = ticket.Id,
                            ModifierId = modifier.Id,
                            Property = property.Name,
                            OldValue = ticketPropertyValue,
                            NewValue = propertyValue,                           
                            ModifiedAt = DateTimeOffset.Now
                        };

                        _unitOfWork.TicketHistoryRecords.Add(ticketHistoryRecord);

                        ticketProperty.SetValue(ticket, property.GetValue(model));
                    }                 
                }                                
            }            

            // Ensure that the assigned developer is assigned to the corresponding project
            if (ticket.AssignedDeveloperId != null)
            {               
                var project = await _unitOfWork.Projects.Get(ticket.ProjectId);

                bool isDeveloperAssigned = project.Users.Select(u => u.Id).Contains(ticket.AssignedDeveloperId);

                if (!isDeveloperAssigned)
                {                    
                    var user = await _unitOfWork.Users.Get(ticket.AssignedDeveloperId);
                    project.Users.Add(user);
                }
            }

            await _unitOfWork.CompleteAsync();

            return RedirectToAction("Details", new { id = ticket.Id });
        }

        [Authorize(Roles = "Developer")]
        [HttpPost]
        public async Task <IActionResult> EditStatus(TicketViewModel model)
        {
            var ticket = await _unitOfWork.Tickets.Get(model.Id);

            if (ticket == null)
            {
                return NotFound();
            }

            var modifier = await GetCurrentUserAsync();

            if (ticket.Status != model.Status)
            {
                var record = new TicketHistoryRecord()
                {
                    Id = Guid.NewGuid().ToString(),
                    TicketId = ticket.Id,
                    ModifierId = modifier.Id,
                    Property = "Status",
                    OldValue = ticket.Status,
                    NewValue = model.Status,                    
                    ModifiedAt = DateTimeOffset.Now,
                };

                _unitOfWork.TicketHistoryRecords.Add(record);

                ticket.Status = model.Status;

                await _unitOfWork.CompleteAsync();
            }          
            
            return RedirectToAction("Details", new { id = ticket.Id });
        }

        [Authorize(Roles = "Admin, Project Manager, Submitter")]
        public async Task<IActionResult> Delete(string id)
        {
            //ticketHistoryRepo.DeleteByTicketId(id)          
            /*foreach (var attachment in ticketAttachmentRepo.GetAttachmentsByTicketId(id))
            {
                attachmentHelper.RemoveUploadedFileAttachment(attachment);
            }*/
            //ticketAttachmentRepo.DeleteByTicketId(id);            
            //ticketCommentRepo.DeleteCommentsByTicketId(id);
            var ticket = await _unitOfWork.Tickets.Get(id);

            if (ticket == null)
            {
                return NotFound();
            }

            _unitOfWork.Tickets.Delete(ticket);

            await _unitOfWork.CompleteAsync();

            return RedirectToAction("ListTickets");
        }
    }
}
