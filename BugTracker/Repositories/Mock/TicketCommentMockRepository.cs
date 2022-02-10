using BugTracker.Models;
using Microsoft.AspNetCore.Identity;

namespace BugTracker.Repositories.Mock
{
    public class TicketCommentMockRepository : ITicketCommentRepository
    {
        public TicketCommentMockRepository(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        private static readonly List<TicketComment> ticketComments = new()
        {
            new TicketComment() 
            { 
                Id = "tc1",
                TicketId = "t1",
                AuthorId = "4687e432-58fc-448a-b639-6288ee716fa0",
                Value = "I will have this fixed promptly",
                CreatedAt = DateTimeOffset.UtcNow
            },
            new TicketComment()
            {
                Id = "tc1",
                TicketId = "t1",
                AuthorId = "ccd193a8-b38b-4414-a318-f4da79c046ae",
                Value = "This will be looked at shortly",
                CreatedAt = DateTimeOffset.UtcNow
            },
            new TicketComment()
            {
                Id = "tc1",
                TicketId = "t2",
                AuthorId = "cd448813-e865-49e8-933a-dff582b72509",
                Value = "This is a test comment",
                CreatedAt = DateTimeOffset.UtcNow
            },
        };
        private readonly UserManager<ApplicationUser> userManager;

        public IEnumerable<TicketComment> GetAllComments()
        {
            ticketComments.ForEach(c =>
            {
                c.Author = userManager.Users.First(u => u.Id == c.AuthorId);
            });
            return ticketComments;
        }

        public IEnumerable<TicketComment> GetCommentsByTicketId(string id)
        {
            List<TicketComment> comments = ticketComments.Where(c => c.TicketId == id).ToList();
            comments.ForEach(c =>
            {
                c.Author = userManager.Users.First(u => u.Id == c.AuthorId);
            });
            return comments;
        }

        public TicketComment GetComment(string id)
        {
            return ticketComments.Find(c => c.Id == id) ?? new TicketComment();
        }

        public TicketComment Create(TicketComment comment)
        {
            ticketComments.Add(comment);            
            return comment;
        }

        public TicketComment Update(TicketComment comment)
        {
            int index = ticketComments.FindIndex(t => t.Id == comment.Id);
            ticketComments[index] = comment;
            return comment;
        }

        public TicketComment Delete(string id)
        {
            TicketComment? comment = ticketComments.Find(c => c.Id == id);            
            ticketComments.Remove(comment);            
            return comment;
        }

        public IEnumerable<TicketComment> DeleteCommentsByTicketId(string ticketId)
        {
            var comments = ticketComments.Where(c => c.TicketId == ticketId);            
            ticketComments.RemoveAll(c => comments.Contains(c));
            return comments;
        }
    }
}
