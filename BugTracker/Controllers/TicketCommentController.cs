using BugTracker.Models;
using BugTracker.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace BugTracker.Controllers
{
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
            ApplicationUser user = await GetCurrentUserAsync();
            TicketComment comment = new()
            {
                Id = Guid.NewGuid().ToString(),
                TicketId = id,
                AuthorId = user.Id,
                Value = model.Value,
                CreatedAt = DateTimeOffset.Now,
                /*Ticket = repository.GetTicketById(id),
                Author = userManager.Users.First(u => u.Id == user.Id)*/
            };
            repo.Create(comment);
            IEnumerable<TicketComment> comments = repo.GetCommentsByTicketId(id);
            ViewBag.Id = id;
            return PartialView("_TicketCommentList", comments.ToPagedList(1, 5));
        }

        [HttpDelete]
        public IActionResult Delete(string id)
        {
            TicketComment comment = repo.Delete(id);
            IEnumerable<TicketComment> comments = repo.GetCommentsByTicketId(comment.TicketId);
            return PartialView("_TicketCommentList", comments.ToPagedList(1, 5));
        }
    }
}
