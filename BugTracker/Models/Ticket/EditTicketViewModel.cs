﻿namespace BugTracker.Models
{
    public class EditTicketViewModel
    {
        public string? Id { get; set; }

        public string? Title { get; set; }
        
        public string? Description { get; set; }

        public string? ProjectName { get; set; }

        public string? AssignedDeveloper { get; set; }

        public string? Type { get; set; }

        public string? Status { get; set; }

        public string? Priority { get; set; }
    }
}