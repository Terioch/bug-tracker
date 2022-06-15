﻿using BugTracker.Models;
using BugTracker.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

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
            user ??= await _unitOfWork.UserManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var roles = await _unitOfWork.UserManager.GetRolesAsync(user);

            if (roles.Contains("Admin"))
            {
                return _unitOfWork.Projects.GetAll();
            }

            return _unitOfWork.UserManager.Users.First(u => u.Id == user.Id).Projects; // May not load project nav properties
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersInRolesOnProject(string projectId, string[]? roles)
        {
            roles ??= _unitOfWork.RoleManager.Roles.Select(r => r.Name).ToArray();   
            //roles ??= new string[] { "Owner", "Admin", "Project Manager", "Developer", "Submitter" };
            var project = await _unitOfWork.Projects.Get(projectId);
            var users = new List<ApplicationUser>();

            foreach (var user in project.Users)
            {
                foreach (string role in roles)
                {
                    if (await _unitOfWork.UserManager.IsInRoleAsync(user, role))
                    {
                        users.Add(user);
                    }
                }
            }

            return users;
        }                

        public async Task<int> GetUsersInRolesCountOnUserRoleProjects(string[]? roles = null)
        {           
            var projects = await GetUserRoleProjects();
            var distinctUsers = new List<ApplicationUser>();

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
