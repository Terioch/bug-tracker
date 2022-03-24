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
                    AuthorId = "cad3865d-5fb5-4e88-9c98-775fbc4610ca",
                    Value = "This will be looked at shortly",
                    CreatedAt = DateTimeOffset.UtcNow
                },
                new TicketComment()
                {
                    Id = "tc2",
                    TicketId = "t1",
                    AuthorId = "338986f3-f9b5-4f0a-8f55-4fdb7ebec83e",
                    Value = "I will have this fixed promptly",
                    CreatedAt = DateTimeOffset.UtcNow
                },
                new TicketComment()
                {
                    Id = "tc3",
                    TicketId = "t1",
                    AuthorId = "338986f3-f9b5-4f0a-8f55-4fdb7ebec83e",
                    Value = "Almost fixed",
                    CreatedAt = DateTimeOffset.UtcNow
                },
                new TicketComment()
                {
                    Id = "tc4",
                    TicketId = "t2",
                    AuthorId = "338986f3-f9b5-4f0a-8f55-4fdb7ebec83e",
                    Value = "This feature is almost complete",
                    CreatedAt = DateTimeOffset.UtcNow
                },
                new TicketComment()
                {
                    Id = "tc5",
                    TicketId = "t3",
                    AuthorId = "04f5c01b-602a-484f-a89d-6fa57db31687",
                    Value = "This is a pressing issue",
                    CreatedAt = DateTimeOffset.UtcNow
                },
                new TicketComment()
                {
                    Id = "tc6",
                    TicketId = "t5",
                    AuthorId = "421553e8-65cc-4416-8596-3d1277c99338",
                    Value = "Not yet functioning correctly",
                    CreatedAt = DateTimeOffset.UtcNow
                },
                new TicketComment()
                {
                    Id = "tc6",
                    TicketId = "t5",
                    AuthorId = "421553e8-65cc-4416-8596-3d1277c99338",
                    Value = "Need another developer here promptly",
                    CreatedAt = DateTimeOffset.UtcNow
                },
                new TicketComment()
                {
                    Id = "tc7",
                    TicketId = "t6",
                    AuthorId = "cad3865d-5fb5-4e88-9c98-775fbc4610ca",
                    Value = "Currently preparing documentation requirements",
                    CreatedAt = DateTimeOffset.UtcNow
                },
                new TicketComment()
                {
                    Id = "tc8",
                    TicketId = "t8",
                    AuthorId = "338986f3-f9b5-4f0a-8f55-4fdb7ebec83e",
                    Value = "This will be finalized by Friday",
                    CreatedAt = DateTimeOffset.UtcNow
                },
                new TicketComment()
                {
                    Id = "tc9",
                    TicketId = "t10",
                    AuthorId = "a111cc04-b2e6-4a2e-9a4f-b74059b1a953",
                    Value = "How is this issue progressing?",
                    CreatedAt = DateTimeOffset.UtcNow
                },
            };
        }
    }
}
