using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BugTracker.Models
{    
    public class ApplicationUser : IdentityUser
    {
        [Column(TypeName = "varchar(100)")]
        public string? FirstName { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string? LastName { get; set; }

        public virtual ICollection<Project> Projects { get; set; } = new HashSet<Project>();

        public virtual ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();

        public virtual ICollection<TicketHistoryRecord> TicketHistoryRecords { get; set; } = new HashSet<TicketHistoryRecord>();

        public virtual ICollection<TicketAttachment> TicketAttachments { get; set; } = new HashSet<TicketAttachment>();

        public virtual ICollection<TicketComment> TicketComments { get; set; } = new HashSet<TicketComment>();
    }
}
