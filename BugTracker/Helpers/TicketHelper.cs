using BugTracker.Models;
using BugTracker.Services;
using Microsoft.AspNetCore.Identity;

namespace BugTracker.Helpers
{
    public class TicketHelper
    {
        private readonly IProjectRepository projectRepository;
        private readonly IUserProjectRepository userProjectRepository;
        private readonly ITicketRepository ticketRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleHelper roleHelper;

        public TicketHelper(IProjectRepository projectRepository, IUserProjectRepository userProjectRepository, ITicketRepository ticketRepository, UserManager<ApplicationUser> userManager, RoleHelper roleHelper)
        {
            this.projectRepository = projectRepository;
            this.userProjectRepository = userProjectRepository;
            this.ticketRepository = ticketRepository;
            this.userManager = userManager;
            this.roleHelper = roleHelper;
        }

        public async Task<IEnumerable<Ticket>> FilterTicketsByRole(IEnumerable<Ticket> tickets, ApplicationUser user)
        {
            List<string> roles = await roleHelper.GetRoleNamesOfUser(user.UserName);
            List<Project> projects = userProjectRepository.GetUserProjects(user.Id);

            foreach (var project in projects)
            {
                project.Tickets = ticketRepository.GetProjectTickets(project.Id);
            }

            if (roles.Contains("Admin") || roles.Contains("Demo Admin"))
            {
                return tickets;
            }
            else if (roles.Contains("Project Manager") || roles.Contains("Demo Project Manager"))
            {
                return projects.SelectMany(p => p.Tickets);
            }
            else if (roles.Contains("Developer") || roles.Contains("Demo Developer"))
            {
                return projects.SelectMany(p => p.Tickets.Where(t => t.AssignedDeveloper == user.UserName));
            }
            return projects.SelectMany(p => p.Tickets.Where(t => t.Submitter == user.UserName));            
        }
    }
}
