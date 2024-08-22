using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManagementApp.Data.Models
{
    public class Company
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string CompanyName { get; set; } = null!;

        [Required]
        [MaxLength(13)]
        public string EIK { get; set; } = null!;

        [MaxLength(15)]
        public string? VATNumber { get; set; }
        [Required]
        [ForeignKey(nameof(AddressId))]
        public int AddressId { get; set; }
        public Address Address { get; set; } = null!;
        
        [Required]
        [MaxLength(255)]
        public string Manager { get; set; } = null!;
        [ForeignKey(nameof(PhoneId))]
        public int PhoneId { get; set; }
        public Phone Phone { get; set; } = null!;
        [ForeignKey(nameof(EmailId))]
        public int EmailId { get; set; }
        public Email Email { get; set; } = null!;
        public ICollection<ClientCompany> ClientCompanies { get; set; } = new List<ClientCompany>();
        public ICollection<CashRegister>? CashRegisters { get; set; } = new List<CashRegister>();
    }
}
