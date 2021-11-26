using BugTracker.Data;
using BugTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;

namespace BugTracker.Services
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

        public Project GetProjectById(string id)
        {
            Project? project = context?.Projects?.Find(id);
            
            if (project == null)
            {
                throw new Exception("Project Not Found");
            }
            return project;
        }

        public Project GetProjectByName(string name)
        {
            Project? project = context?.Projects?.FirstOrDefault(p => p.Name == name);

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
            EntityEntry<Project> attachedProject = context?.Projects.Attach(project);
            attachedProject.State = EntityState.Modified;
            context.SaveChanges();
            return project;
        }
    }
}
