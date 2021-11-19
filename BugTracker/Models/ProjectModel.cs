using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class ProjectModel
    {
        public string? Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }

        public List<UserViewModel>? Users { get; set; }

        public List<TicketModel>? Tickets { get; set; }
    }
}
