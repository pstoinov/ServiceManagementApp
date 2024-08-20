using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using ServiceManagementApp.Data.Enums;

namespace ServiceManagementApp.Data.Models
{
    public class CashRegister
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public Company Company { get; set; } = null!;

        [Required]
        public int AddressId { get; set; }
        [ForeignKey("AddressId")]
        public Address Address { get; set; } = null!;

        [Required]
        public string Manager { get; set; } = null!; 

        [Required]
        [MaxLength(255)]
        public string SiteName { get; set; } = null!;

        [Required]
        public int SiteAddressId { get; set; }
        [ForeignKey("SiteAddressId")]
        public Address SiteAddress { get; set; } = null!;

        [Required]
        public int ContactPhoneId { get; set; }
        [ForeignKey("ContactPhoneId")]
        public Phone ContactPhone { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string RegionalNAPOffice { get; set; } = null!; 

        [Required]
        public Manufacturer Manufacturer { get; set; } 
        [Required]
        [MaxLength(6)]
        public string BIMCertificateNumber { get; set; } = null!; 

        [Required]
        [MaxLength(50)]
        public string SerialNumber { get; set; } = null!; 

        [Required]
        [MaxLength(50)]
        public string FiscalMemoryNumber { get; set; } = null!; 

        [Required]
        [MaxLength(50)]
        public string FDRIDNumber { get; set; } = null!; 

        [Required]
        public DateTime FirstRegistrationDate { get; set; }
        
        [Required]
        public bool IsDisposed { get; set; } = false;
        public ICollection<Contract> Contracts { get; set; } = new List<Contract>(); // Колекция от договори

        public ICollection<CashRegisterRepair> CashRegisterRepairs { get; set; } = new List<CashRegisterRepair>(); // Колекция от ремонти
    }
}
