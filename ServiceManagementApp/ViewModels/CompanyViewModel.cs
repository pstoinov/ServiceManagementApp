namespace ServiceManagementApp.ViewModels
{
    public class CompanyViewModel
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = null!;
        public string EIK { get; set; } = null!;
        public string? VATNumber { get; set; }
        public string? Manager {  get; set; }
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
