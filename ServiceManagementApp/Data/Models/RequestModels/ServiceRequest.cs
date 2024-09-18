using ServiceManagementApp.Data.Enums;
using ServiceManagementApp.Data.Models.ClientModels;
using ServiceManagementApp.Data.Models.RepairModels;
using ServiceManagementApp.Data.Models.ServiceModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManagementApp.Data.Models.RequestModels
{
    public class ServiceRequest
    {
        [Key]
        public int Id { get; set; }

        public int? ClientCompanyId { get; set; } 
        public Company? ClientCompany { get; set; }
        [Required]
        [MaxLength(20)]
        public string RequestNumber { get; set; } = "R000000";

        [Required]
        public DateTime RequestDate { get; set; }
        [Required]
        public DateTime ExpectedCompletionDate { get; set; }

        public DateTime? ResolvedDate { get; set; }


        public int ClientId { get; set; }
        [ForeignKey(nameof(ClientId))]
        public Client Client { get; set; } = null!;

        public int ServiceId {  get; set; }
        [ForeignKey(nameof(ServiceId))]
        public Service? Service { get; set; }

        public int? EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public Employee? Employee { get; set; }

        public Repair? Repair { get; set; }

        [Required]
        public string ProblemDescription { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Device {  get; set; }

        public string? Accessories { get; set; }

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
