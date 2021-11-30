using BugTracker.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTracker.Models
{
    public class UserProjects
    {    
        [Key]
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        [Required]
        public string? ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public Project? Project { get; set; }
    }
}
