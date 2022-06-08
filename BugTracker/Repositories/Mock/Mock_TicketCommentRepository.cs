using BugTracker.Models;
using BugTracker.Contexts.Mock;
using BugTracker.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;

namespace BugTracker.Repositories.Mock
{
    public class Mock_TicketCommentRepository : IRepository<TicketComment>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Mock_TicketCommentRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }      

        public IEnumerable<TicketComment> GetAll()
        {
            MockBugTrackerDbContext.TicketComments.ForEach(c =>
            {
                c.Author = _unitOfWork.UserManager.Users.First(u => u.Id == c.AuthorId);
            });

            return MockBugTrackerDbContext.TicketComments;
        }

        public Task<TicketComment> Get(string id)
        {
            return Task.FromResult(MockBugTrackerDbContext.TicketComments.Find(c => c.Id == id));
        }

        public IEnumerable<TicketComment> Find(Expression<Func<TicketComment, bool>> predicate)
        {
            var comments = MockBugTrackerDbContext.TicketComments.AsQueryable().Where(predicate).ToList();

            comments.ForEach(c =>
            {
                c.Author = _unitOfWork.UserManager.Users.First(u => u.Id == c.AuthorId);
            });

            return comments;
        }      

        public void Add(TicketComment comment)
        {
            MockBugTrackerDbContext.TicketComments.Add(comment);            
        }      

        public void Delete(TicketComment comment)
        {           
            MockBugTrackerDbContext.TicketComments.Remove(comment);                        
        }

        public void DeleteRange(IEnumerable<TicketComment> comments)
        {
            int index = MockBugTrackerDbContext.TicketComments.IndexOf(comments.First());
            MockBugTrackerDbContext.TicketComments.RemoveRange(index, comments.Count());
        }
    }
}
