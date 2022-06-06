using BugTracker.Models;
using System;

namespace BugTracker.Repositories.Interfaces
{
    public interface IProjectRepository : IRepository<Project>
    {      
        Project GetByName(string name);      
    }
}
