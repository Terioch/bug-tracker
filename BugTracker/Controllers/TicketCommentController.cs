using BugTracker.Models;
using BugTracker.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace BugTracker.Controllers
{
    public class TicketCommentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TicketCommentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _unitOfWork.UserManager.GetUserAsync(HttpContext.User);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TicketComment model, string ticketId)
        {
            if (model.Value.Length < 1 || model.Value.Length > 200)
            {
                return new BadRequestObjectResult("Comment must be between 1 and 200 characters");
            }

            var user = await GetCurrentUserAsync();

            var comment = new TicketComment()
            {
                Id = Guid.NewGuid().ToString(),
                TicketId = ticketId,
                AuthorId = user.Id,
                Value = model.Value,
                CreatedAt = DateTimeOffset.UtcNow,
            };
                      
            _unitOfWork.TicketComments.Add(comment);                        

            await _unitOfWork.CompleteAsync();

            var comments = _unitOfWork.TicketComments.Find(c => c.TicketId == ticketId);

            ViewBag.Id = ticketId;

            return PartialView("_TicketCommentList", comments.ToPagedList(1, 8));            
        }

        [HttpGet]
        public IActionResult FilterCommentsByAuthorReturnPartial(string ticketId, string? searchTerm)
        {
            var comments = _unitOfWork.TicketComments.Find(c => c.TicketId == ticketId);

            if (searchTerm == null)
            {
                ViewBag.Id = ticketId;
                return PartialView("_TicketCommentList", comments.ToPagedList(1, 8));
            }

            var filteredComments = comments.Where(c => c.Author.UserName.ToLowerInvariant().Contains(searchTerm));

            ViewBag.Id = ticketId;

            return PartialView("_TicketCommentList", filteredComments.ToPagedList(1, 8));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var comment = await _unitOfWork.TicketComments.Get(id);

            if (comment == null)
            {
                return NotFound();
            }

            _unitOfWork.TicketComments.Delete(comment);            

            return RedirectToAction("Details", "Ticket", new { id = comment.TicketId });
        }
    }
}
