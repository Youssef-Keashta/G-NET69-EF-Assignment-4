using G_NET69_EF_Assignment_4.Models;
using Microsoft.EntityFrameworkCore;

namespace G_NET69_EF_Assignment_4.DbContexts;

public class NationalBankDbContext : DbContext
{
    public DbSet<Branch> Branches { get; set; } = null!;
    public DbSet<Manager> Managers { get; set; } = null!;
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<Account> Accounts { get; set; } = null!;
    public DbSet<CustomerAccount> CustomerAccounts { get; set; } = null!;
    public DbSet<Transaction> Transactions { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=DESKTOP-VGTVSKK;" +
            "Database=NationalBankManagementDb;" +
            "Trusted_Connection=True;" +
            "TrustServerCertificate=True");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NationalBankDbContext).Assembly);
    }
}
