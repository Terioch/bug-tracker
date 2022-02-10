using BugTracker.Models;

namespace BugTracker.Repositories.Mock
{
    public class TicketHistoryMockRepository : ITicketHistoryRepository
    {
        public TicketHistoryMockRepository()
        {            
        }

        private static readonly List<TicketHistoryRecord> ticketHistoryRecords = new()
        {
            new TicketHistoryRecord()
            {
                Id = "th1",
                TicketId = "t1",
                Property = "AssignedDeveloperId",
                OldValue = null,
                NewValue = "4687e432-58fc-448a-b639-6288ee716fa0",
                Modifier = "Rio Stockton",
                ModifiedAt = DateTimeOffset.UtcNow
            },
            new TicketHistoryRecord()
            {
                Id = "th2",
                TicketId = "t1",
                Property = "Status",
                OldValue = "New",
                NewValue = "In Progress",
                Modifier = "Rio Stockton",
                ModifiedAt = DateTimeOffset.UtcNow
            },
            new TicketHistoryRecord()
            {
                Id = "th2",
                TicketId = "t1",
                Property = "Priority",
                OldValue = "High",
                NewValue = "Medium",
                Modifier = "Demo Submitter",
                ModifiedAt = DateTimeOffset.UtcNow
            },           
            new TicketHistoryRecord()
            {
                Id = "th4",
                TicketId = "t2",
                Property = "AssignedDeveloperId",
                OldValue = null,
                NewValue = "cd448813-e865-49e8-933a-dff582b72509",
                Modifier = "Demo Project Manager",
                ModifiedAt = DateTimeOffset.UtcNow
            },
            new TicketHistoryRecord()
            {
                Id = "th4",
                TicketId = "t2",
                Property = "Status",
                OldValue = "New",
                NewValue = "In Progress",
                Modifier = "Demo Project Manager",
                ModifiedAt = DateTimeOffset.UtcNow
            },
            new TicketHistoryRecord()
            {
                Id = "th5",
                TicketId = "t3",
                Property = "Priority",
                OldValue = "Medium",
                NewValue = "High",
                Modifier = "Demo Project Manager",
                ModifiedAt = DateTimeOffset.UtcNow
            },
        };

        public IEnumerable<TicketHistoryRecord> GetAllRecords()
        {
            return ticketHistoryRecords;
        }

        public TicketHistoryRecord GetRecordById(string id)
        {
            return ticketHistoryRecords.Find(r => r.Id == id);            
        }

        public IEnumerable<TicketHistoryRecord> GetRecordsByTicketId(string id)
        {
            return ticketHistoryRecords.Where(t => t.TicketId == id);
        }

        public TicketHistoryRecord Create(TicketHistoryRecord record)
        {
            ticketHistoryRecords.Add(record);            
            return record;
        }

        public TicketHistoryRecord Update(TicketHistoryRecord record)
        {
            int index = ticketHistoryRecords.FindIndex(t => t.Id == record.Id);
            ticketHistoryRecords[index] = record;
            return record;
        }

        public TicketHistoryRecord Delete(string id)
        {
            TicketHistoryRecord? record = ticketHistoryRecords.Find(r => r.Id == id);            
            ticketHistoryRecords.Remove(record);            
            return record;
        }

        public IEnumerable<TicketHistoryRecord> DeleteRecordsByTicketId(string ticketId)
        {
            var records = ticketHistoryRecords.Where(r => r.TicketId == ticketId);
            ticketHistoryRecords.RemoveAll(r => records.Contains(r));            
            return records;
        }        
    }
}
