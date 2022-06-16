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
            return userRoleTickets
                .SelectMany(t => t.TicketHistoryRecords ?? new List<TicketHistoryRecord>())
                .OrderByDescending(r => r.ModifiedAt);
        }
    }
}
