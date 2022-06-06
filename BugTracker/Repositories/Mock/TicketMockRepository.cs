using BugTracker.Models;
using BugTracker.Repositories.Interfaces;
using BugTracker.Contexts.Mock;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;

namespace BugTracker.Repositories.Mock
{
    public class TicketMockRepository : IRepository<Ticket>
    {
        private readonly IProjectRepository projectRepo;  
        private readonly ITicketCommentRepository ticketCommentRepo;
        private readonly ITicketAttachmentRepository ticketAttachmentRepo;
        private readonly ITicketHistoryRepository ticketHistoryRepo;
        private readonly UserManager<ApplicationUser> userManager;

        public TicketMockRepository(IProjectRepository projectRepo, ITicketHistoryRepository ticketHistoryRepo, ITicketAttachmentRepository ticketAttachmentRepo,
            ITicketCommentRepository ticketCommentRepo, UserManager<ApplicationUser> userManager)
        {        
            this.projectRepo = projectRepo;           
            this.ticketCommentRepo = ticketCommentRepo;
            this.ticketAttachmentRepo = ticketAttachmentRepo;
            this.ticketHistoryRepo = ticketHistoryRepo;
            this.userManager = userManager;            
        }

        public static List<Ticket> Tickets { get; set; } = MockTickets.GetTickets();

        public IEnumerable<Ticket> GetAll()
        {               
            Tickets.ForEach(t =>
            {
                t.Project = projectRepo.GetProjectById(t.ProjectId);
                t.Submitter = userManager.Users.First(u => u.Id == t.SubmitterId);
                t.AssignedDeveloper = userManager.Users.FirstOrDefault(u => u.Id == t.AssignedDeveloperId);
            });
            return Tickets;
        }

        public Task<Ticket> Get(string id)
        {
            Ticket ticket = Tickets.First(t => t.Id == id);
            ticket.Project = projectRepo.GetProjectById(ticket.ProjectId);
            ticket.Submitter = userManager.Users.First(u => u.Id == ticket.SubmitterId);
            ticket.AssignedDeveloper = userManager.Users.FirstOrDefault(u => u.Id == ticket.AssignedDeveloperId);
            ticket.TicketHistoryRecords = ticketHistoryRepo.GetRecordsByTicketId(id).ToList();
            ticket.TicketAttachments = ticketAttachmentRepo.GetByTicketId(id).ToList();
            ticket.TicketComments = ticketCommentRepo.GetCommentsByTicketId(id).ToList();
            return Task.FromResult(ticket);
        }

        public IEnumerable<Ticket> Find(Expression<Func<Ticket, bool>> predicate)
        {
            var tickets = Tickets.AsQueryable().Where(predicate).ToList();

            tickets.ForEach(t =>
            {
                t.Project = projectRepo.GetProjectById(t.ProjectId);
                t.Submitter = userManager.Users.First(u => u.Id == t.SubmitterId);
                t.AssignedDeveloper = userManager.Users.FirstOrDefault(u => u.Id == t.AssignedDeveloperId);
            });

            return tickets;
        }        

        public void Add(Ticket ticket)
        {
            Tickets.Add(ticket);            
        }       

        public void Delete(Ticket ticket)
        {
            Tickets.Remove(ticket);
        }
    }
}
