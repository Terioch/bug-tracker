using BugTracker.Data;
using BugTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BugTracker.Services
{
    public class TicketAttachmentDbRepository : ITicketAttachmentRepository
    {
        private readonly BugTrackerDbContext context;

        public TicketAttachmentDbRepository(BugTrackerDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<TicketAttachment> GetAllAttachments()
        {
            return context.TicketAttachments.Include(a => a.Submitter);
        }

        public TicketAttachment GetAttachmentById(string id)
        {
            return context.TicketAttachments
                .Include(a => a.Submitter)
                .First(a => a.Id == id);
        }

        public IEnumerable<TicketAttachment> GetAttachmentsByTicketId(string ticketId)
        {
            return context.TicketAttachments.Include(a => a.Submitter).Where(a => a.TicketId == ticketId);
        }

        public TicketAttachment Create(TicketAttachment attachment)
        {
            context.TicketAttachments.Add(attachment);
            context.SaveChanges();
            return attachment;
        }

        public TicketAttachment Update(TicketAttachment attachment)
        {
            EntityEntry<TicketAttachment> attachedAttachment = context.TicketAttachments.Attach(attachment);
            attachedAttachment.State = EntityState.Modified;
            context.SaveChanges();
            return attachment;
        }

        public TicketAttachment Delete(string id)
        {
            TicketAttachment? attachment = context.TicketAttachments.Find(id);            
            context.TicketAttachments.Remove(attachment);
            context.SaveChanges();
            return attachment;
        }

        public IEnumerable<TicketAttachment> DeleteAttachmentsByTicketId(string ticketId) 
        {
            var attachments = context.TicketAttachments.Where(a => a.TicketId == ticketId);
            context.TicketAttachments.RemoveRange(attachments);
            context.SaveChanges();
            return attachments;
        }        
    }
}
