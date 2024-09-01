using ServiceManagementApp.Data.Enums;
using ServiceManagementApp.Data.Models.ServiceModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManagementApp.Data.Models.RequestModels
{
    public class ServiceRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime RequestDate { get; set; }
        [Required]
        public DateTime ExpectedCompleteTime { get; set; }

        public DateTime? ResolvedDate { get; set; }


        [Required]
        [MaxLength(255)]
        public string ClientName { get; set; } = null!;

        [MaxLength(100)]
        public string? ClientEmail { get; set; }

        [MaxLength(15)]
        public string? ClientPhone { get; set; }

        public int? EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public Employee? Employee { get; set; }

        [Required]
        public string ProblemDescription { get; set; } = null!;

        [Required]
        public ServiceRequestStatus Status { get; set; } = ServiceRequestStatus.New;

        [Required]
        public ServiceRequestPriority Priority { get; set; } = ServiceRequestPriority.Medium;
        [Required]
        public ServiceRequestType RequestType { get; set; } = ServiceRequestType.OnSite;

        [MaxLength(500)]
        public string? ServiceNotes { get; set; } = string.Empty;

        [Required]
        public bool isCashRegister { get; set; } = false;

    }
}
