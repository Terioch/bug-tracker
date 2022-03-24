namespace BugTracker.Contexts
{
    public static class TicketContext
    {        
        public static IEnumerable<string> Types { get; private set; } = new HashSet<string>
        {
            "Bugs/Errors",
            "Feature Requests",
            "Training/Document Requests",
            "Other Comments"
        };

        public static IEnumerable<string> Statuses { get; private set; } = new HashSet<string>
        {
            "New",
            "Open",
            "In Progress",
            "Under Review",
            "Resolved"
        };

        public static IEnumerable<string> Priorities { get; private set; } = new HashSet<string>
        {
            "Low",
            "Medium",
            "High",    
        };
    }
}
