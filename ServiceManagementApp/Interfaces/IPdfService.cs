namespace ServiceManagementApp.Interfaces
{
    public interface IPdfService
    {
        byte[] GenerateSimplePdf();
        byte[] GenerateContractPdf(int contractId);
        //TODO to add more pdfs
    }
}
