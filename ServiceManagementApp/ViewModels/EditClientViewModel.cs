namespace ServiceManagementApp.ViewModels
{
    public class EditClientViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string? Phone { get; set; } = null!;
        public string? Email { get; set; } = null!;
        //public List<CompanyViewModel> Companies { get; set; } = new List<CompanyViewModel>();
    }
}
