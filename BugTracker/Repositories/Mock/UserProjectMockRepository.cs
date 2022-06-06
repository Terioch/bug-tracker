using BugTracker.Contexts.Mock;
using BugTracker.Models;
using BugTracker.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

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

        internal static List<UserProject> UserProjects { get; set; } = MockUserProjects.GetUserProjects();

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

        public IEnumerable<ApplicationUser> Find(Expression<Func<UserProject, bool>> predicate)
        {
            var filteredUserProjects = UserProjects.AsQueryable().Where(predicate);
            var userIds = filteredUserProjects.Select(u => u.UserId).ToList();
            var users = new List<ApplicationUser>();
            
            for (int i = 0; i < userIds.Count; i++)
            {
                ApplicationUser user = userManager.Users.First(u => u.Id == userIds[i]);
                users.Add(user);
            }

            return users;
        }

        public void Add(UserProject userProject)
        {
            UserProjects.Add(userProject);
        }      

        public void Delete(UserProject userProject)
        {           
            UserProjects.Remove(userProject);  
        }        
    }
}
