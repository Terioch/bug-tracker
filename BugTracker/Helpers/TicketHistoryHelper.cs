using BugTracker.Models;
using BugTracker.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BugTracker.Helpers
{
    public class TicketHistoryHelper
    {
        private readonly ITicketHistoryRepository historyRepo;
        private readonly TicketHelper ticketHelper;

        public TicketHistoryHelper(ITicketHistoryRepository historyRepo, TicketHelper ticketHelper)
        {
            this.historyRepo = historyRepo;
            this.ticketHelper = ticketHelper;
        }

        public async Task<IEnumerable<TicketHistoryRecord>> GetUserRoleRecords()
        {
            IEnumerable<Ticket> userRoleTickets = await ticketHelper.GetUserRoleTickets();    
            List<TicketHistoryRecord> userRoleRecords = new();

            foreach (var ticket in userRoleTickets)
            {
                var records = historyRepo.GetRecordsByTicketId(ticket.Id);
                userRoleRecords.AddRange(records);
            }
            return userRoleRecords.OrderByDescending(r => r.ModifiedAt);
        }
    }
}
