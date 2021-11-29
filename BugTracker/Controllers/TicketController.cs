using BugTracker.Areas.Identity.Data;
using BugTracker.Models;
using BugTracker.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace BugTracker.Controllers
{
    public class TicketController : Controller
    {
        private readonly ITicketRepository repository;
        private readonly IProjectRepository projectRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public TicketController(ITicketRepository repository, IProjectRepository projectRepository, UserManager<ApplicationUser> userManager)
        {
            this.repository = repository;
            this.projectRepository = projectRepository;
            this.userManager = userManager;
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return userManager.GetUserAsync(HttpContext.User);
        }

        private Task<ApplicationUser> FindUserByNameAsync(string name)
        {
            return userManager.FindByNameAsync(name);
        }

        public IActionResult ListTickets()
        {
            IEnumerable<Ticket> tickets = repository.GetAllTickets();
            return View(tickets);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTicketViewModel model)
        {
            ApplicationUser Submitter = await GetCurrentUserAsync();            

            Ticket ticket = new()
            {
                Id = Guid.NewGuid().ToString(),
                ProjectId = projectRepository.GetProjectByName(model.ProjectName).Id,
                Submitter = Submitter.UserName,
                AssignedDeveloper = model.AssignedDeveloper,
                Title = model.Title,
                Description = model.Description,
                SubmittedDate = DateTime.Now,                
                Type = model.Type,
                Status = model.Status,
                Priority = model.Priority,
            };

            ticket = repository.Create(ticket);            
            return View("Details", ticket);
        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            Ticket ticket = repository.GetTicketById(id);
            return View(ticket);
        }

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
        public IActionResult Update(EditTicketViewModel model)
        {            
            Ticket ticket = repository.GetTicketById(model.Id);

            if (ticket == null)
            {
                ViewBag.ErrorMessage = $"Ticket with Id {model.Id} cannot be found";
                return View("NotFound");
            }

            foreach (var property in ticket.GetType().GetProperties())
            {          
                PropertyInfo? modelProperty = model.GetType().GetProperty(property.Name);
               
                if (modelProperty != null)
                {
                    property.SetValue(ticket, modelProperty.GetValue(model));
                }
            }

            repository.Update(ticket);
            return View("Details", ticket);
        }

        [HttpDelete]
        public IActionResult Delete(string id)
        {
            repository.Delete(id);
            return RedirectToAction("ListTickets", "Ticket");
        }
    }
}
