using BugTracker.Models;
using BugTracker.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BugTracker.Helpers
{
    public class TicketHelper
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleHelper _roleHelper;
        private readonly IHttpContextAccessor? _httpContextAccessor;

        public TicketHelper(IUnitOfWork unitOfWork, RoleHelper roleHelper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;            
            _roleHelper = roleHelper;
            _httpContextAccessor = httpContextAccessor;            
        }

        public async Task<IEnumerable<Ticket>> GetUserRoleTickets(ApplicationUser? user = null)
        {
            user ??= await _unitOfWork.UserManager.GetUserAsync(_httpContextAccessor.HttpContext.User);            
            var roles = await _unitOfWork.UserManager.GetRolesAsync(user);                       

            if (roles.Contains("Admin"))
            {
                return _unitOfWork.Tickets.GetAll();
            }
            else if (roles.Contains("Project Manager"))
            {               
                return user.Projects.SelectMany(p => p.Tickets ?? new List<Ticket>()).OrderByDescending(t => t.CreatedAt);               
            }
            else if (roles.Contains("Developer"))
            {
                return user.Projects.SelectMany(p => p.Tickets.Where(t => t.AssignedDeveloperId == user.Id)).OrderByDescending(t => t.CreatedAt);                  
            }

            return user.Projects.SelectMany(p => p.Tickets.Where(t => t.SubmitterId == user.Id)).OrderByDescending(t => t.CreatedAt);          
        }

        public async Task<bool> IsAuthorizedToEdit(ApplicationUser user, string ticketId)
        {
            var roles = await _unitOfWork.UserManager.GetRolesAsync(user);
            var ticket = await _unitOfWork.Tickets.Get(ticketId);

            if (roles.Contains("Admin"))
            {
                return true;
            }   
            else if (roles.Contains("Project Manager"))
            {
                return user.Projects
                    .SelectMany(p => p.Tickets)
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
            var roles = await _unitOfWork.UserManager.GetRolesAsync(user);
            var ticket = await _unitOfWork.Tickets.Get(ticketId);

            if (roles.Contains("Developer"))
            {
                return user.Id == ticket.AssignedDeveloperId;
            }

            return false;
        }

        public async Task<bool> IsAuthorizedToDelete(ApplicationUser user, string ticketId)
        {
            var roles = await _unitOfWork.UserManager.GetRolesAsync(user);
            var ticket = await _unitOfWork.Tickets.Get(ticketId);

            if (roles.Contains("Admin"))
            {
                return true;
            }
            else if (roles.Contains("Project Manager"))
            {
                return user.Projects
                    .SelectMany(p => p.Tickets)
                    .Select(t => t.Id)
                    .Contains(ticketId);
            }
            else if (roles.Contains("Submitter"))
            {
                return user.Id == ticket.SubmitterId;
            }         
            
            return false;
        }        

        public async Task<int> GetUserUnresolvedTicketCount()
        {
            var tickets = await GetUserRoleTickets();
            return tickets.SkipWhile(t => t.Status == "Resolved").Count();
        }
    }
}
