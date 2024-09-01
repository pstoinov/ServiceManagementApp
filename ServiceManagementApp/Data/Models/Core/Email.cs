using System.ComponentModel.DataAnnotations;

namespace ServiceManagementApp.Data.Models.Core
{
    public class Email
    {
        public int Id { get; set; }

        [MaxLength(254)]
        [EmailAddress]
        public string? EmailAddress { get; set; } = string.Empty;
    }
}
