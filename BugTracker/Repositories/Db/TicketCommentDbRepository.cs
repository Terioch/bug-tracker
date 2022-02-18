using BugTracker.Data;
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
            IEnumerable<TicketComment>? comments = context.TicketComments;
            return comments ?? new List<TicketComment>();
        }

        public IEnumerable<TicketComment> GetCommentsByTicketId(string id)
        {
            IEnumerable<TicketComment>? comments = context.TicketComments
                .Where(c => c.TicketId == id)
                .Include(c => c.Author);
            return comments ?? new List<TicketComment>();
        }

        public TicketComment GetComment(string id)
        {          
            return context.TicketComments.Find(id) ?? new TicketComment();
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
