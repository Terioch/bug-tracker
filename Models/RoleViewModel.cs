using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class RoleViewModel
    {
        [Required]
        [DisplayName("Name of role")]
        [StringLength(30, MinimumLength = 3)]
        public string Name { get; set; }
    }
}
