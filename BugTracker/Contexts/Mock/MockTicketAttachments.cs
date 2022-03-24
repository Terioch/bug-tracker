using BugTracker.Models;

namespace BugTracker.Contexts.Mock
{
    public class MockTicketAttachments
    {
        public static List<TicketAttachment> GetAttachments()
        {
            return new List<TicketAttachment>()
            {
                new TicketAttachment()
                {
                    Id = "ta1",
                    TicketId = "t1",
                    SubmitterId = "cad3865d-5fb5-4e88-9c98-775fbc4610ca",
                    Name = "Test attachment",
                    FilePath = "attachment2.jpg",
                    CreatedAt = DateTimeOffset.UtcNow,
                },
                new TicketAttachment()
                {
                    Id = "ta2",
                    TicketId = "t1",
                    SubmitterId = "338986f3-f9b5-4f0a-8f55-4fdb7ebec83e",
                    Name = "Attachment 2",
                    FilePath = "attachment3.jpg",
                    CreatedAt = DateTimeOffset.UtcNow
                },
                new TicketAttachment()
                {
                    Id = "ta3",
                    TicketId = "t2",
                    SubmitterId = "421553e8-65cc-4416-8596-3d1277c99338",
                    Name = "Attachment 3",
                    FilePath = "attachment1.jpg",
                    CreatedAt = DateTimeOffset.UtcNow
                },
                new TicketAttachment()
                {
                    Id = "ta4",
                    TicketId = "t4",
                    SubmitterId = "cad3865d-5fb5-4e88-9c98-775fbc4610ca",
                    Name = "Attachment 4",
                    FilePath = "attachment4.jpg",
                    CreatedAt = DateTimeOffset.UtcNow
                },
                new TicketAttachment()
                {
                    Id = "ta5",
                    TicketId = "t4",
                    SubmitterId = "04f5c01b-602a-484f-a89d-6fa57db31687",
                    Name = "Attachment 5",
                    FilePath = "attachment5.jpg",
                    CreatedAt = DateTimeOffset.UtcNow
                },
                new TicketAttachment()
                {
                    Id = "ta6",
                    TicketId = "t6",
                    SubmitterId = "421553e8-65cc-4416-8596-3d1277c99338",
                    Name = "Attachment 6",
                    FilePath = "attachment6.jpg",
                    CreatedAt = DateTimeOffset.UtcNow
                },
                new TicketAttachment()
                {
                    Id = "ta7",
                    TicketId = "t6",
                    SubmitterId = "421553e8-65cc-4416-8596-3d1277c99338",
                    Name = "Attachment 7",
                    FilePath = "attachment7.jpg",
                    CreatedAt = DateTimeOffset.UtcNow
                },
                new TicketAttachment()
                {
                    Id = "ta8",
                    TicketId = "t6",
                    SubmitterId = "04f5c01b-602a-484f-a89d-6fa57db31687",
                    Name = "Attachment 8",
                    FilePath = "attachment8.jpg",
                    CreatedAt = DateTimeOffset.UtcNow
                },
                new TicketAttachment()
                {
                    Id = "ta9",
                    TicketId = "t7",
                    SubmitterId = "338986f3-f9b5-4f0a-8f55-4fdb7ebec83e",
                    Name = "Attachment 9",
                    FilePath = "attachment9.jpg",
                    CreatedAt = DateTimeOffset.UtcNow
                },
                new TicketAttachment()
                {
                    Id = "ta10",
                    TicketId = "t9",
                    SubmitterId = "c92554a2-2201-47d0-97b6-8eb8c52fac75",
                    Name = "Attachment 10",
                    FilePath = "attachment10.jpg",
                    CreatedAt = DateTimeOffset.UtcNow
                },
                new TicketAttachment()
                {
                    Id = "ta11",
                    TicketId = "t10",
                    SubmitterId = "338986f3-f9b5-4f0a-8f55-4fdb7ebec83e",
                    Name = "Attachment 11",
                    FilePath = "attachment11.jpg",
                    CreatedAt = DateTimeOffset.UtcNow
                }
            };
        }
    }
}
