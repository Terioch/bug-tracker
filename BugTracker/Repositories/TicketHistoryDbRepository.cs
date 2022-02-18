using BugTracker.Data;
using BugTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Repositories
{
    public class TicketHistoryDbRepository : ITicketHistoryRepository
    {
        private readonly BugTrackerDbContext context;

        public TicketHistoryDbRepository(BugTrackerDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<TicketHistoryRecord> GetAllRecords()
        {
            return context.TicketHistoryRecords;
        }        

        public TicketHistoryRecord GetRecordById(string id)
        {
            return context.TicketHistoryRecords
                .Include(t => t.Modifier)
                .First(t => t.Id == id);
        }

        public IEnumerable<TicketHistoryRecord> GetRecordsByTicketId(string id)
        {
            return context.TicketHistoryRecords
                .Where(t => t.TicketId == id)
                .Include(t => t.Modifier);
        }

        public TicketHistoryRecord Create(TicketHistoryRecord record)
        {
            context.TicketHistoryRecords.Add(record);
            context.SaveChanges();
            return record;
        }

        public TicketHistoryRecord Update(TicketHistoryRecord record)
        {
            throw new NotImplementedException();
        }

        public TicketHistoryRecord Delete(string id)
        {
            TicketHistoryRecord? record = context.TicketHistoryRecords.Find(id);            
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
    }
}
