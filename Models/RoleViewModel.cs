using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class RoleViewModel
    {     
        public string Id { get; set; }

        [Required(ErrorMessage = "Role Name is required")]
        public string Name { get; set; }

        public List<string> Users { get; set; } = new List<string>();
    }
}
