using BugTracker.Models;
using BugTracker.Repositories.Interfaces;
using BugTracker.Contexts.Mock;
using Microsoft.AspNetCore.Identity;

namespace BugTracker.Repositories.Mock
{
    public class TicketMockRepository : ITicketRepository
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

        public static readonly List<Ticket> tickets = MockTickets.GetTickets();

        public IEnumerable<Ticket> GetAllTickets()
        {               
            tickets.ForEach(t =>
            {
                t.Project = projectRepo.GetProjectById(t.ProjectId);
                t.Submitter = userManager.Users.First(u => u.Id == t.SubmitterId);
                t.AssignedDeveloper = userManager.Users.FirstOrDefault(u => u.Id == t.AssignedDeveloperId);
            });
            return tickets;
        }

        public Ticket GetTicketById(string id)
        {
            Ticket ticket = tickets.First(t => t.Id == id);
            ticket.Project = projectRepo.GetProjectById(ticket.ProjectId);
            ticket.Submitter = userManager.Users.First(u => u.Id == ticket.SubmitterId);
            ticket.AssignedDeveloper = userManager.Users.FirstOrDefault(u => u.Id == ticket.AssignedDeveloperId);
            ticket.TicketHistoryRecords = ticketHistoryRepo.GetRecordsByTicketId(id).ToList();
            ticket.TicketAttachments = ticketAttachmentRepo.GetAttachmentsByTicketId(id).ToList();
            ticket.TicketComments = ticketCommentRepo.GetCommentsByTicketId(id).ToList();
            return ticket;
        }

        public IEnumerable<Ticket> GetTicketsByProjectId(string projectId)
        {
            List<Ticket> tickets = TicketMockRepository.tickets.Where(t => t.ProjectId == projectId).ToList();
            tickets.ForEach(t =>
            {
                t.Project = projectRepo.GetProjectById(t.ProjectId);
                t.Submitter = userManager.Users.First(u => u.Id == t.SubmitterId);
                t.AssignedDeveloper = userManager.Users.FirstOrDefault(u => u.Id == t.AssignedDeveloperId);
            });
            return tickets;
        }        

        public Ticket Create(Ticket ticket)
        {
            tickets.Add(ticket);
            return ticket;
        }

        public Ticket Update(Ticket ticket)
        {
            int index = tickets.FindIndex(t => t.Id == ticket.Id);
            tickets[index] = ticket;
            return ticket;
        }

        public Ticket Delete(string id)
        {
            Ticket ticket = tickets.First(t => t.Id == id);
            tickets.Remove(ticket);
            return ticket;
        }
    }
}
