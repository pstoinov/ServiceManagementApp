namespace ServiceManagementApp.ViewModels
{
    public class CompanyViewModel
    {
        public int Id { get; set; }
        public string? CompanyName { get; set; }
        public string? EIK { get; set; }
        public string? VATNumber { get; set; }
        public string? Manager {  get; set; }
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
