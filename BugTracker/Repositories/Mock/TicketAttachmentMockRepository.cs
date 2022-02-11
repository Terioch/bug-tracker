using BugTracker.Models;
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

        private static readonly List<TicketAttachment> ticketAttachments = new()
        {
            new TicketAttachment()
            {
                Id = "ta1",
                TicketId = "t1",
                SubmitterId = "2ae32131-606d-495c-81cf-86f38875f9a7",
                Name = "Test attachment",
                FilePath = "academic.jpg",
                CreatedAt = DateTimeOffset.UtcNow,
            },
            new TicketAttachment()
            {
                Id = "ta2",
                TicketId = "t1",
                SubmitterId = "cd448813-e865-49e8-933a-dff582b72509",
                Name = "Code Desk",
                FilePath = "code-desk.jpg",
                CreatedAt = DateTimeOffset.UtcNow
            },
            new TicketAttachment()
            {
                Id = "ta3",
                TicketId = "t2",
                SubmitterId = "ccd193a8-b38b-4414-a318-f4da79c046ae",
                Name = "Library",
                FilePath = "bookcase-books-bookshelves-256541.jpg",
                CreatedAt = DateTimeOffset.UtcNow
            },
            new TicketAttachment()
            {
                Id = "ta4",
                TicketId = "t4",
                SubmitterId = "ccd193a8-b38b-4414-a318-f4da79c046ae",
                Name = "Node Network",
                FilePath = "node-network.jpg",
                CreatedAt = DateTimeOffset.UtcNow
            }
        };

        public IEnumerable<TicketAttachment> GetAllAttachments()
        {
            List<TicketAttachment> attachments = ticketAttachments;
            attachments.ForEach(a =>
            {
                a.Submitter = userManager.Users.First(u => u.Id == a.SubmitterId);
            });
            return attachments;
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
            return attachments;
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
