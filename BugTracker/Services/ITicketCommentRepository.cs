using BugTracker.Models;

namespace BugTracker.Services
{
    public interface ITicketCommentRepository
    {
        public IEnumerable<TicketComment> GetAllComments();
        public TicketComment GetComment(string id);
        public TicketComment Create(TicketComment comment);
        public TicketComment Update(TicketComment comment);
        public TicketComment Delete(string id);
    }
}
