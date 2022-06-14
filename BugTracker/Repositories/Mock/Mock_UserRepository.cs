using BugTracker.Models;
using BugTracker.Repositories.Interfaces;
using System.Linq.Expressions;

namespace BugTracker.Repositories.Mock
{
    public class Mock_UserRepository : IUserRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public Mock_UserRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Add(ApplicationUser entity)
        {
            throw new NotImplementedException();
        }

        public void AddProject(Project project, ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public void Delete(ApplicationUser entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteRange(IEnumerable<ApplicationUser> entities)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ApplicationUser> Find(Expression<Func<ApplicationUser, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> GetAsync(string id)
        {
            throw new NotImplementedException();
        }

        public void RemoveProject(Project project, ApplicationUser user)
        {
            throw new NotImplementedException();
        }
    }
}
