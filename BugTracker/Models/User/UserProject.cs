using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTracker.Models
{
    public class UserProject
    {
        public string? Id { get; set; }
      
        [Required]
        public string? UserId { get; set; }

        [Required]
        public string? ProjectId { get; set; }

        public virtual Project? Project { get; set; }

        public virtual ApplicationUser? User { get; set; }
    }
}
