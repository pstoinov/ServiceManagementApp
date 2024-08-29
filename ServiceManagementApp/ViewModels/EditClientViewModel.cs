namespace ServiceManagementApp.ViewModels
{
    public class EditClientViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Phone { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public List<CompanyViewModel> Companies { get; set; } = new List<CompanyViewModel>();
    }
}
