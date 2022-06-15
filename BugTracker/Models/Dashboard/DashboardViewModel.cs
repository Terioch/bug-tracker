using X.PagedList;

namespace BugTracker.Models
{
    public class DashboardViewModel
    {                     
        public int UserRoleProjectCount { get; set; }

        public int UserRoleTicketCount { get; set; }

        public int UserCountOnUserRoleProjects { get; set; }

        public int DeveloperCountOnUserRoleProjects { get; set; }

        public IPagedList<TicketHistoryRecord>? TicketHistoryRecords { get; set; }         
    }    
}
