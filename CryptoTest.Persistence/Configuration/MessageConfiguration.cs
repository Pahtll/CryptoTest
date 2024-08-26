using CryptoTest.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoTest.Persistence.Configuration;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(m => m.Id);
        
        builder
            .Property(m => m.Text)
            .HasMaxLength(128)
            .IsRequired();

        builder
            .Property(m => m.SentAt)
            .IsRequired();
    }
}