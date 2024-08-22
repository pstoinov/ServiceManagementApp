using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Numerics;

namespace ServiceManagementApp.Data.Models.Service
{
    public class Service
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public required string ServiceName { get; set; }

        [Required]
        [MaxLength(13)]
        public required string EIK { get; set; }

        [MaxLength(15)]
        public string? VATNumber { get; set; }
        [ForeignKey(nameof(AddressId))]
        public int AddressId { get; set; }
        public Address Address { get; set; } = null!;
        [ForeignKey(nameof(PhoneId))]
        public int PhoneId { get; set; }
        public Phone Phone { get; set; } = null!;
        [ForeignKey(nameof(EmailId))]
        public int EmailId { get; set; }
        public Email Email { get; set; } = null!;

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();

        public ICollection<Client> Clients { get; set; } = new List<Client>();
    }
}
