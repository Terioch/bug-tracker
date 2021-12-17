using BugTracker.Models;

namespace BugTracker.Services
{
    public interface IUserProjectRepository
    {
        public List<Project> GetProjectsByUserId(string userId);
        public List<ApplicationUser> GetUsersByProjectId(string projectId);
        public UserProject Create(UserProject userProject);
        public UserProject Update(UserProject userProject);
        public UserProject Delete(string userId, string projectId);
    }
}
