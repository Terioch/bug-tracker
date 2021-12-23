using X.PagedList;

namespace BugTracker.Models { 
    public class UserViewModel
    {
        public string? Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? UserName { get; set; }

        public string? Email { get; set; }

        public IPagedList<Project>? Projects { get; set; }

        public IPagedList<Ticket>? Tickets { get; set; }
    }
}
