using BugTracker.Models;

namespace BugTracker.Helpers
{
    public class TicketAttachmentHelper
    {
        private readonly IWebHostEnvironment hostingEnvironment;

        public TicketAttachmentHelper(IWebHostEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
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

        public void RemoveUploadedFileAttachment(TicketAttachment attachment)
        {
            string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "Attachments");
            string completeFilePath = Path.Combine(uploadsFolder, attachment.FilePath);
            FileInfo fileInfo = new(completeFilePath);
            File.Delete(completeFilePath);
            fileInfo.Delete();
        }        
    }
}
