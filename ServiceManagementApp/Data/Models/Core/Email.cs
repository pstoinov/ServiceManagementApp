using System.ComponentModel.DataAnnotations;

namespace ServiceManagementApp.Data.Models.Core
{
    public class Email
    {
        public int Id { get; set; }

        [EmailAddress]
        [Required]
        public string EmailAddress { get; set; } = null!;
    }
}
