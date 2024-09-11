namespace ServiceManagementApp.Interfaces
{
    public interface IPdfService
    {
        byte[] GenerateSimplePdf();
        byte[] GenerateContractPdf(int contractId);
        byte[] GenerateCashRegisterRepairAcceptanceForm(int cashRegisterRepairId);
        byte[] GenerateRepairRequestPdf(int RequestId);
        //TODO to add more pdfs
    }
}
