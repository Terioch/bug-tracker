using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTracker.Models
{
    public class UserProject
    {
        public string? Id { get; set; }
      
        [Required]
        [ForeignKey(nameof(User))]
        public string? UserId { get; set; }

        [Required]
        [ForeignKey(nameof(Project))]
        public string? ProjectId { get; set; }     

        public virtual ApplicationUser? User { get; set; }

        public virtual Project? Project { get; set; }
    }
}
