using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceManagementApp.Data;
using ServiceManagementApp.Data.Enums;
using ServiceManagementApp.Data.Models.RequestModels;
using ServiceManagementApp.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceManagementApp.Controllers
{
    public class RequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET: Requests/Create
        public IActionResult Create()
        {
            PopulateDropdowns(); // Зареждаме ViewBag с енумите за dropdown менюта
            return View(new RequestViewModel());
        }

        // POST: Requests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RequestViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Запазване на заявката в базата данни
                var serviceRequest = new ServiceRequest
                {
                    ClientName = model.ClientName,
                    ClientPhone = model.ClientPhone,
                    RequestType = model.RequestType,  // Енум, получен директно от модела
                    Status = model.Status,  // Енум, получен директно от модела
                    Priority = model.Priority,  // Енум, получен директно от модела
                    ProblemDescription = model.ProblemDescription,
                    RequestDate = model.RequestDate
                };

                _context.ServiceRequests.Add(serviceRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateDropdowns(); // Ако моделът не е валиден, зареждаме dropdown менютата отново
            return View(model);
        }

        // Метод за попълване на dropdown менютата
        private void PopulateDropdowns()
        {
            ViewBag.RequestTypes = Enum.GetValues(typeof(ServiceRequestType))
                                      .Cast<ServiceRequestType>()
                                      .Select(e => new SelectListItem
                                      {
                                          Value = e.ToString(),  // Подаваме enum стойността като string
                                          Text = e == ServiceRequestType.OnSite ? "В сервиз" : "На адрес"
                                      }).ToList();

            ViewBag.Statuses = Enum.GetValues(typeof(ServiceRequestStatus))
                                   .Cast<ServiceRequestStatus>()
                                   .Select(e => new SelectListItem
                                   {
                                       Value = e.ToString(),  // Подаваме enum стойността като string
                                       Text = e switch
                                       {
                                           ServiceRequestStatus.New => "Нова",
                                           ServiceRequestStatus.InProgress => "В прогрес",
                                           ServiceRequestStatus.OnHold => "Задържана",
                                           ServiceRequestStatus.Completed => "Завършена",
                                           ServiceRequestStatus.Cancelled => "Отменена",
                                           _ => e.ToString()
                                       }
                                   }).ToList();

            ViewBag.Priorities = Enum.GetValues(typeof(ServiceRequestPriority))
                                     .Cast<ServiceRequestPriority>()
                                     .Select(e => new SelectListItem
                                     {
                                         Value = e.ToString(),  // Подаваме enum стойността като string
                                         Text = e switch
                                         {
                                             ServiceRequestPriority.Low => "Нисък",
                                             ServiceRequestPriority.Medium => "Среден",
                                             ServiceRequestPriority.High => "Висок",
                                             _ => e.ToString()
                                         }
                                     }).ToList();
        }
    }
}