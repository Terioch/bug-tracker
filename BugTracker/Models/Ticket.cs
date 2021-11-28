using BugTracker.Areas.Identity.Data;
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

        [Required]
        public string? ProjectId { get; set; }

        [Required]
        [MaxLength(40)]
        public string? Title { get; set; }

        [Required]        
        public string? Description { get; set; }

        [Required]
        public DateTime? SubmittedDate { get; set; }

        [Required]
        public ApplicationUser? Submitter { get; set; }

        [Required]
        public ApplicationUser? AssignedDeveloper { get; set; }

        [Required]
        public string? Type { get; set; }

        [Required]
        public string? Status { get; set; }

        [Required]
        public string? Priority { get; set; }
    }
}
