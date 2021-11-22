﻿using BugTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly IProjectRepository repository;

        public ProjectController(IProjectRepository repository)
        {
            this.repository = repository;
        }

        public IActionResult Index()
        {
            IEnumerable<Project> projects = repository.GetAllProjects();
            return View(projects);
        } 
        
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Project project)
        {
            project.Id = Guid.NewGuid().ToString();
            Project createdProject = repository.Create(project);
            return View("Index", createdProject);
        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            Project project = repository.GetProject(id);
            return View(project);
        }
    }
}
