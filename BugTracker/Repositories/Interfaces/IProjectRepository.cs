using BugTracker.Models;
using System;

namespace BugTracker.Repositories.Interfaces
{
    public interface IProjectRepository : IRepository<Project>
    {
        void AddUser(ApplicationUser user, Project project);

        void DeleteUser(ApplicationUser user, Project project);
    }
}
