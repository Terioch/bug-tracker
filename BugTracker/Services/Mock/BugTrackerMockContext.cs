using BugTracker.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Services.Mock
{
    public class BugTrackerMockContext
    {
        public DbSet<Project>? Projects { get; set; }

        public DbSet<Ticket>? Tickets { get; set; }

        public DbSet<UserProject>? UserProjects { get; set; }

        public DbSet<TicketHistoryRecord>? TicketHistoryRecords { get; set; }

        public DbSet<TicketAttachment>? TicketAttachments { get; set; }

        public DbSet<TicketComment>? TicketComments { get; set; }
    }
}
