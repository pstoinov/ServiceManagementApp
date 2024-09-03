using ServiceManagementApp.Data.Models.Core;
using ServiceManagementApp.Data.Models.RepairModels;
using ServiceManagementApp.Data.Models.RequestModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManagementApp.Data.Models.ClientModels
{
    public class Client
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string FullName { get; set; } = string.Empty;
        [ForeignKey(nameof(PhoneId))]
        public int PhoneId { get; set; }
        public Phone Phone { get; set; } = null!;
        [ForeignKey(nameof(EmailId))]
        public int EmailId { get; set; }
        public Email Email { get; set; } = null!;

        public ICollection<ClientCompany> ClientCompanies { get; set; } = new List<ClientCompany>();
        public ICollection<Repair> Repairs { get; set; } = new List<Repair>();
        public ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();

    }
}
