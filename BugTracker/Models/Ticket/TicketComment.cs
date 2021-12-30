using System.ComponentModel.DataAnnotations;

namespace BugTracker.Models
{
    public class TicketComment
    {
        public string? Id { get; set; }

        public string? TicketId { get; set; }

        public string? AuthorId { get; set; }

        [Required]
        public string? Value { get; set; }

        [Required]
        public DateTimeOffset? CreatedAt { get; set; }

        public virtual Ticket? Ticket { get; set; }

        public virtual ApplicationUser? Author { get; set; }
    }
}
