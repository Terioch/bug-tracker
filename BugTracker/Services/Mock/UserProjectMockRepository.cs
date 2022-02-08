using BugTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BugTracker.Services.Mock
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

        private static List<UserProject> userProjects = new()
        {
            new UserProject()
            {
                Id = "up1",
                UserId = "cd448813-e865-49e8-933a-dff582b72509",
                ProjectId = "p1"
            },
            new UserProject()
            {
                Id = "up2",
                UserId = "2ae32131-606d-495c-81cf-86f38875f9a7",
                ProjectId = "p1"
            },
            new UserProject()
            {
                Id = "up3",
                UserId = "ccd193a8-b38b-4414-a318-f4da79c046ae",
                ProjectId = "p1"
            },
            new UserProject()
            {
                Id = "up4",
                UserId = "fb37911c-7ceb-42ff-afc3-24b3bd189d9c",
                ProjectId = "p2"
            },
            new UserProject()
            {
                Id = "up5",
                UserId = "cd448813-e865-49e8-933a-dff582b72509",
                ProjectId = "p2"
            },
        };       

        public List<Project> GetProjectsByUserId(string userId)
        {
            IEnumerable<UserProject> filteredUserProjects = userProjects.Where(u => u.UserId == userId);
            var projectIds = filteredUserProjects.Select(u => u.ProjectId).ToList();
            List<Project> projects = new();

            for (int i = 0; i < projectIds.Count; i++)
            {
                Project project = projectRepo.GetProjectById(projectIds[i]);
                projects.Add(project);
            }
            return projects;
        }

        public List<ApplicationUser> GetUsersByProjectId(string projectId)
        {
            IEnumerable<UserProject> filteredUserProjects = userProjects.Where(u => u.ProjectId == projectId);
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
            userProjects.Add(userProject);
            return userProject;
        }

        public UserProject Update(UserProject userProject)
        {
            int index = userProjects.FindIndex(u => u.Id == userProject.Id);
            userProjects[index] = userProject;
            return userProject;
        }

        public UserProject Delete(string userId, string projectId)
        {
            UserProject? userProject = userProjects.Where(u => u.UserId == userId && u.ProjectId == projectId).FirstOrDefault();
            userProjects.Remove(userProject);  
            return userProject;
        }        
    }
}
