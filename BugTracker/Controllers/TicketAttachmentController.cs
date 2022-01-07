using Microsoft.AspNetCore.Mvc;

namespace BugTracker.Controllers
{
    public class TicketAttachmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
