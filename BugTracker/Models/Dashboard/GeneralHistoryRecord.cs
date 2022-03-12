using System.ComponentModel.DataAnnotations;

namespace BugTracker.Models
{
    public class GeneralHistoryRecord
    {
        public string? Id { get; set; }

        [Required]
        public string? TypeId { get; set; }

        public string? ModifierId { get; set; }

        public string? Action { get; set; }

        public DateTimeOffset? ModifiedAt { get; set; }

        public virtual ApplicationUser? Modifier { get; set; }
    }
}
