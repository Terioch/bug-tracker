using BugTracker.Models;

namespace BugTracker.Repositories.Interfaces
{
    public interface ITicketCommentRepository : IRepository<TicketComment>
    {      
        public void DeleteByTicketId(string ticketId);
    }
}
