﻿using BugTracker.Models;
using BugTracker.Repositories.Interfaces;
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

        public TicketHelper(IProjectRepository projectRepo, IUserProjectRepository userProjectRepo, 
            ITicketRepository ticketRepo, UserManager<ApplicationUser> userManager, 
            RoleHelper roleHelper, IHttpContextAccessor httpContextAccessor)
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
            var roles = await userManager.GetRolesAsync(user);
            var assignedProjects = userProjectRepo.GetProjectsByUserId(user.Id);
            IEnumerable<Ticket> tickets;

            if (roles.Contains("Admin"))
            {
                return ticketRepo.GetAllTickets();
            }
            else if (roles.Contains("Project Manager"))
            {               
                tickets = assignedProjects.SelectMany(p => p.Tickets ?? new List<Ticket>());               
            }
            else if (roles.Contains("Developer"))
            {
                tickets = assignedProjects.SelectMany(p => p.Tickets.Where(t => t.AssignedDeveloperId == user.Id));                   
            }
            tickets = assignedProjects.SelectMany(p => p.Tickets.Where(t => t.SubmitterId == user.Id));
            return tickets.OrderByDescending(t => t.CreatedAt);
        }

        public async Task<bool> IsAuthorizedToEdit(ApplicationUser user, string ticketId)
        {
            IList<string> roles = await userManager.GetRolesAsync(user);
            Ticket ticket = ticketRepo.GetTicketById(ticketId);

            if (roles.Contains("Admin"))
            {
                return true;
            }   
            else if (roles.Contains("Project Manager"))
            {
                return userProjectRepo
                    .GetProjectsByUserId(user.Id)
                    .SelectMany(p => p.Tickets ?? new List<Ticket>())
                    .Select(t => t.Id)
                    .Contains(ticketId);
            }
            else if (roles.Contains("Submitter"))
            {
                return user.Id == ticket.SubmitterId;
            }
            else if (roles.Contains("Developer"))
            {
                return user.Id == ticket.AssignedDeveloperId;
            }
            return false;
        }

        public async Task<bool> IsAssignedDeveloper(ApplicationUser user, string ticketId)
        {
            IList<string> roles = await userManager.GetRolesAsync(user);
            Ticket ticket = ticketRepo.GetTicketById(ticketId);

            if (roles.Contains("Developer"))
            {
                return user.Id == ticket.AssignedDeveloperId;
            }
            return false;
        }

        public async Task<bool> IsAuthorizedToDelete(ApplicationUser user, string ticketId)
        {
            IList<string> roles = await userManager.GetRolesAsync(user);
            Ticket ticket = ticketRepo.GetTicketById(ticketId);

            if (roles.Contains("Admin"))
            {
                return true;
            }
            else if (roles.Contains("Project Manager"))
            {
                return userProjectRepo
                    .GetProjectsByUserId(user.Id)
                    .SelectMany(p => p.Tickets ?? new List<Ticket>())
                    .Select(t => t.Id)
                    .Contains(ticketId);
            }
            else if (roles.Contains("Submitter"))
            {
                return user.Id == ticket.SubmitterId;
            }           
            return false;
        }

        public async Task<int> GetUserTicketCount()
        {
            var tickets = await GetUserRoleTickets();
            return tickets.Count();
        }

        public async Task<int> GetUserUnresolvedTicketCount()
        {
            var tickets = await GetUserRoleTickets();
            return tickets.SkipWhile(t => t.Status == "Resolved").Count();
        }
    }
}
