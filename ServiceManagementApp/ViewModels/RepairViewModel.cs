using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceManagementApp.ViewModels
{
    public class RepairViewModel
    {
        public int Id { get; set; }

        [Required]
        public DateTime StartRepairDate { get; set; } = DateTime.Now; 

        public DateTime? EndRepairDate { get; set; } 

        public DateTime? TakenByClient { get; set; }

        public string Status { get; set; } = string.Empty; 

        [Required]
        [MaxLength(1000)]
        public string ProblemDescription { get; set; } = string.Empty; 

        [MaxLength(1000)]
        public string? RepairDescription { get; set; } 

        public decimal? RepairCost { get; set; } 

        [Required]
        public int ClientId { get; set; } 

        [Required]
        public int EmployeeId { get; set; } 

        [Required]
        public int ServiceRequestId { get; set; } 

        // Полета за рендериране на имена във View-то
        public string ClientName { get; set; } = string.Empty; // Не е задължително, но ще се използва за показване на името на клиента
        public string EmployeeName { get; set; } = string.Empty; // Не е задължително, но ще се използва за показване на името на техника
        public string RequestNumber { get; set; } = string.Empty; // Не е задължително, но ще се използва за показване на номера на заявката
    }
}
