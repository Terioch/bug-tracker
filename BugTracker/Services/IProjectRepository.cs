using BugTracker.Models;
using System;

namespace BugTracker.Services
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
