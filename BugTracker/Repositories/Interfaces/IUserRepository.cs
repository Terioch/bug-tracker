using BugTracker.Models;

namespace BugTracker.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        void AddProject(Project project, ApplicationUser user);

        void RemoveProject(Project project, ApplicationUser user);
    }
}
