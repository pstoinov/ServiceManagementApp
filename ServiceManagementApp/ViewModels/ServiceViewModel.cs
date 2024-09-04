using System.ComponentModel.DataAnnotations;

namespace ServiceManagementApp.ViewModels
{
    public class ServiceViewModel
    {
        public int Id { get; set; }
        [Required]
        public string ServiceName { get; set; } = null!;
        [Required]
        public string EIK { get; set; } = null!;
        public string? VATNumber { get; set; } = string.Empty;
        //Address
        [Required]
        public string City { get; set; } = null!;
        [Required]
        public string Street { get; set; } = null!;
        [Required]
        public string Number { get; set; } = null!;
        public string? Block { get; set; } = string.Empty;
        [Required]
        public string PhoneNumber { get; set; } = null!;
        [Required]
        public string EmailAddress { get; set; } = null!;
        public string? LogoUrl { get; set; } = string.Empty;

        public ICollection<EmployeeViewModel> Employees { get; set; } = [];
    }
}
