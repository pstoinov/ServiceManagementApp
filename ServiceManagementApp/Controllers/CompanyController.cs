using Microsoft.AspNetCore.Mvc;
using ServiceManagementApp.Data.Models.ClientModels;
using ServiceManagementApp.Data.Models.Core;
using ServiceManagementApp.Data;
using ServiceManagementApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ServiceManagementApp.Controllers
{
    [Authorize]
    public class CompanyController : Controller
    {

        private readonly ApplicationDbContext _context;

        public CompanyController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult Index()
        {
            var companies = _context.Companies
                .Include(c => c.Address)
                .Select(c => new CompanyViewModel
                {
                    Id = c.Id,
                    CompanyName = c.CompanyName,
                    EIK = c.EIK,
                    VATNumber = c.VATNumber,
                    Manager = c.Manager,
                    City = c.Address != null ? c.Address.City : string.Empty,
                    Street = c.Address != null ? c.Address.Street : string.Empty,
                    Number = c.Address != null ? c.Address.Number : string.Empty,
                    Block = c.Address != null ? c.Address.Block : string.Empty,
                    Phone = c.Phone != null ? c.Phone.PhoneNumber : string.Empty,
                    Email = c.Email != null ? c.Email.EmailAddress : string.Empty
                }).ToList();

            return View(companies);
            

        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CompanyViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if company with the same EIK exists
                if (_context.Companies.Any(c => c.EIK == model.EIK))
                {
                    ModelState.AddModelError("EIK", "A company with this EIK already exists.");
                    return View(model);
                }

                var address = new Address
                {
                    City = model.City,
                    Street = model.Street,
                    Number = model.Number,
                    Block = model.Block
                };

                var company = new Company
                {
                    CompanyName = model.CompanyName,
                    EIK = model.EIK,
                    VATNumber = model.VATNumber ?? string.Empty,
                    Manager = model.Manager,
                    Address = address,
                    PhoneId = model.PhoneId,
                    EmailId = model.EmailId
                };

                _context.Addresses.Add(address);
                _context.Companies.Add(company);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }


    }
}