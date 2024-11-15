namespace ServiceManagementApp.Interfaces
{
    public interface IFiscalSerService
    {
        byte[] GenerateFiscalSerFile(int month, int year);
    }
}
