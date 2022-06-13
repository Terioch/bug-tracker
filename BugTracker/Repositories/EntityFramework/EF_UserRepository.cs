using BugTracker.Contexts;
using BugTracker.Models;
using BugTracker.Repositories.EF;
using BugTracker.Repositories.Interfaces;

namespace BugTracker.Repositories.EF
{
    public class EF_UserRepository : EF_Repository<ApplicationUser>, IUserRepository
    {
        private readonly BugTrackerDbContext _db;

        public EF_UserRepository(BugTrackerDbContext db) : base(db)
        {
            _db = db;
        }

        public void AddProject(Project project, ApplicationUser user)
        {
            user.Projects.Add(project);
        }

        public void RemoveProject(Project project, ApplicationUser user)
        {
            user.Projects.Remove(project);
        }
    }
}
