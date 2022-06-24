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
        private readonly IUnitOfWork _unitOfWork;
        private readonly TicketHelper _ticketHelper;

        public TicketHistoryController(IUnitOfWork unitOfWork, TicketHelper ticketHelper)
        {
            _unitOfWork = unitOfWork;
            _ticketHelper = ticketHelper;
        }

        [HttpGet]
        public IActionResult FilterTicketHistoryReturnPartial(string ticketId, string? searchTerm)
        {       
            var records = _unitOfWork.TicketHistoryRecords.Find(r => r.TicketId == ticketId);
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
            var userRoleTickets = await _ticketHelper.GetUserRoleTickets();
            var userRoleRecords = userRoleTickets.SelectMany(t => t.TicketHistoryRecords ?? new List<TicketHistoryRecord>());

            if (searchTerm == null)
            {
                return PartialView("~/Views/Dashboard/_DashboardTicketHistoryList.cshtml", userRoleRecords.ToPagedList(1, 6));
            }

            var filteredRecords = new List<TicketHistoryRecord>();

            foreach (var record in userRoleRecords)
            {
                var ticket = await _unitOfWork.Tickets.Get(record.TicketId);

                if (ticket.Title.ToLowerInvariant().Contains(searchTerm) || record.Property.ToLowerInvariant().Contains(searchTerm))
                {
                    filteredRecords.Add(record);
                }
            }                       

            return PartialView("~/Views/Dashboard/_DashboardTicketHistoryList.cshtml", filteredRecords.ToPagedList(1, 6));
        }
    }
}
