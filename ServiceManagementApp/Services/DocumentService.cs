using Xceed.Words.NET;
using System.IO;
using Microsoft.EntityFrameworkCore;
using ServiceManagementApp.Data;

public class DocumentService
{
    private readonly ApplicationDbContext _context;

    public DocumentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public byte[] GenerateContractDocx(int contractId)
    {
        var contract = _context.Contracts
            .Include(c => c.Company)
            .Include(c => c.CashRegister)
            .Include(c => c.Service)
            .FirstOrDefault(c => c.Id == contractId);

        if (contract == null)
            return null;

        // Път до шаблона
        string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "contractTemplate.docx");

        using (var docxStream = new MemoryStream())
        {
            using (var document = DocX.Load(templatePath))
            {
                document.ReplaceText("{ContractNumber}", contract.ContractNumber);
                document.ReplaceText("{CompanyName}", contract.Company.CompanyName);
                document.ReplaceText("{CompanyEIK}", contract.Company.EIK);
                document.ReplaceText("{CashRegisterSerial}", contract.CashRegister.SerialNumber);
                document.ReplaceText("{FiscalMemoryNumber}", contract.CashRegister.FiscalMemoryNumber);
                document.ReplaceText("{Manufacturer}", contract.CashRegister.Manufacturer.ToString);
                document.ReplaceText("{StartDate}", contract.StartDate.ToString("dd.MM.yyyy"));
                document.ReplaceText("{EndDate}", contract.EndDate.ToString("dd.MM.yyyy"));
                document.ReplaceText("City", contract.Company.Address.City);
                document.ReplaceText("Street", contract.Company.Address.Street);
                document.ReplaceText("Number", contract.Company.Address.Number);



                document.SaveAs(docxStream);
            }

            return docxStream.ToArray();
        }
    }
}
