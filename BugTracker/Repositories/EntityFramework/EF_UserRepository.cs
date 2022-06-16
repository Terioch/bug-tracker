using BugTracker.Contexts;
using BugTracker.Models;
using BugTracker.Repositories.EF;
using BugTracker.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Repositories.EF
{
    public class EF_UserRepository : EF_Repository<ApplicationUser>, IUserRepository
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
                .Include(u => u.TicketHistoryRecords)
                .Include(u => u.TicketAttachments)
                .Include(u => u.TicketComments)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public void AddProject(Project project, ApplicationUser user)
        {
            user.Projects.Add(project);
        }

        public void RemoveProject(Project project, ApplicationUser user)
        {
            user.Projects.Remove(project);
        }
    }
}
