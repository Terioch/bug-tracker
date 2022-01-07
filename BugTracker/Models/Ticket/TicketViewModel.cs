using BugTracker.Models;
using X.PagedList;

namespace BugTracker.Models
{
    public class TicketViewModel
    {
        public string? Id { get; set; }
        
        public string? ProjectId { get; set; }

        public string? SubmitterId { get; set; }

        public string? AssignedDeveloperId { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTime? SubmittedDate { get; set; }     

        public string? Type { get; set; }

        public string? Status { get; set; }

        public string? Priority { get; set; }

        public virtual Project? Project { get; set; }

        public virtual ApplicationUser? AssignedDeveloper { get; set; }

        public virtual ApplicationUser? Submitter { get; set; }       

        public IPagedList<TicketHistoryRecord>? TicketHistoryRecords { get; set; }

        public IPagedList<TicketAttachment>? TicketAttachments { get; set; }

        public IPagedList<TicketComment>? TicketComments { get; set; }
    }
}
