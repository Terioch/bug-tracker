using BugTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using BugTracker.Helpers;
using BugTracker.Repositories.Interfaces;

namespace BugTracker.Controllers
{
    [Authorize(Roles = "Admin, Project Manager, Developer, Submitter")]
    public class TicketAttachmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly TicketAttachmentHelper _attachmentHelper;

        public TicketAttachmentController(IUnitOfWork unitOfWork, IWebHostEnvironment hostingEnvironment, TicketAttachmentHelper attachmentHelper)
        {
            _unitOfWork = unitOfWork;
            _hostingEnvironment = hostingEnvironment;
            _attachmentHelper = attachmentHelper;
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _unitOfWork.UserManager.GetUserAsync(HttpContext.User);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(string ticketId, string attachmentName, IFormFile fileAttachment)
        {            
            if (ModelState.IsValid)
            {
                if (!_attachmentHelper.IsValidAttachment(fileAttachment.FileName))
                {
                    TempData["Error"] = "The attachment you attempted to upload is invalid";
                    return RedirectToAction("Details", "Ticket", new { id = ticketId });
                }

                var submitter = await GetCurrentUserAsync();              
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "attachments");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + fileAttachment.FileName;
                string completeFilePath = Path.Combine(uploadsFolder, uniqueFileName);
                
                // Open file stream and upload attachment
                var stream = new FileStream(completeFilePath, FileMode.Create);
                await fileAttachment.CopyToAsync(stream);
                stream.Close();

                var attachment = new TicketAttachment()
                {
                    Id = Guid.NewGuid().ToString(),
                    TicketId = ticketId,
                    SubmitterId = submitter.Id,
                    Name = attachmentName,
                    FilePath = uniqueFileName,
                    CreatedAt = DateTimeOffset.UtcNow,
                };

                _unitOfWork.TicketAttachments.Add(attachment);

                await _unitOfWork.Complete();

                return RedirectToAction("Details", "Ticket", new { id = ticketId });
            }           

            TempData["Error"] = string.Join(" ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));

            return RedirectToAction("Details", "Ticket", new { id = ticketId });
        }
        
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var attachment = await _unitOfWork.TicketAttachments.Get(id);

            if (attachment == null)
            {
                return NotFound();
            }

            return View(attachment);
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(TicketAttachment model, IFormFile fileAttachment)
        {
            var attachment = await _unitOfWork.TicketAttachments.Get(model.Id);

            if (attachment == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (!_attachmentHelper.IsValidAttachment(fileAttachment.FileName))
                {
                    TempData["Error"] = "The attachment you attempted to upload is invalid";
                    return RedirectToAction("Details", "Ticket", new { id = attachment.TicketId });
                }

                // Remove current attachment from uploads
                _attachmentHelper.RemoveUploadedFileAttachment(attachment);

                // Update attachment with new values
                attachment.Name = model.Name;
                attachment.FilePath = Guid.NewGuid().ToString() + "_" + fileAttachment.FileName;

                // Upload and save new attachment
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Attachments");
                string updatedCompleteFilePath = Path.Combine(uploadsFolder, attachment.FilePath);
                var stream = new FileStream(updatedCompleteFilePath, FileMode.Create);

                await fileAttachment.CopyToAsync(stream);
                stream.Close();

                await _unitOfWork.Complete();
                                                                             
                return RedirectToAction("Details", "Ticket", new { id = attachment.TicketId });
            }

            return View(attachment);
        }
        
        public async Task<IActionResult> Delete(string id) 
        {
            var attachment = await _unitOfWork.TicketAttachments.Get(id);

            if (attachment == null)
            {
                return NotFound();
            }
            
            _attachmentHelper.RemoveUploadedFileAttachment(attachment);

            _unitOfWork.TicketAttachments.Delete(attachment);

            await _unitOfWork.Complete();

            return RedirectToAction("Details", "Ticket", new { id = attachment.TicketId });
        }
    }
}
