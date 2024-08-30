using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceManagementApp.ViewModels
{
    public class EditCompanyViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The company name is required.")]
        [MaxLength(255)]
        public string CompanyName { get; set; } = null!;

        [Required(ErrorMessage = "The EIK is required.")]
        [MaxLength(13)]
        public string EIK { get; set; } = null!;

        [MaxLength(15)]
        public string? VATNumber { get; set; }

        [Required(ErrorMessage = "The phone number is required.")]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "The email is required.")]
        [EmailAddress(ErrorMessage = "The email address is not valid.")]
        public string Email { get; set; } = null!;


        // A list of clients associated with the company
        public List<ClientViewModel> Clients { get; set; } = new();

        // Dropdown list for clients
        public List<SelectListItem> ClientList { get; set; } = new();
    }
}
