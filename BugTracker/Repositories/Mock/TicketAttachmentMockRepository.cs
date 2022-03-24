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

        public static List<TicketAttachment> TicketAttachments { get; set; } = MockTicketAttachments.GetAttachments();

        public IEnumerable<TicketAttachment> GetAllAttachments()
        {
            List<TicketAttachment> attachments = TicketAttachments;
            attachments.ForEach(a =>
            {
                a.Submitter = userManager.Users.First(u => u.Id == a.SubmitterId);
            });
            return attachments.OrderByDescending(a => a.CreatedAt);
        }

        public TicketAttachment GetAttachmentById(string id)
        {
            var attachment = TicketAttachments.Find(a => a.Id == id);          
            attachment.Submitter = userManager.Users.First(u => u.Id == attachment.SubmitterId);
            return attachment;
        }

        public IEnumerable<TicketAttachment> GetAttachmentsByTicketId(string ticketId)
        {
            var attachments = TicketAttachments.Where(a => a.TicketId == ticketId);
            TicketAttachments.ForEach(a =>
            {
                a.Submitter = userManager.Users.First(u => u.Id == a.SubmitterId);
            });
            return attachments.OrderByDescending(a => a.CreatedAt);
        }

        public TicketAttachment Create(TicketAttachment attachment)
        {
            TicketAttachments.Add(attachment);           
            return attachment;
        }

        public TicketAttachment Update(TicketAttachment attachment)
        {
            int index = TicketAttachments.FindIndex(a => a.Id == attachment.Id);
            TicketAttachments[index] = attachment;
            return attachment;
        }

        public TicketAttachment Delete(string id)
        {
            TicketAttachment? attachment = TicketAttachments.Find(a => a.Id == id);
            TicketAttachments.Remove(attachment);           
            return attachment;
        }

        public IEnumerable<TicketAttachment> DeleteAttachmentsByTicketId(string ticketId)
        {
            var attachments = TicketAttachments.Where(a => a.TicketId == ticketId);
            TicketAttachments.RemoveAll(a => attachments.Contains(a));           
            return attachments;
        }        
    }
}
