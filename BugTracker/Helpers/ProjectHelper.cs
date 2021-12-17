using BugTracker.Areas.Identity.Data;
using BugTracker.Models;
using BugTracker.Services;
using Microsoft.AspNetCore.Identity;

namespace BugTracker.Helpers
{
    public class ProjectHelper
    {
        private readonly IUserProjectRepository userProjectRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleHelper roleHelper;

        public ProjectHelper(IUserProjectRepository userProjectRepository, UserManager<ApplicationUser> userManager, RoleHelper roleHelper)
        {
            this.userProjectRepository = userProjectRepository;
            this.userManager = userManager;
            this.roleHelper = roleHelper;
        }

        public bool IsUserInProject(string userName, string projectId)
        {
            var user = userManager.Users.FirstOrDefault(u => u.UserName == userName);
            List<string> userIds = userProjectRepository.GetProjectUsers(projectId);
            return userIds.Contains(user.Id);       
        }        

        public async Task<IEnumerable<Project>> FilterProjectsByRole(IEnumerable<Project> projects, ApplicationUser user)
        {
            List<string> roles = await roleHelper.GetRoleNamesOfUser(user.UserName);

            if (roles.Contains("Admin") || roles.Contains("Demo Admin"))
            {
                return projects;
            }            
            return projects.Where(p => IsUserInProject(user.UserName, p.Id));
        }
    }
}
