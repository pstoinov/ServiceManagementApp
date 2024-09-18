using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServiceManagementApp.Data;
using ServiceManagementApp.Data.Enums;
using ServiceManagementApp.Data.Models.RepairModels;
using ServiceManagementApp.Data.Models.RequestModels;
using ServiceManagementApp.ViewModels;

namespace ServiceManagementApp.Controllers
{
    public class RepairsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RepairsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var repairs = _context.Repairs
                .Where(r => r.Status != ServiceRequestStatus.Completed && r.Status != ServiceRequestStatus.Cancelled)
                .Select(r => new RepairViewModel
                {
                    Id = r.Id,
                    ClientName = r.Client.FullName,
                    EmployeeName = r.Employee.FullName,
                    StartRepairDate = r.StartRepairDate,
                    Status = r.Status,
                    ProblemDescription = r.ProblemDescription,
                    RepairCost = r.RepairCost
                })
                .ToList();

            return View(repairs);
        }

        public IActionResult Create()
        {
            // Взимане на заявки със статус New
            var serviceRequests = _context.ServiceRequests
                .Where(r => r.Status == ServiceRequestStatus.New)
                .Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.RequestNumber + " - " + r.ProblemDescription
                })
                .ToList();

            // Взимане на списък с техници
            var employees = _context.Employees
                .Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.FullName
                })
                .ToList();

            ViewBag.ServiceRequests = serviceRequests;
            ViewBag.Employees = employees;
            PopulateStatusDropdown();
            return View(new RepairViewModel
            {
                StartRepairDate = DateTime.Now,
                Status = ServiceRequestStatus.InProgress
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RepairViewModel model)
        {
            if (ModelState.IsValid)
            {
                var serviceRequest = await _context.ServiceRequests.FindAsync(model.ServiceRequestId);
                if (serviceRequest != null)
                {
                    serviceRequest.Status = ServiceRequestStatus.InProgress;
                    _context.ServiceRequests.Update(serviceRequest);
                }

                var repair = new Repair
                {
                    StartRepairDate = DateTime.Now,
                    //ProblemDescription = serviceRequest.ProblemDescription,
                    //RepairDescription = model.RepairDescription,
                    ClientId = serviceRequest.ClientId,
                    EmployeeId = model.EmployeeId,
                    //RepairCost = model.RepairCost,
                    Status = model.Status,
                    ServiceRequestId = serviceRequest.Id
                };

                _context.Repairs.Add(repair);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            ViewBag.ServiceRequests = _context.ServiceRequests
                .Where(r => r.Status == ServiceRequestStatus.New)
                .Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.RequestNumber + " - " + r.ProblemDescription
                })
                .ToList();

            ViewBag.Employees = _context.Employees
                .Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.FullName
                })
                .ToList();
            PopulateStatusDropdown();

            return View(model);
        }

        


        public IActionResult History()
        {
            return View();
        }
        private void PopulateStatusDropdown()
        {
            ViewBag.Statuses = Enum.GetValues(typeof(ServiceRequestStatus))
                                   .Cast<ServiceRequestStatus>()
                                   .Select(s => new SelectListItem
                                   {
                                       Value = ((int)s).ToString(),
                                       Text = s.ToString(),
                                       Selected = s == ServiceRequestStatus.InProgress
                                   })
                                   .ToList();
        }

    }
}
