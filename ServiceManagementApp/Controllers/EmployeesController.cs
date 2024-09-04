using Microsoft.AspNetCore.Mvc;
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

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

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
            EGN = e.EGN!,
            PictureUrl = e.PictureUrl
        })
        .ToList();

            return View(employees);
        }

        [HttpPost]
        public async Task<IActionResult> PictureUpload(IFormFile picture)
        {
            if (picture != null && picture.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(picture.FileName);

                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var fileStream = new FileStream(uploadPath, FileMode.Create))
                {
                    await picture.CopyToAsync(fileStream);
                }

                return Ok(new { filePath = "/images/" + fileName });
            }

            return BadRequest("Файлът не е валиден.");
        }
        public IActionResult Create()
        {
            // Извличане на списъка с налични сервизи
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

            // Извличане на списъка с позиции (ENUM)
            var positions = Enum.GetValues(typeof(Position))
                                    .Cast<Position>()
                                    .Select(p => new SelectListItem
                                    {
                                        Value = ((int)p).ToString(),
                                        Text = p.ToString()
                                    }).ToList();

            ViewBag.Services = new SelectList(services, "Value", "Text"); // коректно подаване на списъка с сервизи
            ViewBag.Positions = new SelectList(positions, "Value", "Text"); // коректно подаване на списъка с позиции

            return View(new EmployeeViewModel()); // връщане на празен модел на изгледа
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel model, IFormFile PictureUpload)
        {
            if (ModelState.IsValid)
            {
                var phone = new Phone
                {
                    PhoneNumber = model.PhoneNumber
                };

                var email = new Email
                {
                    EmailAddress = model.EmailAddress
                };

                string pictureUrl = string.Empty;


                if (PictureUpload != null && PictureUpload.Length > 0)
                {
                    var filePath = Path.Combine("wwwroot/uploads", PictureUpload.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await PictureUpload.CopyToAsync(stream);
                    }
                }

                pictureUrl = "/uploads/" + PictureUpload.FileName;

                var employee = new Employee
                {
                    FullName = model.FullName,
                    ServiceId = model.ServiceId,
                    Position = model.Position,
                    PhoneNumber = phone,
                    EmailAddress = email,
                    IsCertifiedForCashRegisterRepair = model.IsCertifiedForCashRegisterRepair,
                    EGN = model.EGN,
                    PictureUrl = model.PictureUrl
                };

                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

                ViewBag.Services = new SelectList(_context.Services, "Id", "ServiceName");
                ViewBag.Positions = Enum.GetValues(typeof(Position)).Cast<Position>().Select(p => new SelectListItem { Value = ((int)p).ToString(), Text = p.ToString() });

                return View(model);
        }
        

            // GET: Employees/Edit/5
            public async Task<IActionResult> Edit(int id)
            {
                var employee = await _context.Employees
                    .Include(e => e.PhoneNumber)
                    .Include(e => e.EmailAddress)
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (employee == null)
                {
                    return NotFound();
                }

                var viewModel = new EmployeeViewModel
                {
                    Id = employee.Id,
                    FullName = employee.FullName,
                    ServiceId = employee.ServiceId,
                    Position = employee.Position,
                    EmailAddress = employee.EmailAddress.EmailAddress!,
                    PhoneNumber = employee.PhoneNumber.PhoneNumber!,
                    IsCertifiedForCashRegisterRepair = employee.IsCertifiedForCashRegisterRepair,
                    EGN = employee.EGN!,
                    PictureUrl = employee.PictureUrl
                };

                ViewBag.Services = new SelectList(_context.Services, "Id", "ServiceName", viewModel.ServiceId);
                ViewBag.Positions = Enum.GetValues(typeof(Position)).Cast<Position>().Select(p => new SelectListItem { Value = ((int)p).ToString(), Text = p.ToString() });

                return View(viewModel);
            }

            // POST: Employees/Edit/5
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(int id, EmployeeViewModel model)
            {
                if (id != model.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    var employee = await _context.Employees
                        .Include(e => e.PhoneNumber)
                        .Include(e => e.EmailAddress)
                        .FirstOrDefaultAsync(e => e.Id == id);

                    if (employee == null)
                    {
                        return NotFound();
                    }

                    employee.FullName = model.FullName;
                    employee.ServiceId = model.ServiceId;
                    employee.Position = model.Position;
                    employee.EmailAddress.EmailAddress = model.EmailAddress;
                    employee.PhoneNumber.PhoneNumber = model.PhoneNumber;
                    employee.IsCertifiedForCashRegisterRepair = model.IsCertifiedForCashRegisterRepair;
                    employee.EGN = model.EGN;
                    employee.PictureUrl = model.PictureUrl;

                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Services = new SelectList(_context.Services, "Id", "ServiceName", model.ServiceId);
                ViewBag.Positions = Enum.GetValues(typeof(Position)).Cast<Position>().Select(p => new SelectListItem { Value = ((int)p).ToString(), Text = p.ToString() });

                return View(model);
            }
        
    } 
}
