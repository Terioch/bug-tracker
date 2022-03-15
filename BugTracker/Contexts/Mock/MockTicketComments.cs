using BugTracker.Models;

namespace BugTracker.Contexts.Mock
{
    public class MockTicketComments
    {
        public static List<TicketComment> GetComments()
        {
            return new List<TicketComment>() 
            {
                new TicketComment()
                {
                    Id = "tc1",
                    TicketId = "t1",
                    AuthorId = "ccd193a8-b38b-4414-a318-f4da79c046ae",
                    Value = "This will be looked at shortly",
                    CreatedAt = DateTimeOffset.UtcNow
                },
                new TicketComment()
                {
                    Id = "tc2",
                    TicketId = "t1",
                    AuthorId = "4687e432-58fc-448a-b639-6288ee716fa0",
                    Value = "I will have this fixed promptly",
                    CreatedAt = DateTimeOffset.UtcNow
                },
                new TicketComment()
                {
                    Id = "tc3",
                    TicketId = "t1",
                    AuthorId = "4687e432-58fc-448a-b639-6288ee716fa0",
                    Value = "Almost fixed",
                    CreatedAt = DateTimeOffset.UtcNow
                },
                new TicketComment()
                {
                    Id = "tc4",
                    TicketId = "t2",
                    AuthorId = "4687e432-58fc-448a-b639-6288ee716fa0",
                    Value = "This feature is almost complete",
                    CreatedAt = DateTimeOffset.UtcNow
                },
                new TicketComment()
                {
                    Id = "tc5",
                    TicketId = "t3",
                    AuthorId = "cd448813-e865-49e8-933a-dff582b72509",
                    Value = "This is a pressing issue",
                    CreatedAt = DateTimeOffset.UtcNow
                },
                new TicketComment()
                {
                    Id = "tc6",
                    TicketId = "t5",
                    AuthorId = "2ae32131-606d-495c-81cf-86f38875f9a7",
                    Value = "Not yet functioning correctly",
                    CreatedAt = DateTimeOffset.UtcNow
                },
                new TicketComment()
                {
                    Id = "tc6",
                    TicketId = "t5",
                    AuthorId = "2ae32131-606d-495c-81cf-86f38875f9a7",
                    Value = "Need another developer here promptly",
                    CreatedAt = DateTimeOffset.UtcNow
                },
                new TicketComment()
                {
                    Id = "tc7",
                    TicketId = "t6",
                    AuthorId = "ccd193a8-b38b-4414-a318-f4da79c046ae",
                    Value = "Currently preparing documentation requirements",
                    CreatedAt = DateTimeOffset.UtcNow
                },
                new TicketComment()
                {
                    Id = "tc8",
                    TicketId = "t8",
                    AuthorId = "4687e432-58fc-448a-b639-6288ee716fa0",
                    Value = "This will be finalized by Friday",
                    CreatedAt = DateTimeOffset.UtcNow
                },
                new TicketComment()
                {
                    Id = "tc9",
                    TicketId = "t10",
                    AuthorId = "fb37911c-7ceb-42ff-afc3-24b3bd189d9c",
                    Value = "How is this issue progressing?",
                    CreatedAt = DateTimeOffset.UtcNow
                },
            };
        }
    }
}
