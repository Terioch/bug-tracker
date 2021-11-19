using BugTracker.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;

namespace BugTracker.Models
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
            return context.Projects;
        }

        public Project GetProject(string id)
        {
            Project? project = context.Projects.Find(id);
            
            if (project == null)
            {
                throw new Exception("Project Not Found");
            }
            return project;
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
                throw new Exception("Project Not Found");
            }         

            context.Projects.Remove(project);
            context.SaveChanges();
            return project;
        }        

        public Project Update(Project project)
        {
            EntityEntry<Project> attachedProject = context.Projects.Attach(project);
            attachedProject.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return project;
        }
    }
}
