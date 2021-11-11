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
    [Authorize]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }      

        [HttpGet]
        public IActionResult CreateRole()
        {
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
                    return Redirect("/admin/ListRoles");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(role);
        }

        [HttpGet]
        public async Task<IActionResult> ListRoles()
        {     
            IQueryable<IdentityRole> roles = roleManager.Roles;
            List<RoleViewModel> roleListModel = new();

            foreach (IdentityRole role in roles)
            {
                RoleViewModel roleModel = new();

                foreach (IdentityUser user in userManager.Users)
                {
                    bool isInRole = await userManager.IsInRoleAsync(user, role.Name);

                    if (isInRole)
                    {
                        roleModel.Users.Add(user.UserName);
                    }
                }

                roleListModel.Add(new RoleViewModel
                {
                    Id = role.Id,
                    Name = role.Name,   
                    Users = roleModel.Users
                });                
            }            
            return View(roleListModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id {id} cannot be found";
                return View("NotFound");
            }

            RoleViewModel roleModel = new()
            {
                Id = role.Id,
                Name = role.Name,                
            };

            foreach (IdentityUser user in userManager.Users)
            {
                bool isInRole = await userManager.IsInRoleAsync(user, role.Name);

                if (isInRole)
                {
                    roleModel.Users.Add(user.UserName);
                }                   
            }
            return Json(roleModel);
        }
    }
}
