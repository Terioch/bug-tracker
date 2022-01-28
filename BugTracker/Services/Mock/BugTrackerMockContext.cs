using BugTracker.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Services.Mock
{
    public class BugTrackerMockContext
    {
        public static List<Project>? Projects { get; set; } = new()
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

        public static List<Ticket>? Tickets { get; set; } = new()
        {
            new Ticket()
            {
                Id = "t1",
                ProjectId = "p1",
                SubmitterId = "u1",
                AssignedDeveloperId = "u1",
                Title = "Test Ticket",
                Description = "Testing ticket creation",
                CreatedAt = DateTimeOffset.UtcNow,
                Type = "Bugs/Errors",
                Priority = "Medium",
            },
            new Ticket()
            {
                Id = "t2",
                ProjectId = "p1",
                SubmitterId = "u3",
                AssignedDeveloperId = "u2",
                Title = "Test Ticket 2",
                Description = "Testing login functionality",
                CreatedAt = DateTimeOffset.UtcNow,
                Type = "Feature Requests",
                Priority = "Low",
            },
            new Ticket()
            {
                Id = "t3",
                ProjectId = "p2",
                SubmitterId = "u5",
                AssignedDeveloperId = "u1",
                Title = "Test Ticket 3",
                Description = "This is a test ticket",
                CreatedAt = DateTimeOffset.UtcNow,
                Type = "Other Comments",
                Priority = "High",
            }
        };

        public static List<UserProject>? UserProjects { get; set; } = new();

        public static List<TicketHistoryRecord>? TicketHistoryRecords { get; set; } = new();

        public static List<TicketAttachment>? TicketAttachments { get; set; } = new();

        public static List<TicketComment>? TicketComments { get; set; } = new();
    }
}
