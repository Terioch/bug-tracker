using BugTracker.Models;
using BugTracker.Contexts.Mock;
using BugTracker.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;

namespace BugTracker.Repositories.Mock
{
    public class TicketCommentMockRepository : IRepository<TicketComment>
    {
        private readonly UserManager<ApplicationUser> userManager;

        public TicketCommentMockRepository(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public static List<TicketComment> TicketComments { get; set; } = MockTicketComments.GetComments();        

        public IEnumerable<TicketComment> GetAll()
        {
            TicketComments.ForEach(c =>
            {
                c.Author = userManager.Users.First(u => u.Id == c.AuthorId);
            });

            return TicketComments;
        }

        public Task<TicketComment> Get(string id)
        {
            return Task.FromResult(TicketComments.Find(c => c.Id == id));
        }

        public IEnumerable<TicketComment> Find(Expression<Func<TicketComment, bool>> predicate)
        {
            var comments = TicketComments.AsQueryable().Where(predicate).ToList();

            comments.ForEach(c =>
            {
                c.Author = userManager.Users.First(u => u.Id == c.AuthorId);
            });

            return comments;
        }      

        public void Add(TicketComment comment)
        {
            TicketComments.Add(comment);            
        }      

        public void Delete(TicketComment comment)
        {           
            TicketComments.Remove(comment);                        
        }

        public void DeleteRange(IEnumerable<TicketComment> comments)
        {
            int index = TicketComments.IndexOf(comments.First());
            TicketComments.RemoveRange(index, comments.Count());
        }
    }
}
