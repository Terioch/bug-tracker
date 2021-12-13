using BugTracker.Models;

namespace BugTracker.Services
{
    public interface IUserProjectRepository
    {
        public IEnumerable<string> GetAllUserProjects(string userId);
        public string CreateUserProject(string userId, string projectId);
        public string UpdateUserProject(string userId, string projectId);
        public string DeleteUserProject(string userId);
    }
}
