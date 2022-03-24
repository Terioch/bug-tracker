using BugTracker.Models;
using BugTracker.Contexts.Mock;
using BugTracker.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BugTracker.Repositories.Mock
{
    public class TicketCommentMockRepository : ITicketCommentRepository
    {
        private readonly UserManager<ApplicationUser> userManager;

        public TicketCommentMockRepository(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public static List<TicketComment> TicketComments { get; set; } = MockTicketComments.GetComments();        

        public IEnumerable<TicketComment> GetAllComments()
        {
            TicketComments.ForEach(c =>
            {
                c.Author = userManager.Users.First(u => u.Id == c.AuthorId);
            });
            return TicketComments;
        }

        public IEnumerable<TicketComment> GetCommentsByTicketId(string id)
        {
            List<TicketComment> comments = TicketComments.Where(c => c.TicketId == id).ToList();
            comments.ForEach(c =>
            {
                c.Author = userManager.Users.First(u => u.Id == c.AuthorId);
            });
            return comments;
        }

        public TicketComment GetComment(string id)
        {
            return TicketComments.Find(c => c.Id == id) ?? new TicketComment();
        }

        public TicketComment Create(TicketComment comment)
        {
            TicketComments.Add(comment);            
            return comment;
        }

        public TicketComment Update(TicketComment comment)
        {
            int index = TicketComments.FindIndex(t => t.Id == comment.Id);
            TicketComments[index] = comment;
            return comment;
        }

        public TicketComment Delete(string id)
        {
            TicketComment? comment = TicketComments.Find(c => c.Id == id);            
            TicketComments.Remove(comment);            
            return comment;
        }

        public IEnumerable<TicketComment> DeleteCommentsByTicketId(string ticketId)
        {
            var comments = TicketComments.Where(c => c.TicketId == ticketId);            
            TicketComments.RemoveAll(c => comments.Contains(c));
            return comments;
        }
    }
}
