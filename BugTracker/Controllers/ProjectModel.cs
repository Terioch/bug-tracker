using BugTracker.Models;

namespace BugTracker.Controllers
{
    public class ProjectModel
    {
        public string? ProjectId { get; set; }

        public string? Name { get; set; }

        public List<Ticket>? Tickets { get; set; }
    }
}
