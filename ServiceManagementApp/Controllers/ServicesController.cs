using Microsoft.AspNetCore.Mvc;

namespace ServiceManagementApp.Controllers
{
    public class ServicesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
