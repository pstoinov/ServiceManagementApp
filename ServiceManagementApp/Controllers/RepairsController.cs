using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ServiceManagementApp.Controllers
{
    [Authorize]
    public class RepairsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CurrentRepairs()
        {
            return View();
        }


        public IActionResult History()
        {
            return View();
        }
    }
}
