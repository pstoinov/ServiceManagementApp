using Microsoft.EntityFrameworkCore;
using ServiceManagementApp.Data;
using System.Text;

public class FiscalSerService
{
    private readonly ApplicationDbContext _context;
    private int _cashRegistersCount = 0;

    public FiscalSerService(ApplicationDbContext context)
    {
        _context = context;
    }

    public byte[] GenerateFiscalSerFile(int month, int year)
    {
        var startDate = new DateTime(year, month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);

        // Вземаме всички контракти, свързани с касови апарати, които отговарят на времевия период
        var contracts = _context.Contracts
             .Include(c => c.CashRegister) // Включваме свързания касов апарат
             .ThenInclude(cr => cr.Company) // Включваме компанията, свързана с касовия апарат
             .Include(c => c.CashRegister.ContactPhone) // Включваме телефона на касовия апарат
             .Include(c => c.CashRegister.SiteAddress) // Включваме адреса на касовия апарат
             .Where(c => c.StartDate >= startDate && c.StartDate <= endDate)
             .ToList();

        // Извличаме списъка с касови апарати от всеки контракт
        var cashRegisters = contracts
            .Where(c => c.CashRegister != null) // Уверяваме се, че има валиден касов апарат
            .Select(c => c.CashRegister)
            .ToList();



        foreach (var cashRegister in cashRegisters)
        {
            _cashRegistersCount +=1;
        }

        var builder = new StringBuilder();

        // Първият ред - 00
        builder.AppendLine($"00\t\t\t\t123059763Делта ЕООД\t\t\t\t\t\t\t\t\t\t\u0421тара Загора Свети  Княз Борис I No. 93\t\t\t\t\t\t\t\t0887-979-700   delta.sz.ltd@gmail.com        {startDate:dd.MM.yyyy}{endDate:dd.MM.yyyy}{_cashRegistersCount:D4}");

        foreach (var cashRegister in cashRegisters)
        {
            var company = cashRegister.Company;
            var contract = contracts.FirstOrDefault(c => c.CashRegister.Id == cashRegister.Id);

            // Вторият ред - 01
            builder.AppendLine($"01\t\t\t\t\t{company.EIK}");

            // Третият ред - 02
            builder.AppendLine($"02{company.CompanyName}                                   {company.Address.City}         {company.Address.Street}                        {cashRegister.SiteManager}");

            // Четвърти ред - 03
            builder.AppendLine($"03{cashRegister.SiteName}                                              {cashRegister.SiteAddress.City}         {cashRegister.SiteAddress.Street}                   {cashRegister.ContactPhone}");

            // Петият ред - 04
            builder.AppendLine($"04{cashRegister.RegionalNRAOffice}");

            // Шестият ред - 05
            builder.AppendLine($"05{cashRegister.Model}");

            // Седмият ред - 06
            builder.AppendLine("06       ");

            // Осмият ред - 07
            builder.AppendLine($"07{cashRegister.BIMCertificateNumber}   {cashRegister.BIMCertificateDate:dd.MM.yyyy}");

            // Деветият ред - 08
            builder.AppendLine($"08{cashRegister.SerialNumber}{cashRegister.FiscalMemoryNumber}");

            // Десетият ред - 09
            builder.AppendLine("09Делта ЕООД                                        Стара Загора             Свети  Княз Борис I No. 93                        0887-979-700");

            // 11-ти ред - 10
            builder.AppendLine("10Петър Гицов ");

            // 12-ти ред - 11
            builder.AppendLine($"11{contract.StartDate:dd.MM.yyyy}{contract.StartDate:dd.MM.yyyy}");
        }

        // Последен ред - 99
        builder.AppendLine("99");

        return Encoding.GetEncoding("windows-1251").GetBytes(builder.ToString());
    }
}
