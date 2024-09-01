using Microsoft.AspNetCore.Mvc;
using ServiceManagementApp.Data;
using ServiceManagementApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using ServiceManagementApp.Data.Models.ClientModels;
using ServiceManagementApp.Data.Models.Core;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceManagementApp.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompaniesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateCompanyViewModel model)
        {
            if (ModelState.IsValid)
            {
                var address = new Address
                {
                    City = model.City,
                    Street = model.Street,
                    Number = model.Number,
                    Block = model.Block,
                };

                var phone = new Phone
                {
                    PhoneNumber = model.PhoneNumber
                };

                var email = new Email
                {
                    EmailAddress = model.EmailAddress
                };

                var company = new Company
                {
                    CompanyName = model.CompanyName,
                    EIK = model.EIK,
                    VATNumber = model.VATNumber,
                    Manager = model.Manager,
                    Address = address,
                    Phone = phone,
                    Email = email
                };

                _context.Companies.Add(company);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public IActionResult AssociateClient(int companyId)
        {
            var company = _context.Companies.Find(companyId);

            if (company == null)
            {
                return NotFound();
            }

            var viewModel = new AssociateClientCompanyViewModel
            {
                CompanyId = companyId,
                CompanyName = company.CompanyName,
                Clients = _context.Clients.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.FullName
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AssociateClient(AssociateClientCompanyViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var existingAssociation = _context.ClientCompanies
                    .FirstOrDefault(cc => cc.ClientId == viewModel.SelectedClientId && cc.CompanyId == viewModel.CompanyId);

                if (existingAssociation == null)
                {
                    var clientCompany = new ClientCompany
                    {
                        ClientId = viewModel.SelectedClientId,
                        CompanyId = viewModel.CompanyId
                    };

                    _context.ClientCompanies.Add(clientCompany);
                    _context.SaveChanges();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "This client is already associated with the company.");
                }
            }

            viewModel.Clients = _context.Clients.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.FullName
            }).ToList();

            return View(viewModel);
        }

        public IActionResult Edit(int companyId)
        {
            var company = _context.Companies
                .Include(c => c.Phone)
                .Include(c => c.Email)
                .FirstOrDefault(c => c.Id == companyId);

            if (company == null)
            {
                return NotFound();
            }

            var viewModel = new EditCompanyViewModel
            {
                Id = company.Id,
                CompanyName = company.CompanyName,
                EIK = company.EIK,
                VATNumber = company.VATNumber,
                Phone = company.Phone.PhoneNumber!,
                Email = company.Email.EmailAddress!,
                // Populate other fields as needed
            };

            return View(viewModel);
        }

        public IActionResult Index()
        {
            var companies = _context.Companies
                .Include(c => c.Phone)
                .Include(c => c.Email)
                .Select(c => new CompanyViewModel
                {
                    Id = c.Id,
                    CompanyName = c.CompanyName,
                    EIK = c.EIK,
                    VATNumber = c.VATNumber ?? string.Empty,
                    Phone = c.Phone.PhoneNumber!,
                    Email = c.Email.EmailAddress!
                }).ToList();

            return View(companies);
        }
    }
}
