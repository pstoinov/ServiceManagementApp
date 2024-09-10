using Microsoft.AspNetCore.Mvc;
using ServiceManagementApp.Interfaces;
using System;

namespace ServiceManagementApp.Controllers
{
    [Route("api/pdf")]
    public class PdfController : Controller
    {
        private readonly IPdfService _pdfService;

        public PdfController(IPdfService pdfService)
        {
            _pdfService = pdfService;
        }

        [HttpGet("simple")]
        public IActionResult GenerateRepairRequestPdf()
        {
            
                var pdfData = _pdfService.GenerateRepairRequestPdf();
                if (pdfData == null || pdfData.Length == 0)
                {
                    return NotFound("PDF документът не беше генериран.");
                }
                return File(pdfData, "application/pdf", "simple.pdf");
            
        }

        [HttpGet("contract/{id}")]
        public IActionResult GetContractPdf(int id)
        {
            var pdfData = _pdfService.GenerateContractPdf(id);

            if (pdfData == null || pdfData.Length == 0)
            {
                return NotFound($"PDF документът за договор с ID {id} не беше генериран.");
            }

            Console.WriteLine($"Успешно генериран PDF за договор с ID {id}.");

            Response.Headers.Append("Content-Disposition", "inline; filename=contract_" + id + ".pdf");

            return File(pdfData, "application/pdf"/*, $"contract_{id}.pdf"*/);
        }

        [HttpGet("repair/{id}")]
        public IActionResult GetCashRegisterRepairAcceptanceForm(int id)
        {
            
                var pdfData = _pdfService.GenerateCashRegisterRepairAcceptanceForm(id);
                if (pdfData == null || pdfData.Length == 0)
                {
                    return NotFound($"PDF документът за ремонт с ID {id} не беше генериран.");
                }
                return File(pdfData, "application/pdf", $"repair_{id}.pdf");
            
        }
    }
}
