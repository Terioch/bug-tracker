using System;

namespace BugTracker.Models
{
    public interface IProjectRepository
    {
        IEnumerable<Project> GetAllProjects();
        Project GetProject(string id);
        Project Create(Project project);
        Project Update(Project project);
        Project Delete(string id);
    }
}
