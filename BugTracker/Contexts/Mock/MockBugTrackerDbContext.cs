using BugTracker.Models;

namespace BugTracker.Contexts.Mock
{
    public class MockBugTrackerDbContext
    {       
        public static List<Project> Projects { get; set; } = MockProjects.GetProjects();

        public static List<Ticket> Tickets { get; set; } = MockTickets.GetTickets();

        public static List<UserProject> UserProjects { get; set; } = MockUserProjects.GetUserProjects();

        public static List<TicketHistoryRecord> TicketHistoryRecords { get; set; } = MockTicketHistoryRecords.GetRecords();

        public static List<TicketAttachment> TicketAttachments { get; set; } = MockTicketAttachments.GetAttachments();

        public static List<TicketComment> TicketComments { get; set; } = MockTicketComments.GetComments();
    }
}
