using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Borders;
using iText.Kernel.Colors;
using System.Linq;
using ServiceManagementApp.Interfaces;
using ServiceManagementApp.Data;
using Microsoft.EntityFrameworkCore;
using iText.IO.Image;
using ServiceManagementApp.Data.Models.RepairModels;

namespace ServiceManagementApp.Services
{
    public class PdfService : IPdfService
    {
        private readonly ApplicationDbContext _context;

        public PdfService(ApplicationDbContext context)
        {
            _context = context;
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

                //private byte[] GeneratePdf(Action<Document> contentGenerator)
                //{
                //    using (var stream = new MemoryStream())
                //    {
                //        PdfWriter writer = new PdfWriter(stream);
                //        PdfDocument pdf = new PdfDocument(writer);
                //        Document document = new Document(pdf);

                //        // Добавяне на хедър
                //        AddHeader(document, "path/to/logo.png", "Име на фирма", "Адрес на фирмата", "Телефон и Email");

                //        // Генериране на съдържание чрез делегат
                //        contentGenerator(document);

                //        // Добавяне на футър
                //        AddFooter(document, "Този документ е генериран от софтуер за управление на сервизи.", "© 2024 Вашата Фирма. Всички права запазени.");

                //        document.Close();
                //        return stream.ToArray();
                //    }
                //}

                public byte[] GenerateContractPdf(int contractId)
        {

            using (var stream = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(stream);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                document.Add(new Paragraph($"Contract ID: {contractId}"));
                // Добави друга необходима информация

                document.Close();
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
        private void AddHeader(Document document, string logoPath, string companyName, string companyAddress, string contactInfo)
        {
            // Лого
            ImageData imageData = ImageDataFactory.Create(logoPath);
            Image logo = new Image(imageData).ScaleAbsolute(50, 50).SetFixedPosition(20, 760);

            // Информация за фирмата
            Paragraph info = new Paragraph()
                .Add(companyName + "\n")
                .Add(companyAddress + "\n")
                .Add(contactInfo)
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetFixedPosition(400, 760, 200);

            document.Add(logo);
            document.Add(info);
        }

        private void AddFooter(Document document, string footerText, string copyrightText)
        {
            int numberOfPages = document.GetPdfDocument().GetNumberOfPages();
            for (int i = 1; i <= numberOfPages; i++)
            {
                document.ShowTextAligned(new Paragraph(footerText)
                    .SetFontSize(10)
                    .SetTextAlignment(TextAlignment.CENTER),
                    297.5f, 20, i, TextAlignment.CENTER, VerticalAlignment.BOTTOM, 0);

                document.ShowTextAligned(new Paragraph(copyrightText)
                    .SetFontSize(8)
                    .SetTextAlignment(TextAlignment.CENTER),
                    297.5f, 10, i, TextAlignment.CENTER, VerticalAlignment.BOTTOM, 0);
            }
        }
    }
}
