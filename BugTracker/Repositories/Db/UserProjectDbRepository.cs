using BugTracker.Contexts;
using BugTracker.Models;
using BugTracker.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BugTracker.Repositories.Db
{
    public class UserProjectDbRepository : IUserProjectRepository
    {
        private readonly BugTrackerDbContext context;
        private readonly IProjectRepository projectRepo;
        private readonly UserManager<ApplicationUser> userManager;

        public UserProjectDbRepository(BugTrackerDbContext context, IProjectRepository projectRepo, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.projectRepo = projectRepo;
            this.userManager = userManager;
        }

        public IEnumerable<Project> GetProjectsByUserId(string userId)
        {
            IEnumerable<UserProject> userProjects = context.UserProjects.Where(u => u.UserId == userId);
            var projectIds = userProjects.Select(u => u.ProjectId).ToList();
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
            IEnumerable<UserProject> userProjects = context.UserProjects.Where(u => u.ProjectId == projectId);
            var userIds = userProjects.Select(u => u.UserId).ToList();
            List<ApplicationUser> users = new();

            for (int i = 0; i < userIds.Count; i++)
            {
                ApplicationUser user = userManager.Users.First(u => u.Id == userIds[i]);
                users.Add(user);
            }
            return users;
        }

        public UserProject Add(UserProject userProject)
        {
            context.UserProjects.Add(userProject);
            context.SaveChanges();
            return userProject;
        }

        public UserProject Update(UserProject userProject)
        {
            EntityEntry attachedUserProject = context.UserProjects.Attach(userProject);
            attachedUserProject.State = EntityState.Modified;
            context.SaveChanges();
            return userProject;
        }

        public UserProject Delete(string userId, string projectId)
        {
            UserProject? userProject = context.UserProjects.Where(u => u.UserId == userId && u.ProjectId == projectId).FirstOrDefault();

            if (userProject == null)
            {
                return new UserProject();
            }

            context.UserProjects.Remove(userProject);
            context.SaveChanges();
            return userProject;
        }                     
    }
}
