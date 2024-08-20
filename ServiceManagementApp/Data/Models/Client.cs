using System.ComponentModel.DataAnnotations;

namespace ServiceManagementApp.Data.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string FullName { get; set; }

        public int PhoneId { get; set; }
        public Phone? Phone { get; set; }

        public int EmailId { get; set; }
        public Email? Email { get; set; }

        // Връзка към фирми
        public ICollection<ClientCompany> ClientCompanies { get; set; } = new List<ClientCompany>();
    }
}
