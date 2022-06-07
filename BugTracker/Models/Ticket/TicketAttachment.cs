using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTracker.Models
{
    public class TicketAttachment
    {
        [Key]
        public string? Id { get; set; }

        [ForeignKey(nameof(Ticket))]
        public string? TicketId { get; set; }

        [ForeignKey(nameof(Submitter))]
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
