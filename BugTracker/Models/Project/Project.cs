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
        [Key]
        public string? Id { get; set; }

        [Required]        
        public string? Name { get; set; }

        [Required]
        [StringLength(200)]
        public string? Description { get; set; }       

        public virtual ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
        
        public virtual ICollection<ApplicationUser> Users { get; set; } = new HashSet<ApplicationUser>();
    }
}
