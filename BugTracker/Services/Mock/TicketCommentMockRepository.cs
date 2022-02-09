using BugTracker.Models;

namespace BugTracker.Services.Mock
{
    public class TicketCommentMockRepository : ITicketCommentRepository
    {
        private readonly BugTrackerMockContext context;

        public TicketCommentMockRepository(BugTrackerMockContext context)
        {
            this.context = context;
        }

        private static readonly List<TicketComment> ticketComments = new()
        {
            new TicketComment() 
            { 
                Id = "tc1",
                TicketId = "t1",
                AuthorId = "4687e432-58fc-448a-b639-6288ee716fa0",
                Value = "I will have this fixed promptly",
                CreatedAt = DateTimeOffset.UtcNow
            },
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
                Id = "tc1",
                TicketId = "t2",
                AuthorId = "cd448813-e865-49e8-933a-dff582b72509",
                Value = "This is a test comment",
                CreatedAt = DateTimeOffset.UtcNow
            },
        };

        public IEnumerable<TicketComment> GetAllComments()
        {
            IEnumerable<TicketComment>? comments = ticketComments;
            return comments ?? new List<TicketComment>();
        }

        public IEnumerable<TicketComment> GetCommentsByTicketId(string id)
        {
            return ticketComments.Where(c => c.TicketId == id) ?? new List<TicketComment>();
        }

        public TicketComment GetComment(string id)
        {
            return ticketComments.Find(c => c.Id == id) ?? new TicketComment();
        }

        public TicketComment Create(TicketComment comment)
        {
            ticketComments.Add(comment);            
            return comment;
        }

        public TicketComment Update(TicketComment comment)
        {
            int index = ticketComments.FindIndex(t => t.Id == comment.Id);
            ticketComments[index] = comment;
            return comment;
        }

        public TicketComment Delete(string id)
        {
            TicketComment? comment = ticketComments.Find(c => c.Id == id);            
            ticketComments.Remove(comment);            
            return comment;
        }

        public IEnumerable<TicketComment> DeleteCommentsByTicketId(string ticketId)
        {
            var comments = ticketComments.Where(c => c.TicketId == ticketId);            
            ticketComments.RemoveAll(c => comments.Contains(c));
            return comments;
        }
    }
}
