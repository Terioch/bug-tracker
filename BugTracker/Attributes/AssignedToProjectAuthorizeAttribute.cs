using Microsoft.AspNetCore.Mvc.Filters;

namespace BugTracker.Attributes
{
    public class AssignedToProjectAuthorizeAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
