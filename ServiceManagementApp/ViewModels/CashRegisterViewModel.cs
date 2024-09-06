using ServiceManagementApp.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceManagementApp.ViewModels
{
    public class CashRegisterViewModel
    {
        public int Id { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [Required]
        public int CompanyId { get; set; }

        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string SiteName { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;
        [Required]
        public string Street { get; set; } = string .Empty;
        [Required]
        public string Number { get; set; } = string.Empty;
        public string? Block { get; set; }

        [Required]
        public int SiteAddressId { get; set; }

        public string SiteAddress { get; set; } = string.Empty;
        [Required]
        [MaxLength(255)]
        public string SiteManager { get; set; } = string.Empty;

        [Required]
        public int ContactPhoneId { get; set; }

        public string ContactPhone { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string RegionalNRAOffice { get; set; } = "Пловдив";

        [Required]
        public Manufacturer Manufacturer { get; set; }

        [Required]
        [MaxLength(6)]
        public string BIMCertificateNumber { get; set; } = string.Empty;

        [Required]
        public DateTime? BIMCertificateDate {  get; set; }

        [Required]
        [MaxLength(50)]
        public string SerialNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(15)]
        public string FiscalMemoryNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(10)]
        public string FDRIDNumber { get; set; } = string.Empty;

        [Required]
        public DateTime FirstRegistrationDate { get; set; }

        [Required]
        public bool IsDisposed { get; set; } = false;

        [Required]
        public bool IsRegistered { get; set; } = true;

        public string PhoneSearch { get; set; } = string.Empty;
    }
}
