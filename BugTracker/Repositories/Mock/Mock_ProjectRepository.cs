using BugTracker.Contexts.Mock;
using BugTracker.Models;
using BugTracker.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Linq.Expressions;

namespace BugTracker.Repositories.Mock
{
    public class Mock_ProjectRepository : IProjectRepository
    { 
        private readonly IUnitOfWork _unitOfWork;

        public Mock_ProjectRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }              

        public IEnumerable<Project> GetAll()
        {
            return MockBugTrackerDbContext.Projects;
        }

        public Task<Project> Get(string id)
        {
            var project = MockBugTrackerDbContext.Projects.FirstOrDefault(p => p.Id == id);
            project.Tickets = MockBugTrackerDbContext.Tickets.Where(t => t.ProjectId == id).ToList();

            foreach (var t in project.Tickets)
            {
                t.AssignedDeveloper = _unitOfWork.UserManager.Users.FirstOrDefault(u => u.Id == t.AssignedDeveloperId);
                t.Submitter = _unitOfWork.UserManager.Users.First(u => u.Id == t.SubmitterId);
            }

            var userIds = MockBugTrackerDbContext.UserProjects.Where(up => up.ProjectId == id).Select(up => up.UserId);        
            project.Users = userIds.Select(uid => _unitOfWork.UserManager.Users.First(u => u.Id == uid)).ToList();

            return Task.FromResult(project);
        }

        public IEnumerable<Project> Find(Expression<Func<Project, bool>> predicate)
        {           
            return MockBugTrackerDbContext.Projects.AsQueryable().Where(predicate);
        }              

        public void Add(Project project)
        {
            MockBugTrackerDbContext.Projects.Add(project); 
        }

        public void AddUser(ApplicationUser user, Project project)
        {
            var userProject = new UserProject { Id = Guid.NewGuid().ToString(), UserId = user.Id, ProjectId = project.Id };
            MockBugTrackerDbContext.UserProjects.Add(userProject);
            project.Users.Add(user);
        }        

        public void Delete(Project project)
        {
            MockBugTrackerDbContext.Projects.Remove(project);
        }

        public void DeleteUser(ApplicationUser user, Project project)
        {
            var userProject = new UserProject { Id = Guid.NewGuid().ToString(), UserId = user.Id, ProjectId = project.Id };
            MockBugTrackerDbContext.UserProjects.Remove(userProject);
            project.Users.Remove(user);
        }

        public void DeleteRange(IEnumerable<Project> entities)
        {
            throw new NotImplementedException();
        }       
    }
}
