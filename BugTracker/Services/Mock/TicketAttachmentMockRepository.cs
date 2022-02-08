using BugTracker.Models;
using Microsoft.AspNetCore.Identity;

namespace BugTracker.Services.Mock
{
    public class TicketAttachmentMockRepository
    {
        private readonly BugTrackerMockContext context;
        private readonly ITicketRepository ticketRepo;
        private readonly UserManager<ApplicationUser> userManager;

        public TicketAttachmentMockRepository(BugTrackerMockContext context, ITicketRepository ticketRepo, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.ticketRepo = ticketRepo;
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
                CreatedAt = DateTimeOffset.UtcNow
            },
            new TicketAttachment()
            {
                Id = "ta2",
                TicketId = "t1",
                SubmitterId = "cd448813-e865-49e8-933a-dff582b72509",
                Name = "Test attachment 2",
                FilePath = "code-desk.jpg",
                CreatedAt = DateTimeOffset.UtcNow
            },
            new TicketAttachment()
            {
                Id = "ta3",
                TicketId = "t2",
                SubmitterId = "ccd193a8-b38b-4414-a318-f4da79c046ae",
                Name = "Test attachment 3",
                FilePath = "calendar.jpg",
                CreatedAt = DateTimeOffset.UtcNow
            },            
        };

        public IEnumerable<TicketAttachment> GetAllAttachments()
        {
            List<TicketAttachment> attachments = new();
            ticketAttachments.ForEach(async a =>
            {
                a.Ticket = ticketRepo.GetTicketById(a.TicketId);
                a.Submitter = await userManager.FindByIdAsync(a.SubmitterId);
            });
            return attachments;
        }

        public async Task<TicketAttachment> GetAttachmentById(string id)
        {
            var attachment = ticketAttachments.Find(a => a.Id == id);
            attachment.Ticket = ticketRepo.GetTicketById(attachment.TicketId);
            attachment.Submitter = await userManager.FindByIdAsync(attachment.SubmitterId);
            return attachment;
        }

        public IEnumerable<TicketAttachment> GetAttachmentsByTicketId(string ticketId)
        {
            var attachments = ticketAttachments.Where(a => a.TicketId == ticketId);
            ticketAttachments.ForEach(async a =>
            {
                a.Ticket = ticketRepo.GetTicketById(a.TicketId);
                a.Submitter = await userManager.FindByIdAsync(a.SubmitterId);
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
            throw new NotImplementedException();
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
