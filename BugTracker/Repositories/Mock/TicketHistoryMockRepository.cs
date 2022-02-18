using BugTracker.Models;
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

        private static readonly List<TicketHistoryRecord> ticketHistoryRecords = new()
        {
            new TicketHistoryRecord()
            {
                Id = "th1",
                TicketId = "t1",
                ModifierId = "cd448813-e865-49e8-933a-dff582b72509",
                Property = "AssignedDeveloperId",
                OldValue = null,
                NewValue = "4687e432-58fc-448a-b639-6288ee716fa0",                
                ModifiedAt = DateTimeOffset.UtcNow
            },
            new TicketHistoryRecord()
            {
                Id = "th2",
                TicketId = "t1",
                ModifierId = "4687e432-58fc-448a-b639-6288ee716fa0",
                Property = "Status",
                OldValue = "New",
                NewValue = "In Progress",                
                ModifiedAt = DateTimeOffset.UtcNow
            },
            new TicketHistoryRecord()
            {
                Id = "th3",
                TicketId = "t1",
                ModifierId = "ccd193a8-b38b-4414-a318-f4da79c046ae",
                Property = "Priority",
                OldValue = "High",
                NewValue = "Medium",             
                ModifiedAt = DateTimeOffset.UtcNow
            },
            new TicketHistoryRecord()
            {
                Id = "th4",
                TicketId = "t1",
                ModifierId = "cd448813-e865-49e8-933a-dff582b72509",
                Property = "Priority",
                OldValue = "Medium",
                NewValue = "Low",
                ModifiedAt = DateTimeOffset.UtcNow
            },
            new TicketHistoryRecord()
            {
                Id = "th5",
                TicketId = "t2",
                ModifierId = "ccd193a8-b38b-4414-a318-f4da79c046ae",
                Property = "AssignedDeveloperId",
                OldValue = null,
                NewValue = "cd448813-e865-49e8-933a-dff582b72509",               
                ModifiedAt = DateTimeOffset.UtcNow
            },
            new TicketHistoryRecord()
            {
                Id = "th6",
                TicketId = "t2",
                ModifierId = "4687e432-58fc-448a-b639-6288ee716fa0",
                Property = "Status",
                OldValue = "New",
                NewValue = "In Progress",       
                ModifiedAt = DateTimeOffset.UtcNow
            },
            new TicketHistoryRecord()
            {
                Id = "th7",
                TicketId = "t3",
                ModifierId = "2ae32131-606d-495c-81cf-86f38875f9a7",
                Property = "Title",
                OldValue = "Add project user bug",
                NewValue = "Assign project user bug",               
                ModifiedAt = DateTimeOffset.UtcNow
            },
            new TicketHistoryRecord()
            {
                Id = "th8",
                TicketId = "t4",
                ModifierId = "fb37911c-7ceb-42ff-afc3-24b3bd189d9c",
                Property = "AssignedDeveloperId",
                OldValue = null,
                NewValue = "cd448813-e865-49e8-933a-dff582b72509",
                ModifiedAt = DateTimeOffset.UtcNow
            },
            new TicketHistoryRecord()
            {
                Id = "th9",
                TicketId = "t4",
                ModifierId = "fb37911c-7ceb-42ff-afc3-24b3bd189d9c",
                Property = "Status",
                OldValue = "In Progress",
                NewValue = "Resolved",
                ModifiedAt = DateTimeOffset.UtcNow
            },
        };       

        public IEnumerable<TicketHistoryRecord> GetAllRecords()
        {
            return ticketHistoryRecords;
        }

        public TicketHistoryRecord GetRecordById(string id)
        {
            TicketHistoryRecord? record = ticketHistoryRecords.Find(r => r.Id == id);
            record.Modifier = userManager.Users.FirstOrDefault(u => u.Id == record.ModifierId);
            return record;
        }

        public IEnumerable<TicketHistoryRecord> GetRecordsByTicketId(string id)
        {
            List<TicketHistoryRecord> records = ticketHistoryRecords.Where(t => t.TicketId == id).ToList();
            records.ForEach(r =>
            {
                r.Modifier = userManager.Users.FirstOrDefault(u => u.Id == r.ModifierId);
            });
            return records;
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
