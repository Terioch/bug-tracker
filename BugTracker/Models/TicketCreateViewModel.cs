using BugTracker.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace BugTracker.Models
{
    public class TicketCreateViewModel
    {
        public string? Id { get; set; }        

        [Required]
        [MaxLength(40)]
        public string? Title { get; set; }

        [Required]
        public string? ProjectName { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public DateTime? SubmittedDate { get; set; }      

        [Required]
        public ApplicationUser? AssignedDeveloperName { get; set; }

        [Required]
        public string? Type { get; set; }

        [Required]
        public string? Status { get; set; }

        [Required]
        public string? Priority { get; set; }
    }
}
