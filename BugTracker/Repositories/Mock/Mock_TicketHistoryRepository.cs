using BugTracker.Contexts.Mock;
using BugTracker.Models;
using BugTracker.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;

namespace BugTracker.Repositories.Mock
{
    public class Mock_TicketHistoryRepository : IRepository<TicketHistoryRecord>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Mock_TicketHistoryRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public static List<TicketHistoryRecord> TicketHistoryRecords { get; set; } = MockTicketHistoryRecords.GetRecords();

        public IEnumerable<TicketHistoryRecord> GetAll()
        {
            TicketHistoryRecords.ForEach(r =>
            {
                r.Ticket = MockBugTrackerDbContext.Tickets.Find(t => t.Id == r.TicketId);
                r.Modifier = _unitOfWork.UserManager.Users.First(u => u.Id == r.ModifierId);
            });

            return TicketHistoryRecords.OrderByDescending(r => r.ModifiedAt);
        }

        public Task<TicketHistoryRecord> GetAsync(string id)
        {
            var record = TicketHistoryRecords.Find(r => r.Id == id);            
            record.Ticket = MockBugTrackerDbContext.Tickets.Find(t => t.Id == record.TicketId);
            record.Modifier = _unitOfWork.UserManager.Users.First(u => u.Id == record.ModifierId);
            return Task.FromResult(record);
        }

        public IEnumerable<TicketHistoryRecord> Find(Expression<Func<TicketHistoryRecord, bool>> predicate)
        {
            var records = TicketHistoryRecords.AsQueryable().Where(predicate).ToList();

            records.ForEach(r =>
            {
                r.Ticket = MockBugTrackerDbContext.Tickets.Find(t => t.Id == r.TicketId);
                r.Modifier = _unitOfWork.UserManager.Users.First(u => u.Id == r.ModifierId);
            });

            return records.OrderByDescending(r => r.ModifiedAt);
        }

        public void Add(TicketHistoryRecord record)
        {
            TicketHistoryRecords.Add(record);            
        }

        public void Delete(TicketHistoryRecord record)
        {               
            TicketHistoryRecords.Remove(record);                        
        }

        public void DeleteRange(IEnumerable<TicketHistoryRecord> records)
        {                              
            int index = TicketHistoryRecords.IndexOf(records.First());
            TicketHistoryRecords.RemoveRange(index, records.Count());
        }        
    }
}
