using System.ComponentModel.DataAnnotations;

namespace BugTracker.Models
{
    public class TicketAttachment
    {
        public string? Id { get; set; }

        public string? TicketId { get; set; }

        public string? SubmitterId { get; set; }

        [Required]
        [StringLength(40)]
        public string? Name { get; set; }        

        [Required]
        public string? FilePath { get; set; }

        [Required]
        public DateTimeOffset? CreatedAt { get; set; }

        public virtual Ticket? Ticket { get; set; }

        public virtual ApplicationUser? Submitter { get; set; }
    }
}
