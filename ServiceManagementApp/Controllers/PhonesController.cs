using Microsoft.AspNetCore.Mvc;
using ServiceManagementApp.Data;

namespace ServiceManagementApp.Controllers
{
    public class PhonesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PhonesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult SearchPhones(string term)
        {
            var phones = _context.Phones
                .Where(p => p.PhoneNumber.Contains(term))
                .Select(p => new
                {
                    id = p.Id,
                    phoneNumber = p.PhoneNumber
                })
                .ToList();

            return Json(phones);
        }
    }
}
