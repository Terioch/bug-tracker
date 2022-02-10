using BugTracker.Models;

namespace BugTracker.Repositories
{
    public interface ITicketAttachmentRepository
    {
        public IEnumerable<TicketAttachment> GetAllAttachments();
        public TicketAttachment GetAttachmentById(string id);
        public IEnumerable<TicketAttachment> GetAttachmentsByTicketId(string ticketId);
        public TicketAttachment Create(TicketAttachment attachment);
        public TicketAttachment Update(TicketAttachment attachment);
        public TicketAttachment Delete(string id);
        public IEnumerable<TicketAttachment> DeleteAttachmentsByTicketId(string ticketId);
    }
}
