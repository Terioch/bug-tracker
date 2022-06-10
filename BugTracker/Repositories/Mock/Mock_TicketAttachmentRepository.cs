using BugTracker.Contexts.Mock;
using BugTracker.Models;
using BugTracker.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;
using System.Text;

namespace BugTracker.Repositories.Mock
{
    public class Mock_TicketAttachmentRepository : IRepository<TicketAttachment>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Mock_TicketAttachmentRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }       

        public IEnumerable<TicketAttachment> GetAll()
        {
            List<TicketAttachment> attachments = MockBugTrackerDbContext.TicketAttachments;

            attachments.ForEach(a =>
            {
                a.Submitter = _unitOfWork.UserManager.Users.First(u => u.Id == a.SubmitterId);
            });

            return attachments.OrderByDescending(a => a.CreatedAt);
        }

        public Task<TicketAttachment> GetAsync(string id)
        {
            var attachment = MockBugTrackerDbContext.TicketAttachments.Find(a => a.Id == id);          
            attachment.Submitter = _unitOfWork.UserManager.Users.First(u => u.Id == attachment.SubmitterId);
            return Task.FromResult(attachment);
        }

        public IEnumerable<TicketAttachment> Find(Expression<Func<TicketAttachment, bool>> predicate)
        {
            var attachments = MockBugTrackerDbContext.TicketAttachments.AsQueryable().Where(predicate);

            MockBugTrackerDbContext.TicketAttachments.ForEach(a =>
            {
                a.Submitter = _unitOfWork.UserManager.Users.First(u => u.Id == a.SubmitterId);
            });

            return attachments.OrderByDescending(a => a.CreatedAt);
        }

        public void Add(TicketAttachment attachment)
        {
            MockBugTrackerDbContext.TicketAttachments.Add(attachment);                       
        }       

        public void Delete(TicketAttachment attachment)
        {
            MockBugTrackerDbContext.TicketAttachments.Remove(attachment);           
        }

        public void DeleteRange(IEnumerable<TicketAttachment> attachments)
        {
            int index = MockBugTrackerDbContext.TicketAttachments.IndexOf(attachments.First());
            MockBugTrackerDbContext.TicketAttachments.RemoveRange(index, attachments.Count());
        }
    }
}
