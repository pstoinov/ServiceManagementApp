using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServiceManagementApp.Data;
using ServiceManagementApp.Data.Models.ClientModels;
using ServiceManagementApp.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceManagementApp.Controllers
{
    public class ContractsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly DocumentService _documentService;

        public ContractsController(ApplicationDbContext context, DocumentService documentService)
        {
            _context = context;
            _documentService = documentService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var contracts = await _context.Contracts
                .Include(c => c.CashRegister)
                .Include(c => c.Company)
                .Include(c => c.Service)
                .Where(c => c.IsActive) 
                .Select(c => new ContractViewModel
                {
                    Id = c.Id,
                    ContractNumber = c.ContractNumber,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    CompanyId = c.CompanyId,
                    CashRegisterId = c.CashRegisterId,
                    ServiceId = c.ServiceId,
                    CompanyName = c.Company.CompanyName, 
                    CashRegisterSerialNumber = c.CashRegister.SerialNumber, 
                })
                .ToListAsync();

            return View(contracts);
        }


        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Services = new SelectList(_context.Services.Where(s => s.IsCashRegisterService), "Id", "ServiceName");
            var viewModel = new ContractViewModel
            {
                ContractNumber = GenerateContractNumber(),
                StartDate = DateTime.Now
            };

            

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContractViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.EndDate = viewModel.StartDate.AddMonths(viewModel.ContractDurationMonths);

                var contract = new Contract
                {
                    ContractNumber = viewModel.ContractNumber,
                    StartDate = viewModel.StartDate,
                    EndDate = viewModel.EndDate,
                    IsActive = true,
                    CompanyId = viewModel.CompanyId,
                    ServiceId = viewModel.ServiceId,
                    CashRegisterId = viewModel.CashRegisterId
                };

                _context.Contracts.Add(contract);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Services = new SelectList(_context.Services, "Id", "Name", viewModel.ServiceId);
            return View(viewModel);
        }

        [HttpGet]
        [Route("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contracts.FindAsync(id);
            if (contract == null)
            {
                return NotFound();
            }

            var viewModel = new ContractViewModel
            {
                ContractNumber = contract.ContractNumber,
                StartDate = contract.StartDate,
                EndDate = contract.EndDate,
                CompanyId = contract.CompanyId,
                ServiceId = contract.ServiceId,
                CashRegisterId = contract.CashRegisterId
            };

            ViewBag.Services = new SelectList(_context.Services, "Id", "Name", contract.ServiceId);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ContractViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var contract = await _context.Contracts.FindAsync(id);
                    if (contract == null)
                    {
                        return NotFound();
                    }

                    contract.StartDate = viewModel.StartDate;
                    contract.EndDate = viewModel.StartDate.AddMonths(viewModel.ContractDurationMonths);
                    contract.CompanyId = viewModel.CompanyId;
                    contract.ServiceId = viewModel.ServiceId;
                    contract.CashRegisterId = viewModel.CashRegisterId;

                    _context.Contracts.Update(contract);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContractExists(viewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Services = new SelectList(_context.Services, "Id", "Name", viewModel.ServiceId);

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Renew(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contracts
                .Include(c => c.Company)
                .Include(c => c.CashRegister)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contract == null)
            {
                return NotFound();
            }

            ViewBag.Services = new SelectList(_context.Services.Where(s => s.IsCashRegisterService), "Id", "ServiceName");

            var viewModel = new ContractViewModel
            {
                ContractNumber = GenerateContractNumber(),
                StartDate = contract.StartDate,
                EndDate = contract.EndDate,
                CompanyId = contract.CompanyId,
                CompanyName = contract.Company.CompanyName, 
                CashRegisterId = contract.CashRegisterId,
                CashRegisterSerialNumber = contract.CashRegister.SerialNumber, 
                ServiceId = contract.ServiceId
            };

            return View("Renew", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Renew(ContractViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var oldContract = await _context.Contracts.FindAsync(viewModel.Id);
            if (oldContract != null)
            {
                oldContract.IsActive = false;
                _context.Contracts.Update(oldContract);
            }
            

            var newContract = new Contract
            {
                ContractNumber = viewModel.ContractNumber,
                StartDate = viewModel.StartDate,
                EndDate = viewModel.EndDate,
                CompanyId = viewModel.CompanyId,
                ServiceId = viewModel.ServiceId,
                CashRegisterId = viewModel.CashRegisterId,
                IsActive = true
            };

            _context.Contracts.Add(newContract);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        private bool ContractExists(int id)
        {
            return _context.Contracts.Any(e => e.Id == id);
        }

        private string GenerateContractNumber()
        {
            var lastContract = _context.Contracts.OrderByDescending(c => c.Id).FirstOrDefault();
            int nextNumber = lastContract != null ? int.Parse(lastContract.ContractNumber.Substring(1)) + 1 : 1;
            return $"C{nextNumber.ToString("D6")}";
        }

        // Метод за търсене на компании (autocomplete)
        //[HttpGet]
        //public async Task<JsonResult> SearchCompanies(string term)
        //{
        //    var companies = await _context.Companies
        //        .Where(c => c.CompanyName.Contains(term))
        //        .Select(c => new { id = c.Id, name = c.CompanyName })
        //        .ToListAsync();

        //    return Json(companies);
        //}

        [HttpGet("contract/{id}")]
        public IActionResult GetContractDocx(int id)
        {
            var docxData = _documentService.GenerateContractDocx(id);
            if (docxData == null)
            {
                return NotFound($"Документът за договор с ID {id} не беше генериран.");
            }

            // Връщаме документа като Word файл
            return File(docxData, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", $"contract_{id}.docx");
        }
    }


}

