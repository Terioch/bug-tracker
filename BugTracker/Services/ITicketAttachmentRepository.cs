using BugTracker.Models;

namespace BugTracker.Services
{
    public interface ITicketAttachmentRepository
    {
        public IEnumerable<TicketAttachment> GetAllAttachments();
        public TicketAttachment GetAttachmentById(string id);
        public TicketAttachment Create(TicketAttachment attachment);
        public TicketAttachment Update(TicketAttachment attachment);
        public TicketAttachment Delete(string id);
    }
}
