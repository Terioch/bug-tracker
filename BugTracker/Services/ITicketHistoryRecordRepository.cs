using BugTracker.Models;

namespace BugTracker.Services
{
    public interface ITicketHistoryRecordRepository
    {
        public IEnumerable<TicketHistoryRecord> GetAllRecords();
        public IEnumerable<TicketHistoryRecord> GetRecordsByTicket(string ticketId);
        public TicketHistoryRecord GetRecordById(string id);
        public TicketHistoryRecord Create(TicketHistoryRecord record);
        public TicketHistoryRecord Update(TicketHistoryRecord record);
        public TicketHistoryRecord Delete(string id);
    }
}
