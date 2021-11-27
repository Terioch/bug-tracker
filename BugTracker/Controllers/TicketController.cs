using BugTracker.Models;
using BugTracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace BugTracker.Controllers
{
    public class TicketController : Controller
    {
        private readonly ITicketRepository repository;
        private readonly IProjectRepository projectRepository;

        public TicketController(ITicketRepository repository, IProjectRepository projectRepository)
        {
            this.repository = repository;
            this.projectRepository = projectRepository;
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
        public IActionResult Create(TicketViewModel viewTicket)
        {
            Ticket ticket = new()
            {
                Id = Guid.NewGuid().ToString(),
                ProjectId = projectRepository.GetProjectByName(viewTicket.ProjectName).Id,
                Title = viewTicket.Title,
                Description = viewTicket.Description,
                SubmittedDate = DateTime.Now,
                Submitter = "",
                AssignedDeveloper = viewTicket.AssignedDeveloper,
                Type = viewTicket.Type,
                Status = viewTicket.Status,
                Priority = viewTicket.Priority,
            };
            // ticket = repository.Create(ticket);
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
