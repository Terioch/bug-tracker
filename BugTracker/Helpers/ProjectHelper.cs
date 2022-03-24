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
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly RoleHelper roleHelper;
        private readonly string? userName;

        public ProjectHelper(IProjectRepository projectRepo, IUserProjectRepository userProjectRepo, UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager, RoleHelper roleHelper, IHttpContextAccessor httpContextAccessor)
        {
            this.projectRepo = projectRepo;
            this.userProjectRepo = userProjectRepo;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.roleHelper = roleHelper;
            userName = httpContextAccessor.HttpContext.User.Identity.Name;
        }        

        public bool IsUserInProject(string projectId)
        {
            ApplicationUser user = userManager.Users.First(u => u.UserName == userName);
            IEnumerable<ApplicationUser> users = userProjectRepo.GetUsersByProjectId(projectId);
            return users.Contains(user);       
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

        public async Task<IEnumerable<Project>> GetUserRoleProjects(ApplicationUser? user = null)
        {                 
            user ??= userManager.Users.First(u => u.UserName == userName);
            var roles = await userManager.GetRolesAsync(user);

            if (roles.Contains("Admin"))
            {
                return projectRepo.GetAllProjects();
            }            
            return userProjectRepo.GetProjectsByUserId(user.Id);     
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersInRolesOnProject(string projectId, string[]? roles = null)
        {
            roles ??= roleManager.Roles.Select(r => r.Name).ToArray();
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

        public async Task<int> GetUserProjectCount()
        {
            var projects = await GetUserRoleProjects();
            return projects.Count();
        }    

        public async Task<int> GetUsersInRolesCountOnUserRoleProjects(string[]? roles = null)
        {           
            var projects = await GetUserRoleProjects();
            HashSet<ApplicationUser> distinctUsers = new();

            foreach (var project in projects)
            {
                var users = await GetUsersInRolesOnProject(project.Id, roles);
                foreach (var user in users)
                {
                    distinctUsers.Add(user);
                }                
            }
            return distinctUsers.Count;
        }
    }
}
