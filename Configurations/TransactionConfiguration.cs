using G_NET69_EF_Assignment_4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace G_NET69_EF_Assignment_4.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(transaction => transaction.TransactionNumber);

        builder.Property(transaction => transaction.Amount)
            .HasPrecision(18, 2);
        builder.Property(transaction => transaction.TransactionType)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();
        builder.Property(transaction => transaction.Note)
            .HasMaxLength(500);
        builder.Property(transaction => transaction.AccountNumber)
            .HasMaxLength(30)
            .IsRequired();

        builder.HasOne(transaction => transaction.Account)
            .WithMany(account => account.Transactions)
            .HasForeignKey(transaction => transaction.AccountNumber)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
