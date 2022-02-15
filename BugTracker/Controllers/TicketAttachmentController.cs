using BugTracker.Models;
using BugTracker.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using BugTracker.Helpers;

namespace BugTracker.Controllers
{
    [Authorize(Roles = "Admin, Project Manager, Submitter")]
    public class TicketAttachmentController : Controller
    {
        private readonly ITicketAttachmentRepository repo;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly TicketAttachmentHelper attachmentHelper;

        public TicketAttachmentController(ITicketAttachmentRepository repo, UserManager<ApplicationUser> userManager, IWebHostEnvironment hostingEnvironment, TicketAttachmentHelper attachmentHelper)
        {
            this.repo = repo;
            this.userManager = userManager;
            this.hostingEnvironment = hostingEnvironment;
            this.attachmentHelper = attachmentHelper;
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
                if (!attachmentHelper.IsValidAttachment(fileAttachment.FileName))
                {
                    TempData["Error"] = "The attachment you attempted to upload is invalid";
                    return RedirectToAction("Details", "Ticket", new { id = ticketId });
                }

                ApplicationUser submitter = await GetCurrentUserAsync();              
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "Attachments");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + fileAttachment.FileName;
                string completeFilePath = Path.Combine(uploadsFolder, uniqueFileName);
                
                // Open file stream and upload attachment
                FileStream stream = new(completeFilePath, FileMode.Create);
                await fileAttachment.CopyToAsync(stream);
                stream.Close();

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
        public async Task<IActionResult> Edit(TicketAttachment model, IFormFile fileAttachment)
        {
            TicketAttachment attachment = repo.GetAttachmentById(model.Id);             
            
            if (ModelState.IsValid)
            {
                if (!attachmentHelper.IsValidAttachment(fileAttachment.FileName))
                {
                    TempData["Error"] = "The attachment you attempted to upload is invalid";
                    return RedirectToAction("Details", "Ticket", new { id = attachment.TicketId });
                }

                // Remove currently uploaded attachment                
                attachmentHelper.RemoveUploadedFileAttachment(attachment);

                // Update attachment with new values
                attachment.Name = model.Name;
                attachment.FilePath = Guid.NewGuid().ToString() + "_" + fileAttachment.FileName;

                // Upload and save new attachment
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "Attachments");
                string updatedCompleteFilePath = Path.Combine(uploadsFolder, attachment.FilePath);
                FileStream stream = new(updatedCompleteFilePath, FileMode.Create);
                await fileAttachment.CopyToAsync(stream);
                stream.Close();
                repo.Update(attachment);                                                               
                return RedirectToAction("Details", "Ticket", new { id = attachment.TicketId });
            }
            return View(attachment);
        }
        
        public IActionResult Delete(string id) 
        {                               
            TicketAttachment attachment = repo.Delete(id);
            attachmentHelper.RemoveUploadedFileAttachment(attachment);
            return RedirectToAction("Details", "Ticket", new { id = attachment.TicketId });
        }
    }
}
