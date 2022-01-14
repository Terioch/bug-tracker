using BugTracker.Models;
using BugTracker.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BugTracker.Helpers
{    
    public class TicketHelper
    {
        private readonly IProjectRepository projectRepo;
        private readonly IUserProjectRepository userProjectRepo;
        private readonly ITicketRepository ticketRepo;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleHelper roleHelper;
        private readonly ClaimsPrincipal? claimUser;

        public TicketHelper(IProjectRepository projectRepo, IUserProjectRepository userProjectRepo, ITicketRepository ticketRepo, UserManager<ApplicationUser> userManager, RoleHelper roleHelper, IHttpContextAccessor httpContextAccessor)
        {
            this.projectRepo = projectRepo;
            this.userProjectRepo = userProjectRepo;
            this.ticketRepo = ticketRepo;
            this.userManager = userManager;
            this.roleHelper = roleHelper;
            claimUser = httpContextAccessor.HttpContext.User;
        }

        public async Task<IEnumerable<Ticket>> GetUserRoleTickets(ApplicationUser? user = null)
        {            
            user ??= await userManager.GetUserAsync(claimUser);
            List<string> roles = await roleHelper.GetRoleNamesOfUser(user.UserName);
            List<Project> projects = userProjectRepo.GetProjectsByUserId(user.Id);            

            if (roles.Contains("Admin") || roles.Contains("Demo Admin"))
            {
                return ticketRepo.GetAllTickets();
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

        public async Task<bool> IsAuthorizedToEdit(ApplicationUser user, string ticketId)
        {
            List<string> roles = await roleHelper.GetRoleNamesOfUser(user.UserName);
            Ticket ticket = ticketRepo.GetTicketById(ticketId);

            if (roles.Contains("Admin") || roles.Contains("Demo Admin"))
            {
                return true;
            }   
            else if (roles.Contains("Project Manager") || roles.Contains("Demo Project Manager"))
            {
                return userProjectRepo
                    .GetProjectsByUserId(user.Id)
                    .SelectMany(p => p.Tickets ?? new List<Ticket>())
                    .Select(t => t.Id)
                    .Contains(ticketId);
            }
            else if (roles.Contains("Submitter") || roles.Contains("Demo Submitter"))
            {
                return user.Id == ticket.SubmitterId;
            }                              
            return false;
        }

        public async Task<bool> IsOnlyAuthorizedToUpdateStatus(ApplicationUser user, string ticketId)
        {
            List<string> roles = await roleHelper.GetRoleNamesOfUser(user.UserName);
            Ticket ticket = ticketRepo.GetTicketById(ticketId);

            if (roles.Contains("Developer") || roles.Contains("Demo Developer"))
            {
                return user.Id == ticket.AssignedDeveloperId;
            }
            return false;
        }

        public async Task<bool> IsAuthorizedToDelete(ApplicationUser user, string ticketId)
        {
            List<string> roles = await roleHelper.GetRoleNamesOfUser(user.UserName);
            Ticket ticket = ticketRepo.GetTicketById(ticketId);

            if (roles.Contains("Admin") || roles.Contains("Demo Admin"))
            {
                return true;
            }
            else if (roles.Contains("Project Manager") || roles.Contains("Demo Project Manager"))
            {
                return userProjectRepo
                    .GetProjectsByUserId(user.Id)
                    .SelectMany(p => p.Tickets ?? new List<Ticket>())
                    .Select(t => t.Id)
                    .Contains(ticketId);
            }
            else if (roles.Contains("Submitter") || roles.Contains("Demo Submitter"))
            {
                return user.Id == ticket.SubmitterId;
            }           
            return false;
        }
    }
}
