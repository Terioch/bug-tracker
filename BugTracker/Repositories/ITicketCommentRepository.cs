using BugTracker.Models;

namespace BugTracker.Repositories
{
    public interface ITicketCommentRepository
    {
        public IEnumerable<TicketComment> GetAllComments();
        public IEnumerable<TicketComment> GetCommentsByTicketId(string id);
        public TicketComment GetComment(string id);
        public TicketComment Create(TicketComment comment);
        public TicketComment Update(TicketComment comment);
        public TicketComment Delete(string id);
        public IEnumerable<TicketComment> DeleteCommentsByTicketId(string ticketId);
    }
}
