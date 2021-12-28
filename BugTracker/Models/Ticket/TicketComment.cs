using System.ComponentModel.DataAnnotations;

namespace BugTracker.Models
{
    public class TicketComment
    {
        public string? Id { get; set; }

        [Required]
        public string? TicketId { get; set; }

        [Required]
        public string? AuthorId { get; set; }

        [Required]
        public string? Value { get; set; }

        [Required]
        public DateTime? CreatedAt { get; set; }

        public virtual Ticket? Ticket { get; set; }

        public virtual ApplicationUser? Author { get; set; }
    }
}
