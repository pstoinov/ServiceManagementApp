using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Numerics;

namespace ServiceManagementApp.Data.Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public required string CompanyName { get; set; }

        [Required]
        [MaxLength(13)]
        public required string EIK { get; set; }

        [MaxLength(15)]
        public string? VATNumber { get; set; }
        [Required]
        public int AddressId { get; set; }
        public Address Address { get; set; } = null!;

        public int PhoneId { get; set; }
        public Phone Phone { get; set; } = null!;

        public int EmailId { get; set; }
        public Email Email { get; set; } = null!;

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();

        public ICollection<Client> Clients { get; set; } = new List<Client>();
    }
}
