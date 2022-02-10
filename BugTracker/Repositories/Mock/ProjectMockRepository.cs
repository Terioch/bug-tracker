using BugTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Repositories.Mock
{
    public class ProjectMockRepository : IProjectRepository
    {
        public ProjectMockRepository()
        {                      
        }

        private static readonly List<Project> projects = new()
        {
            new Project
            {
                Id = "p1",
                Name = "Bug Tracker",
                Description = "A Bug/Issue Tracker MVC project.",                
            },
            new Project
            {
                Id = "p2",
                Name = "Technology Blog",
                Description = "A programming tutorial blog built using .Net Web API and React.JS."
            },
            new Project
            {
                Id = "p3",
                Name = "Demo Project 1",
                Description = "This is a demo project."
            },
            new Project
            {
                Id = "p4",
                Name = "Demo Project 2",
                Description = "This is a demo project."
            }
        };       

        public IEnumerable<Project> GetAllProjects()
        {            
            return projects;           
        }       

        public Project GetProjectById(string id)
        {            
            Project project = projects.First(p => p.Id == id);
            // project.Tickets = ticketRepo.GetTicketsByProjectId(project.Id);
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
