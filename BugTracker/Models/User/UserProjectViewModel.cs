﻿using BugTracker.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class UserProjectViewModel
    {
        public string? Id { get; set; }

        public string? UserName { get; set; }

        public string? Email { get; set; }

        public List<string>? Projects { get; set; } = new();
    }
}
