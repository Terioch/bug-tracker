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

        public IEnumerable<ProjectModel> GetAllProjects()
        {            
            return context.Projects;
        }

        public ProjectModel GetProject(string id)
        {
            ProjectModel? project = context.Projects.Find(id);
            
            if (project == null)
            {
                throw new Exception("Project Not Found");
            }
            return project;
        }

        public ProjectModel Create(ProjectModel project)
        {
            context.Projects.Add(project);
            context.SaveChanges();
            return project;
        }

        public ProjectModel Delete(string id)
        {
            ProjectModel? project = context.Projects.Find(id);

            if (project == null)
            {
                throw new Exception("Project Not Found");
            }         

            context.Projects.Remove(project);
            context.SaveChanges();
            return project;
        }        

        public ProjectModel Update(ProjectModel project)
        {
            EntityEntry<ProjectModel> attachedProject = context.Projects.Attach(project);
            attachedProject.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return project;
        }
    }
}
