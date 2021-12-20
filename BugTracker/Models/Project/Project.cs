using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace BugTracker.Models
{
    public class Project
    {
        public string? Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }

        public virtual IEnumerable<ApplicationUser>? Users { get; set; } = new HashSet<ApplicationUser>();

        public virtual IEnumerable<Ticket>? Tickets { get; set; } = new HashSet<Ticket>();
    }
}
