using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManagementApp.Data.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; } = null!;
        [Required]
        public string Position { get; set; } = null!;

        [Required]
        public int PhoneId { get; set; }

        [ForeignKey("PhoneId")]
        public Phone EmployeePhoneNumber { get; set; } = null!;

        [Required]
        public int EmailId { get; set; }

        [ForeignKey("EmailId")]
        public Email EmployeeEmailAddress { get; set; } = null!;

        public string? EGN { get; set; }


        public bool IsCertifiedForCashRegisterRepair { get; set; } = false;
    }
}
