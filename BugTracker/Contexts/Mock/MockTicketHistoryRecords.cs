using BugTracker.Models;

namespace BugTracker.Contexts.Mock
{
    public class MockTicketHistoryRecords
    {
        public static List<TicketHistoryRecord> GetRecords()
        {
            return new List<TicketHistoryRecord>()
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
                    OldValue = "New",
                    NewValue = "In Progress",
                    ModifiedAt = DateTimeOffset.UtcNow
                },
                new TicketHistoryRecord()
                {
                    Id = "th10",
                    TicketId = "t4",
                    ModifierId = "cd448813-e865-49e8-933a-dff582b72509",
                    Property = "Status",
                    OldValue = "In Progress",
                    NewValue = "Under Review",
                    ModifiedAt = DateTimeOffset.UtcNow
                },
                new TicketHistoryRecord()
                {
                    Id = "th11",
                    TicketId = "t5",
                    ModifierId = "2ae32131-606d-495c-81cf-86f38875f9a7",
                    Property = "AssignedDeveloperId",
                    OldValue = null,
                    NewValue = "4687e432-58fc-448a-b639-6288ee716fa0",
                    ModifiedAt = DateTimeOffset.UtcNow
                },
                new TicketHistoryRecord()
                {
                    Id = "th12",
                    TicketId = "t5",
                    ModifierId = "2ae32131-606d-495c-81cf-86f38875f9a7",
                    Property = "AssignedDeveloperId",
                    OldValue = "4687e432-58fc-448a-b639-6288ee716fa0",
                    NewValue = null,
                    ModifiedAt = DateTimeOffset.UtcNow
                },
                new TicketHistoryRecord()
                {
                    Id = "th13",
                    TicketId = "t7",
                    ModifierId = "4687e432-58fc-448a-b639-6288ee716fa0",
                    Property = "Status",
                    OldValue = "New",
                    NewValue = "In Progress",
                    ModifiedAt = DateTimeOffset.UtcNow
                },
                new TicketHistoryRecord()
                {
                    Id = "th14",
                    TicketId = "t7",
                    ModifierId = "4687e432-58fc-448a-b639-6288ee716fa0",
                    Property = "Description",
                    OldValue = "Issue regarding role claims",
                    NewValue = "Role claims are not being added correctly",
                    ModifiedAt = DateTimeOffset.UtcNow
                },
                new TicketHistoryRecord()
                {
                    Id = "th15",
                    TicketId = "t8",
                    ModifierId = "cd448813-e865-49e8-933a-dff582b72509",
                    Property = "Priority",
                    OldValue = "High",
                    NewValue = "Low",
                    ModifiedAt = DateTimeOffset.UtcNow
                },
                new TicketHistoryRecord()
                {
                    Id = "th16",
                    TicketId = "t8",
                    ModifierId = "4687e432-58fc-448a-b639-6288ee716fa0",
                    Property = "Status",
                    OldValue = "New",
                    NewValue = "In Progress",
                    ModifiedAt = DateTimeOffset.UtcNow
                },
                new TicketHistoryRecord()
                {
                    Id = "th17",
                    TicketId = "t9",
                    ModifierId = "fb37911c-7ceb-42ff-afc3-24b3bd189d9c",
                    Property = "Type",
                    OldValue = "Other Comments",
                    NewValue = "Feature Requests",
                    ModifiedAt = DateTimeOffset.UtcNow
                },
                new TicketHistoryRecord()
                {
                    Id = "th18",
                    TicketId = "t9",
                    ModifierId = "fb37911c-7ceb-42ff-afc3-24b3bd189d9c",
                    Property = "AssignedDeveloperId",
                    OldValue = null,
                    NewValue = "4687e432-58fc-448a-b639-6288ee716fa0",
                    ModifiedAt = DateTimeOffset.UtcNow
                },
                new TicketHistoryRecord()
                {
                    Id = "th19",
                    TicketId = "t9",
                    ModifierId = "0de2c4ff-6923-4b30-8d8a-e7b90b5edcbf",
                    Property = "Status",
                    OldValue = "New",
                    NewValue = "In Progress",
                    ModifiedAt = DateTimeOffset.UtcNow
                },
                new TicketHistoryRecord()
                {
                    Id = "th20",
                    TicketId = "t9",
                    ModifierId = "0de2c4ff-6923-4b30-8d8a-e7b90b5edcbf",
                    Property = "Status",
                    OldValue = "In Progress",
                    NewValue = "Under Review",
                    ModifiedAt = DateTimeOffset.UtcNow
                },
                new TicketHistoryRecord()
                {
                    Id = "th21",
                    TicketId = "t9",
                    ModifierId = "fb37911c-7ceb-42ff-afc3-24b3bd189d9c",
                    Property = "Status",
                    OldValue = "Under Review",
                    NewValue = "Resolved",
                    ModifiedAt = DateTimeOffset.UtcNow
                },
                new TicketHistoryRecord()
                {
                    Id = "th22",
                    TicketId = "t10",
                    ModifierId = "2ae32131-606d-495c-81cf-86f38875f9a7",
                    Property = "AssignedDeveloperId",
                    OldValue = null,
                    NewValue = "4687e432-58fc-448a-b639-6288ee716fa0",
                    ModifiedAt = DateTimeOffset.UtcNow
                },
                new TicketHistoryRecord()
                {
                    Id = "th23",
                    TicketId = "t10",
                    ModifierId = "2ae32131-606d-495c-81cf-86f38875f9a7",
                    Property = "Status",
                    OldValue = "New",
                    NewValue = "In Progress",
                    ModifiedAt = DateTimeOffset.UtcNow
                },
            };
        }
    }
}
