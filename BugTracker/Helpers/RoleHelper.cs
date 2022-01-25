using BugTracker.Models;
using Microsoft.AspNetCore.Identity;

namespace BugTracker.Helpers
{
    public class RoleHelper
    {
        private readonly UserManager<ApplicationUser> userManager;

        public RoleHelper(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<bool> IsUserInRole(string userName, string role)
        {
            var user = userManager.Users.FirstOrDefault(u => u.UserName == userName);           
            return await userManager.IsInRoleAsync(user, role);
        }

        public async Task<List<string>> GetRoleNamesOfUser(string userName)
        {
            var user = await userManager.FindByNameAsync(userName);
            return (List<string>)await userManager.GetRolesAsync(user);
        }

        public string SelectHighestLevelRole(List<string> roles)
        {
            List<string> systemRoles = new() { "Admin", "Project Manager", "Developer", "Submitter" };
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
