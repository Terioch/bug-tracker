using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class Ticket
    {
        public string? Id { get; set; }

        public string? ProjectId { get; set; } 

        public string? Description { get; set; }

        public DateTime? SubmittedDate { get; set; }

        public string? Submitter { get; set; }

        public string? AssignedDeveloper { get; set; }

        public string? Type { get; set; }

        public bool? Status { get; set; }

        public string? Priority { get; set; }
    }
}
