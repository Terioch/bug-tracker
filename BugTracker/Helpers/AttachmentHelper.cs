

namespace BugTracker.Helpers
{
    public class TicketAttachmentHelper
    {
        public TicketAttachmentHelper()
        {

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
    }
}
