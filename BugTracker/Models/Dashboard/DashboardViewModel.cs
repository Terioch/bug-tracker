using X.PagedList;

namespace BugTracker.Models
{
    public class DashboardViewModel
    {
        public ChartData? TicketTypeData { get; set; }

        public ChartData? TicketStatusData { get; set; }

        public ChartData? TicketPriorityData { get; set; }
       
        public IPagedList<TicketHistoryRecord>? TicketHistoryRecords { get; set; }

        public ICollection<Project> Projects { get; set; } = new HashSet<Project>();

        public ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();

        public ICollection<ApplicationUser> Users { get; set; } = new HashSet<ApplicationUser>();     
    }    
}
