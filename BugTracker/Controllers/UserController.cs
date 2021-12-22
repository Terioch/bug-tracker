﻿using Microsoft.AspNetCore.Mvc;
using BugTracker.Models;
using BugTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using X.PagedList;

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

        public IActionResult ListUsers(int? page)
        {
            /*List<UserProjectViewModel> users = new();

            foreach (var user in userManager.Users)
            {
                List<Project> projects = repository.GetProjectsByUserId(user.Id);               

                users.Add(new()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Projects = projects,
                });
            }*/
            IPagedList<ApplicationUser> users = userManager.Users.ToPagedList(page ?? 1, 8);
            return View(users);
        }

        public IActionResult Details(string id)
        {
            ApplicationUser? user = userManager.Users.FirstOrDefault(u => u.Id == id);
            return View(user);
        }
    }
}
