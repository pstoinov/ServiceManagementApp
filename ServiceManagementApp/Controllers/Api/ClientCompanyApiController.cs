using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceManagementApp.Data;
using ServiceManagementApp.Data.Models.ClientModels;
using ServiceManagementApp.ViewModels;

namespace ServiceManagementApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientCompanyApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClientCompanyApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("associate")]
        public async Task<IActionResult> AssociateClientWithCompany([FromBody] AssociateClientCompanyViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingAssociation = await _context.ClientCompanies
                    .FirstOrDefaultAsync(cc => cc.ClientId == model.SelectedClientId && cc.CompanyId == model.CompanyId);

                if (existingAssociation != null)
                {
                    return BadRequest("This client is already associated with the company.");
                }

                var clientCompany = new ClientCompany
                {
                    ClientId = model.SelectedClientId,
                    CompanyId = model.CompanyId
                };

                await _context.ClientCompanies.AddAsync(clientCompany);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Client associated successfully." });
            }

            return BadRequest("Invalid data.");
        }

        [HttpGet("associations")]
        public async Task<IActionResult> GetAssociations()
        {
            var associations = await _context.ClientCompanies
                .Include(cc => cc.Client)
                .Include(cc => cc.Company)
                .Select(cc => new
                {
                    ClientName = cc.Client.FullName,
                    //CompanyName = cc.Company.CompanyName,
                    CompanyEIK = cc.Company.EIK
                })
                .ToListAsync();

            return Ok(associations);
        }
    }
}
