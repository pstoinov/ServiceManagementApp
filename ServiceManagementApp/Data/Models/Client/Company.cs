using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ServiceManagementApp.Data.Models.Service;

namespace ServiceManagementApp.Data.Models.Client
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
        public ICollection<Repair>? Repairs { get; set; } = new List<Repair>();
        public ICollection<ServiceRequest>? ServiceRequests { get; set; } = new List<ServiceRequest>();
    }
}
