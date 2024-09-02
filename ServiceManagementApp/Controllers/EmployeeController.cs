using Microsoft.AspNetCore.Mvc;

namespace ServiceManagementApp.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
