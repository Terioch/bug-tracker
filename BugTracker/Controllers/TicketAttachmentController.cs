using BugTracker.Models;
using BugTracker.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using System.IO;

namespace BugTracker.Controllers
{
    [Authorize(Roles = "Admin, Project Manager, Submitter")]
    public class TicketAttachmentController : Controller
    {
        private readonly ITicketAttachmentRepository repo;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment hostingEnvironment;

        public TicketAttachmentController(ITicketAttachmentRepository repo, UserManager<ApplicationUser> userManager, IWebHostEnvironment hostingEnvironment)
        {
            this.repo = repo;
            this.userManager = userManager;
            this.hostingEnvironment = hostingEnvironment;
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return userManager.GetUserAsync(HttpContext.User);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(string ticketId, string attachmentName, IFormFile fileAttachment)
        {            
            if (ModelState.IsValid)
            {
                ApplicationUser submitter = await GetCurrentUserAsync();
                string extension = Path.GetExtension(fileAttachment.FileName);
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + fileAttachment.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                if (extension == "jpg" || extension == "png" || extension == "jpeg")
                {                    
                    fileAttachment.CopyTo(new FileStream(filePath, FileMode.Create));
                }                              

                TicketAttachment attachment = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    TicketId = ticketId,
                    SubmitterId = submitter.Id,
                    Name = attachmentName,
                    FilePath = uniqueFileName,
                    CreatedAt = DateTimeOffset.Now,
                };                
                repo.Create(attachment);   
            }
            return RedirectToAction("Details", "Ticket", new { id = ticketId });
        }
        
        [HttpGet]
        public IActionResult Edit(string id)
        {
            TicketAttachment attachment = repo.GetAttachmentById(id);
            return View(attachment);
        }
        
        [HttpPost]
        public IActionResult Edit(TicketAttachment model)
        {
            TicketAttachment attachment = repo.GetAttachmentById(model.Id);   
            
            if (attachment == null)
            {
                ViewBag.ErrorMessage = $"Ticket Attachment with id { model.Id } could not found.";
                return View("NotFound");
            }
            
            if (ModelState.IsValid)
            {
                // Remove current uploaded attachment
                string? extension = Path.GetExtension(model.FilePath);
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "Images");
                string filePath = Path.Combine(uploadsFolder, attachment.FilePath);
                System.IO.File.Delete(filePath);

                if (extension == "jpg" || extension == "png" || extension == "jpeg")
                {
                    attachment.Name = model.Name;
                    attachment.FilePath = Guid.NewGuid().ToString() + "_" + model.FilePath;
                    repo.Update(attachment);
                }                
                return RedirectToAction("Details", "Ticket", new { id = attachment.TicketId });
            }
            return View(model);
        }
        
        public IActionResult Delete(string id) 
        {                   
            if (id == null)
            {
                ViewBag.ErrorMessage = $"Ticket Attachment with id { id ?? "null" } could not found.";
                return View("NotFound");
            }

            // Remove uploaded file

            TicketAttachment attachment = repo.Delete(id);
            return RedirectToAction("Details", "Ticket", new { id = attachment.TicketId });
        }
    }
}
