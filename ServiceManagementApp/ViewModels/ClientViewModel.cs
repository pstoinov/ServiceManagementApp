using System.ComponentModel.DataAnnotations;

namespace ServiceManagementApp.ViewModels
{
    public class ClientViewModel
    {
        [Required]
        public string FullName { get; set; } = null!;
        public string? Phone { get; set; }
        [EmailAddress(ErrorMessage = "Моля, въведете валиден имейл адрес.")]
        public string? Email { get; set; }
    }
}
