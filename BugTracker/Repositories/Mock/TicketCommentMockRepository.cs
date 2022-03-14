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

        private static readonly List<TicketComment> ticketComments = MockTicketComments.GetComments();        

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
