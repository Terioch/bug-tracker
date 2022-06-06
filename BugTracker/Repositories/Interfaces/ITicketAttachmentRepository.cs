using BugTracker.Models;

namespace BugTracker.Repositories.Interfaces
{
    public interface ITicketAttachmentRepository : IRepository<TicketAttachment>
    {
        public void DeleteByTicketId(string ticketId);
    }
}
