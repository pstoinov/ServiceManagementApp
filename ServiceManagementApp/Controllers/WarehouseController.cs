using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceManagementApp.Data;
using ServiceManagementApp.Data.Models.Wherehouse;
using ServiceManagementApp.ViewModels;
using System.Threading.Tasks;

namespace ServiceManagementApp.Controllers
{
    public class WarehouseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WarehouseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Warehouse
        public async Task<IActionResult> Index()
        {
            var parts = await _context.Parts
                .Select(p => new PartViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price
                }).ToListAsync();

            return View(parts);
        }

        // GET: Warehouse/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Warehouse/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PartViewModel partViewModel)
        {
            if (ModelState.IsValid)
            {
                var part = new Part
                {
                    Name = partViewModel.Name,
                    Price = partViewModel.Price
                };

                _context.Add(part);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(partViewModel);
        }

        // GET: Warehouse/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var part = await _context.Parts.FindAsync(id);
            if (part == null)
            {
                return NotFound();
            }

            var partViewModel = new PartViewModel
            {
                Id = part.Id,
                Name = part.Name,
                Price = part.Price
            };

            return View(partViewModel);
        }

        // POST: Warehouse/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PartViewModel partViewModel)
        {
            if (id != partViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var part = new Part
                {
                    Id = partViewModel.Id,
                    Name = partViewModel.Name,
                    Price = partViewModel.Price
                };

                try
                {
                    _context.Update(part);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartExists(part.Id))
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
            return View(partViewModel);
        }

        private bool PartExists(int id)
        {
            return _context.Parts.Any(e => e.Id == id);
        }
    }
}
