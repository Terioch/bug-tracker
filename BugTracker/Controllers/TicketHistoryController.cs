using Microsoft.AspNetCore.Mvc;
using BugTracker.Models;
using BugTracker.Repositories;
using X.PagedList;
using System.Dynamic;

namespace BugTracker.Controllers
{
    public class TicketHistoryController : Controller
    {
        private readonly ITicketHistoryRepository repo;

        public TicketHistoryController(ITicketHistoryRepository repo)
        {
            this.repo = repo;
        }

        public IActionResult FilterHistoryReturnPartial(string id, string? searchTerm)
        {
            IEnumerable<TicketHistoryRecord> records = repo.GetRecordsByTicketId(id);
            dynamic dataObject = new ExpandoObject();
            dataObject.Id = id;
            ViewBag.Data = dataObject;

            if (searchTerm == null)
            {
                return PartialView("_TicketHistoryList", records.ToPagedList(1, 5));
            }

            var filteredRecords = records.Where(c => 
                c.Property.ToLowerInvariant().Contains(searchTerm)
                || c.Modifier.UserName.ToLowerInvariant().Contains(searchTerm));
          
            return PartialView("_TicketHistoryList", filteredRecords.ToPagedList(1, 5));
        }
    }
}
