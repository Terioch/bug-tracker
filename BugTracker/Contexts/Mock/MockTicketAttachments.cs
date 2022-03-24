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
                    SubmitterId = "ccd193a8-b38b-4414-a318-f4da79c046ae",
                    Name = "Test attachment",
                    FilePath = "attachment2.jpg",
                    CreatedAt = DateTimeOffset.UtcNow,
                },
                new TicketAttachment()
                {
                    Id = "ta2",
                    TicketId = "t1",
                    SubmitterId = "4687e432-58fc-448a-b639-6288ee716fa0",
                    Name = "Attachment 2",
                    FilePath = "attachment3.jpg",
                    CreatedAt = DateTimeOffset.UtcNow
                },
                new TicketAttachment()
                {
                    Id = "ta3",
                    TicketId = "t2",
                    SubmitterId = "2ae32131-606d-495c-81cf-86f38875f9a7",
                    Name = "Attachment 3",
                    FilePath = "attachment1.jpg",
                    CreatedAt = DateTimeOffset.UtcNow
                },
                new TicketAttachment()
                {
                    Id = "ta4",
                    TicketId = "t4",
                    SubmitterId = "ccd193a8-b38b-4414-a318-f4da79c046ae",
                    Name = "Attachment 4",
                    FilePath = "attachment4.jpg",
                    CreatedAt = DateTimeOffset.UtcNow
                },
                new TicketAttachment()
                {
                    Id = "ta5",
                    TicketId = "t4",
                    SubmitterId = "cd448813-e865-49e8-933a-dff582b72509",
                    Name = "Attachment 5",
                    FilePath = "attachment5.jpg",
                    CreatedAt = DateTimeOffset.UtcNow
                },
                new TicketAttachment()
                {
                    Id = "ta6",
                    TicketId = "t6",
                    SubmitterId = "2ae32131-606d-495c-81cf-86f38875f9a7",
                    Name = "Attachment 6",
                    FilePath = "attachment6.jpg",
                    CreatedAt = DateTimeOffset.UtcNow
                },
                new TicketAttachment()
                {
                    Id = "ta7",
                    TicketId = "t6",
                    SubmitterId = "2ae32131-606d-495c-81cf-86f38875f9a7",
                    Name = "Attachment 7",
                    FilePath = "attachment7.jpg",
                    CreatedAt = DateTimeOffset.UtcNow
                },
                new TicketAttachment()
                {
                    Id = "ta8",
                    TicketId = "t6",
                    SubmitterId = "cd448813-e865-49e8-933a-dff582b72509",
                    Name = "Attachment 8",
                    FilePath = "attachment8.jpg",
                    CreatedAt = DateTimeOffset.UtcNow
                },
                new TicketAttachment()
                {
                    Id = "ta9",
                    TicketId = "t7",
                    SubmitterId = "4687e432-58fc-448a-b639-6288ee716fa0",
                    Name = "Attachment 9",
                    FilePath = "attachment9.jpg",
                    CreatedAt = DateTimeOffset.UtcNow
                },
                new TicketAttachment()
                {
                    Id = "ta10",
                    TicketId = "t9",
                    SubmitterId = "0de2c4ff-6923-4b30-8d8a-e7b90b5edcbf",
                    Name = "Attachment 10",
                    FilePath = "attachment10.jpg",
                    CreatedAt = DateTimeOffset.UtcNow
                },
                new TicketAttachment()
                {
                    Id = "ta11",
                    TicketId = "t10",
                    SubmitterId = "4687e432-58fc-448a-b639-6288ee716fa0",
                    Name = "Attachment 11",
                    FilePath = "attachment11.jpg",
                    CreatedAt = DateTimeOffset.UtcNow
                }
            };
        }
    }
}
