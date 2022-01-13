﻿using BugTracker.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BugTracker.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(SignInManager<ApplicationUser> signInManager)
        {
            this.signInManager = signInManager;
        }       

        public IList<AuthenticationScheme>? ExternalLogins { get; set; }

        public InputModel? Input { get; set; }

        public class InputModel
        {
            public string? UserName { get; set; }

            public string? Password { get; set; }
        }

        public IActionResult DisplayDemoLoginForm()
        {
            return View("DemoLoginForm");
        }

        public async Task<IActionResult> LoginWithDemoAccount(string role)
        {
            string returnUrl = "/Project/ListProjects";
            Input.Password = "7sH73jfdFr5!";

            switch (role)
            {
                case "Admin":
                    Input.UserName = "Demo Admin";
                    break;
                case "Project Manager":
                    Input.UserName = "Demo Project Manager";
                    break;
                case "Developer":
                    Input.UserName = "Demo Developer";
                    break;
                case "Submitter":
                    Input.UserName = "Demo Submitter";
                    break;
            }

            ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            var result = await signInManager.PasswordSignInAsync("Demo Admin", "7sH73jfdFr5!", false, lockoutOnFailure: false);

            if (result.Succeeded)
            {             
                return LocalRedirect(returnUrl);
            }
            if (result.RequiresTwoFactor)
            {
                return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = false });
            }
            if (result.IsLockedOut)
            {              
                return RedirectToPage("./Lockout");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return RedirectToAction("DisplayDemoLoginForm");
            }
            return View();
        }
    }
}
