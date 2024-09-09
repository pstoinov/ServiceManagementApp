using ServiceManagementApp.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceManagementApp.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; } = null!;


        public int ServiceId { get; set; }
        public string? ServiceName { get; set; } = string.Empty;

        [Required]
        public Position Position { get; set; }
        [Required]
        public string EmailAddress { get; set; } = null!;

        [Required]
        public string PhoneNumber { get; set; } = null!;

        public bool IsCertifiedForCashRegisterRepair { get; set; } =false;
        [MaxLength(10)]
        public string? EGN { get; set; } = string.Empty;

        //public string? PictureUrl { get; set; } = string.Empty;
    }
}
