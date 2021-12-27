using System.ComponentModel.DataAnnotations;
using X.PagedList;

namespace BugTracker.Models
{
    public class ProjectViewModel
    {        
        public string? Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }

        public string? ToBeAssignedUserId { get; set; }

        public IEnumerable<ApplicationUser>? UnassignedUsers { get; set; } = new List<ApplicationUser>();

        public IPagedList<ApplicationUser>? Users { get; set; }

        public IPagedList<Ticket>? Tickets { get; set; }
    }
}
