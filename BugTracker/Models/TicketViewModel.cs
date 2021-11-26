using System.ComponentModel.DataAnnotations;

namespace BugTracker.Models
{
    public class TicketViewModel
    {
        [Required]
        [MaxLength(40)]
        public string? Title { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public DateTime? SubmittedDate { get; set; }

        [Required]
        public string? Submitter { get; set; }

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
