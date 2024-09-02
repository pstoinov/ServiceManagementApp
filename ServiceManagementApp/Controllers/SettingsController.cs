using Microsoft.AspNetCore.Mvc;

namespace ServiceManagementApp.Controllers
{
    public class SettingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
