using Microsoft.AspNetCore.Mvc;
using BugTracker.Models;
using BugTracker.Helpers;
using BugTracker.Contexts;
using BugTracker.Repositories.Interfaces;
using X.PagedList;
using Microsoft.AspNetCore.Identity;

namespace BugTracker.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ProjectHelper _projectHelper;
        private readonly TicketHelper _ticketHelper;
        private readonly TicketHistoryHelper _ticketHistoryHelper;        

        public DashboardController(ProjectHelper projectHelper, TicketHelper ticketHelper, TicketHistoryHelper ticketHistoryHelper)
        {
            _projectHelper = projectHelper;
            _ticketHelper = ticketHelper;
            _ticketHistoryHelper = ticketHistoryHelper;
        }

        public async Task<IActionResult> Index(int? historyPage)
        {            
            var userRoleProjects = await _projectHelper.GetUserRoleProjects();
            var userRoleTickets = await _ticketHelper.GetUserRoleTickets();
            var userRoleRecords = userRoleTickets.SelectMany(t => t.TicketHistoryRecords ?? new List<TicketHistoryRecord>());

            var model = new DashboardViewModel()
            {                               
                TicketHistoryRecords = userRoleRecords.Take(50).ToPagedList(historyPage ?? 1, 6),    
                UserRoleProjectCount = userRoleProjects.Count(),
                UserRoleTicketCount = userRoleTickets.Count(),
                UserCountOnUserRoleProjects = await _projectHelper.GetUsersInRolesCountOnUserRoleProjects(),
                DeveloperCountOnUserRoleProjects = await _projectHelper.GetUsersInRolesCountOnUserRoleProjects(new string[] { "Developer" })
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
