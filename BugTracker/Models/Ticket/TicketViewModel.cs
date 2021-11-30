using BugTracker.Models;

namespace BugTracker.Models
{
    public class TicketViewModel
    {
        public string? Id { get; set; }
        
        public string? ProjectName { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTime? SubmittedDate { get; set; }

        public string? Submitter { get; set; }

        public string? AssignedDeveloper { get; set; }

        public string? Type { get; set; }

        public string? Status { get; set; }

        public string? Priority { get; set; }

        public List<TicketHistoryRecord> HistoryRecords { get; set; } = new();        
    }
}
