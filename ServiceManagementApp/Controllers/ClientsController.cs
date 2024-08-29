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

        [HttpGet]
        public IActionResult Index()
        {
            var clients = _context.Clients.Select(static c => new ClientViewModel
            {
                Id = c.Id,
                FullName = c.FullName,
                Phone = c.Phone != null ? c.Phone.PhoneNumber : string.Empty, // Извличане на телефонния номер
                Email = c.Email != null ? c.Email.EmailAddress : string.Empty // Извличане на email адреса
            }).ToList();

            return View(clients);
        }

        [HttpGet]
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
        [HttpGet]
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

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var client = _context.Clients
        .Include(c => c.ClientCompanies)
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

        [HttpGet]
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

        [HttpGet]
        public IActionResult EditCompany(int companyId)
        {
            var company = _context.Companies.Find(companyId);

            if (company == null)
            {
                return NotFound();
            }

            var viewModel = new EditCompanyViewModel
            {
                Id = company.Id,
                CompanyName = company.CompanyName,
                EIK = company.EIK,
                // Добавяне на други нужни полета
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCompany(EditCompanyViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var company = _context.Companies.Find(viewModel.Id);

                if (company == null)
                {
                    return NotFound();
                }

                company.CompanyName = viewModel.CompanyName;
                company.EIK = viewModel.EIK;
                // Актуализиране на други полета

                _context.SaveChanges();
                return RedirectToAction("Edit", new { id = viewModel.ClientId });
            }

            return View(viewModel);
        }

        [HttpGet]
        public JsonResult GetClientDetails(int clientId)
        {
            var client = _context.Clients
        .Include(c => c.ClientCompanies)
            .ThenInclude(cc => cc.Company)
        .FirstOrDefault(c => c.Id == clientId);

            if (client == null)
            {
                return Json(new { success = false, message = "Client not found." });
            }

            var clientDetails = new
            {
                FullName = client.FullName,
                Phone = client.Phone?.PhoneNumber ?? string.Empty, // Проверка дали Phone е null
                Email = client.Email?.EmailAddress ?? string.Empty, // Проверка дали Email е null
                Companies = client.ClientCompanies?.Select(cc => new CompanyViewModel
                {
                    CompanyName = cc.Company?.CompanyName ?? "N/A", // Проверка дали CompanyName е null
                    EIK = cc.Company?.EIK ?? "N/A" // Проверка дали EIK е null
                }).ToList() ?? new List<CompanyViewModel>() // Ако ClientCompanies е null, връща празен списък от правилния тип
            };

            return Json(clientDetails);
        }


        public IActionResult Reports()
        {
            return View();
        }
    }
}
