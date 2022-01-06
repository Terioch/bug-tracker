using BugTracker.Models;
using BugTracker.Services;
using Microsoft.AspNetCore.Identity;

namespace BugTracker.Helpers
{
    public class ProjectHelper
    {
        private readonly IProjectRepository projectRepo;
        private readonly IUserProjectRepository userProjectRepo;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleHelper roleHelper;
        private readonly string? userName;

        public ProjectHelper(IProjectRepository projectRepo, IUserProjectRepository userProjectRepo, UserManager<ApplicationUser> userManager, RoleHelper roleHelper, IHttpContextAccessor httpContextAccessor)
        {
            this.projectRepo = projectRepo;
            this.userProjectRepo = userProjectRepo;
            this.userManager = userManager;
            this.roleHelper = roleHelper;
            userName = httpContextAccessor.HttpContext.User.Identity.Name;
        }        

        public bool IsUserInProject(string projectId)
        {
            ApplicationUser user = userManager.Users.First(u => u.UserName == userName);
            List<ApplicationUser> users = userProjectRepo.GetUsersByProjectId(projectId);
            return users.Contains(user);       
        }        

        public async Task<IEnumerable<Project>> GetUserRoleProjects(ApplicationUser? user = null)
        {
            List<string> roles = await roleHelper.GetRoleNamesOfUser(userName);
            user ??= userManager.Users.First(u => u.UserName == userName);

            if (roles.Contains("Admin") || roles.Contains("Demo Admin"))
            {
                return projectRepo.GetAllProjects();
            }            
            return userProjectRepo.GetProjectsByUserId(user.Id);     
        }

        public Project GetFullProject(string id)
        {
            return new Project();
        }
    }
}
