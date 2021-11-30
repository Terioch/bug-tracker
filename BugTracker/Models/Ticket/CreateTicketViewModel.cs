using BugTracker.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace BugTracker.Models
{
    public class CreateTicketViewModel
    {               
        [Required]
        [MaxLength(40)]
        public string? Title { get; set; }        

        [Required]
        public string? Description { get; set; }

        [Required]
        public string? ProjectName { get; set; }

        [Required]
        public string? AssignedDeveloper { get; set; }

        [Required]
        public string? Type { get; set; }

        [Required]
        public string? Status { get; set; }

        [Required]
        public string? Priority { get; set; }
    }
}
