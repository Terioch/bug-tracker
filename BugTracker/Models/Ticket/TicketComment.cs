using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTracker.Models
{
    public class TicketComment
    {
        [Key]
        public string? Id { get; set; }

        [ForeignKey(nameof(Ticket))]
        public string? TicketId { get; set; }

        [ForeignKey(nameof(Author))]
        public string? AuthorId { get; set; }

        [StringLength(200)]
        [Required]
        public string? Value { get; set; }

        [Required]
        public DateTimeOffset? CreatedAt { get; set; }

        public virtual Ticket? Ticket { get; set; }

        public virtual ApplicationUser? Author { get; set; }
    }
}
