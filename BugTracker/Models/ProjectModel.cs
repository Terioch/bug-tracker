using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class ProjectModel
    {
        public string? ProjectId { get; set; }

        public string? Name { get; set; }

        public List<TicketModel>? Tickets { get; set; }
    }
}
