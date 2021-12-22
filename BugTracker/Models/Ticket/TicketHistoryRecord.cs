using BugTracker.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTracker.Models
{
    public class TicketHistoryRecord
    {
        public string? Id { get; set; }        

        [Required]
        public string? TicketId { get; set; }        

        [Required]
        public string? Property { get; set; }

        public string? OldValue { get; set; }

        public string? NewValue { get; set; }

        [Required]
        public string? Modifier { get; set; }

        [Required]
        public DateTime? DateChanged { get; set; }        
    }
}
