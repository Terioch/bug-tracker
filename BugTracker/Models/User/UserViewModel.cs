using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class UserViewModel
    {
        public string? Id { get; set; }

        public string? UserName { get; set; }

        public List<string>? Projects { get; set; } = new();
    }
}
