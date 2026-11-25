using G_NET69_EF_Assignment_4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace G_NET69_EF_Assignment_4.Configurations;

public class ManagerConfiguration : IEntityTypeConfiguration<Manager>
{
    public void Configure(EntityTypeBuilder<Manager> builder)
    {
        builder.HasKey(manager => manager.Id);

        builder.Property(manager => manager.FullName)
            .HasMaxLength(150)
            .IsRequired();
        builder.Property(manager => manager.Email)
            .HasMaxLength(255)
            .IsRequired();
        builder.Property(manager => manager.PhoneNumber)
            .HasMaxLength(30)
            .IsRequired();
        builder.HasIndex(manager => manager.Email).IsUnique();

        builder.HasData(
            new Manager
            {
                Id = 1,
                FullName = "Ahmed Hassan",
                Email = "ahmed.hassan@nationalbank.example",
                PhoneNumber = "01012345678",
                HireDate = new DateTime(2018, 5, 1)
            },
            new Manager
            {
                Id = 2,
                FullName = "Mariam Ali",
                Email = "mariam.ali@nationalbank.example",
                PhoneNumber = "01098765432",
                HireDate = new DateTime(2020, 8, 15)
            });
    }
}
