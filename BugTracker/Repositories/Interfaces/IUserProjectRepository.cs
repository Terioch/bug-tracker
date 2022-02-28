using BugTracker.Models;

namespace BugTracker.Repositories.Interfaces
{
    public interface IUserProjectRepository
    {
        public IEnumerable<Project> GetProjectsByUserId(string userId);
        public IEnumerable<ApplicationUser> GetUsersByProjectId(string projectId);
        public UserProject Create(UserProject userProject);
        public UserProject Update(UserProject userProject);
        public UserProject Delete(string userId, string projectId);
    }
}
