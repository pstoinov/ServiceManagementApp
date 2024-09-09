using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceManagementApp.Data;
using ServiceManagementApp.Data.Models.Core;
using ServiceManagementApp.Data.Models.ServiceModels;
using ServiceManagementApp.ViewModels;

namespace ServiceManagementApp.Controllers
{
    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var services = _context.Services
                .Include(s => s.Address)
                .Include(s => s.Phone)
                .Include(s => s.Email)
                .Select(s => new ServiceViewModel
                {
                    Id = s.Id,
                    ServiceName = s.ServiceName,
                    EIK = s.EIK,
                    VATNumber = s.VATNumber,
                    PhoneNumber = s.Phone.PhoneNumber!,
                    EmailAddress = s.Email.EmailAddress!
                }).ToList();

            return View(services);
        }
        public IActionResult Create()
        {
            return View();
        }

        // POST: Services/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceViewModel model)
        {
            if (ModelState.IsValid)
            {
                var address = new Address
                {
                    City = model.City,
                    Street = model.Street,
                    Number = model.Number,
                    Block = model.Block
                };

                var phone = new Phone
                {
                    PhoneNumber = model.PhoneNumber
                };

                var email = new Email
                {
                    EmailAddress = model.EmailAddress
                };

                var service = new Service
                {
                    ServiceName = model.ServiceName,
                    EIK = model.EIK,
                    VATNumber = model.VATNumber,
                    Address = address,
                    Phone = phone,
                    Email = email,
                    LogoUrl = model.LogoUrl,
                    IsCashRegisterService = model.IsCashRegisterService
                };

                _context.Add(service);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Services/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var service = await _context.Services
                .Include(s => s.Phone)
                .Include(s => s.Email)
                .Include(s => s.Address)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (service == null)
            {
                return NotFound();
            }

            var viewModel = new ServiceViewModel
            {
                Id = service.Id,
                ServiceName = service.ServiceName,
                EIK = service.EIK,
                VATNumber = service.VATNumber,
                City = service.Address.City ?? string.Empty,
                Street = service.Address.Street ?? string.Empty,
                Number = service.Address.Number ?? string.Empty,
                Block = service.Address.Block,
                PhoneNumber = service.Phone.PhoneNumber!,
                EmailAddress = service.Email.EmailAddress!,
                LogoUrl = service.LogoUrl,
                IsCashRegisterService = service.IsCashRegisterService
            };

            return View(viewModel);
        }

        // POST: Services/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ServiceViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }



            if (ModelState.IsValid)
            {
                var service = await _context.Services
                    .Include(s => s.Phone)
                    .Include(s => s.Email)
                    .Include(s => s.Address)
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (service == null)
                {
                    return NotFound();
                }

                service.ServiceName = model.ServiceName;
                service.EIK = model.EIK;
                service.VATNumber = model.VATNumber;
                service.Address.City = model.City;
                service.Address.Street = model.Street;
                service.Address.Number = model.Number;
                service.Address.Block = model.Block;
                service.Phone.PhoneNumber = model.PhoneNumber;
                service.Email.EmailAddress = model.EmailAddress;
                service.LogoUrl = model.LogoUrl;
                service.IsCashRegisterService = model.IsCashRegisterService;

                _context.Update(service);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
