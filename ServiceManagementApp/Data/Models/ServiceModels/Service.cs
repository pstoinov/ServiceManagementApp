using ServiceManagementApp.Data.Models.ClientModels;
using ServiceManagementApp.Data.Models.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManagementApp.Data.Models.ServiceModels
{
    public class Service
    {
       

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public  string ServiceName { get; set; } = null!;

        [Required]
        [MaxLength(13)]
        public string EIK { get; set; } = null!;

        [MaxLength(15)]
        public string? VATNumber { get; set; } = string.Empty;
        [ForeignKey(nameof(AddressId))]
        public int AddressId { get; set; }
        public Address Address { get; set; } = null!;
        [ForeignKey(nameof(PhoneId))]
        public int PhoneId { get; set; }
        public Phone Phone { get; set; } = null!;
        [ForeignKey(nameof(EmailId))]
        public int EmailId { get; set; }
        public Email Email { get; set; } = null!;

        [MaxLength(2083)]
        public string? LogoUrl { get; set; } = string.Empty;

        public bool IsCashRegisterService { get; set; } = false;
        public ICollection<Employee> Employees { get; set; } = [];
    }
}
