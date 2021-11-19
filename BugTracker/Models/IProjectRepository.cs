using System;

namespace BugTracker.Models
{
    public interface IProjectRepository
    {
        IEnumerable<ProjectModel> GetAllProjects();
        ProjectModel GetProject(string id);
        ProjectModel Create(ProjectModel project);
        ProjectModel Update(ProjectModel project);
        ProjectModel Delete(string id);
    }
}
