using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceManagementApp.Data;
using ServiceManagementApp.Data.Models.Core;
using ServiceManagementApp.ViewModels;

namespace ServiceManagementApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class EditClientApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EditClientApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClient(int id)
        {
            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.Id == id);

            if (client == null) { return NotFound(); }

            var viewModel = new EditClientViewModel
            { 
                Id = client.Id,
                FullName = client.FullName,
                Phone = client.Phone!.PhoneNumber,
                Email = client.Email!.EmailAddress
            };

            return Ok(viewModel);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] EditClientViewModel model)
        {
            if (id != model.Id) 
            {
                return BadRequest();
            }

            var client = await _context.Clients
                .Include(c => c.Phone)
                .Include(c => c.Email)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (client == null) { return NotFound(); };

            if (client.Phone == null)
            {
                client.Phone = new Phone();
            }

            if (client.Email == null)
            {
                client.Email = new Email();
            }

            client.FullName = model.FullName;
            client.Phone.PhoneNumber = model.Phone;
            client.Email.EmailAddress = model.Email;

            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
            return NoContent();
               

        }
    }
}
