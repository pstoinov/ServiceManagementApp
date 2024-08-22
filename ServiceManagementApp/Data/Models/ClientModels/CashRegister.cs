using ServiceManagementApp.Data.Enums;
using ServiceManagementApp.Data.Models.Core;
using ServiceManagementApp.Data.Models.RepairModels;
using ServiceManagementApp.Data.Models.ServiceModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManagementApp.Data.Models.ClientModels
{
    public class CashRegister
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ServiceId { get; set; }
        [ForeignKey(nameof(ServiceId))]
        public Service Service { get; set; } = null!;
        [Required]
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; } = null!;

        [Required]
        public int AddressId { get; set; }
        [ForeignKey(nameof(AddressId))]
        public Address Address { get; set; } = null!;

        [Required]
        public string Manager { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public string SiteName { get; set; } = null!;

        [Required]
        public int SiteAddressId { get; set; }
        [ForeignKey(nameof(SiteAddressId))]
        public Address SiteAddress { get; set; } = null!;

        [Required]
        public int ContactPhoneId { get; set; }
        [ForeignKey(nameof(ContactPhoneId))]
        public Phone ContactPhone { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string RegionalNRAOffice { get; set; } = null!;

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
        //TODO: Да се направи логика за дерегистрация
        public bool IsRegistered { get; set; } = true;

        public ICollection<Contract> Contracts { get; set; } = new List<Contract>(); // Колекция от договори

        public ICollection<CashRegisterRepair> CashRegisterRepairs { get; set; } = new List<CashRegisterRepair>(); // Колекция от ремонти

    }
}
