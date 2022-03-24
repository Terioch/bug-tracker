using BugTracker.Contexts;
using BugTracker.Helpers;
using BugTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace BugTracker.Controllers
{
    public class ChartController : Controller
    {
        private readonly TicketHelper ticketHelper;

        public ChartController(TicketHelper ticketHelper)
        {
            this.ticketHelper = ticketHelper;
        }

        public async Task<JsonResult> GetTicketTypeData()
        {
            IEnumerable<Ticket> tickets = await ticketHelper.GetUserRoleTickets();
            ChartData chartDataSet = new();

            foreach (string type in TicketContext.Types)
            {
                chartDataSet.Labels.Add(type);
                chartDataSet.Values.Add(tickets.Count(t => t.Type == type));
            };
            return Json(chartDataSet);
        }

        public async Task<JsonResult> GetTicketStatusData()
        {
            IEnumerable<Ticket> tickets = await ticketHelper.GetUserRoleTickets();
            ChartData chartDataSet = new();

            foreach (string status in TicketContext.Statuses)
            {
                chartDataSet.Labels.Add(status);
                chartDataSet.Values.Add(tickets.Count(t => t.Status == status));
            }
            return Json(chartDataSet);
        }

        public async Task<JsonResult> GetTicketPriorityData()
        {
            IEnumerable<Ticket> tickets = await ticketHelper.GetUserRoleTickets();
            ChartData chartDataSet = new();

            foreach (string priority in TicketContext.Priorities)
            {
                chartDataSet.Labels.Add(priority);
                chartDataSet.Values.Add(tickets.Count(t => t.Priority == priority));
            }
            return Json(chartDataSet);
        }
    }
}
