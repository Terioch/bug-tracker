using BugTracker.Models;
using System;

namespace BugTracker.Repositories.Interfaces
{
    public interface IProjectRepository
    {
        IEnumerable<Project> GetAllProjects();
        Project GetProjectById(string id);
        Project GetProjectByName(string name);
        Project Create(Project project);
        Project Update(Project project);
        Project Delete(string id);
    }
}
