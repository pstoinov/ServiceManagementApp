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

        [HttpGet]
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
                    ContactPhone = c.ContactPhone.PhoneNumber,
                    SerialNumber = c.SerialNumber
                }).ToList();

            return View(cashRegisters);
        }

        [HttpGet]
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
                if (cashRegisterViewModel.ContactPhoneId == 0 && !string.IsNullOrEmpty(cashRegisterViewModel.PhoneSearch))
                {
                    var newPhone = new Phone
                    {
                        PhoneNumber = cashRegisterViewModel.PhoneSearch
                    };
                    _context.Phones.Add(newPhone);
                    await _context.SaveChangesAsync();

                    cashRegisterViewModel.ContactPhoneId = newPhone.Id; // TODO: Създаването на нов телефон не работи, работи само функцията за търсене в базата с данни
                }

                var existingAddress = _context.Addresses
           .FirstOrDefault(a => a.City == cashRegisterViewModel.City && a.Street == cashRegisterViewModel.Street &&
                                a.Number == cashRegisterViewModel.Number && a.Block == cashRegisterViewModel.Block);

                int addressId;
                if (existingAddress != null)
                {
                    addressId = existingAddress.Id;
                }
                else
                {
                    // Ако не съществува, създаваме нов запис за адрес
                    var newAddress = new Address
                    {
                        City = cashRegisterViewModel.City,
                        Street = cashRegisterViewModel.Street,
                        Number = cashRegisterViewModel.Number,
                        Block = cashRegisterViewModel.Block
                    };

                    _context.Addresses.Add(newAddress);
                    await _context.SaveChangesAsync();
                    addressId = newAddress.Id;
                }


                var cashRegister = new CashRegister
                {
                    ServiceId = cashRegisterViewModel.ServiceId,
                    CompanyId = cashRegisterViewModel.CompanyId,
                    SiteName = cashRegisterViewModel.SiteName,
                    SiteManager = cashRegisterViewModel.SiteManager,
                    SiteAddressId = addressId,
                    ContactPhoneId = cashRegisterViewModel.ContactPhoneId,
                    RegionalNRAOffice = cashRegisterViewModel.RegionalNRAOffice,
                    Manufacturer = cashRegisterViewModel.Manufacturer,
                    BIMCertificateNumber = cashRegisterViewModel.BIMCertificateNumber,
                    BIMCertificateDate = cashRegisterViewModel.BIMCertificateDate,
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

        [HttpGet]
        [Route("SearchCashRegisters")]
        public async Task<JsonResult> SearchCashRegisters(string term)
        {
            //if (string.IsNullOrWhiteSpace(term))
            //{
            //    return Json(new List<object>());  // Връщаме празен резултат
            //}
            var cashRegisters = await _context.CashRegisters
                .Where(cr => cr.SerialNumber.Contains(term))
                .Select(cr => new { id = cr.Id, serialNumber = cr.SerialNumber })
                .ToListAsync();

            return Json(cashRegisters);
        }

    }
}
