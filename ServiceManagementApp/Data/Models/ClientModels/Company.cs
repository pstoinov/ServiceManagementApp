﻿using ServiceManagementApp.Data.Models.Core;
using ServiceManagementApp.Data.Models.RepairModels;
using ServiceManagementApp.Data.Models.RequestModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManagementApp.Data.Models.ClientModels
{
    public class Company
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [MaxLength(13)]
        public string EIK { get; set; } = string.Empty;

        [MaxLength(15)]
        public string? VATNumber { get; set; }
        
        [Required]
        public int AddressId { get; set; }
        [ForeignKey(nameof(AddressId))]
        public Address Address { get; set; } = null!;

        [Required]
        [MaxLength(80)]
        public string Manager { get; set; } = string.Empty;
        [ForeignKey(nameof(PhoneId))]
        public int PhoneId { get; set; }
        public Phone Phone { get; set; } = null!;
        [ForeignKey(nameof(EmailId))]
        public int EmailId { get; set; }
        public Email Email { get; set; } = null!;
        public ICollection<ClientCompany> ClientCompanies { get; set; } = new List<ClientCompany>();
        public ICollection<CashRegister> CashRegisters { get; set; } = new List<CashRegister>();
        public ICollection<Repair> Repairs { get; set; } = new List<Repair>();
        public ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();
    }
}
