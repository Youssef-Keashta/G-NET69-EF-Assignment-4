using G_NET69_EF_Assignment_4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace G_NET69_EF_Assignment_4.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(customer => customer.Id);

        builder.Property(customer => customer.FullName)
            .HasMaxLength(150)
            .IsRequired();
        builder.Property(customer => customer.Email)
            .HasMaxLength(255)
            .IsRequired();
        builder.Property(customer => customer.PhoneNumber)
            .HasMaxLength(30)
            .IsRequired();
        builder.Property(customer => customer.Address)
            .HasMaxLength(250)
            .IsRequired();
        builder.Property(customer => customer.NationalId)
            .HasMaxLength(14)
            .IsRequired();
        builder.Property(customer => customer.CustomerType)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.HasIndex(customer => customer.NationalId).IsUnique();
        builder.HasIndex(customer => customer.Email).IsUnique();
    }
}
