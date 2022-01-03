using System.ComponentModel.DataAnnotations;

namespace BugTracker.Models
{
    public class TicketComment
    {
        public string? Id { get; set; }

        public string? TicketId { get; set; }

        public string? AuthorId { get; set; }

        [StringLength(200, MinimumLength = 1)]
        [Required]
        public string? Value { get; set; }

        [Required]
        public DateTimeOffset? CreatedAt { get; set; }

        public virtual Ticket? Ticket { get; set; }

        public virtual ApplicationUser? Author { get; set; }
    }
}
