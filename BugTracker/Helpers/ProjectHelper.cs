using BugTracker.Areas.Identity.Data;
using BugTracker.Services;
using Microsoft.AspNetCore.Identity;

namespace BugTracker.Helpers
{
    public class ProjectHelper
    {
        private readonly IUserProjectRepository userProjectRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public ProjectHelper(IUserProjectRepository userProjectRepository, UserManager<ApplicationUser> userManager)
        {
            this.userProjectRepository = userProjectRepository;
            this.userManager = userManager;
        }

        public bool IsUserInProject(string userName, string projectId)
        {
            var user = userManager.Users.FirstOrDefault(u => u.UserName == userName);
            List<string> userIds = userProjectRepository.GetProjectUsers(projectId);
            return userIds.Contains(user.Id);       
        }

        public async Task<bool> IsUserInRole(string userName, string role)
        {
            var user = userManager.Users.FirstOrDefault(u => u.UserName == userName);
            return await userManager.IsInRoleAsync(user, role);          
        }
    }
}
