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
                    ModifierId = "04f5c01b-602a-484f-a89d-6fa57db31687",
                    Property = "AssignedDeveloperId",
                    OldValue = null,
                    NewValue = "338986f3-f9b5-4f0a-8f55-4fdb7ebec83e",
                    ModifiedAt = DateTimeOffset.UtcNow
                },
                new TicketHistoryRecord()
                {
                    Id = "th2",
                    TicketId = "t1",
                    ModifierId = "338986f3-f9b5-4f0a-8f55-4fdb7ebec83e",
                    Property = "Status",
                    OldValue = "New",
                    NewValue = "In Progress",
                    ModifiedAt = DateTimeOffset.UtcNow
                },
                new TicketHistoryRecord()
                {
                    Id = "th3",
                    TicketId = "t1",
                    ModifierId = "cad3865d-5fb5-4e88-9c98-775fbc4610ca",
                    Property = "Priority",
                    OldValue = "High",
                    NewValue = "Medium",
                    ModifiedAt = DateTimeOffset.UtcNow
                },
                new TicketHistoryRecord()
                {
                    Id = "th4",
                    TicketId = "t1",
                    ModifierId = "04f5c01b-602a-484f-a89d-6fa57db31687",
                    Property = "Priority",
                    OldValue = "Medium",
                    NewValue = "Low",
                    ModifiedAt = DateTimeOffset.UtcNow
                },
                new TicketHistoryRecord()
                {
                    Id = "th5",
                    TicketId = "t2",
                    ModifierId = "cad3865d-5fb5-4e88-9c98-775fbc4610ca",
                    Property = "AssignedDeveloperId",
                    OldValue = null,
                    NewValue = "04f5c01b-602a-484f-a89d-6fa57db31687",
                    ModifiedAt = DateTimeOffset.UtcNow
                },
                new TicketHistoryRecord()
                {
                    Id = "th6",
                    TicketId = "t2",
                    ModifierId = "338986f3-f9b5-4f0a-8f55-4fdb7ebec83e",
                    Property = "Status",
                    OldValue = "New",
                    NewValue = "In Progress",
                    ModifiedAt = DateTimeOffset.UtcNow
                },
                new TicketHistoryRecord()
                {
                    Id = "th7",
                    TicketId = "t3",
                    ModifierId = "421553e8-65cc-4416-8596-3d1277c99338",
                    Property = "Title",
                    OldValue = "Add project user bug",
                    NewValue = "Assign project user bug",
                    ModifiedAt = DateTimeOffset.UtcNow
                },
                new TicketHistoryRecord()
                {
                    Id = "th8",
                    TicketId = "t4",
                    ModifierId = "a111cc04-b2e6-4a2e-9a4f-b74059b1a953",
                    Property = "AssignedDeveloperId",
                    OldValue = null,
                    NewValue = "04f5c01b-602a-484f-a89d-6fa57db31687",
                    ModifiedAt = DateTimeOffset.UtcNow
                },
                new TicketHistoryRecord()
                {
                    Id = "th9",
                    TicketId = "t4",
                    ModifierId = "a111cc04-b2e6-4a2e-9a4f-b74059b1a953",
                    Property = "Status",
                    OldValue = "New",
                    NewValue = "In Progress",
                    ModifiedAt = DateTimeOffset.UtcNow
                },
                new TicketHistoryRecord()
                {
                    Id = "th10",
                    TicketId = "t4",
                    ModifierId = "04f5c01b-602a-484f-a89d-6fa57db31687",
                    Property = "Status",
                    OldValue = "In Progress",
                    NewValue = "Under Review",
                    ModifiedAt = DateTimeOffset.UtcNow
                },
                new TicketHistoryRecord()
                {
                    Id = "th11",
                    TicketId = "t5",
                    ModifierId = "421553e8-65cc-4416-8596-3d1277c99338",
                    Property = "AssignedDeveloperId",
                    OldValue = null,
                    NewValue = "338986f3-f9b5-4f0a-8f55-4fdb7ebec83e",
                    ModifiedAt = DateTimeOffset.UtcNow
                },
                new TicketHistoryRecord()
                {
                    Id = "th12",
                    TicketId = "t5",
                    ModifierId = "421553e8-65cc-4416-8596-3d1277c99338",
                    Property = "AssignedDeveloperId",
                    OldValue = "338986f3-f9b5-4f0a-8f55-4fdb7ebec83e",
                    NewValue = null,
                    ModifiedAt = DateTimeOffset.UtcNow
                },
                new TicketHistoryRecord()
                {
                    Id = "th13",
                    TicketId = "t7",
                    ModifierId = "338986f3-f9b5-4f0a-8f55-4fdb7ebec83e",
                    Property = "Status",
                    OldValue = "New",
                    NewValue = "In Progress",
                    ModifiedAt = DateTimeOffset.UtcNow
                },
                new TicketHistoryRecord()
                {
                    Id = "th14",
                    TicketId = "t7",
                    ModifierId = "338986f3-f9b5-4f0a-8f55-4fdb7ebec83e",
                    Property = "Description",
                    OldValue = "Issue regarding role claims",
                    NewValue = "Role claims are not being added correctly",
                    ModifiedAt = DateTimeOffset.UtcNow
                },
                new TicketHistoryRecord()
                {
                    Id = "th15",
                    TicketId = "t8",
                    ModifierId = "04f5c01b-602a-484f-a89d-6fa57db31687",
                    Property = "Priority",
                    OldValue = "High",
                    NewValue = "Low",
                    ModifiedAt = DateTimeOffset.UtcNow
                },
                new TicketHistoryRecord()
                {
                    Id = "th16",
                    TicketId = "t8",
                    ModifierId = "338986f3-f9b5-4f0a-8f55-4fdb7ebec83e",
                    Property = "Status",
                    OldValue = "New",
                    NewValue = "In Progress",
                    ModifiedAt = DateTimeOffset.UtcNow
                },
                new TicketHistoryRecord()
                {
                    Id = "th17",
                    TicketId = "t9",
                    ModifierId = "a111cc04-b2e6-4a2e-9a4f-b74059b1a953",
                    Property = "Type",
                    OldValue = "Other Comments",
                    NewValue = "Feature Requests",
                    ModifiedAt = DateTimeOffset.UtcNow
                },
                new TicketHistoryRecord()
                {
                    Id = "th18",
                    TicketId = "t9",
                    ModifierId = "a111cc04-b2e6-4a2e-9a4f-b74059b1a953",
                    Property = "AssignedDeveloperId",
                    OldValue = null,
                    NewValue = "338986f3-f9b5-4f0a-8f55-4fdb7ebec83e",
                    ModifiedAt = DateTimeOffset.UtcNow
                },
                new TicketHistoryRecord()
                {
                    Id = "th19",
                    TicketId = "t9",
                    ModifierId = "c92554a2-2201-47d0-97b6-8eb8c52fac75",
                    Property = "Status",
                    OldValue = "New",
                    NewValue = "In Progress",
                    ModifiedAt = DateTimeOffset.UtcNow
                },
                new TicketHistoryRecord()
                {
                    Id = "th20",
                    TicketId = "t9",
                    ModifierId = "c92554a2-2201-47d0-97b6-8eb8c52fac75",
                    Property = "Status",
                    OldValue = "In Progress",
                    NewValue = "Under Review",
                    ModifiedAt = DateTimeOffset.UtcNow
                },
                new TicketHistoryRecord()
                {
                    Id = "th21",
                    TicketId = "t9",
                    ModifierId = "a111cc04-b2e6-4a2e-9a4f-b74059b1a953",
                    Property = "Status",
                    OldValue = "Under Review",
                    NewValue = "Resolved",
                    ModifiedAt = DateTimeOffset.UtcNow
                },
                new TicketHistoryRecord()
                {
                    Id = "th22",
                    TicketId = "t10",
                    ModifierId = "421553e8-65cc-4416-8596-3d1277c99338",
                    Property = "AssignedDeveloperId",
                    OldValue = null,
                    NewValue = "338986f3-f9b5-4f0a-8f55-4fdb7ebec83e",
                    ModifiedAt = DateTimeOffset.UtcNow
                },
                new TicketHistoryRecord()
                {
                    Id = "th23",
                    TicketId = "t10",
                    ModifierId = "421553e8-65cc-4416-8596-3d1277c99338",
                    Property = "Status",
                    OldValue = "New",
                    NewValue = "In Progress",
                    ModifiedAt = DateTimeOffset.UtcNow
                },
            };
        }
    }
}
