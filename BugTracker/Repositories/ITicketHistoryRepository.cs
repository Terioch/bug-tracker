using BugTracker.Models;

namespace BugTracker.Repositories
{
    public interface ITicketHistoryRepository
    {
        public IEnumerable<TicketHistoryRecord> GetAllRecords();
        public IEnumerable<TicketHistoryRecord> GetRecordsByTicketId(string ticketId);
        public TicketHistoryRecord GetRecordById(string id);
        public TicketHistoryRecord Create(TicketHistoryRecord record);
        public TicketHistoryRecord Update(TicketHistoryRecord record);
        public TicketHistoryRecord Delete(string id);
        public IEnumerable<TicketHistoryRecord> DeleteRecordsByTicketId(string ticketId);
    }
}
