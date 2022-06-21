using BugTracker.Models;
using BugTracker.Repositories.Interfaces;
using System.Security.Claims;

namespace BugTracker.Helpers
{
    public class TicketAttachmentHelper
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TicketAttachmentHelper(IUnitOfWork unitOfWork, IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsValidAttachment(string filePath)
        {
            string extension = Path.GetExtension(filePath);
            bool isValid = false;

            if (extension == ".png" || extension == ".jpg" || extension == ".jpeg")
            {
                isValid = true;
            }

            return isValid;
        }

        public async Task<bool> IsAuthorizedToManage(string id, ApplicationUser? user = null)
        {
            user ??= await _unitOfWork.Users.Get(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var roles = await _unitOfWork.UserManager.GetRolesAsync(user);
            var attachment = await _unitOfWork.TicketAttachments.Get(id);

            if (roles.Contains("Admin"))
            {
                return true;
            }
            else if (roles.Contains("Project Manager"))
            {
                return user.Projects
                    .SelectMany(p => p.Tickets)
                    .SelectMany(t => t.TicketAttachments ?? new List<TicketAttachment>())
                    .Any(a => a.Id == id);
            }
            else if (roles.Contains("Submitter") || roles.Contains("Developer"))
            {
                return user.Id == attachment.SubmitterId;
            }

            return false;
        }

        public void RemoveUploadedFileAttachment(TicketAttachment attachment)
        {
            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Attachments");
            string completeFilePath = Path.Combine(uploadsFolder, attachment.FilePath);
            var fileInfo = new FileInfo(completeFilePath);
            File.Delete(completeFilePath);
            fileInfo.Delete();
        }
        
        public void UploadMockAttachments()
        {
            string[] fileAttachmentPaths = new string[] { "academic.jpg", "bookcase-books-bookshelves-256541.jpg", "code-desk.jpg", "countryside-house.jpg", "node-network.jpg" };
            foreach (string filePath in fileAttachmentPaths)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Attachments");
                string completeFilePath = Path.Combine(uploadsFolder, filePath);
                // var stream = new FileStream(completeFilePath, FileMode.Create);
                // await fileAttachment.CopyToAsync(stream);
                var fileInfo = new FileInfo(completeFilePath);
                fileInfo.CopyTo(completeFilePath); 
                // stream.Close();
            }            
        }
    }
}
