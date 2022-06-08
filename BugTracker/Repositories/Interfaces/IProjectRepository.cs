using BugTracker.Models;
using System;

namespace BugTracker.Repositories.Interfaces
{
    public interface IProjectRepository : IRepository<Project>
    {
        void AddUser(ApplicationUser user, string projectId);

        void DeleteUser(ApplicationUser user, string projectId);
    }
}
