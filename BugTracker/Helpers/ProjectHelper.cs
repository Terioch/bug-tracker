using BugTracker.Models;
using BugTracker.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BugTracker.Helpers
{
    public class ProjectHelper
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleHelper _roleHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProjectHelper(IUnitOfWork unitofWork, RoleHelper roleHelper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitofWork;
            _roleHelper = roleHelper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> IsUserOnProject(string userId, string projectId)
        {
            var project = await _unitOfWork.Projects.Get(projectId);
            return project.Users.Any(u => u.Id == userId);
        }

        public async Task<bool> IsAuthorizedToManageUsers(ApplicationUser user, string projectId)
        {
            List<string> roles = await _roleHelper.GetRoleNamesOfUser(user.UserName);

            if (roles.Contains("Admin"))
            {
                return true;
            }
            else if (roles.Contains("Project Manager"))
            {               
                var project = await _unitOfWork.Projects.Get(projectId);
                return project.Users.Any(u => u.Id == user.Id);
            }

            return false;
        }

        public async Task<IEnumerable<Project>> GetUserRoleProjects(ApplicationUser? user = null)
        {
            //var currentUser = await _unitOfWork.UserManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            user ??= await _unitOfWork.Users.Get(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var roles = await _unitOfWork.UserManager.GetRolesAsync(user);

            if (roles.Contains("Admin"))
            {
                return _unitOfWork.Projects.GetAll();
            }
            
            return user.Projects;            
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersInRolesOnProject(string projectId, string[]? roles = null)
        {
            roles ??= _unitOfWork.RoleManager.Roles.Select(r => r.Name).ToArray();             
            var project = await _unitOfWork.Projects.Get(projectId);
            var projectUsers = project.Users.ToList();
            var users = new List<ApplicationUser>();

            for (int i = 0; i < projectUsers.Count; i++)
            {
                for (int j = 0; j < roles.Length; j++)
                {
                    if (await _unitOfWork.UserManager.IsInRoleAsync(projectUsers[i], roles[j]))
                    {
                        users.Add(projectUsers[i]);
                    }
                }
            }

            return users;
        }                

        public async Task<int> GetUsersInRolesCountOnUserRoleProjects(string[]? roles = null)
        {           
            var projects = await GetUserRoleProjects();
            var distinctUsers = new HashSet<ApplicationUser>();
            var projectList = projects.ToList();

            for (int i = 0; i < projectList.Count; i++)
            {
                var users = await GetUsersInRolesOnProject(projectList[i].Id, roles);

                foreach (var user in users)
                {                   
                    distinctUsers.Add(user);
                }                
            }

            return distinctUsers.Count;
        }
    }
}
