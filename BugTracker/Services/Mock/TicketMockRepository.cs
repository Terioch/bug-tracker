using BugTracker.Models;

namespace BugTracker.Services.Mock
{
    public class TicketMockRepository : ITicketRepository
    {
        private readonly BugTrackerMockContext context;

        public TicketMockRepository(BugTrackerMockContext context)
        {
            this.context = context;
        }


        private static List<Ticket> tickets = new()
        {
            new Ticket()
            {
                Id = "t1",
                ProjectId = "p1",
                SubmitterId = "u1",
                AssignedDeveloperId = "u1",
                Title = "Test Ticket",
                Description = "Testing ticket creation",
                CreatedAt = DateTimeOffset.UtcNow,
                Type = "Bugs/Errors",
                Priority = "Medium",
            },
            new Ticket()
            {
                Id = "t2",
                ProjectId = "p1",
                SubmitterId = "u3",
                AssignedDeveloperId = "u2",
                Title = "Test Ticket 2",
                Description = "Testing login functionality",
                CreatedAt = DateTimeOffset.UtcNow,
                Type = "Feature Requests",
                Priority = "Low",
            },
            new Ticket()
            {
                Id = "t3",
                ProjectId = "p2",
                SubmitterId = "u5",
                AssignedDeveloperId = "u1",
                Title = "Test Ticket 3",
                Description = "This is a test ticket",
                CreatedAt = DateTimeOffset.UtcNow,
                Type = "Other Comments",
                Priority = "High",
            }
        };

        public IEnumerable<Ticket> GetAllTickets()
        {
            return tickets;
        }

        public Ticket GetTicketById(string id)
        {
            return tickets.First(t => t.Id == id);
        }

        public List<Ticket> GetTicketsByProjectId(string projectId)
        {
            return tickets.Where(t => t.ProjectId == projectId).ToList();
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
