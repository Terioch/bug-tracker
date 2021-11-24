using BugTracker.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace BugTracker.Models
{
    public class UserProjects
    {        
        [Key]
        public string? UserId { get; set; }

        [Key]
        public string? ProjectId { get; set; }

        public virtual ApplicationUser? User { get; set; }

        public virtual Project? Project { get; set; }
    }
}
