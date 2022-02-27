using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace BugTracker.Models
{
    public class Ticket
    {
        public string? Id { get; set; }

        [Required]
        public string? ProjectId { get; set; }

        [Required]
        public string? SubmitterId { get; set; }

        public string? AssignedDeveloperId { get; set; }

        [Required]
        [StringLength(40)]
        public string? Title { get; set; }

        [Required]  
        [StringLength(200)]
        public string? Description { get; set; }

        [Required]
        public DateTimeOffset? CreatedAt { get; set; }       

        [Required]
        public string? Type { get; set; }

        [Required]
        public string? Status { get; set; }

        [Required]
        public string? Priority { get; set; }

        public virtual Project? Project { get; set; }

        public virtual ApplicationUser? AssignedDeveloper { get; set; }

        public virtual ApplicationUser? Submitter { get; set; }

        public virtual ICollection<TicketHistoryRecord>? TicketHistoryRecords { get; set; } = new HashSet<TicketHistoryRecord>();

        public virtual ICollection<TicketAttachment>? TicketAttachments { get; set; } = new HashSet<TicketAttachment>();

        public virtual ICollection<TicketComment> TicketComments { get; set; } = new HashSet<TicketComment>();       
    }
}
