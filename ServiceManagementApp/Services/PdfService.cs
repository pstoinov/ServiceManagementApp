using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout;
using ServiceManagementApp.Interfaces;
using iText.IO.Image;
using iText.Layout.Properties;

namespace ServiceManagementApp.Services
{
    public class PdfService : IPdfService
    {


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
