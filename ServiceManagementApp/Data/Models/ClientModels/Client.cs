using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ServiceManagementApp.Data.Models.Core;
using ServiceManagementApp.Data.Models.RepairModels;
using ServiceManagementApp.Data.Models.RequestModels;

namespace ServiceManagementApp.Data.Models.ClientModels
{
    public class Client
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string FullName { get; set; } = null!;
        [ForeignKey(nameof(PhoneId))]
        public int PhoneId { get; set; }
        public Phone? Phone { get; set; }
        [ForeignKey(nameof(EmailId))]
        public int EmailId { get; set; }
        public Email? Email { get; set; }

        public ICollection<ClientCompany>? ClientCompanies { get; set; } = new List<ClientCompany>();
        public ICollection<Repair>? Repairs { get; set; } = new List<Repair>();
        public ICollection<ServiceRequest>? ServiceRequests { get; set; } = new List<ServiceRequest>();

    }
}
