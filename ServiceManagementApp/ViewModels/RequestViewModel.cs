using ServiceManagementApp.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceManagementApp.ViewModels
{
    public class RequestViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string RequestNumber { get; set; } = "R000000";

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

        [Required]
        public string ProblemDescription { get; set; } = null!;

        [MaxLength(255)]
        public string? Device { get; set; }

        public string? Accessories { get; set; }

        [Required]
        public ServiceRequestPriority Priority { get; set; } = ServiceRequestPriority.Medium;

        [Required]
        public ServiceRequestStatus Status { get; set; }

        [Required]
        public ServiceRequestType RequestType { get; set; }

        public bool isCashRegister { get; set; } = false;
    }
}
