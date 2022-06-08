using BugTracker.Models;
using BugTracker.Repositories.Interfaces;
using BugTracker.Contexts.Mock;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;

namespace BugTracker.Repositories.Mock
{
    public class Mock_TicketRepository : IRepository<Ticket>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Mock_TicketRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Ticket> GetAll()
        {               
            MockBugTrackerDbContext.Tickets.ForEach(async t =>
            {
                t.Project = await _unitOfWork.Projects.Get(t.ProjectId);
                t.Submitter = _unitOfWork.UserManager.Users.First(u => u.Id == t.SubmitterId);
                t.AssignedDeveloper = _unitOfWork.UserManager.Users.FirstOrDefault(u => u.Id == t.AssignedDeveloperId);
            });

            return MockBugTrackerDbContext.Tickets;
        }

        public async Task<Ticket> Get(string id)
        {
            var ticket = MockBugTrackerDbContext.Tickets.FirstOrDefault(t => t.Id == id);

            ticket.Project = await _unitOfWork.Projects.Get(ticket.ProjectId);
            ticket.Submitter = _unitOfWork.UserManager.Users.First(u => u.Id == ticket.SubmitterId);
            ticket.AssignedDeveloper = _unitOfWork.UserManager.Users.FirstOrDefault(u => u.Id == ticket.AssignedDeveloperId);
            ticket.TicketHistoryRecords = _unitOfWork.TicketHistoryRecords.Find(r => r.TicketId == ticket.Id).ToList();
            ticket.TicketAttachments = _unitOfWork.TicketAttachments.Find(a => a.TicketId == ticket.Id).ToList();
            ticket.TicketComments = _unitOfWork.TicketComments.Find(c => c.TicketId == ticket.Id).ToList();

            return ticket;
        }

        public IEnumerable<Ticket> Find(Expression<Func<Ticket, bool>> predicate)
        {
            var tickets = MockBugTrackerDbContext.Tickets.AsQueryable().Where(predicate).ToList();

            tickets.ForEach(async t =>
            {
                t.Project = await _unitOfWork.Projects.Get(t.ProjectId);
                t.Submitter = _unitOfWork.UserManager.Users.First(u => u.Id == t.SubmitterId);
                t.AssignedDeveloper = _unitOfWork.UserManager.Users.FirstOrDefault(u => u.Id == t.AssignedDeveloperId);
            });

            return tickets;
        }        

        public void Add(Ticket ticket)
        {
            MockBugTrackerDbContext.Tickets.Add(ticket);            
        }       

        public void Delete(Ticket ticket)
        {
            MockBugTrackerDbContext.Tickets.Remove(ticket);
        }

        public void DeleteRange(IEnumerable<Ticket> entities)
        {
            throw new NotImplementedException();
        }
    }
}
