using BugTracker.Contexts;
using BugTracker.Models;
using BugTracker.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace BugTracker.Repositories.Db
{
    public class EF_TicketAttachmentRepository : EF_Repository<TicketAttachment>, IRepository<TicketAttachment>
    {
        private readonly BugTrackerDbContext _db;

        public EF_TicketAttachmentRepository(BugTrackerDbContext db) : base(db)
        {
            _db = db;
        }

        public override IEnumerable<TicketAttachment> GetAll()
        {
            return _db.TicketAttachments
                .Include(a => a.Ticket)
                .Include(a => a.Submitter)
                .OrderByDescending(a => a.CreatedAt);
        }

        public override async Task<TicketAttachment> Get(string id)
        {
            return await _db.TicketAttachments
                .Include(a => a.Ticket)
                .Include(a => a.Submitter)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public override IEnumerable<TicketAttachment> Find(Expression<Func<TicketAttachment, bool>> predicate)
        {
            return _db.TicketAttachments
                .Where(predicate)
                .Include(a => a.Ticket)
                .Include(a => a.Submitter)                
                .OrderByDescending(a => a.CreatedAt);
        }       
    }
}
