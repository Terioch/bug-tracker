using BugTracker.Models;
using BugTracker.Repositories.Interfaces;

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
            throw new NotImplementedException();
        }
    }
}
