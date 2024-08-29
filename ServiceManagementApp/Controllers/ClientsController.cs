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
    }
}
