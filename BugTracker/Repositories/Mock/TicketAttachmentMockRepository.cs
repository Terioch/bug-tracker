using BugTracker.Contexts.Mock;
using BugTracker.Models;
using BugTracker.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;
using System.Text;

namespace BugTracker.Repositories.Mock
{
    public class TicketAttachmentMockRepository : IRepository<TicketAttachment>
    {
        private readonly UserManager<ApplicationUser> userManager;

        public TicketAttachmentMockRepository(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public static List<TicketAttachment> TicketAttachments { get; set; } = MockTicketAttachments.GetAttachments();

        public IEnumerable<TicketAttachment> GetAll()
        {
            List<TicketAttachment> attachments = TicketAttachments;

            attachments.ForEach(a =>
            {
                a.Submitter = userManager.Users.First(u => u.Id == a.SubmitterId);
            });

            return attachments.OrderByDescending(a => a.CreatedAt);
        }

        public Task<TicketAttachment> Get(string id)
        {
            var attachment = TicketAttachments.Find(a => a.Id == id);          
            attachment.Submitter = userManager.Users.First(u => u.Id == attachment.SubmitterId);
            return Task.FromResult(attachment);
        }

        public IEnumerable<TicketAttachment> Find(Expression<Func<TicketAttachment, bool>> predicate)
        {
            var attachments = TicketAttachments.AsQueryable().Where(predicate);

            TicketAttachments.ForEach(a =>
            {
                a.Submitter = userManager.Users.First(u => u.Id == a.SubmitterId);
            });

            return attachments.OrderByDescending(a => a.CreatedAt);
        }

        public void Add(TicketAttachment attachment)
        {
            TicketAttachments.Add(attachment);                       
        }       

        public void Delete(TicketAttachment attachment)
        {     
            TicketAttachments.Remove(attachment);           
        }

        public void DeleteRange(IEnumerable<TicketAttachment> attachments)
        {
            int index = TicketAttachments.IndexOf(attachments.First());
            TicketAttachments.RemoveRange(index, attachments.Count());
        }
    }
}
