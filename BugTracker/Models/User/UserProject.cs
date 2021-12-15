using BugTracker.Areas.Identity.Data;
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
    }
}
