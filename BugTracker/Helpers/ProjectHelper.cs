using BugTracker.Models;
using BugTracker.Services;
using Microsoft.AspNetCore.Identity;

namespace BugTracker.Helpers
{
    public class ProjectHelper
    {
        private readonly IProjectRepository projectRepository;
        private readonly IUserProjectRepository userProjectRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleHelper roleHelper;
        private readonly string? userName;

        public ProjectHelper(IProjectRepository projectRepository, IUserProjectRepository userProjectRepository, UserManager<ApplicationUser> userManager, RoleHelper roleHelper, IHttpContextAccessor httpContextAccessor)
        {
            this.projectRepository = projectRepository;
            this.userProjectRepository = userProjectRepository;
            this.userManager = userManager;
            this.roleHelper = roleHelper;
            userName = httpContextAccessor.HttpContext.User.Identity.Name;
        }

        public bool IsUserInProject(string userName, string projectId)
        {
            var user = userManager.Users.FirstOrDefault(u => u.UserName == userName);
            List<ApplicationUser> users = userProjectRepository.GetProjectUsers(projectId);
            return users.Contains(user);       
        }        

        public async Task<IEnumerable<Project>> GetProjectsForUser()
        {
            List<string> roles = await roleHelper.GetRoleNamesOfUser(userName);
            var user = userManager.Users.FirstOrDefault(u => u.UserName == userName);

            if (roles.Contains("Admin") || roles.Contains("Demo Admin"))
            {
                return projectRepository.GetAllProjects();
            }            
            return userProjectRepository.GetUserProjects(user.Id);
        }
    }
}
