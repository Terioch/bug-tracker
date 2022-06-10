using System.Linq.Expressions;

namespace BugTracker.Repositories.Interfaces
{
    public interface IRepository<T> where T : class, new()
    {
        IEnumerable<T> GetAll();

        Task<T> GetAsync(string id);

        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        void Add(T entity);

        void Delete(T entity);

        void DeleteRange(IEnumerable<T> entities);
    }
}
