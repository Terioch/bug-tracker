using BugTracker.Models;

namespace BugTracker.Services.Mock
{
    public class TicketMockRepository
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
                Id = "1",
                ProjectId = "p1",
                SubmitterId = "u1",
                AssignedDeveloperId = "u1",
                Title = "Test Ticket",
                Description = "Testing ticket creation",
                CreatedAt = DateTimeOffset.UtcNow,
                Type = "Bugs/Errors",
                Priority = "Medium",
            }
        };
    }
}
