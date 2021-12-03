using BugTracker.Areas.Identity.Data;
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

        public IEnumerable<ApplicationUser>? Users { get; set; }

        public IPagedList<Ticket>? Tickets { get; set; }
    }
}
