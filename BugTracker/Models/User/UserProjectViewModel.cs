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

        public List<Project>? Projects { get; set; } = new();
    }
}
