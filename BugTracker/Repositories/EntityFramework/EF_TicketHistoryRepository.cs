using BugTracker.Contexts;
using BugTracker.Models;
using BugTracker.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BugTracker.Repositories.EF
{
    public class EF_TicketHistoryRepository : EF_Repository<TicketHistoryRecord>, IRepository<TicketHistoryRecord>
    {
        private readonly BugTrackerDbContext _db;

        public EF_TicketHistoryRepository(BugTrackerDbContext db) : base(db)
        {
            _db = db;
        }

        public override IEnumerable<TicketHistoryRecord> GetAll()   
        {
            return _db.TicketHistoryRecords
                .Include(t => t.Ticket)
                .Include(t => t.Modifier)
                .OrderByDescending(t => t.ModifiedAt);
        }        

        public override async Task<TicketHistoryRecord> Get(string id)
        {
            return await _db.TicketHistoryRecords
                .Include(t => t.Ticket)
                .Include(t => t.Modifier)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public override IEnumerable<TicketHistoryRecord> Find(Expression<Func<TicketHistoryRecord, bool>> predicate)
        {
            return _db.TicketHistoryRecords
                .Where(predicate)
                .Include(t => t.Ticket)
                .Include(t => t.Modifier)
                .OrderByDescending(t => t.ModifiedAt);
        }                     
    }
}
