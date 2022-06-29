using BugTracker.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTracker.Models
{
    public class TicketHistoryRecord
    {
        [Key]
        public string? Id { get; set; }        
       
        [ForeignKey(nameof(Ticket))]
        public string? TicketId { get; set; }

        [ForeignKey(nameof(Modifier))]
        public string? ModifierId { get; set; }

        [Required]
        public string? Property { get; set; }
        
        public string? OldValue { get; set; }
  
        public string? NewValue { get; set; }        

        [Required]
        public DateTimeOffset? ModifiedAt { get; set; }

        public virtual Ticket? Ticket { get; set; }
     
        public virtual ApplicationUser? Modifier { get; set; }
    }
}
