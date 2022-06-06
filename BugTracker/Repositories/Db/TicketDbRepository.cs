using BugTracker.Contexts;
using BugTracker.Models;
using BugTracker.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace BugTracker.Repositories.Db
{
    public class TicketDbRepository : DbRepository<Ticket>, IRepository<Ticket>
    {
        private readonly BugTrackerDbContext _db;        

        public TicketDbRepository(BugTrackerDbContext db) : base(db)
        {
            _db = db;
        }

        public override IEnumerable<Ticket> GetAll()
        {            
             return _db.Tickets
                .Include(t => t.Project)
                .Include(t => t.Submitter)
                .Include(t => t.AssignedDeveloper)
                .OrderByDescending(t => t.CreatedAt);            
        }

        public override async Task<Ticket> Get(string id)
        {
            return await _db.Tickets
                .Include(t => t.Project)
                .Include(t => t.Submitter)
                .Include(t => t.AssignedDeveloper)
                .Include(t => t.TicketHistoryRecords)
                    .ThenInclude(t => t.Modifier)
                .Include(t => t.TicketAttachments)
                .Include(t => t.TicketComments)
                    .ThenInclude(c => c.Author)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public override IEnumerable<Ticket> Find(Expression<Func<Ticket, bool>> predicate)
        {
            return _db.Tickets
                .Where(predicate)
                .Include(t => t.Project)
                .Include(t => t.Submitter)
                .Include(t => t.AssignedDeveloper)
                .OrderByDescending(t => t.CreatedAt);
        }               
    }
}
