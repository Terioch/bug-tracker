using BugTracker.Models;
using Microsoft.AspNetCore.Identity;

namespace BugTracker.Helpers
{
    public class RoleHelper
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleHelper(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> IsUserInRole(string userName, string role)
        {
            var user = await _userManager.FindByNameAsync(userName);
            return await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<List<string>> GetRoleNamesOfUser(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            return (List<string>)await _userManager.GetRolesAsync(user);
        }

        public string SelectHighestLevelRole(List<string> roles)
        {
            string[] systemRoles = new string[] { "Admin", "Project Manager", "Developer", "Submitter" };

            foreach (var role in systemRoles)
            {
                if (roles.Contains(role))
                {
                    return role;
                }
            }

            return "";
        }
    }
}
