﻿using Microsoft.AspNetCore.Mvc;
using ServiceManagementApp.Data;
using ServiceManagementApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceManagementApp.Data.Models.Core;
using ServiceManagementApp.Data.Models.ServiceModels;
using ServiceManagementApp.Data.Enums;

namespace ServiceManagementApp.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(ApplicationDbContext context, ILogger<EmployeesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var employees = _context.Employees
            .Include(e => e.Service)
            .Include(e => e.PhoneNumber)
            .Include(e => e.EmailAddress)
            .Select(e => new EmployeeViewModel
            {
                Id = e.Id,
                FullName = e.FullName,
                ServiceId = e.ServiceId,
                ServiceName = e.Service.ServiceName,
                Position = e.Position,
                PhoneNumber = e.PhoneNumber.PhoneNumber!,
                EmailAddress = e.EmailAddress.EmailAddress!,
                IsCertifiedForCashRegisterRepair = e.IsCertifiedForCashRegisterRepair,
                EGN = e.EGN!
                //PictureUrl = e.PictureUrl
            }).ToList();

            return View(employees);
        }
        [HttpGet]
        public IActionResult Create()
        {



            var services = _context.Services.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.ServiceName
            }).ToList();

            if (!services.Any())
            {
                services.Add(new SelectListItem
                {
                    Value = "",
                    Text = "No services available"
                });
            }

            var positions = Enum.GetValues(typeof(Position))
                                .Cast<Position>()
                                .Select(p => new SelectListItem
                                {
                                    Value = ((int)p).ToString(),
                                    Text = p.ToString()
                                }).ToList();

            ViewBag.Services = new SelectList(services, "Value", "Text");
            ViewBag.Positions = new SelectList(positions, "Value", "Text");

            return View(new EmployeeViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogError(error.ErrorMessage);
                }
                return View(model); // Връщане на формата със съобщения за грешки
            }
            if (ModelState.IsValid)
            {
                var phone = new Phone { PhoneNumber = model.PhoneNumber };
                var email = new Email { EmailAddress = model.EmailAddress };

                _context.Phones.Add(phone);
                _context.Emails.Add(email);
                await _context.SaveChangesAsync();


                var employee = new Employee
                {
                    FullName = model.FullName,
                    ServiceId = model.ServiceId,
                    Position = model.Position,
                    PhoneId = phone.Id,
                    EmailId = email.Id,
                    IsCertifiedForCashRegisterRepair = model.IsCertifiedForCashRegisterRepair,
                    EGN = model.EGN,
                };

                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Services = new SelectList(_context.Services, "Id", "ServiceName");
            ViewBag.Positions = Enum.GetValues(typeof(Position)).Cast<Position>().Select(p => new SelectListItem { Value = ((int)p).ToString(), Text = p.ToString() });

            return View(model);
        }
    }
}
