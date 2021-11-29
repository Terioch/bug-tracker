using BugTracker.Areas.Identity.Data;
using BugTracker.Models;
using BugTracker.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
            Ticket ticket = repository.GetTicket(id);
            return View(ticket);
        }

        [HttpPut]
        public IActionResult Update(Ticket ticket)
        {
            repository.Update(ticket);
            return View(ticket);
        }

        [HttpDelete]
        public IActionResult Delete(string id)
        {
            repository.Delete(id);
            return RedirectToAction("ListTickets", "Ticket");
        }
    }
}
