using ServiceManagementApp.Data.Enums;
using ServiceManagementApp.Data.Models.ClientModels;
using ServiceManagementApp.Data.Models.RequestModels;
using ServiceManagementApp.Data.Models.ServiceModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManagementApp.Data.Models.RepairModels
{
    public class Repair
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime StartRepairDate { get; set; }

        public DateTime? EndRepairDate { get; set; }

        public DateTime? TakenByClient { get; set; }

        [Required]
        [MaxLength(1000)]
        public string ProblemDescription { get; set; } = string.Empty; // Описание какво е отркил като проблем сервизният техник

        [MaxLength(1000)]
        public string? RepairDescription { get; set; } // Описание на извършения ремонт, не е задължително

        public ICollection<RepairPart>? RepairPart { get; set; } = new List<RepairPart>(); // Списък от вложени части, не е задължително

        [Required]
        public ServiceRequestStatus? Status { get; set; } = ServiceRequestStatus.InProgress;
        
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? RepairCost { get; set; } // Цена на ремонта

        [Required]
        public int ClientId { get; set; }
        [ForeignKey(nameof(ClientId))]
        public Client Client { get; set; } = null!; // Задължително поле за клиента

        [Required]
        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; } = null!;

        public int? CompanyId { get; set; } // Поле за фирма, не е задължително
        [ForeignKey(nameof(CompanyId))]
        public Company? Company { get; set; }

        public int ServiceRequestId { get; set; }
        [ForeignKey(nameof(ServiceRequestId))]
        public ServiceRequest ServiceRequest { get; set; } = null!;



        public bool? IsOnSiteRepair { get; set; } = false; 
        public bool? IsRemoteRepair { get; set; } = false;
    }
}
