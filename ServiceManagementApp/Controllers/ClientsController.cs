using Microsoft.AspNetCore.Mvc;
using ServiceManagementApp.Data;
using ServiceManagementApp.Data.Models.ClientModels;
using ServiceManagementApp.ViewModels;
using ServiceManagementApp.Data.Models.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace ServiceManagementApp.Controllers
{
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
                Phone = c.Phone != null ? c.Phone.PhoneNumber : string.Empty,
                Email = c.Email != null ? c.Email.EmailAddress : string.Empty
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

        public IActionResult Reports()
        {
            return View();
        }
        public IActionResult Edit(int clientId)
        {
            var client = _context.Clients
                .Include(c => c.Phone)
                .Include(c => c.Email)
                .FirstOrDefault(c => c.Id == clientId);
            if (client == null)
            {
                return NotFound();
            }
            var model = new EditClientViewModel
            {
                Id = client.Id,
                FullName = client.FullName,
                Phone = client.Phone!.PhoneNumber,
                Email = client.Email!.EmailAddress
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> FindClients(string term)
        {
            var clients = await _context.Clients
       .Include(c => c.Phone)
       .Include(c => c.Email)
       .Where(c => c.FullName.Contains(term) || c.Phone.PhoneNumber.Contains(term) || c.Email.EmailAddress.Contains(term))
       .Select(c => new {
           id = c.Id,
           name = c.FullName,
           phone = c.Phone.PhoneNumber,
           email = c.Email.EmailAddress
       })
       .ToListAsync();

            return Json(clients);
        }
    }
}
