using BugTracker.Models;

namespace BugTracker.Contexts.Mock
{
    public static class MockTickets
    {
        public static List<Ticket> GetTickets()
        {
            return new List<Ticket>()
            {
                new Ticket()
                {
                    Id = "t1",
                    ProjectId = "p1",
                    SubmitterId = "ccd193a8-b38b-4414-a318-f4da79c046ae",
                    AssignedDeveloperId = "4687e432-58fc-448a-b639-6288ee716fa0",
                    Title = "Ticket modification issue",
                    Description = "Null exception when modifying a tickets assigned developer to none",
                    CreatedAt = DateTimeOffset.UtcNow,
                    Type = "Bugs/Errors",
                    Status = "In Progress",
                    Priority = "High",
                },
                new Ticket()
                {
                    Id = "t2",
                    ProjectId = "p1",
                    SubmitterId = "2ae32131-606d-495c-81cf-86f38875f9a7",
                    AssignedDeveloperId = "4687e432-58fc-448a-b639-6288ee716fa0",
                    Title = "Ticket comment section",
                    Description = "Add a ticket comment section",
                    CreatedAt = DateTimeOffset.UtcNow,
                    Type = "Feature Requests",
                    Status = "Resolved",
                    Priority = "Medium",
                },
                new Ticket()
                {
                    Id = "t3",
                    ProjectId = "p1",
                    SubmitterId = "2ae32131-606d-495c-81cf-86f38875f9a7",
                    AssignedDeveloperId = null,
                    Title = "Assign project user bug",
                    Description = "Fix issue when assigning a user to a project",
                    CreatedAt = DateTimeOffset.UtcNow,
                    Type = "Bugs/Errors",
                    Status = "New",
                    Priority = "High",
                },
                new Ticket()
                {
                    Id = "t4",
                    ProjectId = "p1",
                    SubmitterId = "ccd193a8-b38b-4414-a318-f4da79c046ae",
                    AssignedDeveloperId = "cd448813-e865-49e8-933a-dff582b72509",
                    Title = "Additional developers required",
                    Description = "A need for more developers to support this project due to an overload of new feature requirements",
                    CreatedAt = DateTimeOffset.UtcNow,
                    Type = "Other Comments",
                    Status = "Under Review",
                    Priority = "Medium",
                },
                new Ticket()
                {
                    Id = "t5",
                    ProjectId = "p1",
                    SubmitterId = "2ae32131-606d-495c-81cf-86f38875f9a7",
                    AssignedDeveloperId = null,
                    Title = "Edit profile functionality",
                    Description = "Allow registered users to edit their account information",
                    CreatedAt = DateTimeOffset.UtcNow,
                    Type = "Feature Requests",
                    Status = "Open",
                    Priority = "Medium",
                },
                new Ticket()
                {
                    Id = "t6",
                    ProjectId = "p1",
                    SubmitterId = "2ae32131-606d-495c-81cf-86f38875f9a7",
                    AssignedDeveloperId = null,
                    Title = "Documentation for role management",
                    Description = "Documentation is required for administrators when assigning roles",
                    CreatedAt = DateTimeOffset.UtcNow,
                    Type = "Training/Document Requests",
                    Status = "New",
                    Priority = "Medium",
                },
                new Ticket()
                {
                    Id = "t7",
                    ProjectId = "p2",
                    SubmitterId = "2ae32131-606d-495c-81cf-86f38875f9a7",
                    AssignedDeveloperId = "4687e432-58fc-448a-b639-6288ee716fa0",
                    Title = "Authorization issues",
                    Description = "Role claims are not being added correctly",
                    CreatedAt = DateTimeOffset.UtcNow,
                    Type = "Bugs/Errors",
                    Status = "In Progress",
                    Priority = "High",
                },
                new Ticket()
                {
                    Id = "t8",
                    ProjectId = "p2",
                    SubmitterId = "cd448813-e865-49e8-933a-dff582b72509",
                    AssignedDeveloperId = "4687e432-58fc-448a-b639-6288ee716fa0",
                    Title = "Documentation for admin section",
                    Description = "More documentation/instruction is required for administrators",
                    CreatedAt = DateTimeOffset.UtcNow,
                    Type = "Training/Document Requests",
                    Status = "In Progress",
                    Priority = "Low",
                },
                new Ticket()
                {
                    Id = "t9",
                    ProjectId = "p2",
                    SubmitterId = "2ae32131-606d-495c-81cf-86f38875f9a7",
                    AssignedDeveloperId = "0de2c4ff-6923-4b30-8d8a-e7b90b5edcbf",
                    Title = "New theming",
                    Description = "Add a new journal-like theme that is clean and crisp",
                    CreatedAt = DateTimeOffset.UtcNow,
                    Type = "Feature Requests",
                    Status = "Under Review",
                    Priority = "Medium",
                },
                new Ticket()
                {
                    Id = "t10",
                    ProjectId = "p2",
                    SubmitterId = "2ae32131-606d-495c-81cf-86f38875f9a7",
                    AssignedDeveloperId = "4687e432-58fc-448a-b639-6288ee716fa0",
                    Title = "Dashboard chart display is broken",
                    Description = "Chart titles are not shown and overall display is unresponsive",
                    CreatedAt = DateTimeOffset.UtcNow,
                    Type = "Bugs/Errors",
                    Status = "In Progress",
                    Priority = "High",
                },
                new Ticket()
                {
                    Id = "t11",
                    ProjectId = "p3",
                    SubmitterId = "2ae32131-606d-495c-81cf-86f38875f9a7",
                    AssignedDeveloperId = "4687e432-58fc-448a-b639-6288ee716fa0",
                    Title = "Test Ticket",
                    Description = "This is a test ticket",
                    CreatedAt = DateTimeOffset.UtcNow,
                    Type = "Bugs/Errors",
                    Status = "Under Review",
                    Priority = "Medium",
                },
                new Ticket()
                {
                    Id = "t12",
                    ProjectId = "p3",
                    SubmitterId = "ccd193a8-b38b-4414-a318-f4da79c046ae",
                    AssignedDeveloperId = "0de2c4ff-6923-4b30-8d8a-e7b90b5edcbf",
                    Title = "Test Ticket 2",
                    Description = "This is a test ticket",
                    CreatedAt = DateTimeOffset.UtcNow,
                    Type = "Feature Requests",
                    Status = "Under Review",
                    Priority = "Low",
                },
                new Ticket()
                {
                    Id = "t13",
                    ProjectId = "p3",
                    SubmitterId = "2ae32131-606d-495c-81cf-86f38875f9a7",
                    AssignedDeveloperId = null,
                    Title = "Test Ticket 3",
                    Description = "This is a test ticket",
                    CreatedAt = DateTimeOffset.UtcNow,
                    Type = "Bugs/Errors",
                    Status = "New",
                    Priority = "High",
                },
                new Ticket()
                {
                    Id = "t14",
                    ProjectId = "p3",
                    SubmitterId = "2ae32131-606d-495c-81cf-86f38875f9a7",
                    AssignedDeveloperId = "4687e432-58fc-448a-b639-6288ee716fa0",
                    Title = "Test Ticket 4",
                    Description = "This is a test ticket",
                    CreatedAt = DateTimeOffset.UtcNow,
                    Type = "Training/Document Requests",
                    Status = "Open",
                    Priority = "Low",
                },
                new Ticket()
                {
                    Id = "t15",
                    ProjectId = "p4",
                    SubmitterId = "cd448813-e865-49e8-933a-dff582b72509",
                    AssignedDeveloperId = "0de2c4ff-6923-4b30-8d8a-e7b90b5edcbf",
                    Title = "Test Ticket 5",
                    Description = "This is a test ticket",
                    CreatedAt = DateTimeOffset.UtcNow,
                    Type = "Bugs/Errors",
                    Status = "In Progress",
                    Priority = "High",
                },
                new Ticket()
                {
                    Id = "t16",
                    ProjectId = "p4",
                    SubmitterId = "ccd193a8-b38b-4414-a318-f4da79c046ae",
                    AssignedDeveloperId = "4687e432-58fc-448a-b639-6288ee716fa0",
                    Title = "Test Ticket 6",
                    Description = "This is a test ticket",
                    CreatedAt = DateTimeOffset.UtcNow,
                    Type = "Bugs/Errors",
                    Status = "Resolved",
                    Priority = "Medium",
                }
            };
        }
    }
}
