using Microsoft.AspNetCore.Mvc;
using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using BugTracker.Areas.Identity.Data;

namespace BugTracker.Controllers
{
    [Authorize]
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
                    return Redirect("/role/ListRoles");
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

            foreach (var role in roles)
            {
                RoleViewModel roleModel = new();

                foreach (var user in userManager.Users)
                {
                    bool isInRole = await userManager.IsInRoleAsync(user, role.Name);

                    if (isInRole)
                    {
                        roleModel.Users.Add($"{user.FirstName} {user.LastName}");
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
        public IActionResult GetCurrentRoleReturnPartial(int pageIndex)
        {
            IQueryable<IdentityRole> roles = roleManager.Roles;
            IdentityRole currentRole = new();

            // TODO: Get current role based on the page index

            return PartialView("_roleCard");
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] RoleViewModel model)
        {
            IdentityRole role = await roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id {model.Id} cannot be found";
                return View("NotFound");
            }

            role.Name = model.Name;
            IdentityResult result = await roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                return Json(model);
            } 
           
            List<IdentityError> errors = new();

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);                
                errors.Add(error);                
            }
            
            return Json(new { errors });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id {id} cannot be found";
                return View("NotFound");
            }

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
                        model.Users.Add($"{user.FirstName} {user.LastName}");
                    }
                }

                return PartialView("_RoleCard", model);
            }

            List<IdentityError> errors = new();

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
                errors.Add(error);
            }

            return Content(errors.ToString());
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            IQueryable<ApplicationUser> users = userManager.Users;
            return Json(users);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserRoleViewModel model)
        {
            IdentityRole role = await roleManager.FindByIdAsync(model.RoleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id {model.RoleId} cannot be found";
                return View("NotFound");
            }

            ApplicationUser user = await userManager.FindByIdAsync(model.UserId);
            bool isInRole = await userManager.IsInRoleAsync(user, role.Name);           

            if (!isInRole)
            {
                IdentityResult result = await userManager.AddToRoleAsync(user, role.Name);

                if (result.Succeeded)
                {
                    return Json(model.RoleId);
                }

                List<IdentityError> errors = new();

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    errors.Add(error);
                }

                return Json(errors);
            }
            return Json(model);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveUser([FromBody] UserRoleViewModel model)
        {
            IdentityRole role = await roleManager.FindByIdAsync(model.RoleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id {model.RoleId} cannot be found";
                return View("NotFound");
            }

            ApplicationUser user = await userManager.FindByIdAsync(model.UserId);
            bool isInRole = await userManager.IsInRoleAsync(user, role.Name);

            if (isInRole)
            {
                IdentityResult result = await userManager.RemoveFromRoleAsync(user, role.Name);

                if (result.Succeeded)
                {
                    return Json(model.RoleId);
                }

                List<IdentityError> errors = new();

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    errors.Add(error);
                }

                return Json(errors);
            }

            return Json(model);
        }
    }
}
