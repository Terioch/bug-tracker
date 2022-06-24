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
            
            TempData["TicketId"] = ticketId;

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
            var userRoleRecords = userRoleTickets.SelectMany(t => t.TicketHistoryRecords ?? new List<TicketHistoryRecord>()).ToList();

            if (searchTerm == null)
            {
                return PartialView("~/Views/Dashboard/_DashboardTicketHistoryList.cshtml", userRoleRecords.ToPagedList(1, 6));
            }

            var filteredRecords = new List<TicketHistoryRecord>();

            for (int i = 0; i < userRoleRecords.Count; i++)
            {
                var ticket = await _unitOfWork.Tickets.Get(userRoleRecords[i].TicketId);

                if (ticket.Title.ToLowerInvariant().Contains(searchTerm) || userRoleRecords[i].Property.ToLowerInvariant().Contains(searchTerm))
                {
                    filteredRecords.Add(userRoleRecords[i]);
                }
            }                       

            return PartialView("~/Views/Dashboard/_DashboardTicketHistoryList.cshtml", filteredRecords.ToPagedList(1, 6));
        }
    }
}
