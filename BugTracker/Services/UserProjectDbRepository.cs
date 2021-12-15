using BugTracker.Data;
using BugTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BugTracker.Services
{
    public class UserProjectDbRepository : IUserProjectRepository
    {
        private readonly BugTrackerDbContext context;

        public UserProjectDbRepository(BugTrackerDbContext context)
        {
            this.context = context;
        }

        public List<string> GetUserProjects(string userId)
        {
            IEnumerable<UserProject> userProjects = context.UserProjects.Where(u => u.UserId == userId);
            List<string> projectIds = new();

            foreach (var userProject in userProjects)
            {
                projectIds.Add(userProject.ProjectId);
            }
            return projectIds;
        }

        public List<string> GetProjectUsers(string projectId)
        {
            IEnumerable<UserProject> userProjects = context.UserProjects.Where(u => u.ProjectId == projectId);
            List<string> userIds = new();

            foreach (var userProject in userProjects)
            {
                userIds.Add(userProject.UserId);
            }
            return userIds;
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
                throw new Exception("UserProject not found");
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
