using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceManagementApp.Data;
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

        // Създай и другите действия като Create, Edit, Delete
    }
}
