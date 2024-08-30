using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceManagementApp.Data;
using ServiceManagementApp.Data.Models.Core;
using ServiceManagementApp.ViewModels;

namespace ServiceManagementApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class EditCompanyApiController : Controller
    {
        
       
            private readonly ApplicationDbContext _context;

            public EditCompanyApiController(ApplicationDbContext context)
            {
                _context = context;
            }

            [HttpGet("{id}")]
            public async Task<IActionResult> GetCompany(int id)
            {
                var company = await _context.Companies
                    .Include(c => c.ClientCompanies)
                    .ThenInclude(cc => cc.Client)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (company == null)
                {
                    return NotFound();
                }

            var viewModel = new EditCompanyViewModel
            {
                Id = company.Id,
                CompanyName = company.CompanyName,
                EIK = company.EIK,
                VATNumber = company.VATNumber,
                Phone = company.Phone.PhoneNumber!,
                Email = company.Email.EmailAddress!,
               // Clients = await _context.ClientCompanies
               //.Where(cc => cc.CompanyId == company.Id)
               //.Select(cc => new ClientViewModel
               //{
               //    Id = cc.Client.Id,
               //    FullName = cc.Client.FullName
               //}).ToListAsync()
            };

            return Ok(viewModel);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> Edit(int id, [FromBody] EditCompanyViewModel model)
            {
            if (id != model.Id)
            {
                return BadRequest();
            }

            var company = await _context.Companies
                .Include(c => c.Phone)
                .Include(c => c.Email)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (company == null)
            {
                return NotFound();
            }

            if (company.Phone == null)
            {
                company.Phone = new Phone();
            }

            if (company.Email == null)
            {
                company.Email = new Email();
            }

            company.CompanyName = model.CompanyName;
            company.EIK = model.EIK;
            company.VATNumber = model.VATNumber;
            company.Phone.PhoneNumber = model.Phone;
            company.Email.EmailAddress = model.Email;

            _context.Companies.Update(company);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        }
    }

