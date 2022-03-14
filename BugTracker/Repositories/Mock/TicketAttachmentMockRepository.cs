using BugTracker.Contexts.Mock;
using BugTracker.Models;
using BugTracker.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace BugTracker.Repositories.Mock
{
    public class TicketAttachmentMockRepository : ITicketAttachmentRepository
    {
        private readonly UserManager<ApplicationUser> userManager;

        public TicketAttachmentMockRepository(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        private static readonly List<TicketAttachment> ticketAttachments = MockTicketAttachments.GetAttachments();

        public IEnumerable<TicketAttachment> GetAllAttachments()
        {
            List<TicketAttachment> attachments = ticketAttachments;
            attachments.ForEach(a =>
            {
                a.Submitter = userManager.Users.First(u => u.Id == a.SubmitterId);
            });
            return attachments.OrderByDescending(a => a.CreatedAt);
        }

        public TicketAttachment GetAttachmentById(string id)
        {
            var attachment = ticketAttachments.Find(a => a.Id == id);          
            attachment.Submitter = userManager.Users.First(u => u.Id == attachment.SubmitterId);
            return attachment;
        }

        public IEnumerable<TicketAttachment> GetAttachmentsByTicketId(string ticketId)
        {
            var attachments = ticketAttachments.Where(a => a.TicketId == ticketId);
            ticketAttachments.ForEach(a =>
            {
                a.Submitter = userManager.Users.First(u => u.Id == a.SubmitterId);
            });
            return attachments.OrderByDescending(a => a.CreatedAt);
        }

        public TicketAttachment Create(TicketAttachment attachment)
        {
            ticketAttachments.Add(attachment);           
            return attachment;
        }

        public TicketAttachment Update(TicketAttachment attachment)
        {
            int index = ticketAttachments.FindIndex(a => a.Id == attachment.Id);
            ticketAttachments[index] = attachment;
            return attachment;
        }

        public TicketAttachment Delete(string id)
        {
            TicketAttachment? attachment = ticketAttachments.Find(a => a.Id == id);
            ticketAttachments.Remove(attachment);           
            return attachment;
        }

        public IEnumerable<TicketAttachment> DeleteAttachmentsByTicketId(string ticketId)
        {
            var attachments = ticketAttachments.Where(a => a.TicketId == ticketId);
            ticketAttachments.RemoveAll(a => attachments.Contains(a));           
            return attachments;
        }        
    }
}
