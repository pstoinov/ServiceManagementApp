using ServiceManagementApp.Data.Enums;
using ServiceManagementApp.Data.Models.Core;
using ServiceManagementApp.Data.Models.RepairModels;
using ServiceManagementApp.Data.Models.RequestModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManagementApp.Data.Models.ServiceModels
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; } = null!;
        [Required]
        public Position Position { get; set; }

        [ForeignKey(nameof(PhoneId))]
        public int PhoneId { get; set; }
        public Phone EmployeePhoneNumber { get; set; } = null!;

        [ForeignKey(nameof(EmailId))]
        public int EmailId { get; set; }
        public Email EmployeeEmailAddress { get; set; } = null!;

        public string? EGN { get; set; }

        [ForeignKey(nameof(ServiceId))]
        public int ServiceId { get; set; }
        public Service Service { get; set; } = null!;

        ICollection<Repair>? Repairs { get; set; } = new HashSet<Repair>();
        ICollection<CashRegisterRepair>? CashRegisterRepairs { get; set; } = new HashSet<CashRegisterRepair>();

        ICollection<ServiceRequest>? ServiceRequests { get; set; } = new HashSet<ServiceRequest>();

        public bool IsCertifiedForCashRegisterRepair { get; set; } = false;
    }
}
