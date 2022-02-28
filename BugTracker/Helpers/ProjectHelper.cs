using BugTracker.Models;
using BugTracker.Repositories.Interfaces;
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

        public ProjectHelper(IProjectRepository projectRepo, IUserProjectRepository userProjectRepo, UserManager<ApplicationUser> userManager, 
            RoleHelper roleHelper, IHttpContextAccessor httpContextAccessor)
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
            IEnumerable<ApplicationUser> users = userProjectRepo.GetUsersByProjectId(projectId);
            return users.Contains(user);       
        }        

        public async Task<IEnumerable<Project>> GetUserRoleProjects(ApplicationUser? user = null)
        {
            List<string> roles = await roleHelper.GetRoleNamesOfUser(userName);
            user ??= userManager.Users.First(u => u.UserName == userName);

            if (roles.Contains("Admin"))
            {
                return projectRepo.GetAllProjects();
            }            
            return userProjectRepo.GetProjectsByUserId(user.Id);     
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersInRolesOnProject(string projectId, string[] roles)
        {
            Project project = projectRepo.GetProjectById(projectId);
            List<ApplicationUser> users = new();

            foreach (var user in project.Users)
            {
                foreach (string role in roles)
                {
                    if (await userManager.IsInRoleAsync(user, role))
                    {
                        users.Add(user);
                    }
                }
            }
            return users;
        }

        public async Task<bool> IsUserAuthorizedToManageUsers(ApplicationUser user, string projectId)
        {
            List<string> roles = await roleHelper.GetRoleNamesOfUser(user.UserName);

            if (roles.Contains("Admin"))
            {
                return true;
            }
            else if (roles.Contains("Project Manager"))
            {
                return userProjectRepo.GetProjectsByUserId(user.Id).Select(p => p.Id).Contains(projectId);
            }            
            return false;
        }
    }
}
