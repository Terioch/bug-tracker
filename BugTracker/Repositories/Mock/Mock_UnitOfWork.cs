using BugTracker.Models;
using BugTracker.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BugTracker.Repositories.Mock
{
    public class Mock_UnitOfWork : IUnitOfWork
    {        
        private readonly IUnitOfWork _unitOfWork;

        public Mock_UnitOfWork(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            Projects = new Mock_ProjectRepository(unitOfWork);
        }     

        public IRepository<Project> Projects { get; private set; }

        public IRepository<Ticket> Tickets { get; private set; }

        public IRepository<TicketHistoryRecord> TicketHistoryRecords { get; private set; }

        public IRepository<TicketAttachment> TicketAttachments { get; private set; }

        public IRepository<TicketComment> TicketComments { get; private set; }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        public RoleManager<IdentityRole> RoleManager { get; private set; }

        public int Complete()
        {
            Console.WriteLine("Simulating database operations...");
            return 0;
        }
    }
}
