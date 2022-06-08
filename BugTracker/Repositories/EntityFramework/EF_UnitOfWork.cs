using BugTracker.Contexts;
using BugTracker.Models;
using BugTracker.Repositories.EF;
using BugTracker.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BugTracker.Repositories.EntityFramework
{
    public class EF_UnitOfWork : IUnitOfWork
    {
        private readonly BugTrackerDbContext _db;

        public EF_UnitOfWork(BugTrackerDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            Projects = new EF_ProjectRepository(db);
            Tickets = new EF_TicketRepository(db);
            TicketHistoryRecords = new EF_TicketHistoryRepository(db);
            TicketAttachments = new EF_TicketAttachmentRepository(db);
            TicketComments = new EF_TicketCommentRepository(db);
            Users = new EF_UserRepository(db);
            UserManager = userManager;
            RoleManager = roleManager;
        }

        public IProjectRepository Projects { get; private set; }

        public IRepository<Ticket> Tickets { get; private set; }

        public IRepository<TicketHistoryRecord> TicketHistoryRecords { get; private set; }

        public IRepository<TicketAttachment> TicketAttachments { get; private set; }

        public IRepository<TicketComment> TicketComments { get; private set; }

        public IUserRepository Users { get; private set; }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        public RoleManager<IdentityRole> RoleManager { get; private set; }

        public async Task<int> Complete()
        {
            return await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _db.Dispose();
        }       
    }
}
