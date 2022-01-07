using BugTracker.Models;
using BugTracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace BugTracker.Controllers
{
    public class TicketAttachmentController : Controller
    {
        private readonly ITicketAttachmentRepository repo;

        public TicketAttachmentController(ITicketAttachmentRepository repo)
        {
            this.repo = repo;
        }

        public IActionResult Create(string ticketId, string attachmentName, IFormFile attachmentFile)
        {
            TicketAttachment attachment = new()
            {
                Id = Guid.NewGuid().ToString(),
                TicketId = ticketId,
                Name = attachmentName,
                FilePath = "attachmentFile.FilePath",
            };
            // repo.Create(attachment);
            return RedirectToAction("ListTickets", "Ticket");
        }
    }
}
