using Microsoft.AspNetCore.Mvc;
using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace BugTracker.Controllers
{        
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> ListRoles()
        {
            var roles = roleManager.Roles.ToList();        
            UserRoleViewModel model = new()
            {
                Users = userManager.Users.ToList()
            };          
            
            for (int i = 0; i < roles.Count; i++) 
            {
                List<string> usersInRole = new();

                for (int j = 0; j < model.Users.Count; j++)
                {
                    bool isInRole = await userManager.IsInRoleAsync(model.Users[j], roles[i].Name);

                    if (isInRole)
                    {
                        usersInRole.Add(model.Users[j].UserName);
                    }
                }

                model.Roles.Add(new RoleViewModel
                {
                    Id = roles[i].Id,
                    Name = roles[i].Name,                   
                    Users = usersInRole,
                });              
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentRoleReturnPartial(int id)
        {
            IQueryable<IdentityRole> roles = roleManager.Roles;
            RoleViewModel newRole = new();
            int roleIndex = id;
            int pointerIndex = 0;

            // Check that index is within a valid range
            if (roleIndex < 0) roleIndex = roles.Count() - 1;
            else if (roleIndex >= roles.Count()) roleIndex = 0;

            foreach (var role in roles)
            {
                if (pointerIndex == roleIndex)
                {
                    newRole = new()
                    {
                        Id = role.Id,
                        Name = role.Name,
                    };

                    foreach (var user in userManager.Users)
                    {
                        bool isInRole = await userManager.IsInRoleAsync(user, role.Name);

                        if (isInRole)
                        {
                            newRole.Users.Add(user.UserName);
                        }
                    }
                }
                pointerIndex++;
            }
            return PartialView("_roleCard", newRole);
        }

        [Authorize(Roles = "Owner")]
        [HttpGet]
        public IActionResult Create()
        {            
            return View();
        }

        [Authorize(Roles = "Owner")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleViewModel role)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new()
                {
                    Name = role.Name                    
                };

                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(role);
        }                       

        [Authorize(Roles = "Owner")]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            return View(role);
        }

        [Authorize(Roles = "Owner")]
        [HttpPost]
        public async Task<IActionResult> Edit(IdentityRole model)
        {            
            IdentityRole role = await roleManager.FindByIdAsync(model.Id);

            if (ModelState.IsValid)
            {
                role.Name = model.Name;

                IdentityResult result = await roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return RedirectToAction("ListRoles");
            }            
            return View(role);
        }

        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            List<ApplicationUser> users = userManager.Users.ToList();
            IdentityResult result = await roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                RoleViewModel model = new()
                {
                    Id = id,
                    Name = role.Name,
                };

                for (int i = 0; i < users.Count; i++)
                {  
                    if (await userManager.IsInRoleAsync(users[i], role.Name))
                    {
                        model.Users.Add(users[i].UserName);
                    }
                }

                return RedirectToAction("ListRoles");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return RedirectToAction("ListRoles");
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            IQueryable<ApplicationUser> users = userManager.Users;
            return Json(users);
        }

        [Authorize(Roles = "Owner")]
        [HttpPost]
        public async Task<IActionResult> AddUser(UserRoleViewModel model)
        {        
            IdentityRole role = await roleManager.FindByIdAsync(model.RoleId);            
            ApplicationUser user = await userManager.FindByIdAsync(model.UserId);
            bool isInRole = await userManager.IsInRoleAsync(user, role.Name);                     

            if (isInRole)
            {
                TempData["Error"] = "The user you're attempting to add is already assigned to this role";
            }
        
            IdentityResult result = await userManager.AddToRoleAsync(user, role.Name);

            if (result.Succeeded)
            {
                return RedirectToAction("ListRoles");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }                            

            return RedirectToAction("ListRoles");
        }

        [Authorize(Roles = "Owner")]
        [HttpPost]
        public async Task<IActionResult> RemoveUser(UserRoleViewModel model)
        {
            IdentityRole role = await roleManager.FindByIdAsync(model.RoleId);           
            ApplicationUser user = await userManager.FindByIdAsync(model.UserId);  
            bool isInRole = await userManager.IsInRoleAsync(user, role.Name);

            if (!isInRole)
            {
                TempData["Error"] = "The user you're attempting to remove is not assigned to this role";
            }
            
            IdentityResult result = await userManager.RemoveFromRoleAsync(user, role.Name);               

            if (result.Succeeded)
            {
                return RedirectToAction("ListRoles");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            
            return RedirectToAction("ListRoles");
        }
    }
}
