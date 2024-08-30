using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceManagementApp.ViewModels
{
    public class AssociateClientCompanyViewModel
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = null!;
        public int SelectedClientId { get; set; }
        public List<SelectListItem> Clients { get; set; } = new();
    }
}
