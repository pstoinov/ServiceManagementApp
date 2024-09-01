using ServiceManagementApp.Data.Models.ServiceModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManagementApp.Data.Models.ClientModels
{
    public class Contract
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string ContractNumber { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; } = null!;

        [Required]
        public int ServiceId { get; set; }
        [ForeignKey(nameof(ServiceId))]
        public Service Service { get; set; } = null!;
        [Required]
        public int CashRegisterId { get; set; }
        [ForeignKey(nameof(CashRegisterId))]
        public CashRegister CashRegister { get; set; } = null!;
    }
}
