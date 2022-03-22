using BugTracker.Contexts.Mock;
using BugTracker.Models;
using BugTracker.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BugTracker.Repositories.Mock
{
    public class TicketHistoryMockRepository : ITicketHistoryRepository
    {
        private readonly UserManager<ApplicationUser> userManager;

        public TicketHistoryMockRepository(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public static List<TicketHistoryRecord> TicketHistoryRecords { get; set; } = MockTicketHistoryRecords.GetRecords();

        public IEnumerable<TicketHistoryRecord> GetAllRecords()
        {
            TicketHistoryRecords.ForEach(r =>
            {
                r.Ticket = TicketMockRepository.Tickets.Find(t => t.Id == r.TicketId);
                r.Modifier = userManager.Users.First(u => u.Id == r.ModifierId);
            });
            return TicketHistoryRecords.OrderByDescending(r => r.ModifiedAt);
        }

        public TicketHistoryRecord GetRecordById(string id)
        {
            TicketHistoryRecord? record = TicketHistoryRecords.Find(r => r.Id == id);
            record.Ticket = TicketMockRepository.Tickets.Find(t => t.Id == record.TicketId);
            record.Modifier = userManager.Users.First(u => u.Id == record.ModifierId);
            return record;
        }

        public IEnumerable<TicketHistoryRecord> GetRecordsByTicketId(string id)
        {
            List<TicketHistoryRecord> records = TicketHistoryRecords.Where(t => t.TicketId == id).ToList();
            records.ForEach(r =>
            {
                r.Ticket = TicketMockRepository.Tickets.Find(t => t.Id == r.TicketId);
                r.Modifier = userManager.Users.First(u => u.Id == r.ModifierId);
            });
            return records.OrderByDescending(r => r.ModifiedAt);
        }

        public TicketHistoryRecord Create(TicketHistoryRecord record)
        {
            TicketHistoryRecords.Add(record);            
            return record;
        }

        public TicketHistoryRecord Update(TicketHistoryRecord record)
        {
            int index = TicketHistoryRecords.FindIndex(t => t.Id == record.Id);
            TicketHistoryRecords[index] = record;
            return record;
        }

        public TicketHistoryRecord Delete(string id)
        {
            TicketHistoryRecord? record = TicketHistoryRecords.Find(r => r.Id == id);            
            TicketHistoryRecords.Remove(record);            
            return record;
        }

        public IEnumerable<TicketHistoryRecord> DeleteRecordsByTicketId(string ticketId)
        {
            var records = TicketHistoryRecords.Where(r => r.TicketId == ticketId);
            TicketHistoryRecords.RemoveAll(r => records.Contains(r));            
            return records;
        }        
    }
}
