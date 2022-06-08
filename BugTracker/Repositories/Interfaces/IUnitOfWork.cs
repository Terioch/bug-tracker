using BugTracker.Models;
using Microsoft.AspNetCore.Identity;

namespace BugTracker.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Project> Projects { get; }

        IRepository<Ticket> Tickets { get;  }

        IRepository<TicketHistoryRecord> TicketHistoryRecords { get; }

        IRepository<TicketAttachment> TicketAttachments { get; }

        IRepository<TicketComment> TicketComments { get; }

        UserManager<ApplicationUser> UserManager { get; }

        RoleManager<IdentityRole> RoleManager { get; }

        Task<int> Complete();
    }
}
