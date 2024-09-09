using Microsoft.AspNetCore.Identity;
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
        public string FullName { get; set; } = string.Empty;
        [Required]
        public Position Position { get; set; }

        [ForeignKey(nameof(PhoneId))]
        public int PhoneId { get; set; }
        public Phone PhoneNumber { get; set; } = null!;

        [ForeignKey(nameof(EmailId))]
        public int EmailId { get; set; }
        public Email EmailAddress { get; set; } = null!;

        public string? EGN { get; set; }

        //public string? UserId { get; set; }
        //public IdentityUser? User { get; set; }

        [ForeignKey(nameof(ServiceId))]
        public int ServiceId { get; set; }
        public Service Service { get; set; } = null!;

        public bool IsCertifiedForCashRegisterRepair { get; set; } = false;
        [MaxLength(2083)]
        public string? PictureUrl { get; set; }



    }
}
