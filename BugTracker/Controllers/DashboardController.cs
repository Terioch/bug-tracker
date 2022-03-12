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
        private readonly ITicketHistoryRepository ticketHistoryRepo;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly TicketHelper ticketHelper;        
        private readonly ChartHelper chartHelper;

        public DashboardController(ProjectHelper projectHelper, TicketHelper ticketHelper, ITicketHistoryRepository ticketHistoryRepo, 
            UserManager<ApplicationUser> userManager, ChartHelper chartHelper)
        {
            this.ticketHelper = ticketHelper;
            this.projectHelper = projectHelper;           
            this.ticketHistoryRepo = ticketHistoryRepo;
            this.userManager = userManager;
            this.chartHelper = chartHelper;
        }

        public async Task<IActionResult> Index()
        {          
            DashboardViewModel model = new()
            {                               
                TicketHistoryRecords = ticketHistoryRepo.GetAllRecords().ToPagedList(),
            };
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
