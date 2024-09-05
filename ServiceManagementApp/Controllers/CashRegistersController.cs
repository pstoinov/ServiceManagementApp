using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServiceManagementApp.Data;
using ServiceManagementApp.Data.Enums;
using ServiceManagementApp.Data.Models.ClientModels;
using ServiceManagementApp.Data.Models.Core;
using ServiceManagementApp.ViewModels;

namespace ServiceManagementApp.Controllers
{
    public class CashRegistersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CashRegistersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cashRegisters = _context.CashRegisters
                .Include(c => c.Company)
                .Include(c => c.SiteAddress)
                .Include(c => c.ContactPhone)
                .Select(c => new CashRegisterViewModel
                {
                    Id = c.Id,
                    CompanyName = c.Company.CompanyName,
                    SiteName = c.SiteName,
                    SiteAddress = $"{c.SiteAddress.City}, {c.SiteAddress.Street} {c.SiteAddress.Number}",
                    ContactPhone = c.ContactPhone.PhoneNumber!,
                    SerialNumber = c.SerialNumber
                }).ToList();

            return View(cashRegisters);
        }

        public IActionResult Create()
        {
            ViewBag.Services = GetServices();
            ViewBag.Manufacturers = GetManufacturers();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CashRegisterViewModel cashRegisterViewModel)
        {
            if (ModelState.IsValid)
            {
                // Проверяваме дали е избран телефон от autocomplete
                if (cashRegisterViewModel.ContactPhoneId == 0 && !string.IsNullOrEmpty(cashRegisterViewModel.PhoneSearch))
                {
                    // Ако няма съвпадение и е въведен нов номер, създаваме нов запис за телефон
                    var newPhone = new Phone
                    {
                        PhoneNumber = cashRegisterViewModel.PhoneSearch
                    };
                    _context.Phones.Add(newPhone);
                    await _context.SaveChangesAsync();

                    cashRegisterViewModel.ContactPhoneId = newPhone.Id; // Свързваме новия телефон с обекта
                }

                var cashRegister = new CashRegister
                {
                    ServiceId = cashRegisterViewModel.ServiceId,
                    CompanyId = cashRegisterViewModel.CompanyId,
                    SiteName = cashRegisterViewModel.SiteName,
                    SiteAddressId = cashRegisterViewModel.SiteAddressId,
                    ContactPhoneId = cashRegisterViewModel.ContactPhoneId,
                    RegionalNRAOffice = cashRegisterViewModel.RegionalNRAOffice,
                    Manufacturer = cashRegisterViewModel.Manufacturer,
                    BIMCertificateNumber = cashRegisterViewModel.BIMCertificateNumber,
                    SerialNumber = cashRegisterViewModel.SerialNumber,
                    FiscalMemoryNumber = cashRegisterViewModel.FiscalMemoryNumber,
                    FDRIDNumber = cashRegisterViewModel.FDRIDNumber,
                    FirstRegistrationDate = cashRegisterViewModel.FirstRegistrationDate,
                    IsDisposed = cashRegisterViewModel.IsDisposed,
                    IsRegistered = cashRegisterViewModel.IsRegistered
                };

                _context.Add(cashRegister);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cashRegisterViewModel);
        }

        private IEnumerable<SelectListItem> GetManufacturers()
        {
            return Enum.GetValues(typeof(Manufacturer))
                       .Cast<Manufacturer>()
                       .Select(m => new SelectListItem
                       {
                           Value = ((int)m).ToString(),
                           Text = m.ToString()
                       });
        }

        private IEnumerable<SelectListItem> GetServices()
        {
            return _context.Services
                       .Where(s => (bool)s.IsCashRegisterService)
                       .Select(s => new SelectListItem
                       {
                           Value = s.Id.ToString(), 
                           Text = s.ServiceName
                       }).ToList();
        }

       

    }
}
