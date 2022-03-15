using BugTracker.Contexts.Mock;
using BugTracker.Models;
using BugTracker.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Repositories.Mock
{
    public class ProjectMockRepository : IProjectRepository
    {
        private readonly UserManager<ApplicationUser> userManager;

        public ProjectMockRepository(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        private static readonly List<Project> projects = MockProjects.GetProjects();      

        public IEnumerable<Project> GetAllProjects()
        {            
            return projects;           
        }       

        public Project GetProjectById(string id)
        {            
            Project? project = projects.Find(p => p.Id == id);
            project.Tickets = MockTickets.GetTickets().Where(t => t.ProjectId == id).ToList();
            var userIds = MockUserProjects.GetUserProjects().Where(up => up.ProjectId == id).Select(up => up.UserId);
            project.Users = userIds.Select(uid => userManager.Users.First(u => u.Id == uid)).ToList();

            /*project.Users = userManager.Users
                .Where(u => MockUserProjects.GetUserProjects()
                .Where(up => up.ProjectId == id)
                .Select(up => up.UserId)
                .Any(uid => uid == u.Id))
                .ToList();*/
            return project;
        }

        public Project GetProjectByName(string name)
        {
            return projects.First(p => p.Name == name);
        }

        public Project Create(Project project)
        {
            projects.Add(project);
            return project;
        }

        public Project Update(Project project)
        {
            int index = projects.FindIndex(p => p.Id == project.Id);
            projects[index] = project;
            return project;
        }

        public Project Delete(string id)
        {
            Project project = projects.First(p => p.Id == id);
            projects.Remove(project);
            return project;
        }
    }
}
