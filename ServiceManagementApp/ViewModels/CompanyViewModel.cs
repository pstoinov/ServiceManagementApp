using System.ComponentModel.DataAnnotations;

namespace ServiceManagementApp.ViewModels
{
    public class CompanyViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]

        [MaxLength(255)]
        public string CompanyName { get; set; } =string.Empty;

        [Required]
        [MaxLength(13)]
        public string EIK { get; set; } = string.Empty; 

        [MaxLength(15)]
        public string? VATNumber { get; set; }

        [Required]
        [MaxLength(80)]
        public string Manager { get; set; } = string.Empty;

        public int AddressId { get; set; }

        // Address Details
        [Required]
        [MaxLength(100)]
        public string? City { get; set; }

        [MaxLength(255)]
        public string? Street { get; set; }

        [MaxLength(5)]
        public string? Number { get; set; }

        [MaxLength(5)]
        public string? Block { get; set; }

        public int PhoneId { get; set; }
        public string Phone { get; set; } = string.Empty;
        public int EmailId { get; set; }
        public string Email { get; set; } = string.Empty ;
    }
}
