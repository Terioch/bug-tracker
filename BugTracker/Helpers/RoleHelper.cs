using BugTracker.Areas.Identity.Data;
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

        public async Task<IList<string>> GetRoleNamesOfUser(string userName)
        {
            var user = await userManager.FindByNameAsync(userName);
            return await userManager.GetRolesAsync(user);
        }
    }
}
