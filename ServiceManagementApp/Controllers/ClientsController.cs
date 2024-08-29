using Microsoft.AspNetCore.Mvc;
using ServiceManagementApp.Data;
using ServiceManagementApp.Data.Models.ClientModels;
using ServiceManagementApp.ViewModels;
using ServiceManagementApp.Data.Models.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ServiceManagementApp.Controllers
{
    [Authorize]
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var clients = _context.Clients.Select(static c => new ClientViewModel
            {
                FullName = c.FullName,
                Phone = c.Phone != null ? c.Phone.PhoneNumber : string.Empty, // Извличане на телефонния номер
                Email = c.Email != null ? c.Email.EmailAddress : string.Empty // Извличане на email адреса
            }).ToList();

            return View(clients);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ClientViewModel clientViewModel)
        {
            if (ModelState.IsValid)
            {
                var phone = new Phone { PhoneNumber = clientViewModel.Phone };
                var email = new Email { EmailAddress = clientViewModel.Email };

                var client = new Client
                {
                    FullName = clientViewModel.FullName,
                    Phone = phone,
                    Email = email
                };

                _context.Clients.Add(client);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(clientViewModel);
        }
        // Action for Adding a Company to a Client
        public IActionResult AddCompany(int clientId)
        {
            var client = _context.Clients.Find(clientId);

            if (client == null)
            {
                return NotFound();
            }

            var viewModel = new AddCompanyViewModel
            {
                ClientId = clientId,
                ClientName = client.FullName
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCompany(AddCompanyViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var company = new Company
                {
                    CompanyName = viewModel.CompanyName,
                    EIK = viewModel.EIK,
                    VATNumber = viewModel.VATNumber,
                    Manager = viewModel.Manager,
                    Address = new Address { /* Set address properties here */ },
                    Phone = new Phone { PhoneNumber = viewModel.Phone },
                    Email = new Email { EmailAddress = viewModel.Email }
                };

                _context.Companies.Add(company);
                _context.SaveChanges();

                // Link the company to the client
                var clientCompany = new ClientCompany
                {
                    ClientId = viewModel.ClientId,
                    CompanyId = company.Id
                };

                _context.ClientCompanies.Add(clientCompany);
                _context.SaveChanges();

                return RedirectToAction("Details", new { id = viewModel.ClientId });
            }

            return View(viewModel);
        }
        public IActionResult Edit(int id)
        {
            var client = _context.Clients
            .Include(c => (IEnumerable<ClientCompany>?)c.ClientCompanies!)
            .ThenInclude(cc => cc.Company)
            .FirstOrDefault(c => c.Id == id);

            if (client == null)
            {
                return NotFound();
            }

            var viewModel = new EditClientViewModel
            {
                Id = client.Id,
                FullName = client.FullName,
                Phone = client.Phone?.PhoneNumber ?? string.Empty,
                Email = client.Email?.EmailAddress ?? string.Empty,
                Companies = client.ClientCompanies?.Select(cc => new CompanyViewModel
                {
                    Id = cc.CompanyId,
                    CompanyName = cc.Company.CompanyName,
                    EIK = cc.Company.EIK
                }).ToList() ?? new List<CompanyViewModel>()
            };

            return View(viewModel);
        }

        public IActionResult GetCompanyDetails(int companyId)
        {
            var company = _context.Companies.FirstOrDefault(c => c.Id == companyId);
            if (company == null)
            {
                return NotFound();
            }

            var companyViewModel = new CompanyViewModel
            {
                Id = company.Id,
                CompanyName = company.CompanyName,
                EIK = company.EIK,
                // Populate other fields as needed
            };

            return Json(companyViewModel);
        }

            public IActionResult Reports()
        {
            return View();
        }
    }
}
