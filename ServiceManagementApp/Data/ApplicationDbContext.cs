using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServiceManagementApp.Data.Models.ClientModels;
using ServiceManagementApp.Data.Models.Core;
using ServiceManagementApp.Data.Models.RepairModels;
using ServiceManagementApp.Data.Models.RequestModels;
using ServiceManagementApp.Data.Models.ServiceModels;
using ServiceManagementApp.Data.Models.Wherehouse;

namespace ServiceManagementApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Service> Services { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CashRegister> CashRegisters { get; set; }
        public DbSet<CashRegisterRepair> CashRegisterRepairs { get; set; }
        public DbSet<Repair> Repairs { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<ClientCompany> ClientCompanies { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            

            // CashRegister
            modelBuilder.Entity<CashRegister>()
            .HasOne(cr => cr.Service)
            .WithMany()
            .HasForeignKey(cr => cr.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CashRegister>()
                .HasOne(cr => cr.Company)
                .WithMany(c => c.CashRegisters)
                .HasForeignKey(cr => cr.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CashRegister>()
                .HasOne(cr => cr.SiteAddress)
                .WithMany()
                .HasForeignKey(cr => cr.SiteAddressId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CashRegister>()
                .HasOne(cr => cr.ContactPhone)
                .WithMany()
                .HasForeignKey(cr => cr.ContactPhoneId)
                .OnDelete(DeleteBehavior.Restrict);
           // Employee
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Service)
                .WithMany(s => s.Employees)
                .HasForeignKey(e => e.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.EmployeePhoneNumber)
                .WithMany()
                .HasForeignKey(e => e.PhoneId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.EmployeeEmailAddress)
                .WithMany()
                .HasForeignKey(e => e.EmailId)
                .OnDelete(DeleteBehavior.Restrict);
            //Contract
            modelBuilder.Entity<Contract>()
                .HasOne(c => c.Service)
                .WithMany() 
                .HasForeignKey(c => c.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Contract>()
                .HasOne(c => c.Company)
                .WithMany() 
                .HasForeignKey(c => c.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Contract>()
                .HasOne(c => c.CashRegister)
                .WithMany(cr => cr.Contracts) 
                 .HasForeignKey(c => c.CashRegisterId)
                .OnDelete(DeleteBehavior.Restrict);

            // ClientCompany
            modelBuilder.Entity<ClientCompany>()
                .HasKey(cc => new { cc.ClientId, cc.CompanyId });

            modelBuilder.Entity<ClientCompany>()
                .HasOne(cc => cc.Client)
                .WithMany(c => c.ClientCompanies)
                .HasForeignKey(cc => cc.ClientId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<ClientCompany>()
                .HasOne(cc => cc.Company)
                .WithMany(c => c.ClientCompanies)
                .HasForeignKey(cc => cc.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
