namespace ServiceManagementApp.ViewModels
{
    public class EditCompanyViewModel
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = null!;
        public string EIK { get; set; } = null!;
        public string? VATNumber { get; set; }
        public string Manager { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Address { get; set; }
        public int ClientId { get; set; }
    }
}
