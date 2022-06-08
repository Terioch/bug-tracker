using BugTracker.Models;
using BugTracker.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BugTracker.Repositories.Mock
{
    public class Mock_UnitOfWork : IUnitOfWork
    {       
        public Mock_UnitOfWork(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {        
            Projects = new Mock_ProjectRepository(unitOfWork);
            Tickets = new Mock_TicketRepository(unitOfWork);
            TicketHistoryRecords = new Mock_TicketHistoryRepository(unitOfWork);
            TicketAttachments = new Mock_TicketAttachmentRepository(unitOfWork);
            TicketComments = new Mock_TicketCommentRepository(unitOfWork);
            UserManager = userManager;
            RoleManager = roleManager;
        }

        public IRepository<Project> Projects { get; private set; }

        public IRepository<Ticket> Tickets { get; private set; }

        public IRepository<TicketHistoryRecord> TicketHistoryRecords { get; private set; }

        public IRepository<TicketAttachment> TicketAttachments { get; private set; }

        public IRepository<TicketComment> TicketComments { get; private set; }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        public RoleManager<IdentityRole> RoleManager { get; private set; }

        public Task<int> Complete()
        {
            Console.WriteLine("Simulating database operations...");
            return Task.FromResult(0);
        }

        public void Dispose()
        {            
            Console.WriteLine("Simulating the disposing of unmanaged resources...");
        }
    }
}
