﻿using BugTracker.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace BugTracker.Models
{
    public class Project
    {
        public string? Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }

        public IEnumerable<ApplicationUser>? Users { get; set; } = new List<ApplicationUser>();

        public IEnumerable<Ticket>? Tickets { get; set; } = new List<Ticket>();
    }
}
