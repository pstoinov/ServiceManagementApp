using Microsoft.AspNetCore.Mvc;
using ServiceManagementApp.Data;
using ServiceManagementApp.Data.Models.ClientModels;
using Microsoft.EntityFrameworkCore;
using ServiceManagementApp.ViewModels;
using ServiceManagementApp.Data.Models.Core;
using ServiceManagementApp.Data.Models.ServiceModels;

namespace ServiceManagementApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeesApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.Service)
                .Include(e => e.PhoneNumber)
                .Include(e => e.EmailAddress)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            var viewModel = new EmployeeViewModel
            {
                Id = employee.Id,
                FullName = employee.FullName,
                ServiceId = employee.ServiceId,
                ServiceName = employee.Service.ServiceName,
                Position = employee.Position,
                PhoneNumber = employee.PhoneNumber.PhoneNumber!,
                EmailAddress = employee.EmailAddress.EmailAddress!,
                IsCertifiedForCashRegisterRepair = employee.IsCertifiedForCashRegisterRepair,
                EGN = employee.EGN!,
            };

            return Ok(viewModel);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] EmployeeViewModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    // Логиране на грешките в ModelState
            //    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            //    return BadRequest(new { message = "Invalid data", errors = errors });
            //}

            // Създаване на нови обекти за телефон и имейл
            var phone = new Phone
            {
                PhoneNumber = model.PhoneNumber
            };

            var email = new Email
            {
                EmailAddress = model.EmailAddress
            };

            // Създаване на нов обект за служител
            var employee = new Employee
            {
                FullName = model.FullName,
                ServiceId = model.ServiceId,
                Position = model.Position,
                PhoneNumber = phone,
                EmailAddress = email,
                IsCertifiedForCashRegisterRepair = model.IsCertifiedForCashRegisterRepair,
                EGN = model.EGN,
                //PictureUrl = model.PictureUrl
            };

            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Employee created successfully." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] EmployeeViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                // Логиране на грешките в ModelState
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new { message = "Invalid data", errors = errors });
            }

            var employee = await _context.Employees
                .Include(e => e.PhoneNumber)
                .Include(e => e.EmailAddress)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            if (employee.PhoneNumber == null)
            {
                employee.PhoneNumber = new Phone();
            }

            if (employee.EmailAddress == null)
            {
                employee.EmailAddress = new Email();
            }

            employee.FullName = model.FullName;
            employee.ServiceId = model.ServiceId;
            employee.Position = model.Position;
            employee.PhoneNumber.PhoneNumber = model.PhoneNumber;
            employee.EmailAddress.EmailAddress = model.EmailAddress;
            employee.IsCertifiedForCashRegisterRepair = model.IsCertifiedForCashRegisterRepair;
            employee.EGN = model.EGN;
            //employee.PictureUrl = model.PictureUrl;

            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
