namespace BugTracker.Models
{
    public class DashboardViewModel
    {
        public ChartData? TicketTypeData { get; set; }

        public ChartData? TicketStatusData { get; set; }

        public ChartData? TicketPriorityData { get; set; }

        // Add property for recent activity

    }    
}
