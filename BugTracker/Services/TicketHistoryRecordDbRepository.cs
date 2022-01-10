using BugTracker.Data;
using BugTracker.Models;

namespace BugTracker.Services
{
    public class TicketHistoryRecordDbRepository : ITicketHistoryRecordRepository
    {
        private readonly BugTrackerDbContext context;

        public TicketHistoryRecordDbRepository(BugTrackerDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<TicketHistoryRecord> GetAllRecords()
        {
            return context.TicketHistoryRecords;
        }

        public List<TicketHistoryRecord> GetRecordsByTicket(string id)
        {
            List<TicketHistoryRecord> records = new();

            foreach (var record in context.TicketHistoryRecords)
            {
                if (record.TicketId == id)
                {
                    records.Add(record);
                }
            }
            return records;
        }

        public TicketHistoryRecord GetRecordById(string id)
        {
            TicketHistoryRecord? record = context.TicketHistoryRecords.Find(id);

            if (record == null)
            {
                throw new Exception("Ticket history record Not Found");
            }
            return record;
        }

        public TicketHistoryRecord Create(TicketHistoryRecord record)
        {
            context.TicketHistoryRecords.Add(record);
            context.SaveChanges();
            return record;
        }

        public TicketHistoryRecord Delete(string id)
        {
            TicketHistoryRecord? record = context.TicketHistoryRecords.Find(id);

            if (record == null)
            {
                throw new NullReferenceException("Ticket history record Not Found");
            }
            context.TicketHistoryRecords.Remove(record);
            context.SaveChanges();
            return record;
        }

        public IEnumerable<TicketHistoryRecord> DeleteRecordsByTicketId(string ticketId)
        {
            var records = context.TicketHistoryRecords.Where(r => r.TicketId == ticketId);
            context.TicketHistoryRecords.RemoveRange(records);
            context.SaveChanges();
            return records;
        }

        public TicketHistoryRecord Update(TicketHistoryRecord record)
        {
            throw new NotImplementedException();
        }
    }
}
