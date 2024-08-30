using System.ComponentModel.DataAnnotations;

namespace ServiceManagementApp.ViewModels
{
    public class CreateCompanyViewModel
    {
        [Required]
        public string CompanyName { get; set; } = null!;

        [Required]
        [MaxLength(13)]
        public string EIK { get; set; } = null!;

        [MaxLength(15)]
        public string? VATNumber { get; set; }

        [Required]
        public string Manager { get; set; } = null!;

        [Required]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; } = null!;

        [Required]
        public string City { get; set; } = null!;

        [Required]
        public string Street { get; set; } = null!;

        [Required]
        public string Number { get; set; } = null!;

        public string? Block { get; set; }
    }
}
