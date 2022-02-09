using BugTracker.Models;
using Microsoft.AspNetCore.Identity;

namespace BugTracker.Services.Mock
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

        public static readonly List<Ticket> tickets = new()
        {
            new Ticket()
            {
                Id = "t1",
                ProjectId = "p1",
                SubmitterId = "ccd193a8-b38b-4414-a318-f4da79c046ae",
                AssignedDeveloperId = "4687e432-58fc-448a-b639-6288ee716fa0",
                Title = "Test Ticket",
                Description = "Testing ticket creation",
                CreatedAt = DateTimeOffset.UtcNow,
                Type = "Bugs/Errors",
                Status = "In Progress",
                Priority = "Medium",
            },
            new Ticket()
            {
                Id = "t2",
                ProjectId = "p1",
                SubmitterId = "2ae32131-606d-495c-81cf-86f38875f9a7",
                AssignedDeveloperId = "cd448813-e865-49e8-933a-dff582b72509",
                Title = "Test Ticket 2",
                Description = "Testing login functionality",
                CreatedAt = DateTimeOffset.UtcNow,
                Type = "Feature Requests",
                Status = "In Progress",
                Priority = "Low",
            },
            new Ticket()
            {
                Id = "t3",
                ProjectId = "p2",
                SubmitterId = "2ae32131-606d-495c-81cf-86f38875f9a7",
                AssignedDeveloperId = null,
                Title = "Test Ticket 3",
                Description = "This is a test ticket",
                CreatedAt = DateTimeOffset.UtcNow,
                Type = "Other Comments",
                Status = "New",
                Priority = "High",
            }
        };

        public IEnumerable<Ticket> GetAllTickets()
        {            
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

        public List<Ticket> GetTicketsByProjectId(string projectId)
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
