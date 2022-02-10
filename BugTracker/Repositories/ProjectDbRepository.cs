using BugTracker.Data;
using BugTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;

namespace BugTracker.Repositories
{
    public class ProjectDbRepository : IProjectRepository
    {
        private readonly BugTrackerDbContext context;

        public ProjectDbRepository(BugTrackerDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Project> GetAllProjects()
        {
            return context.Projects
                .Include(p => p.Tickets)
                .Include(p => p.Users)
                .ToList() ?? new List<Project>();
            // return context.Projects.ToList() ?? new List<Project>();
        }

        public Project GetProjectById(string id)
        {
            return context.Projects               
                .Include(p => p.Tickets)
                    .ThenInclude(t => t.Submitter)
                .Include(p => p.Tickets)
                    .ThenInclude(t => t.AssignedDeveloper)
                 .Include(p => p.Users)
                .First(p => p.Id == id);
        }

        public Project GetProjectByName(string name)
        {
            return context.Projects        
                .Include(p => p.Tickets)
                    .ThenInclude(t => t.Submitter)
                .Include(p => p.Tickets)
                    .ThenInclude(t => t.AssignedDeveloper)
                .Include(p => p.Users)
                .First(p => p.Name == name);
        }

        public Project Create(Project project)
        {
            context.Projects.Add(project);
            context.SaveChanges();
            return project;
        }

        public Project Delete(string id)
        {
            Project? project = context.Projects.Find(id);

            if (project == null)
            {
                throw new NullReferenceException("Project Not Found");
            }         

            context.Projects.Remove(project);
            context.SaveChanges();
            return project;
        }        

        public Project Update(Project project)
        {
            EntityEntry<Project>? attachedProject = context.Projects.Attach(project);
            attachedProject.State = EntityState.Modified;
            context.SaveChanges();
            return project;
        }
    }
}
