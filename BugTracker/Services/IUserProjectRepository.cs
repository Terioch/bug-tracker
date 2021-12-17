using BugTracker.Models;

namespace BugTracker.Services
{
    public interface IUserProjectRepository
    {
        public List<Project> GetUserProjects(string userId);
        public List<ApplicationUser> GetProjectUsers(string projectId);
        public UserProject Create(UserProject userProject);
        public UserProject Update(UserProject userProject);
        public UserProject Delete(string userId, string projectId);
    }
}
