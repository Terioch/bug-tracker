using BugTracker.Contexts;
using BugTracker.Models;
using BugTracker.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BugTracker.Repositories.Db
{
    public class TicketCommentDbRepository : ITicketCommentRepository
    {
        private readonly BugTrackerDbContext context;

        public TicketCommentDbRepository(BugTrackerDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<TicketComment> GetAllComments()
        {          
            return context.TicketComments
                .Include(c => c.Ticket)
                .Include(c => c.Author)
                .OrderByDescending(c => c.CreatedAt);
        }

        public IEnumerable<TicketComment> GetCommentsByTicketId(string id)
        {
            return context.TicketComments
                .Where(c => c.TicketId == id)
                .Include(c => c.Ticket)
                .Include(c => c.Author)
                .OrderByDescending(c => c.CreatedAt);
        }

        public TicketComment GetComment(string id)
        {          
            return context.TicketComments
                .Include(c => c.Ticket)
                .Include(c => c.Author)
                .First(c => c.Id == id) ?? new TicketComment();
        }

        public TicketComment Create(TicketComment comment)
        {
            context.TicketComments.Add(comment);
            context.SaveChanges();
            return comment;
        }

        public TicketComment Update(TicketComment comment)
        {
            EntityEntry<TicketComment> attachedComment = context.TicketComments.Attach(comment);
            attachedComment.State = EntityState.Modified;
            context.SaveChanges();
            return comment;
        }

        public TicketComment Delete(string id)
        {
            TicketComment? comment = context.TicketComments.Find(id);            
            context.TicketComments.Remove(comment);
            context.SaveChanges();
            return comment;
        }

        public IEnumerable<TicketComment> DeleteCommentsByTicketId(string ticketId)
        {
            var comments = context.TicketComments.Where(c => c.TicketId == ticketId);
            context.TicketComments.RemoveRange(comments);
            context.SaveChanges();
            return comments;
        }        
    }
}
