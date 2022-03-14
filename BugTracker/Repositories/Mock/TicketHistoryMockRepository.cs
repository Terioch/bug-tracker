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

        private static readonly List<TicketHistoryRecord> ticketHistoryRecords = MockTicketHistoryRecords.GetRecords();

        public IEnumerable<TicketHistoryRecord> GetAllRecords()
        {
            ticketHistoryRecords.ForEach(r =>
            {
                r.Ticket = MockTickets.GetTickets().Find(t => t.Id == r.TicketId);
                r.Modifier = userManager.Users.First(u => u.Id == r.ModifierId);
            });
            return ticketHistoryRecords.OrderByDescending(r => r.ModifiedAt);
        }

        public TicketHistoryRecord GetRecordById(string id)
        {
            TicketHistoryRecord? record = ticketHistoryRecords.Find(r => r.Id == id);
            record.Ticket = MockTickets.GetTickets().Find(t => t.Id == record.TicketId);
            record.Modifier = userManager.Users.First(u => u.Id == record.ModifierId);
            return record;
        }

        public IEnumerable<TicketHistoryRecord> GetRecordsByTicketId(string id)
        {
            List<TicketHistoryRecord> records = ticketHistoryRecords.Where(t => t.TicketId == id).ToList();
            records.ForEach(r =>
            {
                r.Ticket = MockTickets.GetTickets().Find(t => t.Id == r.TicketId);
                r.Modifier = userManager.Users.First(u => u.Id == r.ModifierId);
            });
            return records.OrderByDescending(r => r.ModifiedAt);
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
