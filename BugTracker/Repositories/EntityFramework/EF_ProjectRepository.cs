using BugTracker.Contexts;
using BugTracker.Models;
using BugTracker.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BugTracker.Repositories.EF
{
    public class EF_ProjectRepository : EF_Repository<Project>, IProjectRepository
    {
        private readonly ApplicationDbContext _db;

        public EF_ProjectRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public override IEnumerable<Project> GetAll()
        {            
            return _db.Projects
                .Include(p => p.Tickets)
                .Include(p => p.Users);
        }

        public override async Task<Project> Get(string id)
        {
            return await _db.Projects               
                .Include(p => p.Tickets)
                    .ThenInclude(t => t.Submitter)
                .Include(p => p.Tickets)
                    .ThenInclude(t => t.AssignedDeveloper)
                 .Include(p => p.Users)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        
        public override IEnumerable<Project> Find(Expression<Func<Project, bool>> predicate)
        {
            return _db.Projects.Where(predicate).Include(p => p.Tickets)
                    .ThenInclude(t => t.Submitter)
                .Include(p => p.Tickets)
                    .ThenInclude(t => t.AssignedDeveloper)
                 .Include(p => p.Users);
        }

        public void AddUser(ApplicationUser user, Project project)
        {            
            project.Users.Add(user);
        }

        public void DeleteUser(ApplicationUser user, Project project)
        {            
            project.Users.Remove(user);
        }
    }
}
