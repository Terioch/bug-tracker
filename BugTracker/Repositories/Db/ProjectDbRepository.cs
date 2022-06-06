using BugTracker.Contexts;
using BugTracker.Models;
using BugTracker.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BugTracker.Repositories.Db
{
    public class ProjectDbRepository : DbRepository<Project>, IRepository<Project>
    {
        private readonly BugTrackerDbContext _db;

        public ProjectDbRepository(BugTrackerDbContext db) : base(db)
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
    }
}
