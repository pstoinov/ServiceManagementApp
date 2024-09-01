using System.ComponentModel.DataAnnotations;

namespace ServiceManagementApp.Data.Models.Core
{
    public class Phone
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string? PhoneNumber { get; set; } = string.Empty;

    }
}
