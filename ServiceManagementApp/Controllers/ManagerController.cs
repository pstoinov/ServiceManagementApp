using Microsoft.AspNetCore.Mvc;

namespace ServiceManagementApp.Controllers
{
    public class Manager : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Settings()
        {
            return View();
        }
    }
}
