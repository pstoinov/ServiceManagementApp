using System.ComponentModel.DataAnnotations;

namespace ServiceManagementApp.ViewModels
{
    public class AddCompanyViewModel
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public string CompanyName { get; set; } = null!;

        [Required]
        [MaxLength(13)]
        public string EIK { get; set; } = null!;

        [MaxLength(15)]
        public string? VATNumber { get; set; }

        [Required]
        [MaxLength(80)]
        public string Manager { get; set; } = null!;

        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
