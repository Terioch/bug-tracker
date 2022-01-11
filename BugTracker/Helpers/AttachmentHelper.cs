

namespace BugTracker.Helpers
{
    public class AttachmentHelper
    {
        public AttachmentHelper()
        {

        }

        public bool IsValidAttachment(string filePath)
        {
            string extension = Path.GetExtension(filePath);
            bool isValid = false;

            if (extension == "png" || extension == "jpg" || extension == "jpeg")
            {
                isValid = true;
            }
            return isValid;
        }
    }
}
