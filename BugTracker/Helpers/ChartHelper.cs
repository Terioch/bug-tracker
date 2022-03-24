using BugTracker.Contexts;
using BugTracker.Helpers;
using BugTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace BugTracker.Helpers
{
    public class ChartHelper
    {
        private readonly TicketHelper ticketHelper;

        public ChartHelper(TicketHelper ticketHelper)
        {
            this.ticketHelper = ticketHelper;
        }

        public ChartData GetTicketTypeData(IEnumerable<Ticket> tickets)
        {
            ChartData chartDataSet = new();            

            foreach (string type in TicketContext.Types)
            {
                chartDataSet.Labels.Add(type);
                chartDataSet.Values.Add(tickets.Count(t => t.Type == type));
            };
            return chartDataSet;
        }

        public ChartData GetTicketStatusData(IEnumerable<Ticket> tickets)
        {
            ChartData chartDataSet = new();

            foreach (string status in TicketContext.Statuses)
            {
                chartDataSet.Labels.Add(status);
                chartDataSet.Values.Add(tickets.Count(t => t.Status == status));
            }
            return chartDataSet;
        }

        public ChartData GetTicketPriorityData(IEnumerable<Ticket> tickets)
        {
            ChartData chartDataSet = new();

            foreach (string priority in TicketContext.Priorities)
            {
                chartDataSet.Labels.Add(priority);
                chartDataSet.Values.Add(tickets.Count(t => t.Priority == priority));
            }
            return chartDataSet;
        }
    }
}
