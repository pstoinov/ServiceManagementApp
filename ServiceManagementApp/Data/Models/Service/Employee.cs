using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManagementApp.Data.Models.Service
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; } = null!;
        [Required]
        public string Position { get; set; } = null!;

        [ForeignKey(nameof(PhoneId))]
        public int PhoneId { get; set; }
        public Phone EmployeePhoneNumber { get; set; } = null!;

        [ForeignKey(nameof(EmailId))]
        public int EmailId { get; set; }
        public Email EmployeeEmailAddress { get; set; } = null!;

        public string? EGN { get; set; }


        public bool IsCertifiedForCashRegisterRepair { get; set; } = false;
    }
}
