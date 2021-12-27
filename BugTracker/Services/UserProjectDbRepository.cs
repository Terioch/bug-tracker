using BugTracker.Data;
using BugTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BugTracker.Services
{
    public class UserProjectDbRepository : IUserProjectRepository
    {
        private readonly BugTrackerDbContext context;
        private readonly IProjectRepository projectRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public UserProjectDbRepository(BugTrackerDbContext context, IProjectRepository projectRepository, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.projectRepository = projectRepository;
            this.userManager = userManager;
        }

        public List<Project> GetProjectsByUserId(string userId)
        {
            IEnumerable<UserProject> userProjects = context.UserProjects.Where(u => u.UserId == userId);
            List<Project> projects = new();

            foreach (var userProject in userProjects)
            {
                Project project = projectRepository.GetProjectById(userProject.ProjectId);
                projects.Add(project);
            }
            return projects;
        }

        public List<ApplicationUser> GetUsersByProjectId(string projectId)
        {
            IEnumerable<UserProject> userProjects = context.UserProjects.Where(u => u.ProjectId == projectId);
            List<ApplicationUser> users = new();

            foreach (var userProject in userProjects)
            {
                ApplicationUser user = userManager.Users.First(u => u.Id == userProject.UserId);
                users.Add(user);
            }
            return users;
        }

        public UserProject Create(UserProject userProject)
        {
            context.UserProjects.Add(userProject);
            context.SaveChanges();
            return userProject;
        }

        public UserProject Delete(string userId, string projectId)
        {
            UserProject userProject = context.UserProjects.Where(u => u.UserId == userId && u.ProjectId == projectId).FirstOrDefault();

            if (userProject == null)
            {
                return new UserProject();
            }

            context.UserProjects.Remove(userProject);
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
    }
}
