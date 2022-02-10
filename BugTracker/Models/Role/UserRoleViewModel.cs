using System.ComponentModel.DataAnnotations;

namespace BugTracker.Models
{
    public class UserRoleViewModel
    {
        public string? UserId { get; set; }

        public string? RoleId { get; set; }

        public List<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();

        public List<RoleViewModel>? Roles { get; set; } = new List<RoleViewModel>();
    }
}
