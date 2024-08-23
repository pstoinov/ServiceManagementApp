using Microsoft.AspNetCore.Mvc;
using ServiceManagementApp.Interfaces;

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
        public IActionResult GetSimplePdf()
        {
            var pdfData = _pdfService.GenerateSimplePdf();
            return File(pdfData, "application/pdf", "simple.pdf");
        }

        [HttpGet("contract/{id}")]
        public IActionResult GetContractPdf(int id)
        {
            var pdfData = _pdfService.GenerateContractPdf(id);
            return File(pdfData, "application/pdf", $"contract_{id}.pdf");
        }
    }
}
