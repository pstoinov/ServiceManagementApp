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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ClientCompany>()
                .HasKey(cc => new { cc.ClientId, cc.CompanyId });
        }
    }
}
