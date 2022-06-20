using BugTracker.Contexts;
using BugTracker.Models;
using BugTracker.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BugTracker.Repositories.EF
{
    public class EF_UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor? _httpContextAccessor;

        public EF_UnitOfWork(
            ApplicationDbContext db, 
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager, 
            IConfiguration config, 
            IHttpContextAccessor? httpContextAccessor)
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
            _config = config;
            _httpContextAccessor = httpContextAccessor;
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
            var loggedInUser = await Users.Get(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (loggedInUser.Email == _config["OwnerCredentials:Email"])
            {
                return await _db.SaveChangesAsync();
            }

            //return await Task.FromResult(1);
            return await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _db.Dispose();
        }       
    }
}
