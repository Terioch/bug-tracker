using BugTracker.Contexts;
using BugTracker.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BugTracker.Repositories.EF
{
    public class EF_Repository<T> : IRepository<T> where T : class, new()
    {
        private readonly ApplicationDbContext _db;

        public EF_Repository(ApplicationDbContext db)
        {
            _db = db;
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _db.Set<T>();
        }

        public virtual async Task<T> Get(string id)
        {
            return await _db.Set<T>().FindAsync(id);
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _db.Set<T>().Where(predicate);
        }

        public void Add(T entity)
        {
            _db.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _db.Set<T>().Remove(entity);
        }
        
        public void DeleteRange(IEnumerable<T> entities)
        {
            _db.Set<T>().RemoveRange(entities);
        }
    }
}
