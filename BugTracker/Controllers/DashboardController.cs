using Microsoft.AspNetCore.Mvc;
using BugTracker.Models;
using BugTracker.Helpers;
using BugTracker.Contexts;

namespace BugTracker.Controllers
{
    public class DashboardController : Controller
    {
        private readonly TicketHelper ticketHelper;
        private readonly ChartHelper chartHelper;

        public DashboardController(TicketHelper ticketHelper, ChartHelper chartHelper)
        {
            this.ticketHelper = ticketHelper;
            this.chartHelper = chartHelper;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Ticket> tickets = await ticketHelper.GetUserRoleTickets();
            DashboardViewModel dashboardData = new()
            {
                TicketTypeData = chartHelper.GetTicketTypeData(tickets),
                TicketStatusData = chartHelper.GetTicketStatusData(tickets),
                TicketPriorityData = chartHelper.GetTicketPriorityData(tickets)
            };         
            return View(dashboardData);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
