using BugTracker.Models;

namespace BugTracker.Services.Mock
{
    public class ProjectMockRepository : IProjectRepository
    {
        static readonly List<Project> projects = new();       

        public Project Create(Project project)
        {
            projects.Add(project);
            return project;
        }       

        public IEnumerable<Project> GetAllProjects()
        {
            projects.Add(new Project
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Bug Tracker",
                Description = "A Bug/Issue Tracker MVC project."
            });
            projects.Add(new Project
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Technology Blog",
                Description = "A programming tutorial blog built using .Net Web API and React.JS."
            });
            projects.Add(new Project
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Demo Project 0",
                Description = "This is a demo project."
            });
            projects.Add(new Project
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Demo Project 1",
                Description = "This is a demo project."
            });
            return projects;
        }

        public Project GetProjectById(string id)
        {
            return projects.First(p => p.Id == id);
        }

        public Project GetProjectByName(string name)
        {
            throw new NotImplementedException();
        }

        public Project Update(Project project)
        {
            throw new NotImplementedException();
        }

        public Project Delete(string id)
        {
            throw new NotImplementedException();
        }
    }
}
