namespace BugTracker.Models { 
    public class UserViewModel
    {
        public string? Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? UserName { get; set; }

        public string? Email { get; set; }

        public IEnumerable<Project> Projects { get; set; } = new List<Project>();

        public IEnumerable<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
