using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceManagementApp.ViewModels
{
    public class EditCompanyViewModel
    {
        public int Id { get; set; }

        public string CompanyName { get; set; } = null!;

        public string EIK { get; set; } = null!;

        public string? VATNumber { get; set; }

        public string Phone { get; set; } = null!;

        public string Email { get; set; } = null!;


        //// A list of clients associated with the company
        //public List<ClientViewModel> Clients { get; set; } = new();

        //// Dropdown list for clients
        //public List<SelectListItem> ClientList { get; set; } = new();
    }
}
