using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ServiceManagementApp.Controllers
{
    [Authorize]
    public class RequestsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CurrentRequests()
        {
            return View();
        }

        public IActionResult NewRequest()
        {
            return View();
        }

        public IActionResult History()
        {
            return View();
        }
    }
}
