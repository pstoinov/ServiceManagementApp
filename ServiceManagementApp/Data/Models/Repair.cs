using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ServiceManagementApp.Data.Models
{
    public class Repair
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime StartRepairDate { get; set; }

        public DateTime? EndRepairDate { get; set; } // Може да бъде null, ако ремонтът не е приключил

        [Required]
        [MaxLength(1000)]
        public string ProblemDescription { get; set; } = null!; // Описание на проблема

        [MaxLength(1000)]
        public string? RepairDescription { get; set; } // Описание на извършения ремонт, не е задължително

        public ICollection<RepairPart>? RepairPart { get; set; } = new List<RepairPart>(); // Списък от вложени части, не е задължително

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal RepairCost { get; set; } // Цена на ремонта

        [Required]
        public int ClientId { get; set; }
        [ForeignKey(nameof(ClientId))]
        public Client Client { get; set; } = null!; // Задължително поле за клиента

        [Required]
        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; }

        public int? CompanyId { get; set; } // Поле за фирма, не е задължително
        [ForeignKey(nameof(CompanyId))]
        public Company? Company { get; set; }

        public bool IsOnSiteRepair { get; set; } = false; // Поле за това дали ремонтът е на адрес при клиента
    }
}
