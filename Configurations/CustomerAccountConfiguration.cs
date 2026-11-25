using G_NET69_EF_Assignment_4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace G_NET69_EF_Assignment_4.Configurations;

public class CustomerAccountConfiguration : IEntityTypeConfiguration<CustomerAccount>
{
    public void Configure(EntityTypeBuilder<CustomerAccount> builder)
    {
        builder.HasKey(link => new { link.CustomerId, link.AccountNumber });

        builder.Property(link => link.AccountNumber)
            .HasMaxLength(30)
            .IsRequired();
        builder.Property(link => link.OwnershipType)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();
        builder.Property(link => link.AccountStatus)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.HasOne(link => link.Customer)
            .WithMany(customer => customer.CustomerAccounts)
            .HasForeignKey(link => link.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(link => link.Account)
            .WithMany(account => account.CustomerAccounts)
            .HasForeignKey(link => link.AccountNumber)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
