using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceManagementApp.Data;
using ServiceManagementApp.Data.Enums;
using ServiceManagementApp.Data.Models;
using System.Diagnostics;

namespace ServiceManagementApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var today = DateTime.Today;

            // Извличане на данни за графиките
            var newRequestsToday = _context.ServiceRequests
                .Where(r => r.Status == ServiceRequestStatus.New && r.RequestDate >= today)
                .Count();

            var completedRequestsToday = _context.ServiceRequests
                .Where(r => r.Status == ServiceRequestStatus.Completed && r.RequestDate >= today)
                .Count();

            var inProgressRequestsToday = _context.ServiceRequests
                .Where(r => r.Status == ServiceRequestStatus.InProgress && r.RequestDate >= today)
                .Count();

            var onHoldRequestsToday = _context.ServiceRequests
                .Where(r => r.Status == ServiceRequestStatus.OnHold && r.RequestDate >= today)
                .Count();

            // Предаване на данните към View-то
            ViewBag.NewRequests = newRequestsToday;
            ViewBag.CompletedRequests = completedRequestsToday;
            ViewBag.InProgressRequests = inProgressRequestsToday;
            ViewBag.OnHoldRequests = onHoldRequestsToday;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
