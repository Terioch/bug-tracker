using BugTracker.Contexts;
using BugTracker.Models;
using BugTracker.Repositories.EF;
using BugTracker.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Repositories.EF
{
    public class EF_UserRepository : EF_Repository<ApplicationUser>, IRepository<ApplicationUser>
    {
        private readonly ApplicationDbContext _db;

        public EF_UserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }        

        public override async Task<ApplicationUser> Get(string id)
        {           
            return await _db.Users
                .Include(u => u.Projects)
                    .ThenInclude(p => p.Tickets)
                    .ThenInclude(t => t.Submitter)                    
                .Include(u => u.Projects)
                    .ThenInclude(p => p.Tickets)
                    .ThenInclude(t => t.AssignedDeveloper)
                .Include(u => u.Projects)
                    .ThenInclude(p => p.Tickets)
                    .ThenInclude(t => t.TicketHistoryRecords)
                .FirstOrDefaultAsync(u => u.Id == id);
        }   
    }
}
