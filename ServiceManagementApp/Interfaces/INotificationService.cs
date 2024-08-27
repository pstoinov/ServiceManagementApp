namespace ServiceManagementApp.Interfaces
{
    public interface INotificationService
    {
        Task SendAsync(string recpipient, string subject, string message);
    }
}
