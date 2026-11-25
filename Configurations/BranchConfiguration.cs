using G_NET69_EF_Assignment_4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace G_NET69_EF_Assignment_4.Configurations;

public class BranchConfiguration : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.HasKey(branch => branch.Code);

        builder.Property(branch => branch.Code)
            .HasMaxLength(20)
            .IsRequired();
        builder.Property(branch => branch.Name)
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(branch => branch.Address)
            .HasMaxLength(250)
            .IsRequired();
        builder.Property(branch => branch.PhoneNumber)
            .HasMaxLength(30)
            .IsRequired();

        builder.HasOne(branch => branch.Manager)
            .WithOne(manager => manager.Branch)
            .HasForeignKey<Branch>(branch => branch.ManagerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new Branch
            {
                Code = "CAI-01",
                Name = "Cairo Main Branch",
                Address = "15 Tahrir Square, Cairo",
                PhoneNumber = "0223456789",
                ManagerId = 1
            },
            new Branch
            {
                Code = "ALX-01",
                Name = "Alexandria Branch",
                Address = "10 Saad Zaghloul Street, Alexandria",
                PhoneNumber = "0334567890",
                ManagerId = 2
            });
    }
}
