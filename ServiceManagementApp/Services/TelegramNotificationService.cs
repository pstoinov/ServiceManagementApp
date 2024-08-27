using Newtonsoft.Json;

namespace ServiceManagementApp.Services
{
    public class TelegramNotificationService
    {
        private readonly string _botToken;
        private readonly HttpClient _httpClient;

        public TelegramNotificationService(string botToken)
        {
            _botToken = botToken;
            _httpClient = new HttpClient();
        }

        public async Task SendAsync(string recipient, string subject, string message)
        {
            var telegramMessage = new
            {
                chat_id = recipient,
                text = $"{subject}\n\n{message}"
            };

            var json = JsonConvert.SerializeObject(telegramMessage);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            await _httpClient.PostAsync($"https://api.telegram.org/bot{_botToken}/sendMessage", content);
        }
    }
}
