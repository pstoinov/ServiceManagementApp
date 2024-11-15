using Microsoft.AspNetCore.Mvc;
using ServiceManagementApp.Interfaces;
using System;

namespace ServiceManagementApp.Controllers
{
    [Route("api/fiscalser")]
    public class FiscalSerController : Controller
    {
        private readonly IFiscalSerService _fiscalSerService;

        public FiscalSerController(IFiscalSerService fiscalSerService)
        {
            _fiscalSerService = fiscalSerService;
        }

        [HttpGet("generate/{month}/{year}")]
        public IActionResult GenerateFiscalSerFile(int month, int year)
        {
            try
            {
                // Генерираме файла като масив от байтове
                var fiscalSerData = _fiscalSerService.GenerateFiscalSerFile(month, year);

                if (fiscalSerData == null || fiscalSerData.Length == 0)
                {
                    return NotFound($"Файлът за НАП за месец {month} и година {year} не беше генериран.");
                }

                // Задаваме Content-Disposition header за изтегляне
                Response.Headers.Append("Content-Disposition", $"attachment; filename=fiscal_ser_{year}_{month:00}.txt");

                // Връщаме файла с правилния mime type и име на файл
                return File(fiscalSerData, "text/plain");
            }
            catch (Exception ex)
            {
                // Ако има грешка, я връщаме на клиента
                return BadRequest($"Възникна грешка при генериране на файла: {ex.Message}");
            }
        }
    }
}
