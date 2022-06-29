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
            //user ??= await _unitOfWork.UserManager.GetUserAsync(_httpContextAccessor.HttpContext.User);            
            user ??= await _unitOfWork.Users.Get(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var roles = await _unitOfWork.UserManager.GetRolesAsync(user);
            var orderedUserTickets = user.Projects.SelectMany(p => p.Tickets).OrderByDescending(t => t.CreatedAt);

            if (roles.Contains("Admin"))
            {
                return _unitOfWork.Tickets.GetAll();
            }
            else if (roles.Contains("Project Manager"))
            {                            
                return orderedUserTickets;
            }
            else if (roles.Contains("Developer"))
            {              
                return orderedUserTickets.Where(t => t.AssignedDeveloperId == user.Id);                  
            }
            
            return orderedUserTickets.Where(t => t.SubmitterId == user.Id);
        }

        public async Task<bool> IsAuthorizedToEdit(string ticketId, ApplicationUser? user = null)
        {
            user ??= await _unitOfWork.Users.Get(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
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

        public async Task<bool> IsAssignedDeveloper(string ticketId, ApplicationUser? user = null)
        {
            user ??= await _unitOfWork.Users.Get(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var roles = await _unitOfWork.UserManager.GetRolesAsync(user);
            var ticket = await _unitOfWork.Tickets.Get(ticketId);

            if (roles.Contains("Developer"))
            {
                return user.Id == ticket.AssignedDeveloperId;
            }

            return false;
        }

        public async Task<bool> IsAuthorizedToDelete(string ticketId, ApplicationUser? user = null)
        {
            user ??= await _unitOfWork.Users.Get(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
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
