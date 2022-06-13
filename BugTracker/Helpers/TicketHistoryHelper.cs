using BugTracker.Models;
using BugTracker.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BugTracker.Helpers
{
    public class TicketHistoryHelper
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly TicketHelper _ticketHelper;

        public TicketHistoryHelper(IUnitOfWork unitOfWork, TicketHelper ticketHelper)
        {
            _unitOfWork = unitOfWork;
            _ticketHelper = ticketHelper;
        }

        public async Task<IEnumerable<TicketHistoryRecord>> GetUserRoleRecords()
        {
            var userRoleTickets = await _ticketHelper.GetUserRoleTickets();    
            var userRoleRecords = new List<TicketHistoryRecord>();

            foreach (var ticket in userRoleTickets)
            {          
                var records = _unitOfWork.TicketHistoryRecords.Find(r => r.TicketId == ticket.Id);
                userRoleRecords.AddRange(records);
            }

            return userRoleRecords.OrderByDescending(r => r.ModifiedAt);
        }
    }
}
