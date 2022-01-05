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
    // [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ListRoles()
        {
            IQueryable<IdentityRole> roles = roleManager.Roles;
            UserRoleViewModel model = new();
            int roleIndex = 0;

            foreach (var role in roles)
            {
                List<string> users = new();

                foreach (var user in userManager.Users)
                {
                    bool isInRole = await userManager.IsInRoleAsync(user, role.Name);

                    if (isInRole)
                    {
                        users.Add(user.UserName);
                    }
                }

                model.Roles.Add(new RoleViewModel
                {
                    Id = role.Id,
                    Name = role.Name,
                    Index = roleIndex,
                    Users = users
                });
                roleIndex++;
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CreateRole()
        {
            RoleViewModel roleModel = new();
            var role = roleManager.Roles.ToList()[3];
            foreach (var user in userManager.Users)
            {
                bool isInRole = await userManager.IsInRoleAsync(user, role.Name);

                if (isInRole)
                {
                    roleModel.Users.Add($"{user.FirstName} {user.LastName}");
                }
            }
            Console.WriteLine(roleModel.Users);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel role)
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
                        Index = roleIndex,
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

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            return View(role);
        }

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

        //[HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);          

            IdentityResult result = await roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                RoleViewModel model = new()
                {
                    Id = id,
                    Name = role.Name,
                };

                foreach (var user in userManager.Users)
                {
                    bool isInRole = await userManager.IsInRoleAsync(user, role.Name);

                    if (isInRole)
                    {
                        model.Users.Add(user.UserName);
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

        [HttpPost]
        public async Task<IActionResult> AddUser(UserRoleViewModel model)
        {
            IdentityRole role = await roleManager.FindByIdAsync(model.RoleId);            

            ApplicationUser user = await userManager.FindByIdAsync(model.UserId);
            bool isInRole = await userManager.IsInRoleAsync(user, role.Name);         

            if (!isInRole)
            {
                IdentityResult result = await userManager.AddToRoleAsync(user, role.Name);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }                
            }
            return RedirectToAction("ListRoles");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUser(UserRoleViewModel model)
        {
            IdentityRole role = await roleManager.FindByIdAsync(model.RoleId);           

            ApplicationUser user = await userManager.FindByIdAsync(model.UserId);  
            bool isInRole = await userManager.IsInRoleAsync(user, role.Name);            

            if (isInRole)
            {
                IdentityResult result = await userManager.RemoveFromRoleAsync(user, role.Name);               

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return RedirectToAction("ListRoles");
        }
    }
}
