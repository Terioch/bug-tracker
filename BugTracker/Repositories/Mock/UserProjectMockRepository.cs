using BugTracker.Contexts.Mock;
using BugTracker.Models;
using BugTracker.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BugTracker.Repositories.Mock
{
    public class UserProjectMockRepository : IUserProjectRepository
    {
        private readonly IProjectRepository projectRepo;
        private readonly UserManager<ApplicationUser> userManager;

        public UserProjectMockRepository(IProjectRepository projectRepo, UserManager<ApplicationUser> userManager)
        {
            this.projectRepo = projectRepo;
            this.userManager = userManager;
        }

        public static List<UserProject> UserProjects { get; set; } = MockUserProjects.GetUserProjects();

        public IEnumerable<Project> GetProjectsByUserId(string userId)
        {
            IEnumerable<UserProject> filteredUserProjects = UserProjects.Where(u => u.UserId == userId);
            var projectIds = filteredUserProjects.Select(u => u.ProjectId).ToList();
            List<Project> projects = new();

            for (int i = 0; i < projectIds.Count; i++)
            {
                Project project = projectRepo.GetProjectById(projectIds[i]);
                projects.Add(project);
            }
            return projects;
        }

        public IEnumerable<ApplicationUser> GetUsersByProjectId(string projectId)
        {
            IEnumerable<UserProject> filteredUserProjects = UserProjects.Where(u => u.ProjectId == projectId);
            var userIds = filteredUserProjects.Select(u => u.UserId).ToList();
            List<ApplicationUser> users = new();
            
            for (int i = 0; i < userIds.Count; i++)
            {
                ApplicationUser user = userManager.Users.First(u => u.Id == userIds[i]);
                users.Add(user);
            }
            return users;
        }

        public UserProject Create(UserProject userProject)
        {
            UserProjects.Add(userProject);
            return userProject;
        }

        public UserProject Update(UserProject userProject)
        {
            int index = UserProjects.FindIndex(u => u.Id == userProject.Id);
            UserProjects[index] = userProject;
            return userProject;
        }

        public UserProject Delete(string userId, string projectId)
        {
            UserProject? userProject = UserProjects.Where(u => u.UserId == userId && u.ProjectId == projectId).FirstOrDefault();
            UserProjects.Remove(userProject);  
            return userProject;
        }        
    }
}
