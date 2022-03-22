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
        private readonly TicketHistoryHelper ticketHistoryHelper;

        public DashboardController(TicketHistoryHelper ticketHistoryHelper)
        {          
            this.ticketHistoryHelper = ticketHistoryHelper;
        }

        public async Task<IActionResult> Index(int? historyPage)
        {
            IEnumerable<TicketHistoryRecord> historyRecords = await ticketHistoryHelper.GetUserRoleRecords();
            DashboardViewModel model = new()
            {                               
                TicketHistoryRecords = historyRecords.ToPagedList(historyPage ?? 1, 6),
            };
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
