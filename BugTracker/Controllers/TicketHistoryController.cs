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
        private readonly TicketHistoryHelper _historyHelper;

        public TicketHistoryController(IUnitOfWork unitOfWork, TicketHistoryHelper helper)
        {
            _unitOfWork = unitOfWork;
            _historyHelper = helper;
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
            var records = await _historyHelper.GetUserRoleRecords();           

            if (searchTerm == null)
            {
                return PartialView("~/Views/Dashboard/_DashboardTicketHistoryList.cshtml", records.ToPagedList(1, 6));
            }

            var filteredRecords = new List<TicketHistoryRecord>();

            foreach (var record in records)
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
