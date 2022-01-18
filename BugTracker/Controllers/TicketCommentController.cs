using BugTracker.Models;
using BugTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace BugTracker.Controllers
{
    [Authorize]
    public class TicketCommentController : Controller
    {
        private readonly ITicketCommentRepository repo;
        private readonly UserManager<ApplicationUser> userManager;

        public TicketCommentController(ITicketCommentRepository repo, UserManager<ApplicationUser> userManager)
        {
            this.repo = repo;
            this.userManager = userManager;
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return userManager.GetUserAsync(HttpContext.User);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TicketComment model, string id)
        {
            if (model.Value.Length < 1 || model.Value.Length > 200)
            {
                return new BadRequestObjectResult("Comment must be between 1 and 200 characters");
            }

            ApplicationUser user = await GetCurrentUserAsync();
            TicketComment comment = new()
            {
                Id = Guid.NewGuid().ToString(),
                TicketId = id,
                AuthorId = user.Id,
                Value = model.Value,
                CreatedAt = DateTimeOffset.Now,
            };
                      
            repo.Create(comment);
            IEnumerable<TicketComment> comments = repo.GetCommentsByTicketId(id);
            ViewBag.Id = id;
            return PartialView("_TicketCommentList", comments.ToPagedList(1, 5));            
        }

        [HttpGet]
        public IActionResult FilterCommentsByAuthorReturnPartial(string id, string? searchTerm)
        {
            IEnumerable<TicketComment> comments = repo.GetCommentsByTicketId(id);

            if (searchTerm == null)
            {
                ViewBag.Id = id;
                return PartialView("_TicketCommentList", comments.ToPagedList(1, 5));
            }

            var filteredComments = comments.Where(c => c.Author.UserName.ToLowerInvariant().Contains(searchTerm));
            ViewBag.Id = id;
            return PartialView("_TicketCommentList", filteredComments.ToPagedList(1, 5));
        }

        public IActionResult Delete(string id)
        {
            TicketComment comment = repo.Delete(id);            
            return RedirectToAction("Details", "Ticket", new { id = comment.TicketId });
        }
    }
}
