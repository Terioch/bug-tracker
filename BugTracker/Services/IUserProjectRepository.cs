using BugTracker.Models;

namespace BugTracker.Services
{
    public interface IUserProjectRepository
    {
        public List<string> GetUserProjects(string userId);
        public List<string> GetProjectUsers(string projectId);
        public UserProject CreateUserProject(UserProject userProject);
        public UserProject UpdateUserProject(UserProject userProject);
        public UserProject DeleteUserProject(string userId);
    }
}
