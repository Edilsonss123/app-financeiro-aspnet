using Fina.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fina.Api.Data.Mappings
{
    public class TransactionMapping : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transaction");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Title)
                .IsRequired()
                .HasColumnType("NVARCHAR")
                .HasMaxLength(80);
            builder.Property(c => c.Type)
                .IsRequired()   
                .HasColumnType("SMALLINT");
            builder.Property(c => c.Amount)
                .IsRequired()   
                .HasColumnType("DECIMAL(19,4)");
            builder.Property(c => c.CreateAt)
                .IsRequired();
            builder.Property(c => c.PaidOrReceivedAt)
                .IsRequired(false);
            builder.Property(c => c.UserId)
                .IsRequired()
                .HasColumnType("VARCHAR")
                .HasMaxLength(160);
        }
    }
}