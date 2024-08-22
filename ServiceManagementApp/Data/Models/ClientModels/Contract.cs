using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ServiceManagementApp.Data.Models.ServiceModels;

namespace ServiceManagementApp.Data.Models.ClientModels
{
    public class Contract
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string ContractNumber { get; set; } = null!;

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

        public int CashRegisterId { get; set; }
        [ForeignKey(nameof(CashRegisterId))]
        public CashRegister CashRegister { get; set; } = null!;
    }
}
