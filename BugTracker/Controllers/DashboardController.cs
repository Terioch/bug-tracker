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
        private readonly ProjectHelper projectHelper;
        private readonly ITicketRepository ticketRepo;
        private readonly TicketHistoryHelper ticketHistoryHelper;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly TicketHelper ticketHelper;        
        private readonly ChartHelper chartHelper;

        public DashboardController(ProjectHelper projectHelper, TicketHelper ticketHelper, TicketHistoryHelper ticketHistoryHelper, 
            UserManager<ApplicationUser> userManager, ChartHelper chartHelper)
        {
            this.ticketHelper = ticketHelper;
            this.projectHelper = projectHelper;           
            this.ticketHistoryHelper = ticketHistoryHelper;
            this.userManager = userManager;
            this.chartHelper = chartHelper;
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
