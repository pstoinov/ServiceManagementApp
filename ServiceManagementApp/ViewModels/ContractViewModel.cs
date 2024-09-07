using System.ComponentModel.DataAnnotations;

namespace ServiceManagementApp.ViewModels
{
    public class ContractViewModel
    {
        public int Id { get; set; }

        public string ContractNumber { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public int ContractDurationMonths { get; set; }

        public DateTime EndDate { get; set; } 

        [Required]
        public int CompanyId { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [Required]
        public int CashRegisterId { get; set; }

        public bool IsActive { get; set; } = true;

        public string CompanyName { get; set; } = string.Empty;
        public string CashRegisterSerialNumber { get; set; } = string.Empty;
    }
}
