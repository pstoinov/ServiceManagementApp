using Microsoft.AspNetCore.Mvc;

namespace ServiceManagementApp.Controllers
{
    public class WarehouseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
