using BugTracker.Models;
using BugTracker.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BugTracker.Helpers
{    
    public class TicketHelper
    {
        private readonly IProjectRepository projectRepository;
        private readonly IUserProjectRepository userProjectRepository;
        private readonly ITicketRepository ticketRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleHelper roleHelper;
        private readonly ClaimsPrincipal? claimUser;

        public TicketHelper(IProjectRepository projectRepository, IUserProjectRepository userProjectRepository, ITicketRepository ticketRepository, UserManager<ApplicationUser> userManager, RoleHelper roleHelper, IHttpContextAccessor httpContextAccessor)
        {
            this.projectRepository = projectRepository;
            this.userProjectRepository = userProjectRepository;
            this.ticketRepository = ticketRepository;
            this.userManager = userManager;
            this.roleHelper = roleHelper;
            claimUser = httpContextAccessor.HttpContext.User;
        }

        public async Task<IEnumerable<Ticket>> GetUserRoleTickets(ApplicationUser? user = null)
        {            
            user ??= await userManager.GetUserAsync(claimUser);
            List<string> roles = await roleHelper.GetRoleNamesOfUser(user.UserName);
            List<Project> projects = userProjectRepository.GetProjectsByUserId(user.Id);

            /* foreach (var project in projects)
            {
                project.Tickets = ticketRepository.GetTicketsByProjectId(project.Id);
            }*/

            if (roles.Contains("Admin") || roles.Contains("Demo Admin"))
            {
                return ticketRepository.GetAllTickets();
            }
            else if (roles.Contains("Project Manager") || roles.Contains("Demo Project Manager"))
            {
                return projects.SelectMany(p => p.Tickets ?? new List<Ticket>());
            }
            else if (roles.Contains("Developer") || roles.Contains("Demo Developer"))
            {
                return projects.SelectMany(p => p.Tickets.Where(t => t.AssignedDeveloperId == user.Id));                
            }
            return projects.SelectMany(p => p.Tickets.Where(t => t.SubmitterId == user.Id));            
        }

        public async Task<bool> IsAuthorizedToCreate(ApplicationUser user)
        {
            List<string> roles = await roleHelper.GetRoleNamesOfUser(user.UserName);

            if (roles.Contains("Admin") || roles.Contains("Demo Admin"))
            {
                return true;
            }
            else if (roles.Contains("Project Manager") || roles.Contains("Demo Project Manager") || roles.Contains("Submitter") || roles.Contains("Demo Submitter"))
            {
                List<Project>? projects = userProjectRepository.GetProjectsByUserId(user.Id);

                if (!projects.Any())
                {
                    return false;
                }
                return true;
            }
            return false;               
        }
    }
}
