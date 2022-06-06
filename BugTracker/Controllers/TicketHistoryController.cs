using Microsoft.AspNetCore.Mvc;
using BugTracker.Models;
using X.PagedList;
using System.Dynamic;
using BugTracker.Repositories.Interfaces;
using BugTracker.Helpers;

namespace BugTracker.Controllers
{
    public class TicketHistoryController : Controller
    {
        private readonly ITicketHistoryRepository repo;
        private readonly ITicketRepository ticketRepo;
        private readonly TicketHistoryHelper helper;

        public TicketHistoryController(ITicketHistoryRepository repo, ITicketRepository ticketRepo, TicketHistoryHelper helper)
        {
            this.repo = repo;
            this.ticketRepo = ticketRepo;
            this.helper = helper;
        }

        [HttpGet]
        public IActionResult FilterTicketHistoryReturnPartial(string ticketId, string? searchTerm)
        {
            var records = repo.GetRecordsByTicketId(ticketId);
            dynamic dataObject = new ExpandoObject();
            dataObject.Id = ticketId;
            ViewBag.Data = dataObject;

            if (searchTerm == null)
            {
                return PartialView("_TicketHistoryList", records.ToPagedList(1, 5));
            }

            var filteredRecords = records.Where(r => 
                r.Property.ToLowerInvariant().Contains(searchTerm)
                || r.Modifier.UserName.ToLowerInvariant().Contains(searchTerm));
          
            return PartialView("_TicketHistoryList", filteredRecords.ToPagedList(1, 5));
        }

        [HttpGet]
        public async Task<IActionResult> FilterUserRoleTicketsHistoryReturnPartial(string? searchTerm)
        {
            var records = await helper.GetUserRoleRecords();           

            if (searchTerm == null)
            {
                return PartialView("~/Views/Dashboard/_DashboardTicketHistoryList.cshtml", records.ToPagedList(1, 6));
            }

            var filteredRecords = records.Where(r =>
            {
                Ticket ticket = ticketRepo.Get(r.TicketId);
                return ticket.Title.ToLowerInvariant().Contains(searchTerm)
                || r.Property.ToLowerInvariant().Contains(searchTerm);
            });            

            return PartialView("~/Views/Dashboard/_DashboardTicketHistoryList.cshtml", filteredRecords.ToPagedList(1, 6));
        }
    }
}
