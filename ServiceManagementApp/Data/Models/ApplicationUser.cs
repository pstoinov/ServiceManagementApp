using Microsoft.AspNetCore.Identity;

namespace ServiceManagementApp.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;

        public int ClientId { get; set; }
        public Client Client { get; set; } = null!;
    }
}
