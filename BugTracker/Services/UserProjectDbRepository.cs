using BugTracker.Data;
using BugTracker.Models;

namespace BugTracker.Services
{
    public class UserProjectDbRepository : IUserProjectRepository
    {
        private readonly BugTrackerDbContext context;

        public UserProjectDbRepository(BugTrackerDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<string> GetAllUserProjects(string userId)
        {
            throw new NotImplementedException();
        }

        public string CreateUserProject(string userId, string projectId)
        {
            throw new NotImplementedException();
        }

        public string DeleteUserProject(string userId)
        {
            throw new NotImplementedException();
        }        

        public string UpdateUserProject(string userId, string projectId)
        {
            throw new NotImplementedException();
        }
    }
}
