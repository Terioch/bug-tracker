﻿using Microsoft.AspNetCore.Mvc;
using BugTracker.Areas.Identity.Data;
using BugTracker.Models;
using BugTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BugTracker.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserProjectRepository repository;
        private readonly IProjectRepository projectRepository;

        public UserController(UserManager<ApplicationUser> userManager, IUserProjectRepository repository, IProjectRepository projectRepository) 
        {
            this.userManager = userManager;
            this.repository = repository;
            this.projectRepository = projectRepository;
        }

        public IActionResult ListUserProjects()
        {
            List<UserProjectViewModel> users = new();

            foreach (var user in userManager.Users)
            {
                List<string> projectIds = repository.GetUserProjects(user.Id);
                List<string> projects = new();
                
                foreach (var id in projectIds)
                {
                    Project project = projectRepository.GetProjectById(id);
                    projects.Add(project.Name);
                }

                users.Add(new()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Projects = projects
                });
            }
            return View(users);
        }
    }
}
