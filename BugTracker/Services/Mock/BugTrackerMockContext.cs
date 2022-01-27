using BugTracker.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Services.Mock
{
    public class BugTrackerMockContext
    {
        public List<Project>? Projects { get; set; } = new()
        {
            new Project
            {
                Id = "p1",
                Name = "Bug Tracker",
                Description = "A Bug/Issue Tracker MVC project."
            },
            new Project
            {
                Id = "p2",
                Name = "Technology Blog",
                Description = "A programming tutorial blog built using .Net Web API and React.JS."
            },
            new Project
            {
                Id = "p3",
                Name = "Demo Project 1",
                Description = "This is a demo project."
            },
            new Project
            {
                Id = "p4",
                Name = "Demo Project 2",
                Description = "This is a demo project."
            }
        };

        public List<Ticket>? Tickets { get; set; }

        public List<UserProject>? UserProjects { get; set; }

        public List<TicketHistoryRecord>? TicketHistoryRecords { get; set; }

        public List<TicketAttachment>? TicketAttachments { get; set; }

        public List<TicketComment>? TicketComments { get; set; }        
    }
}
