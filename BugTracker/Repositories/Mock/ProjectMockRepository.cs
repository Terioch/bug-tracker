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

        public static List<Project> Projects { get; set; } = MockProjects.GetProjects();      

        public IEnumerable<Project> GetAllProjects()
        {            
            return Projects;           
        }       

        public Project GetProjectById(string id)
        {            
            Project? project = Projects.Find(p => p.Id == id);
            List<Ticket> allTickets = TicketMockRepository.Tickets;
            project.Tickets = allTickets.Where(t => t.ProjectId == id).ToList();
            foreach (var t in project.Tickets)
            {
                t.AssignedDeveloper = userManager.Users.FirstOrDefault(u => u.Id == t.AssignedDeveloperId);
                t.Submitter = userManager.Users.First(u => u.Id == t.SubmitterId);
            }
            var userIds = UserProjectMockRepository.UserProjects.Where(up => up.ProjectId == id).Select(up => up.UserId);
            project.Users = userIds.Select(uid => userManager.Users.First(u => u.Id == uid)).ToList();                           
            return project;
        }

        public Project GetProjectByName(string name)
        {
            return Projects.First(p => p.Name == name);            
        }

        public Project Create(Project project)
        {
            Projects.Add(project);
            return project;
        }

        public Project Update(Project project)
        {
            int index = Projects.FindIndex(p => p.Id == project.Id);
            Projects[index] = project;
            return project;
        }

        public Project Delete(string id)
        {
            Project project = Projects.First(p => p.Id == id);
            Projects.Remove(project);
            return project;
        }
    }
}
