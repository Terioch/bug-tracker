using System.ComponentModel.DataAnnotations;

namespace BugTracker.Models
{
    public class GeneralHistoryRecord
    {
        public string? Id { get; set; }

        [Required]
        public string? Description { get; set; }
    }
}
