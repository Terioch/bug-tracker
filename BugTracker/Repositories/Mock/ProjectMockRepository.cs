using BugTracker.Contexts.Mock;
using BugTracker.Models;
using BugTracker.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Linq.Expressions;

namespace BugTracker.Repositories.Mock
{
    public class ProjectMockRepository : IRepository<Project>
    {
        private readonly UserManager<ApplicationUser> userManager;

        public ProjectMockRepository(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        internal static List<Project> Projects { get; set; } = MockProjects.GetProjects();      

        public IEnumerable<Project> GetAll()
        {
            return Projects;
        }

        public Task<Project> Get(string id)
        {
            var project = Projects.FirstOrDefault(p => p.Id == id);
            var allTickets = TicketMockRepository.Tickets;
            project.Tickets = allTickets.Where(t => t.ProjectId == id).ToList();

            foreach (var t in project.Tickets)
            {
                t.AssignedDeveloper = userManager.Users.FirstOrDefault(u => u.Id == t.AssignedDeveloperId);
                t.Submitter = userManager.Users.First(u => u.Id == t.SubmitterId);
            }

            var userIds = UserProjectMockRepository.UserProjects.Where(up => up.ProjectId == id).Select(up => up.UserId);
            project.Users = userIds.Select(uid => userManager.Users.First(u => u.Id == uid)).ToList();

            return Task.FromResult(project);
        }

        public IEnumerable<Project> Find(Expression<Func<Project, bool>> predicate)
        {           
            return Projects.AsQueryable().Where(predicate);
        }              

        public void Add(Project project)
        {
            Projects.Add(project);
        }      

        public void Delete(Project project)
        {           
            Projects.Remove(project);
        }
    }
}
