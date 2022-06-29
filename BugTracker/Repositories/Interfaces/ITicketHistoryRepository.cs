using BugTracker.Models;

namespace BugTracker.Repositories.Interfaces
{
    public interface ITicketHistoryRepository : IRepository<TicketHistoryRecord>
    {           
        public void DeleteByTicketId(string ticketId);
    }
}
