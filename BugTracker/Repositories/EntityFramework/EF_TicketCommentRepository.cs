using BugTracker.Contexts;
using BugTracker.Models;
using BugTracker.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace BugTracker.Repositories.EF
{
    public class EF_TicketCommentRepository : EF_Repository<TicketComment>, IRepository<TicketComment>
    {
        private readonly BugTrackerDbContext _db;

        public EF_TicketCommentRepository(BugTrackerDbContext db) : base(db)
        {
            _db = db;
        }

        public override IEnumerable<TicketComment> GetAll()
        {          
            return _db.TicketComments
                .Include(c => c.Ticket)
                .Include(c => c.Author)
                .OrderByDescending(c => c.CreatedAt);
        }

        public override async Task<TicketComment> Get(string id)
        {
            return await _db.TicketComments
                .Include(c => c.Ticket)
                .Include(c => c.Author)
                .FirstOrDefaultAsync(c => c.Id == id) ?? new TicketComment();
        }

        public override IEnumerable<TicketComment> Find(Expression<Func<TicketComment, bool>> predicate)
        {
            return _db.TicketComments
                .Where(predicate)
                .Include(c => c.Ticket)
                .Include(c => c.Author)
                .OrderByDescending(c => c.CreatedAt);
        }                           
    }
}
