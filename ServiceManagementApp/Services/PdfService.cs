﻿using iText.Kernel.Pdf;
using iText.Kernel;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Borders;
using ServiceManagementApp.Interfaces;
using ServiceManagementApp.Data;
using ServiceManagementApp.Data.Models.ClientModels;
using Microsoft.EntityFrameworkCore;
using iText.IO.Image;
using ServiceManagementApp.Data.Models.RepairModels;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.IO.Font;
using iText.Html2pdf;
using iText.Html2pdf.Resolver.Font;
using iText.Layout.Font;
using iText.Forms;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using iText.StyledXmlParser.Jsoup.Select;

namespace ServiceManagementApp.Services
{
    public class PdfService : IPdfService
    {
        private readonly ApplicationDbContext _context;

        public PdfService(ApplicationDbContext context)
        {
            _context = context;
        }




        public byte[] GenerateRepairRequestPdf(int requestId)
        {
            string htmlTemplatePath = "Templates/RequestTemplate.html"; // Path to your HTML file
            string htmlContent = File.ReadAllText(htmlTemplatePath);

            var repairRequest =  _context.ServiceRequests
                .Include(r => r.Client)
                .FirstOrDefault(r => r.Id == requestId);

            //Замяна на плейсхолдерите с данни от заявката
            htmlContent = htmlContent.Replace("{{ClientName}}", repairRequest?.Client.FullName ?? "N/A");
            htmlContent = htmlContent.Replace("{{RequestNumber}}", repairRequest?.RequestNumber ?? "N/A");
            htmlContent = htmlContent.Replace("{{StarDate}}", repairRequest?.RequestDate.ToString("dd.MM.yyyy"));


            using (var stream = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(stream);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                //TODO: Set the base URI for resources (like images) or use base64 in html!
                ConverterProperties properties = new ConverterProperties();
                //properties.SetBaseUri("http://localhost:5190/");
                //HtmlConverter.ConvertToPdf(htmlContent, pdf, properties);

                
                
                var elements = HtmlConverter.ConvertToElements(htmlContent, properties);
                foreach (var element in elements)
                {
                    document.Add((IBlockElement)element);
                }

                


                document.Close(); // Затваряме документа
                return stream.ToArray(); // Връщаме PDF-а като byte[]

            }
        }





        public byte[] GenerateClientServiceCard(int repairId)
        {
            using (var stream = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(stream);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                // Заглавие
                Paragraph title = new Paragraph("Карта за обслужване на клиенти")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(20)
                    .SetBold();
                document.Add(title);

                // Информация за дата и време
                document.Add(new Paragraph("Дата: ____________"));
                document.Add(new Paragraph("Час на пристигане: ____________"));
                document.Add(new Paragraph("Час на тръгване: ____________"));

                // Извличане на ремонт, сервизен техник и части от базата данни
                var repair = _context.Repairs
                    .Include(r => r.Employee)
                    .Include(r => r.RepairPart!)
                        .ThenInclude(rp => rp.Part)
                    .FirstOrDefault(r => r.Id == repairId);

                if (repair == null)
                {
                    throw new Exception("Repair not found.");
                }

                var technicianName = repair.Employee.FullName;
                var repairParts = repair.RepairPart?.ToList() ?? new List<RepairPart>(); // Ако RepairPart е null, използваме празен списък

                // Секция A: Вложени материали и резервни части
                document.Add(new Paragraph("\nA. Вложени материали и резервни части:").SetBold());
                Table materialsTable = new Table(4).UseAllAvailableWidth();
                materialsTable.AddHeaderCell("Ед. цена");
                materialsTable.AddHeaderCell("Брой");
                materialsTable.AddHeaderCell("Цена");
                materialsTable.AddHeaderCell("Материал/Част");

                foreach (var repairPart in repairParts)
                {
                    materialsTable.AddCell(repairPart.Part.Price.ToString("C"));
                    materialsTable.AddCell(repairPart.Quantity.ToString());
                    materialsTable.AddCell((repairPart.Part.Price * repairPart.Quantity).ToString("C"));
                    materialsTable.AddCell(repairPart.Part.Name);
                }

                // Добавяне на празни редове, ако частите са по-малко от 6
                for (int i = repairParts.Count; i < 6; i++)
                {
                    materialsTable.AddCell("");
                    materialsTable.AddCell("");
                    materialsTable.AddCell("");
                    materialsTable.AddCell("");
                }

                document.Add(materialsTable);

                // Секция B: Извършени инсталации, ремонти и настройки
                document.Add(new Paragraph("\nB. Извършени инсталации, ремонти и настройки:").SetBold());
                Table repairsTable = new Table(3).UseAllAvailableWidth();
                repairsTable.AddHeaderCell("Време /часове/");
                repairsTable.AddHeaderCell("Цена");
                repairsTable.AddHeaderCell("Описание на работата");

                for (int i = 0; i < 7; i++)
                {
                    repairsTable.AddCell("");
                    repairsTable.AddCell("");
                    repairsTable.AddCell("");
                }

                document.Add(repairsTable);

                // Транспортни разходи
                document.Add(new Paragraph("\nТранспортни разходи (отиване и връщане):"));
                document.Add(new Paragraph("В рамките на града: 5лв.; Извън града: 0.45лв./км"));

                // Крайна сума без ДДС
                document.Add(new Paragraph("\nКрайна сума без ДДС: ____________"));

                // Подписи
                document.Add(new Paragraph($"\nСервизен инженер: {technicianName}").SetBold());
                document.Add(new Paragraph("Подпис: ___________________________"));
                document.Add(new Paragraph("Упълномощен представител на клиента: ___________________________"));
                document.Add(new Paragraph("Подпис: ___________________________"));
                document.Add(new Paragraph("/с подписа си клиентът удостоверява отстраняването на проблема/"));

                // Забележки и препоръки
                document.Add(new Paragraph("\nЗабележка/Препоръка/Констатация:").SetBold());
                for (int i = 0; i < 6; i++)
                {
                    document.Add(new Paragraph("________________________________________________________________________"));
                }

                document.Close();
                return stream.ToArray();
            }
        }

        public byte[] GenerateCashRegisterRepairAcceptanceForm(int cashRegisterRepairId)
        {
            using (var stream = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(stream);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                // Извличане на данни от базата
                var repair = _context.CashRegisterRepairs
                    .Include(r => r.CashRegister)
                        .ThenInclude(cr => cr.Manufacturer)
                    .Include(r => r.CashRegister)
                        .ThenInclude(cr => cr.Company)
                    .Include(r => r.CashRegister)
                        .ThenInclude(cr => cr.Service)
                    .FirstOrDefault(r => r.Id == cashRegisterRepairId);

                if (repair == null)
                {
                    throw new Exception("Repair not found.");
                }

                // Извличане на информация за клиента
                var company = repair.CashRegister.Company;
                var clientName = company?.Manager ?? "Неизвестен клиент";  // Използваме името на мениджъра като представител на клиента
                var manufacturerName = repair.CashRegister.Manufacturer;
                var serialNumber = repair.CashRegister.SerialNumber;
                var companyName = company?.CompanyName ?? "Неизвестна компания";
                var serviceName = repair.CashRegister.Service.ServiceName;
                var descriptionOfProblem = repair.DescriptionOfProblem;

                // Шрифтове
                PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                PdfFont regularFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

                // Заглавие
                Paragraph title = new Paragraph("Приемо-Предавателен протокол")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFont(boldFont)
                    .SetFontSize(18);
                document.Add(title);

                // Номер и дата
                Paragraph numberAndDate = new Paragraph($"Номер: {repair.Id} / от дата: {repair.StartRepairDate.ToShortDateString()}")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFont(regularFont)
                    .SetFontSize(14);
                document.Add(numberAndDate);

                // Информация за клиента и касовия апарат
                Paragraph clientInfo = new Paragraph($"Днес, {clientName} остави касов апарат {manufacturerName} със сериен номер {serialNumber}, собственост на {companyName} в сервиза на {serviceName}, със следният проблем: {descriptionOfProblem}.")
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetFont(regularFont)
                    .SetFontSize(14);
                document.Add(clientInfo);

                // Добавяне на празен ред за разстояние
                document.Add(new Paragraph("\n\n"));

                // Подписи - Приел: ... и Предал: ...
                Table signatureTable = new Table(2).UseAllAvailableWidth();
                signatureTable.AddCell(new Cell().Add(new Paragraph("Приел: ..................").SetFont(regularFont).SetFontSize(12)).SetBorder(Border.NO_BORDER));
                signatureTable.AddCell(new Cell().Add(new Paragraph("Предал: ..................").SetFont(regularFont).SetFontSize(12)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER));

                document.Add(signatureTable);

                // Финализиране на документа
                document.Close();
                return stream.ToArray();
            }
        }

        public byte[] GenerateContractPdf(int contractId)
        {

            var contract = _context.Contracts
                .Include(c => c.Company)
                .Include(c => c.CashRegister)
                .ThenInclude(cr => cr.SiteAddress)
                .FirstOrDefault(c => c.Id == contractId);

            if (contract == null)
            {
                throw new Exception("Contract not found.");
            }

            using (var stream = new MemoryStream())
            {
                PdfReader pdfReader = new PdfReader("Templates/contractTemplate.pdf");
                PdfWriter writer = new PdfWriter(stream);
                using (var pdf = new PdfDocument(pdfReader, writer)) // Using блок за PdfDocument
                {
                    PdfAcroForm form = PdfAcroForm.GetAcroForm(pdf, true);
                    string fontPath = Environment.OSVersion.Platform == PlatformID.Win32NT
                        ? @"C:\Windows\Fonts\arial.ttf"
                        : "/usr/share/fonts/truetype/dejavu/DejaVuSans.ttf";
                    PdfFont font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);

                    string fullAddress = $"гр. {contract.CashRegister.SiteAddress.City}, б/ул.{contract.CashRegister.SiteAddress.Street} No: {contract.CashRegister.SiteAddress.Number}";
                    form.GetField("{{Address}}").SetValue(fullAddress).SetFont(font);
                    form.GetField("{{Model}}").SetValue(contract.CashRegister.Model ?? "").SetFont(font);
                    form.GetField("{{CompanyName}}").SetValue(contract.Company?.CompanyName ?? "").SetFont(font);
                    form.GetField("{{EIK}}").SetValue(contract.Company?.EIK ?? "").SetFont(font);
                    form.GetField("{{ContractNumber}}").SetValue(contract.ContractNumber ?? "");
                    form.GetField("{{StartDate}}").SetValue(contract.StartDate.ToString("dd.MM.yyyy"));
                    form.GetField("{{EndDate}}").SetValue(contract.EndDate.ToString("dd.MM.yyyy"));
                    form.GetField("{{Manufacturer}}").SetValue(contract.CashRegister.Manufacturer.ToString()).SetFont(font);
                    form.GetField("{{CashRegisterSerial}}").SetValue(contract.CashRegister.SerialNumber ?? "").SetFont(font);
                    form.GetField("{{FiscalMemoryNumber}}").SetValue(contract.CashRegister.FiscalMemoryNumber ?? "").SetFont(font);

                    form.FlattenFields();
                }
                return stream.ToArray();

            }
        }

        public byte[] GenerateSimplePdf()
        {
            using (var stream = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(stream);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                document.Add(new Paragraph("Hello, iText 7!"));
                document.Close();

                return stream.ToArray();
            }
        }
        

        

        
    }
}
