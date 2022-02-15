using BugTracker.Models;

namespace BugTracker.Helpers
{
    public class AccountHelper
    {
        public AccountHelper()
        {
        }

        public static bool IsAccountEditable(ApplicationUser user)
        {
            string[] demoUsers = new string[4] { "Demo Admin", "Demo Project Manager", "Demo Developer", "Demo Submitter" };
            return demoUsers.All(u => u != user.UserName);
        }
    }
}
